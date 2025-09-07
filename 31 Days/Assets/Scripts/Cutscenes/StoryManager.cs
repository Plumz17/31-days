using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public static StoryManager instance { get; private set; }

    public HashSet<string> completedCutscenes = new HashSet<string>();

    public bool IsCutscenePlaying { get; private set; }

    public void SetCutsceneState(bool isPlaying)
    {
        IsCutscenePlaying = isPlaying;
    }

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
        if (!completedCutscenes.Contains(cutsceneID))
        {
            completedCutscenes.Add(cutsceneID);
            PlayerDataManager.instance.currentData.completedCutscenes.Add(cutsceneID); 
        }
    }

    public bool HasCutscenePlayed(string cutsceneID)
    {
        return completedCutscenes.Contains(cutsceneID);
    }

    public void LoadCutscenesFromPlayerData()
    {
        completedCutscenes = new HashSet<string>(PlayerDataManager.instance.currentData.completedCutscenes);
    }

    public void PrintCompletedCutscenes()
    {
        Debug.Log("Completed Cutscenes:");
        foreach (var cutsceneID in completedCutscenes)
        {
            Debug.Log(cutsceneID);
        }
    }
}