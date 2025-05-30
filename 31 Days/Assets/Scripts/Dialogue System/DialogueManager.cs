using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;


public class DialogueManager : MonoBehaviour
{
    [Header("References")] // Set up references in the inspector
    [Tooltip("The panel that contains the dialogue UI elements.")]
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] Image characterPortrait;
    [SerializeField] TMP_Text characterName;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] GameObject choicesPanel;
    [SerializeField] TMP_Text[] choiceTexts;
    

    [Header("Dialogue Settings")] // Settings for dialogue appearance and behavior
    [Tooltip("The speed at which each word appears in the dialogue text.")]
    [SerializeField] float wordSpeed = 0.05f;
    [SerializeField] Color selectedColor;
    [SerializeField] Color unselectedColor;
    private float choiceInputDelay = 0.1f; // Small delay buffer
    private float choiceInputTimer = 0f;

    private InputActions playerInput;
    private BaseNode currentNode;
    private SingleChoiceNode currentSingleNode;
    private MultipleChoiceNode currentMultipleNode;
    private bool selectingOption = false;
    private bool isTyping = false;
    private int currentLineIndex = 0;
    private int currentOptionIndex = 0;
    Coroutine typingCoroutine;
    

    public bool IsTyping => isTyping;
    public bool IsActive => dialoguePanel.activeInHierarchy;

    private void Awake()
    {
        playerInput = new InputActions();

        dialoguePanel.SetActive(false);
        choicesPanel.SetActive(false);
    }

    private void OnEnable() => playerInput.UI.Enable();
    private void OnDisable() => playerInput.UI.Disable();

    void Update()
    {
        if (selectingOption && currentMultipleNode != null)
        {
            HandleChoiceInput();
        }
    }
    
    public void StartDialogue(BaseNode node) // Starts the dialogue with the provided lines
    {
        currentNode = node;
        currentLineIndex = 0;
        SetUpDialogueUI(node); // Set up the dialogue UI with the node's information

        if (node is SingleChoiceNode single)
        {
            currentSingleNode = single;
            currentMultipleNode = null;
            currentOptionIndex = 0;
            StartTypingLine(single.dialogueLines[currentLineIndex]);
        }
        else if (node is MultipleChoiceNode multiple)
        {
            // Enter choice mode for MultipleChoiceNode
            currentMultipleNode = multiple;
            currentSingleNode = null;
            ShowChoices();
        }
    }

    private void SetUpDialogueUI(BaseNode node) // Sets up the dialogue panel with the provided node's information
    {
        if (node is MultipleChoiceNode)
        {
            choicesPanel.SetActive(true);
            EnterChoiceMode();
        }
        dialoguePanel.SetActive(true);
        characterName.text = node.characterName;
        characterPortrait.sprite = node.characterPortrait;
        playerMovement.SetCanMove(false);
    }

    private void EnterChoiceMode() // Enters the choice mode, disabling player input and enabling UI input
    {
        playerInput.Player.Disable();
        playerInput.UI.Enable();
        selectingOption = true;
        choiceInputTimer = choiceInputDelay;
    }

    private void ExitChoiceMode() // Exits the choice mode and re-enables player input
    {
        playerInput.UI.Disable();
        playerInput.Player.Enable();
        selectingOption = false;
    }

    private void HandleChoiceInput() // Handles input for navigating and selecting choices in a MultipleChoiceNode
    {
        if (choiceInputTimer > 0f)
        {
            choiceInputTimer -= Time.unscaledDeltaTime;
            return; // Wait until delay passes
        }

        if (playerInput.UI.Down.triggered)
        {
            currentOptionIndex = (currentOptionIndex + 1) % currentMultipleNode.options.Length;
            HighlightCurrentOption();
        }
        else if (playerInput.UI.Up.triggered)
        {
            currentOptionIndex = (currentOptionIndex + currentMultipleNode.options.Length - 1) % currentMultipleNode.options.Length;
            HighlightCurrentOption();
        }
        else if (playerInput.UI.Accept.triggered)
        {
            SelectChoice();
        }
    }

    private void SelectChoice()
    {
        BaseNode nextNode = currentMultipleNode.options[currentOptionIndex].nextNode;
        choicesPanel.SetActive(false);
        ExitChoiceMode();

        if (nextNode != null)
            StartDialogue(nextNode);
        else
            EndDialogue();
    }

    private void ShowChoices() // Displays the choices available in a MultipleChoiceNode
    {
        for (int i = 0; i < currentMultipleNode.options.Length; i++)
        {
            if (i < choiceTexts.Length)
            {
                choiceTexts[i].gameObject.SetActive(true);
                choiceTexts[i].text = currentMultipleNode.options[i].optionText;
            }
            else
            {
                choiceTexts[i].gameObject.SetActive(false);
            }

        }

        HighlightCurrentOption();
    }

    private void HighlightCurrentOption()
    {
        int optionsLength = currentMultipleNode.options.Length;

        for (int i = 0; i < optionsLength; i++)
        {
            choiceTexts[i].color = (i == currentOptionIndex) ? selectedColor : unselectedColor;
        }
    }

    public void NextLine() // Advance Dialogue
    {
        if (isTyping) // If currently typing, finish the line immediately
        {
            FinishTyping();
            return;
        }

        if (currentNode is SingleChoiceNode)
        {
            currentLineIndex++; // If the current node is a SingleChoiceNode, advance to the next line

            if (currentLineIndex < currentSingleNode.dialogueLines.Length) // Check if there are more lines to display
            {
                StartTypingLine(currentSingleNode.dialogueLines[currentLineIndex]);
            }

            else
            {
                if (currentSingleNode.nextNode != null) // Check if there is a next node to transition to
                {
                    StartDialogue(currentSingleNode.nextNode); // If so, do the loop all over again with the next node
                }
                else // If no next node, end the dialogue
                {
                    EndDialogue();
                }
            }
        }
    }

    private void StartTypingLine(string line) // Starts typing out the current line of dialogue
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
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);
        if (choicesPanel != null)
            choicesPanel.SetActive(false);
        if (playerMovement != null)
        playerMovement.SetCanMove(true);
    }
}