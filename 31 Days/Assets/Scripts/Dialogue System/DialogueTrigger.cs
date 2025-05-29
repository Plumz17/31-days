using UnityEngine;
using UnityEngine.InputSystem;


public class DialogueTrigger : MonoBehaviour
{
    [Header("References")] //Set Up references in the inspector
    [SerializeField] Transform playerTransform;
    [SerializeField] ExclamationMark exclamationMark;
    [SerializeField] DialogueManager dialogueManager;

    [Header("Dialogue Nodes")] // Set up the starting node for the dialogue
    [Tooltip("The node to start the dialogue from when the player interacts with the NPC.")]
    [SerializeField] BaseNode startingNode;

    InputActions playerInput;

    private bool playerIsClose = false;
    private void OnEnable() => playerInput.Player.Enable();
    private void OnDisable() => playerInput.Player.Disable();

    void Awake()
    {
        playerInput = new InputActions();
    }

    private void Update() // Handle player interaction with the NPC
    {
        if (!playerIsClose || !playerInput.Player.Interact.triggered) return; // Check if player is close and the interact input is triggered

        if (!dialogueManager.IsActive) // When Clicked, If dialogue is not active, start it
        {
            FaceNPCToPlayer();
            exclamationMark.SetVisible(false);
            dialogueManager.StartDialogue(startingNode);
        }
        else if (dialogueManager.IsTyping) // When Clicked, If dialogue is active and currently typing, finish typing
        {
            dialogueManager.FinishTyping();
        }
        else // When Clicked, If dialogue is active and not typing, go to the next line
        {
            dialogueManager.NextLine();
            if (!dialogueManager.IsActive)
            {
                exclamationMark.SetVisible(true);
            }
        }
    }

    private void FaceNPCToPlayer() // Make the NPC face the player when interacting
    {
        Vector3 direction = playerTransform.position - transform.position;
        Transform npcRoot = transform.parent;
        if (npcRoot == null) return;

        if (direction.x > 0)
            npcRoot.localScale = new Vector3(-Mathf.Abs(npcRoot.localScale.x), npcRoot.localScale.y, npcRoot.localScale.z); // Face right
        else
            npcRoot.localScale = new Vector3(Mathf.Abs(npcRoot.localScale.x), npcRoot.localScale.y, npcRoot.localScale.z); // Face left
    }
    
    void OnTriggerEnter2D(Collider2D collision) // Handle player entering the trigger area
    {
        if (!collision.CompareTag("Player")) return;

        playerIsClose = true;
        exclamationMark.SetIsClose(true);
    }

    void OnTriggerExit2D(Collider2D collision) // Handle player exiting the trigger area
    {
        if (collision.CompareTag("Player"))
        {
            playerIsClose = false;
            exclamationMark.SetIsClose(false);
            exclamationMark.SetVisible(true);
            dialogueManager.EndDialogue(); // Clear dialogue when player exits trigger
        }
    }
}
