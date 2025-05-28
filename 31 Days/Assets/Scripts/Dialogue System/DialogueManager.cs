using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TMP_Text dialogueText;

    [Header("Dialogue Settings")]
    [SerializeField] float wordSpeed = 0.05f;

    private string[] dialogueLines;
    int currentLineIndex = 0;
    Coroutine typingCoroutine;
    private bool isTyping = false;
    public bool IsTyping => isTyping;
    public bool IsActive => dialoguePanel.activeInHierarchy;

    public void StartDialogue(string[] lines)
    {
        dialogueLines = lines;
        currentLineIndex = 0;
        dialoguePanel.SetActive(true);
        typingCoroutine = StartCoroutine(TypeCurrentLine());
    }

    private void Awake()
    {
        if (dialoguePanel == null || dialogueText == null)
        {
            Debug.LogError("DialoguePanel or DialogueText is not assigned in the inspector.");
        }
        dialoguePanel.SetActive(false); // Ensure the dialogue panel is hidden at start
    }

    public void FinishTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        dialogueText.text = dialogueLines[currentLineIndex];
        isTyping = false;
    }

    public void NextLine()
    {
        if (currentLineIndex < dialogueLines.Length - 1)
        {
            currentLineIndex++;
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            typingCoroutine = StartCoroutine(TypeCurrentLine());
        }
        else
        {
            EndDialogue();
        }
    }

    private IEnumerator TypeCurrentLine()
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in dialogueLines[currentLineIndex])
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(wordSpeed);
        }
        isTyping = false;
    }

    public void EndDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        dialogueText.text = "";
        dialoguePanel.SetActive(false);
    }
}