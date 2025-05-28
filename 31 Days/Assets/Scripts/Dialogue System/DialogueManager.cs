using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using System;

public class DialogueManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] Sprite characterPortrait;
    [SerializeField] TMP_Text characterName;
    [SerializeField] PlayerMovement playerMovement;

    [Header("Dialogue Settings")]
    [SerializeField] float wordSpeed = 0.05f;
    private BaseNode currentNode;
    private SingleChoiceNode currentSingleNode;
    int currentLineIndex = 0;

    Coroutine typingCoroutine;
    private bool isTyping = false;
    public bool IsTyping => isTyping;
    public bool IsActive => dialoguePanel.activeInHierarchy;


    private void Awake() // Ensure references are set and dialogue panel is hidden at start
    {
        if (dialoguePanel == null || dialogueText == null)
        {
            Debug.LogError("DialoguePanel or DialogueText is not assigned in the inspector.");
        }
        dialoguePanel.SetActive(false); // Ensure the dialogue panel is hidden at start
    }

    public void StartDialogue(BaseNode node) // Starts the dialogue with the provided lines
    {
        currentNode = node;
        currentLineIndex = 0;
        dialoguePanel.SetActive(true);
        playerMovement.SetCanMove(false);
        characterPortrait = node.characterPortrait;
        characterName.text = node.characterName;
        if (node is SingleChoiceNode single)
        {
            currentSingleNode = single;
            StartTypingLine(currentSingleNode.dialogueLines[currentLineIndex]);
        }
    }

    public void NextLine() // Advance Dialogue
    {
        if (isTyping)
        {
            FinishTyping();
            return;
        }

        if (currentNode is SingleChoiceNode)
        {
            currentLineIndex++;

            if (currentLineIndex < currentSingleNode.dialogueLines.Length)
            {
                StartTypingLine(currentSingleNode.dialogueLines[currentLineIndex]);
            }
            
            else
            {
                // Move to next node or end dialogue
                if (currentSingleNode.nextNode != null)
                {
                    StartDialogue(currentSingleNode.nextNode);
                }
                else
                {
                    EndDialogue();
                }
            }
        }
    }

    private void StartTypingLine(string line)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(line));
    } 


    private IEnumerator TypeLine(string line) // Coroutine to type out the current line of dialogue
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(wordSpeed);
        }
        isTyping = false;
    }

    public void FinishTyping() // Completes the current line of dialogue immediately
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
            
        if (currentNode is SingleChoiceNode) 
            dialogueText.text = currentSingleNode.dialogueLines[currentLineIndex];
        isTyping = false;
    }

    public void EndDialogue() // Ends the dialogue and resets the state
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogueText.text = "";
        dialoguePanel.SetActive(false);
        playerMovement.SetCanMove(true); 
    }
}