using UnityEngine;
using System;

public class AreaTriggerController : MonoBehaviour
{
    public string requiredCutsceneID = "TBD";

    void Start()
    {
        RefreshState();
    }

    public void RefreshState()
    {
        if (!string.IsNullOrEmpty(requiredCutsceneID))
        {
            bool hasPlayed = StoryManager.instance.HasCutscenePlayed(requiredCutsceneID);
            gameObject.SetActive(hasPlayed);
        }
    }
}