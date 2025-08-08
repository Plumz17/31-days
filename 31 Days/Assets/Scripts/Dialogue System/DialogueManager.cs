using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private DialogueUIManager uiManager;
    [SerializeField] private DialogueInputHandler inputHandler;
    
    [Header("Typing SFX")]
    [SerializeField] private AudioClip typingSFX;
    [SerializeField] private int charSoundInterval = 2; // play every N characters
    [SerializeField] private float pitchVariation = 0.05f; // optional
    [SerializeField] private AudioSource typingAudioSource;
    public BaseNode LastPlayedNode { get; private set; }
    private DialogueTrigger activeTrigger;
    private int charsSinceLastSFX = 0;

    private PlayerMovement playerMovement;
    
 
    public DialogueUIManager UI => uiManager;
    public DialogueInputHandler Input => inputHandler;

    private BaseNode currentNode;
    private SingleChoiceNode currentSingleNode;
    private MultipleChoiceNode currentMultipleNode;

    private int currentLineIndex = 0;
    private Coroutine typingCoroutine;
    public event Action<BaseNode> OnNodeStarted;

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

    public event Action OnDialogueEnded;

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
        LastPlayedNode = null;
        currentNode = node;
        currentLineIndex = 0;

        uiManager.ShowDialogueUI(node);
        playerMovement = GameObject.FindWithTag("Player")?.GetComponent<PlayerMovement>();
        playerMovement.SetCanMove(false);

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
            inputHandler.EnableChoiceMode(multiple.options.Length);
            inputHandler.OnNavigate += uiManager.HighlightOption;
        }


        if (node.levelUpFlag && !string.IsNullOrEmpty(node.characterName))
            PlayerDataManager.instance.IncreaseConnection(node.characterName);

        if (node.advanceTimeFlag != 0)
            CalenderAndObjectiveManager.instance.AdvanceTimeBlock(node.advanceTimeFlag);

        if (node.saveDataFlag)
            SaveData.Save();

        if (node.newMemberFlag != null)
        {
            PlayerDataManager.instance.AddPartyMember(node.newMemberFlag);
            DuskManager.instance?.UpdateParty();
        }

        if (node.turnFlag)
            OnNodeStarted?.Invoke(node);

        if (node.healFlag)
            DuskManager.instance?.HealParty();

        if (!string.IsNullOrEmpty(node.objectiveFlag))
        {
            CalenderAndObjectiveManager.instance.currentObjective = node.objectiveFlag;
            CalenderAndObjectiveManager.instance.UpdateObjectiveUI();
        }
            
    }

    public void HandleSubmit()
    {
        if (IsTyping)
        {
            FinishTyping();
            return;
        }

        if (currentNode is MultipleChoiceNode multiple)
        {
            int selectedIndex = inputHandler.CurrentOptionIndex;
            BaseNode nextNode = multiple.options[selectedIndex].nextNode;

            if (nextNode != null)
            {
                StartDialogue(nextNode);
            }
            else
            {
                LastPlayedNode = currentNode;
                EndDialogue();
            }

            uiManager.HideChoices();
            inputHandler.OnNavigate -= uiManager.HighlightOption;
            inputHandler.DisableChoiceMode();
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
                LastPlayedNode = currentNode;
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
        charsSinceLastSFX = 0;

        foreach (char c in line)
        {
            uiManager.AppendDialogueChar(c);

            if (!char.IsWhiteSpace(c))
            {
                charsSinceLastSFX++;
                if (charsSinceLastSFX >= charSoundInterval)
                {
                    PlayTypingSFX();
                    charsSinceLastSFX = 0;
                }
            }
            yield return new WaitForSeconds(uiManager.WordSpeed);
        }

        typingCoroutine = null;
        uiManager.IsTyping = false;
    }

    private void PlayTypingSFX()
    {
        if (typingSFX != null && typingAudioSource != null)
        {
            typingAudioSource.pitch = UnityEngine.Random.Range(1f - pitchVariation, 1f + pitchVariation);
            typingAudioSource.PlayOneShot(typingSFX);
        }
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
        playerMovement.SetCanMove(true);
        OnDialogueEnded?.Invoke();

        if (LastPlayedNode != null && LastPlayedNode.transitionFlag != null && LastPlayedNode.transitionFlag.flagID != 0)
        {
            LevelLoader.Instance.LoadNextLevel(LastPlayedNode.transitionFlag.flagID, LastPlayedNode.transitionFlag.position);
        }
    }
}
