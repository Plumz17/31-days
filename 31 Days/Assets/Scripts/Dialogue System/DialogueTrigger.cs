using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue")]
    [Tooltip("The dialogue node to start from when interacting.")]
    [SerializeField] private BaseNode startingNode;

    [Header("Optional")]
    [SerializeField] private Transform npcToFlip;
    [SerializeField] private bool facePlayerOnTalk = true;

    private Transform playerTransform;
    public ExclamationMark exclamationMark;
    public GameObject objectToAppearAtTheEnd; //For Cutscenes
    private bool playerIsClose = false;
    private bool hasPlayedOnce = false;
    private bool isCutscene = false;


    private InputActions inputActions;

    private void Awake()
    {
        playerTransform = GameObject.FindWithTag("Player")?.transform;
        exclamationMark = GetComponentInChildren<ExclamationMark>(true);

        inputActions = new InputActions();
        inputActions.Player.Interact.performed += OnInteract;
    }

    private void OnEnable()
    {
        inputActions.Enable();
        if (DialogueManager.instance != null)
            DialogueManager.instance.OnNodeStarted += HandleFlip;
    }

    private void OnDisable()
    {
        inputActions.Disable();
        if (DialogueManager.instance != null)
            DialogueManager.instance.OnNodeStarted -= HandleFlip;
    }

    private void HandleFlip(BaseNode node)
    {
        FlipNPC();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if ((!playerIsClose && !isCutscene) || startingNode == null || hasPlayedOnce)
            return;

        var dialogueManager = DialogueManager.instance;
        if (dialogueManager == null) return;

        if (!dialogueManager.IsActive)
        {
            // Start dialogue
            if (facePlayerOnTalk && playerTransform != null)
                FaceNPCToPlayer();

            exclamationMark?.SetVisible(false);
            dialogueManager.StartDialogue(startingNode);
            dialogueManager.OnDialogueEnded += OnDialogueEnded;
        }
        else
        {
            // Dialogue active
            if (dialogueManager.UI.IsTyping)
            {
                dialogueManager.FinishTyping();
            }
            else
            {
                dialogueManager.HandleSubmit();
            }
        }
    }

    private void FaceNPCToPlayer()
    {
        Vector3 direction = playerTransform.position - transform.position;
        Transform npcRoot = transform.parent != null ? transform.parent : transform;

        float scaleX = Mathf.Abs(npcRoot.localScale.x);
        npcRoot.localScale = new Vector3(direction.x > 0 ? -scaleX : scaleX, npcRoot.localScale.y, npcRoot.localScale.z);
    }

    public void FlipNPC()
    {
        Transform root = npcToFlip != null ? npcToFlip : transform;
        Vector3 scale = root.localScale;
        scale.x *= -1;
        root.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerIsClose = true;
        exclamationMark?.SetIsClose(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerIsClose = false;
        exclamationMark?.SetIsClose(false);


        if (DialogueManager.instance != null && DialogueManager.instance.IsActive)
            DialogueManager.instance.EndDialogue();
    }

    private void OnDialogueEnded()
    {
        DialogueManager.instance.OnDialogueEnded -= OnDialogueEnded;

        var lastNode = DialogueManager.instance.LastPlayedNode;

        if (lastNode != null && lastNode.onlyPlayedOnce)
        {
            hasPlayedOnce = true;
            exclamationMark?.SetVisible(false);
        }
        else
        {
            exclamationMark?.SetVisible(true);
        }

        if (objectToAppearAtTheEnd != null)
        {
            objectToAppearAtTheEnd.SetActive(true);
        }

        isCutscene = false;
    }
    
    public void TriggerDialogue(bool fromCutscene = false)
    {
        if (startingNode == null || (startingNode.onlyPlayedOnce && hasPlayedOnce))
            return;

        var dialogueManager = DialogueManager.instance;
        if (dialogueManager == null) return;

        isCutscene = fromCutscene;

        if (!dialogueManager.IsActive)
        {
            if (facePlayerOnTalk && playerTransform != null)
                FaceNPCToPlayer();

            exclamationMark?.SetVisible(false);
            dialogueManager.StartDialogue(startingNode);
            dialogueManager.OnDialogueEnded += OnDialogueEnded;
        }
    }
}
