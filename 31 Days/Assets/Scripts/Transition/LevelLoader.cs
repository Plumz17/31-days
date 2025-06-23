using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance; // Singleton reference

    [SerializeField] private Animator anim;
    [SerializeField] private float transitionTime = 1;
    public static bool spawnFlipX = false;
    public static Vector3 spawnPosition = Vector3.zero; // Position to spawn player at
    

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

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadNextLevel(int sceneIndex, Vector3 positionToSpawn)
    {
        spawnPosition = positionToSpawn;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                spawnFlipX = sr.flipX;
            }
        }
        StartCoroutine(LoadLevel(sceneIndex));
    }

    IEnumerator LoadLevel(int sceneIndex)
    {
        anim.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneIndex);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = spawnPosition;

            SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.flipX = spawnFlipX;
            }
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
