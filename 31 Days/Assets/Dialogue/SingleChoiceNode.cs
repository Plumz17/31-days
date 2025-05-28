using UnityEngine;

[CreateAssetMenu(fileName = "SingleChoiceNode", menuName = "Dialogue/SingleChoiceNode")]
public class SingleChoiceNode : BaseNode
{
    [TextArea(3, 10)]
    public string[] dialogueLines;  // Multiple lines shown sequentially

    public BaseNode nextNode;       // After last line, go to this node
}
