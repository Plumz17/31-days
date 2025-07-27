using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public static StoryManager instance;

    private HashSet<string> completedCutscenes = new HashSet<string>();

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void MarkCutscenePlayed(string cutsceneID)
    {
        completedCutscenes.Add(cutsceneID);
    }

    public bool HasCutscenePlayed(string cutsceneID)
    {
        return completedCutscenes.Contains(cutsceneID);
    }
}