using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private DialogueUIManager uiManager;
    [SerializeField] private DialogueInputHandler inputHandler;

    public DialogueUIManager UI => uiManager;
    public DialogueInputHandler Input => inputHandler;

    private BaseNode currentNode;
    private SingleChoiceNode currentSingleNode;
    private MultipleChoiceNode currentMultipleNode;

    private int currentLineIndex = 0;
    private Coroutine typingCoroutine;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool IsActive => uiManager.DialoguePanel.activeSelf;
    public bool IsTyping => uiManager.IsTyping;

    public bool CanAdvance
    {
        get
        {
            if (currentNode is SingleChoiceNode single)
            {
                return currentLineIndex < single.dialogueLines.Length - 1 || single.nextNode != null;
            }
            return false;
        }
    }

    public void StartDialogue(BaseNode node)
    {
        currentNode = node;
        currentLineIndex = 0;

        uiManager.ShowDialogueUI(node);

        if (node is SingleChoiceNode single)
        {
            currentSingleNode = single;
            currentMultipleNode = null;
            StartTypingLine(single.dialogueLines[currentLineIndex]);
        }
        else if (node is MultipleChoiceNode multiple)
        {
            currentMultipleNode = multiple;
            currentSingleNode = null;
            uiManager.ShowChoices(multiple.options);
        }

        if (node.levelUpFlag && !string.IsNullOrEmpty(node.characterName))
            PlayerDataManager.instance.IncreaseConnection(node.characterName);

        if (node.advanceTimeFlag != 0)
            CalenderManager.instance.AdvanceTimeBlock(node.advanceTimeFlag);
    }

    public void HandleSubmit()
    {
        if (IsTyping)
        {
            FinishTyping();
            return;
        }

        if (currentNode is MultipleChoiceNode)
        {
            // Choice selection should be handled in DialogueInputHandler
            return;
        }

        if (currentNode is SingleChoiceNode single)
        {
            currentLineIndex++;

            if (currentLineIndex < single.dialogueLines.Length)
            {
                StartTypingLine(single.dialogueLines[currentLineIndex]);
            }
            else if (single.nextNode != null)
            {
                StartDialogue(single.nextNode);
            }
            else
            {
                EndDialogue();
            }
        }
    }

    private void StartTypingLine(string line)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(line));
    }

    private IEnumerator TypeLine(string line)
    {
        uiManager.IsTyping = true;
        uiManager.SetDialogueText("");

        foreach (char c in line)
        {
            uiManager.AppendDialogueChar(c);
            yield return new WaitForSeconds(uiManager.WordSpeed);
        }

        typingCoroutine = null;
        uiManager.IsTyping = false;
    }

    public void FinishTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        if (currentSingleNode != null)
            uiManager.SetDialogueText(currentSingleNode.dialogueLines[currentLineIndex]);

        uiManager.IsTyping = false;
    }

    public void EndDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        uiManager.HideDialogueUI();
        currentNode = null;
        currentSingleNode = null;
        currentMultipleNode = null;
    }
}
