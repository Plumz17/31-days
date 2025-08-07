using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    [System.Serializable]
    public class CutsceneData
    {
        public string cutsceneID;
        public bool playOnStart;
        public GameObject rootObject;

        [Header("Optional Prerequisite")]
        public string requiredCutsceneID;
        public int requiredDay;
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

            if (cutscene.requiredDay != 0 && CalenderAndObjectiveManager.instance.currentDay != cutscene.requiredDay)
                continue;

            cutscene.rootObject.SetActive(true);

            if (cutscene.playOnStart)
            {
                PlayCutscene(cutscene.rootObject.GetComponent<Cutscene>(), cutscene.cutsceneID);
            }

            break;
        }
    }

    public void PlayCutscene(Cutscene data, string id)
    {
        data.PlayCutscene(id);
    }
}