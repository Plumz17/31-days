using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    [SerializeField] private InputActionReference interactInput; //Note to self: There might be a better way to do this... (To Access the Player's Movement)
    [SerializeField] private GameObject exclamationMark;
    private bool playerIsClose;
    Animator exclamationAnim;

    [Header("Dialogue System")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public String[] dialogueLines;
    public float wordSpeed = 0.05f;
    private int currentLineIndex = 0;
    private bool isTyping = false;

    [Header("Bobbing Animation")]
    float originalY;
    [SerializeField] private float floatStrength = 0.2f;

    private void Awake()
    {
        exclamationAnim = exclamationMark.GetComponent<Animator>();
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
        if (collision.gameObject.tag == "Player")
        {
            exclamationAnim.SetBool("isClose", true);
            playerIsClose = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            exclamationAnim.SetBool("isClose", false);
            playerIsClose = false;
            ZeroText();
        }
    }

    void HandlePlayerInteraction()
    {
        if (playerIsClose && interactInput.action.triggered)
        {
            if (!dialoguePanel.activeInHierarchy)
            {
                exclamationMark.GetComponent<SpriteRenderer>().enabled = false;
                HandleDialogue();
            }

            else
            {
                if (isTyping)
                {
                    // If currently typing, skip to the end of the line
                    StopAllCoroutines();
                    dialogueText.text = dialogueLines[currentLineIndex];
                    isTyping = false;
                }
                else
                {
                    NextLine(); // Go to the next line of dialogue
                }
            }
        }
    }


    // All the code below Handles the dialogue system when the player interacts with the NPC
    private void HandleDialogue()
    {
        if (dialoguePanel.activeInHierarchy)
        {
            ZeroText();
        }
        else
        {
            // Start the dialogue
            dialoguePanel.SetActive(true);
            StartCoroutine(Typing()); // Start typing effect
        }
    }

    public void NextLine()
    {
        if (currentLineIndex < dialogueLines.Length - 1)
        {
            currentLineIndex++;
            dialogueText.text = string.Empty;
            StartCoroutine(Typing()); // Start typing effect for the next line
        }
        else
        {
            ZeroText(); // Clear text and reset when all lines are done
        }
    }

    IEnumerator Typing()
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

    private void ZeroText()
    {
        dialogueText.text = string.Empty; // Clear the text
        currentLineIndex = 0;
        dialoguePanel.SetActive(false);
        exclamationMark.GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnDisable()
    {
        interactInput.action.Disable(); // Disable the interaction action when this script is disabled  
    }
}
