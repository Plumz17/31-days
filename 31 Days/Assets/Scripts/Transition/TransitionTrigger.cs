using UnityEngine;

public class TransitionTrigger : MonoBehaviour
{
    private InputActions playerInput;
    [SerializeField] int sceneIndex;
    private bool playerInTrigger = false;
    [SerializeField] private bool isScreenChange = false;
    [SerializeField] private Vector3 spawnPosition = Vector3.zero;
    [SerializeField] private int AdvanceTimeFlag = 0;

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
        if (!playerInTrigger) return;

        // Case 1: Submit pressed manually
        if (playerInput.UI.Submit.triggered)
        {
            // Block manual transitions during dialogue
            if ((DialogueManager.instance != null && DialogueManager.instance.IsActive) ||
                StoryManager.instance.IsCutscenePlaying)
                return;

            PerformTransition();
        }

        // Case 2: Auto screen change (unblocked)
        if (isScreenChange)
        {
            PerformTransition();
        }
    }

    private void PerformTransition()
    {
        if (AdvanceTimeFlag != 0)
            CalenderAndObjectiveManager.instance.AdvanceTimeBlock(AdvanceTimeFlag);

        LevelLoader.instance.LoadNextLevel(sceneIndex, spawnPosition);
        StoryManager.instance.SetCutsceneState(false);
    }

}
