using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public string cutsceneID;

    void Start()
    {
        if (StoryManager.instance.HasCutscenePlayed(cutsceneID))
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        CutsceneManager cutsceneManager = FindFirstObjectByType<CutsceneManager>();
        if (cutsceneManager != null)
        {
            cutsceneManager.PlayCutsceneByID(cutsceneID);
            StoryManager.instance.MarkCutscenePlayed(cutsceneID);
            gameObject.SetActive(false); // Disable trigger after play
        }
        else
        {
            Debug.LogWarning("CutsceneManager not found in scene.");
        }
    }
}