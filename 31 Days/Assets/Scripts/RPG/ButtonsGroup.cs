using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonsGroup : MonoBehaviour
{
    public RPGManager rpgManager;
    public TMP_Text textBox;

    public void OnAttackButton()
    {
        if (rpgManager.isBusy) return;

        rpgManager.isChoosingTarget = true;
        textBox.text = "Choose a target.";
    }

    public void OnDefendButton()
    {
        if (rpgManager.isBusy) return;

        rpgManager.currentUnit.Defend();
        rpgManager.isBusy = true;

        textBox.text = rpgManager.currentUnit.Name + " Defended!";

        StartCoroutine(EndTurnAfterDelay());
    }

    public void OnRunButton()
    {
        rpgManager.EndBattle(true);
    }

    private IEnumerator EndTurnAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        rpgManager.EndTurn(); // Ends the player's turn and starts the next one
    }

}
