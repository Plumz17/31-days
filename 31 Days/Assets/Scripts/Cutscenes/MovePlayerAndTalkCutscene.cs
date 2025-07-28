using UnityEngine;
using System.Collections;

public class MovePlayerAndTalkCutscene : MonoBehaviour
{
    private PlayerMovement player;
    private Transform targetTransform;
    private DialogueTrigger dialogueTrigger; // Optional: triggers dialogue
    public GameObject npcToActivate; // NPC that only appears during cutscene
    public float stopDistance = 0.1f;

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

        if (npcToActivate != null)
        {
            npcToActivate.SetActive(true);

            if (targetTransform == null)
                targetTransform = npcToActivate.transform;

            if (dialogueTrigger == null)
                dialogueTrigger = npcToActivate.GetComponentInChildren<DialogueTrigger>();
        }

        StoryManager.instance.MarkCutscenePlayed(cutsceneID);
        StartCoroutine(StartCutscene());
    }

    private IEnumerator StartCutscene()
    {
        yield return new WaitForSeconds(1f); // Optional delay

        if (targetTransform != null)
        {
            player.WalkToPosition(targetTransform.position, stopDistance);
        }
        else
        {
            Debug.LogWarning("Target Transform is not assigned.");
            yield break;
        }

        // Wait until player reaches destination
        while (Vector2.Distance(player.transform.position, targetTransform.position) > stopDistance)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);


        // Trigger dialogue if available
        if (dialogueTrigger != null)
        {
            dialogueTrigger.TriggerDialogue(); // Your dialogue system call
        }
        
        StoryManager.instance.PrintCompletedCutscenes();

        //npcToActivate.SetActive(false);
    }
}