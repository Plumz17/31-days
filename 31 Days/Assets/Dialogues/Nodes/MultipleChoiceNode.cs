using UnityEngine;

[System.Serializable]
public class DialogueOption
{
    public string optionText;
    public BaseNode nextNode;
}

[CreateAssetMenu(fileName = "MultipleChoiceNode", menuName = "Dialogue/MultipleChoiceNode")]
public class MultipleChoiceNode : BaseNode
{
    public string dialogueLine;
    public DialogueOption[] options;
}
