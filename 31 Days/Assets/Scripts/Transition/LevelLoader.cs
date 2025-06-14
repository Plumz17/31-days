using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance; // Singleton reference

    [SerializeField] private Animator anim;
    [SerializeField] private float transitionTime = 1;

    private void Awake()
    {
        // Make sure there's only one instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadNextLevel(int sceneIndex)
    {
        StartCoroutine(LoadLevel(sceneIndex));
    }

    IEnumerator LoadLevel(int sceneIndex)
    {
        anim.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneIndex);
    }
}
