using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NUnit.Framework.Constraints;

public class DialogueUIManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text characterNameText;
    [SerializeField] private Image dialogueCircle;
    [SerializeField] private Image characterPortrait;
    [SerializeField] private Image dialogueImageUI;
    [SerializeField] private Sprite dialogueImage;
    [SerializeField] private Sprite itemImage;

    [Header("Choices UI")]
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private GameObject[] choiceBoxes;
    [SerializeField] private TMP_Text[] choiceTexts;

    [Header("Typing Settings")]
    [SerializeField] private float wordSpeed = 0.05f;
    [SerializeField] private Color selectedColor = new(0f / 255f, 74f / 255f, 173f / 255f);
    [SerializeField] private Color unselectedColor = Color.white;

    public float WordSpeed => wordSpeed;
    public bool IsTyping { get; set; }

    public GameObject DialoguePanel => dialoguePanel;

    private int currentSelectedIndex = 0;
    private int optionCount = 0;

    public void ShowDialogueUI(BaseNode node)
    {
        dialoguePanel.SetActive(true);
        choicePanel.SetActive(false);
        bool hasCharacter = !string.IsNullOrEmpty(node.characterName) || node.characterPortrait != null;

        characterNameText.gameObject.SetActive(hasCharacter);
        characterPortrait.gameObject.SetActive(hasCharacter);
        dialogueCircle.gameObject.SetActive(hasCharacter);

        if (hasCharacter)
        {
            if (characterNameText != null)
                characterNameText.text = node.characterName;

            if (characterPortrait != null)
                characterPortrait.sprite = node.characterPortrait;

            // Use dialogue image if character exists
            dialogueImageUI.sprite = dialogueImage;
            dialogueImageUI.SetNativeSize();
        }

        else
        {
            // Use item image (or narration image)
            dialogueImageUI.sprite = itemImage;
            dialogueImageUI.SetNativeSize();
        }
    }

    public void HideDialogueUI()
    {
        dialogueText.text = "";
        dialoguePanel.SetActive(false);
        choicePanel.SetActive(false);
    }

    public void SetDialogueText(string text)
    {
        dialogueText.text = text;
    }

    public void AppendDialogueChar(char c)
    {
        dialogueText.text += c;
    }

    public void ShowChoices(DialogueOption[] options)
    {
        choicePanel.SetActive(true);
        optionCount = options.Length;
        currentSelectedIndex = 0;

        for (int i = 0; i < choiceBoxes.Length; i++)
        {
            if (i < optionCount)
            {
                choiceBoxes[i].SetActive(true);
                choiceTexts[i].gameObject.SetActive(true);
                choiceTexts[i].text = options[i].optionText;
            }
            else
            {
                choiceBoxes[i].SetActive(false);
                choiceTexts[i].gameObject.SetActive(false);
            }
        }

        HighlightOption(currentSelectedIndex);
    }

    public void HighlightOption(int index)
    {
        currentSelectedIndex = index;

        for (int i = 0; i < optionCount; i++)
        {
            choiceTexts[i].color = (i == index) ? selectedColor : unselectedColor;
        }
    }

    public void HideChoices()
    {
        choicePanel.SetActive(false);
    }

    public int GetCurrentSelectedIndex() => currentSelectedIndex;
    public int GetOptionCount() => optionCount;
}
