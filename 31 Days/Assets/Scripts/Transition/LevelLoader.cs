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
    private GameObject player;
    private PlayerMovement movement;
    private bool isLoading = false;

    private void Awake()
    {
        if (Instance == null || Instance != this)
        {
            Instance = this;
        }

        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    public void LoadNextLevel(int sceneIndex, Vector3 positionToSpawn) //Called in TransitionTrigger.cs
    {
        if (isLoading) return;

        isLoading = true;
        spawnPosition = positionToSpawn;
        if (player != null)
            spawnFlipX = !movement.GetFacingDirection();

        StartCoroutine(LoadLevel(sceneIndex));
    }

    IEnumerator LoadLevel(int sceneIndex)
    {
        anim.SetTrigger("Start");

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            yield return null;
        }
        yield return new WaitForSeconds(transitionTime);

        operation.allowSceneActivation = true;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) //Called when the scene first loads
    {
        PauseMenu.instance?.Resume();
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            if (PlayerDataManager.instance != null && PlayerDataManager.instance.loadedFromSave)
            {
                player.transform.position = PlayerDataManager.instance.currentData.prevPosition;
                PlayerDataManager.instance.loadedFromSave = false;
            }
            else
            {
                player.transform.position = spawnPosition;
            }

            movement = player.GetComponent<PlayerMovement>();
            movement.SetFacingDirection(!spawnFlipX);
        }
    }

    IEnumerator WaitOneSecond()
    {
        yield return new WaitForSeconds(transitionTime);
    }


    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
