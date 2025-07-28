using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    [System.Serializable]
    public class CutsceneData
    {
        public string cutsceneID;
        public PlayableDirector timeline;
        public bool playOnStart;
        public GameObject rootObject;

        [Header("Optional Prerequisite")]
        public string requiredCutsceneID;
    }

    public CutsceneData[] cutscenes;

     void Start()
    {
        foreach (var cutscene in cutscenes)
        {
            cutscene.rootObject.SetActive(false);
        }

        foreach (var cutscene in cutscenes)
        {
            // Skip if already played
            if (StoryManager.instance.HasCutscenePlayed(cutscene.cutsceneID))
                continue;

            // Check prerequisite if set
            if (!string.IsNullOrEmpty(cutscene.requiredCutsceneID) &&
                !StoryManager.instance.HasCutscenePlayed(cutscene.requiredCutsceneID))
                continue;

            cutscene.rootObject.SetActive(true);

            if (cutscene.playOnStart)
            {
                Debug.Log($"Playing cutscene on start: {cutscene.cutsceneID}");
                PlayCutscene(cutscene);
            }

            break;
        }
    }

    public void PlayCutscene(CutsceneData data)
    {
        data.timeline.stopped += OnCutsceneEnded;
        data.timeline.Play();

        // Disable player input here if needed
    }

    private void OnCutsceneEnded(PlayableDirector director)
    {
        foreach (var cutscene in cutscenes)
        {
            if (cutscene.timeline == director)
            {
                StoryManager.instance.MarkCutscenePlayed(cutscene.cutsceneID);
                cutscene.timeline.stopped -= OnCutsceneEnded;
                break;
            }
        }

        StoryManager.instance.PrintCompletedCutscenes();
        // Re-enable player input here if needed
    }

    public void PlayCutsceneByID(string id)
    {
        foreach (var cutscene in cutscenes)
        {
            if (cutscene.cutsceneID == id)
            {
                PlayCutscene(cutscene);
                return;
            }
        }

        Debug.LogWarning("Cutscene ID not found: " + id);
    }
}