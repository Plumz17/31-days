using UnityEngine;

public class CutsceneDependantActivator : MonoBehaviour
{
    public string prerequisiteCutscene;

    void Start()
    {
        UpdateActiveState();
    }

    void UpdateActiveState()
    {
        if (StoryManager.instance != null)
        {
            gameObject.SetActive(StoryManager.instance.HasCutscenePlayed(prerequisiteCutscene));
        }
    }

    // Optional: expose method to call when time changes
    public void Refresh()
    {
        UpdateActiveState();
    }
}
