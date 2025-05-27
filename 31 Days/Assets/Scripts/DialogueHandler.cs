using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] InputActionReference interactInput;
    [SerializeField] Transform playerTransform;
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] GameObject exclamationMark;

    [Header("Dialogue Settings")]
    [SerializeField] string[] dialogueLines;
    [SerializeField] float wordSpeed = 0.05f;

    [Header("Exclamation Mark Animation")]
    [SerializeField] float floatStrength = 0.2f;

    bool playerIsClose;
    bool isTyping = false;
    int currentLineIndex = 0;
    float originalY;
    Coroutine typingCoroutine;
    Animator exclamationAnim;
    SpriteRenderer exclamationSpriteRenderer;

    private void Awake()
    {
        exclamationAnim = exclamationMark.GetComponent<Animator>();
        exclamationSpriteRenderer = exclamationMark.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        interactInput.action.Enable(); // Enable the interaction action
        dialoguePanel.SetActive(false); // Ensure dialogue panel is hidden at start

        originalY = exclamationMark.transform.position.y; // Store the original Y position for bobbing effect
    }

    void Update()
    {
        HandlePlayerInteraction();
        HandleBobbing();
    }

    private void HandleBobbing()
    {
        exclamationMark.transform.position = new Vector3(exclamationMark.transform.position.x,
            originalY + ((float)Math.Sin(Time.time) * floatStrength),
            exclamationMark.transform.position.z);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            exclamationAnim.SetBool("isClose", true);
            playerIsClose = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            exclamationAnim.SetBool("isClose", false);
            playerIsClose = false;
            ClearDialogue();
        }
    }

    void HandlePlayerInteraction()
    {
        if (!playerIsClose || !interactInput.action.triggered) return;

        if (!dialoguePanel.activeInHierarchy)
        {
            FaceNPCToPlayer(); // Make the player face the NPC when interacting
            ToggleExclamationMark(false);
            StartDialogue();
        }
        else if (isTyping)
        {
            FinishTypingLine();
        }
        else
        {
            NextLine(); // Go to the next line of dialogue
        }
    }

    private void ToggleExclamationMark(bool visible)
    {
        if (exclamationMark != null)
            exclamationSpriteRenderer.enabled = visible;
    }

    // All the code below Handles the dialogue system when the player interacts with the NPC
    private void StartDialogue()
    {
        dialoguePanel.SetActive(true);
        typingCoroutine = StartCoroutine(Typing());
    }

    public void NextLine() // Method to go to the next line of dialogue
    {
        if (currentLineIndex < dialogueLines.Length - 1)
        {
            currentLineIndex++;

            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine); // Stop Typing if it's running
                typingCoroutine = null;
            }

            dialogueText.text = string.Empty;
            typingCoroutine = StartCoroutine(Typing()); // Start typing effect for the next line
        }
        else
        {
            ClearDialogue(); // Clear text and reset when all lines are done
        }
    }

    private IEnumerator Typing() // Coroutine to handle typing effect
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in dialogueLines[currentLineIndex].ToCharArray())
        {
            dialogueText.text += letter; // Add each letter to the text
            yield return new WaitForSeconds(wordSpeed); // Wait for the specified word speed
        }
        isTyping = false; // Typing is done
    }
    
    private void FinishTypingLine()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        dialogueText.text = dialogueLines[currentLineIndex];
        isTyping = false;
    }


    private void ClearDialogue() // Clear the text and reset the dialogue state
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine); // Stop Typing if it's running
            typingCoroutine = null;
        }

        dialogueText.text = string.Empty; // Clear the text
        currentLineIndex = 0;
        dialoguePanel.SetActive(false);
        ToggleExclamationMark(true);
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

    private void OnDisable()
    {
        interactInput.action.Disable(); // Disable the interaction action when this script is disabled  
    }
}
