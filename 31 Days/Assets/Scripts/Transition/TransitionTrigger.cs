using UnityEngine;

public class TransitionTrigger : MonoBehaviour
{
    private InputActions playerInput;
    [SerializeField] int sceneIndex;
    private bool playerInTrigger = false;
    [SerializeField] private bool isRoomChange = false;

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

    void Update()
    {
        if ((playerInTrigger && (playerInput.UI.Accept.triggered || isRoomChange)))
        {
            LevelLoader.Instance.LoadNextLevel(sceneIndex);
        }
    }
}
