using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public MovePlayerAndTalkCutscene cutsceneScript;
    public string cutsceneID;


    void Start()
    {
        if (StoryManager.instance.HasCutscenePlayed(cutsceneID))
        {
            gameObject.SetActive(false); // Disable trigger if already played
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (cutsceneScript != null)
        {
            cutsceneScript.PlayCutscene();
            gameObject.SetActive(false);
        }
        // Let CutsceneManager handle marking the cutscene as completed
    }
}
