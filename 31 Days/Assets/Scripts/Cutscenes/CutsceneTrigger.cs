using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public MovePlayerAndTalkCutscene cutsceneScript;
    public string cutsceneID;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        
        if (StoryManager.instance.HasCutscenePlayed(cutsceneID)) return;

        if (cutsceneScript != null)
        {
            cutsceneScript.PlayCutscene();
            StoryManager.instance.MarkCutscenePlayed(cutsceneID);

            gameObject.SetActive(false);
        }
    }
}
