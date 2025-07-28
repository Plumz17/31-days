using UnityEngine;

public class TransitionTrigger : MonoBehaviour
{
    private InputActions playerInput;
    [SerializeField] int sceneIndex;
    private bool playerInTrigger = false;
    [SerializeField] private bool isScreenChange = false;
    [SerializeField] private Vector3 spawnPosition = Vector3.zero;
    [SerializeField] private string additiveScene = null;

    void Awake()
    {
        playerInput = new InputActions();
        playerInput.Enable(); // Don’t forget this!
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }

    void OnDisable()
    {
        playerInput.Disable();
    }

    void ChangeSceneButton()
    {
        LevelLoader.Instance.LoadNextLevel(sceneIndex, spawnPosition);
    }

    void Update()
    {
        if ((playerInTrigger && (playerInput.UI.Submit.triggered || isScreenChange)))
        {
            LevelLoader.Instance.LoadNextLevel(sceneIndex, spawnPosition, additiveScene);
        }
    }
}
