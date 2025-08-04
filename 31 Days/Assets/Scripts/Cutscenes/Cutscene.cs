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
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.GetComponent<PlayerMovement>();
            }
        }

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

        StoryManager.instance.MarkCutscenePlayed(cutsceneID);
        StartCoroutine(StartCutscene());
    }

    private IEnumerator StartCutscene()
    {
        StoryManager.instance.SetCutsceneState(true);
        if (haveOpeningDialogue)
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

        if (targetTransform != null && stopDistance != 0f)
        {
            player.WalkToPosition(targetTransform.position, stopDistance);
        }

        if (stopDistance != 0f)
        {
            float stepDelay = player.stepDelay; // Access step delay from PlayerMovement
            float stepTimer = 0f;

            // Wait until player reaches destination
            while (Vector2.Distance(player.transform.position, targetTransform.position) > stopDistance)
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

        if (dialogueTrigger != null && !haveOpeningDialogue)
        {
            yield return new WaitForSeconds(stopDistance != 0 ? 0.2f : 1f);
            dialogueTrigger.TriggerDialogue(true);
        }

        if (itemToOpen != null)
        {
            itemToOpen.ForceOpenItem();
        }

        StoryManager.instance.SetCutsceneState(false);
    }
}