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
    }

    public CutsceneData[] cutscenes;

    void Start()
    {
        foreach (var cutscene in cutscenes)
        {
            if (cutscene.playOnStart && !StoryManager.instance.HasCutscenePlayed(cutscene.cutsceneID))
            {
                PlayCutscene(cutscene);
                break; // Optional: only one cutscene per start
            }
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