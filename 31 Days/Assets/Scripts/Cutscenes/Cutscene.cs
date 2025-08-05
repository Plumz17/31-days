using UnityEngine;
using System.Collections;

public class Cutscene : MonoBehaviour
{
    private PlayerMovement player;
    private Transform targetTransform;
    private DialogueTrigger dialogueTrigger; // Optional: triggers dialogue
    public GameObject[] npcToActivate; // NPC that only appears during cutscene
    public float stopDistance = 0.1f; //If Stop distance is 0, don't

    [Header("Optional")]
    public ItemPickUp itemToOpen;
    public Transform GameObjectToGoTo;
    public bool haveOpeningDialogue; //Have Opening Dialogue
    public float cutsceneDelay = 0;

    public void PlayCutscene(string cutsceneID)
    {
        InitializePlayer();
        SetupNPCsAndTarget();
        StoryManager.instance.MarkCutscenePlayed(cutsceneID);
        StartCoroutine(StartCutscene());
    }

    private void SetupNPCsAndTarget()
    {
        if (npcToActivate.Length != 0)
        {
            GameObject mainNPC = npcToActivate[0];
            foreach (GameObject npc in npcToActivate)
            {
                npc.SetActive(true);
            }

            if (targetTransform == null)
                targetTransform = mainNPC.transform;

            if (dialogueTrigger == null)
                dialogueTrigger = mainNPC.GetComponentInChildren<DialogueTrigger>();
        }
        else if (GameObjectToGoTo != null)
        {
            targetTransform = GameObjectToGoTo;
        }
    }

    private void InitializePlayer()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.GetComponent<PlayerMovement>();
            }
        }
    }

    private IEnumerator StartCutscene()
    {
        StoryManager.instance.SetCutsceneState(true);
        if (haveOpeningDialogue)
            yield return RunOpeningDialogue();

        if (player != null && targetTransform != null && stopDistance != 0f)
        {
            player.WalkToPosition(targetTransform.position, stopDistance);
            yield return HandleFootstepSFX();
        }

        if (dialogueTrigger != null && !haveOpeningDialogue)
        {
            yield return new WaitForSeconds(stopDistance != 0 ? 0.2f : 1f);
            dialogueTrigger.TriggerDialogue(true);
        }

        if (itemToOpen != null)
            itemToOpen.ForceOpenItem();

        StoryManager.instance.SetCutsceneState(false);
    }

    private IEnumerator HandleFootstepSFX()
    {
        float stepDelay = player.stepDelay; // Access step delay from PlayerMovement
        float stepTimer = 0f;

        // Wait until player reaches destination
        while (Mathf.Abs(player.transform.position.x - targetTransform.position.x) > stopDistance)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                player.PlayFootstep();
                stepTimer = stepDelay;
            }

            yield return null;
        }
    }

    private IEnumerator RunOpeningDialogue()
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();
        player.SetCanMove(false);

        if (dialogueTrigger != null)
        {
            if (cutsceneDelay > 0f)
                yield return new WaitForSeconds(cutsceneDelay);

            dialogueTrigger.TriggerDialogue(true);

            // Wait until dialogue is finished
            while (DialogueManager.instance != null && DialogueManager.instance.IsActive)
            {
                yield return null;
            }
        }
    }
}