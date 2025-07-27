using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneSceneLoader : MonoBehaviour
{
    public string nextSceneName;

    public void StartFadeAndLoad()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}