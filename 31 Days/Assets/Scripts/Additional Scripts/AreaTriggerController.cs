using UnityEngine;

public class AreaTriggerController : MonoBehaviour
{
    public string requiredCutsceneID = "TBD";

    void Start()
    {
        RefreshState();
    }

    public void RefreshState()
    {
        bool hasPlayed = StoryManager.instance.HasCutscenePlayed(requiredCutsceneID);
        gameObject.SetActive(hasPlayed);
    }
}