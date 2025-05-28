using UnityEngine;
using UnityEngine.InputSystem;


public class DialogueTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] InputActionReference interactInput;
    [SerializeField] Transform playerTransform;
    [SerializeField] ExclamationMark exclamationMark;
    [SerializeField] DialogueManager dialogueManager;

    [Header("Dialogue Lines")]
    [SerializeField] string[] dialogueLines;

    private bool playerIsClose = false;
    private void OnEnable() => interactInput.action.Enable();
    private void OnDisable() => interactInput.action.Disable();

    private void Update()
    {
        if (!playerIsClose || !interactInput.action.triggered) return;

        if (!dialogueManager.IsActive)
        {
            FaceNPCToPlayer(); // Make the player face the NPC when interacting
            exclamationMark.SetVisible(false);
            dialogueManager.StartDialogue(dialogueLines);
        }
        else if (dialogueManager.IsTyping)
        {
            dialogueManager.FinishTyping();
        }
        else
        {
            dialogueManager.NextLine(); // Go to the next line of dialogue
        }
    }

    private void FaceNPCToPlayer()
    {
        Vector3 direction = playerTransform.position - transform.position;
        Transform npcRoot = transform.parent;
        if (npcRoot == null) return;

        if (direction.x > 0)
            npcRoot.localScale = new Vector3(-Mathf.Abs(npcRoot.localScale.x), npcRoot.localScale.y, npcRoot.localScale.z); // Face right
        else
            npcRoot.localScale = new Vector3(Mathf.Abs(npcRoot.localScale.x), npcRoot.localScale.y, npcRoot.localScale.z); // Face left
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerIsClose = true;
        exclamationMark.SetIsClose(true);
    }

    void OnTriggerExit2D(Collider2D collision)
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
