using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterCutscene : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    [TextArea] public string fullText;
    public float characterDelay = 0.05f;
    public float extraPause = 0.5f; // Pause for '|'

    public void StartTyping()
    {
        StopAllCoroutines();
        StartCoroutine(TypeText(fullText));
    }

    private IEnumerator TypeText(string text)
    {
        textUI.text = "";
        foreach (char c in text)
        {
            if (c == '|')
            {
                yield return new WaitForSeconds(extraPause);
                continue; // Don't show the pause marker
            }

            textUI.text += c;
            yield return new WaitForSeconds(characterDelay);
        }
    }
}
