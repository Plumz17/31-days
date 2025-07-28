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
    }

    public CutsceneData[] cutscenes;

    void Awake()
    {
        // Deactivate all cutscenes at start
        foreach (var cutscene in cutscenes)
        {
            if (cutscene.rootObject != null)
                cutscene.rootObject.SetActive(false);
        }
    }

    void Start()
    {
        foreach (var cutscene in cutscenes)
        {
            Debug.Log(!StoryManager.instance.HasCutscenePlayed(cutscene.cutsceneID));
            if (cutscene.playOnStart && !StoryManager.instance.HasCutscenePlayed(cutscene.cutsceneID))
            {
                PlayCutscene(cutscene);
                break; // Optional: only one cutscene per start
            }
        }
    }

    public void PlayCutscene(CutsceneData data)
    {
        if (data.rootObject != null)
            data.rootObject.SetActive(true);

        data.timeline.stopped += OnCutsceneEnded;
        data.timeline.Play();

        // Disable player control here, if needed
    }

    private void OnCutsceneEnded(PlayableDirector director)
    {
        foreach (var cutscene in cutscenes)
        {
            if (cutscene.timeline == director)
            {
                StoryManager.instance.MarkCutscenePlayed(cutscene.cutsceneID);
                cutscene.timeline.stopped -= OnCutsceneEnded;

                // Disable the cutscene object after it's done
                if (cutscene.rootObject != null)
                    cutscene.rootObject.SetActive(false);

                break;
            }
        }

        StoryManager.instance.PrintCompletedCutscenes();
        // Re-enable player control here
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