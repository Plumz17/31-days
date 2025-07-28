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

    void Start()
    {
        foreach (var cutscene in cutscenes)
        {
            cutscene.rootObject.SetActive(false); // Disable all at start
        }

        foreach (var cutscene in cutscenes)
        {
            Debug.Log(!StoryManager.instance.HasCutscenePlayed(cutscene.cutsceneID));
            if (!StoryManager.instance.HasCutscenePlayed(cutscene.cutsceneID))
            {
                Debug.Log(cutscene.cutsceneID);
                cutscene.rootObject.SetActive(true); // Only activate first unplayed one
                break;
            }
        }
    }

    public void PlayCutscene(CutsceneData data)
    {
        if (data.rootObject != null)
            data.rootObject.SetActive(true); // Ensure cutscene is active

        Debug.Log("Subscribing to stopped on " + data.timeline.name);
        data.timeline.stopped += OnCutsceneEnded;
        data.timeline.Play();

        // TODO: disable player input here
    }

    private void OnCutsceneEnded(PlayableDirector director)
    {
        Debug.Log("Cutscene Ends");
        foreach (var cutscene in cutscenes)
        {
            if (cutscene.timeline == director)
            {
                StoryManager.instance.MarkCutscenePlayed(cutscene.cutsceneID);
                Debug.Log("Marked Cutscene Played!");
                cutscene.timeline.stopped -= OnCutsceneEnded;

                if (cutscene.rootObject != null)
                    cutscene.rootObject.SetActive(false); // Clean up cutscene object

                break;
            }
        }

        StoryManager.instance.PrintCompletedCutscenes();

        // TODO: re-enable player input here
    }
}