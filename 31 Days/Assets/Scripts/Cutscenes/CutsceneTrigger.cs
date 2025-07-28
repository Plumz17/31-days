using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public MovePlayerAndTalkCutscene cutsceneScript;
    public string cutsceneID;

    void Start()
    {
        if (StoryManager.instance.HasCutscenePlayed(cutsceneID))
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (cutsceneScript != null)
        {
            cutsceneScript.PlayCutscene(cutsceneID);

            gameObject.SetActive(false);
        }
    }
}
