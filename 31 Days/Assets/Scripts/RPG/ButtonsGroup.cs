using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class ButtonsGroup : MonoBehaviour
{
    public RPGManager rpgManager;
    public TMP_Text textBox;

    public List<GameObject> ActiveButtons = new List<GameObject>();

    public void OnAttackButton()
    {
        if (rpgManager.isBusy) return;

        rpgManager.isChoosingTarget = true;
        textBox.text = "Choose a target.";
        rpgManager.currentAction = "attack";
        ShowBackButton(true);
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
        yield return new WaitForSeconds(rpgManager.waitingTime);
        rpgManager.EndTurn(); // Ends the player's turn and starts the next one
    }

    public void OnBackButton()
    {
        rpgManager.currentAction = ""; //reset current action
        rpgManager.isChoosingTarget = false;
        ShowBackButton(false);
        textBox.text = rpgManager.currentUnit.Name + "'s Turn. Choose an action.";
    }

    public void OnCheckButton()
    {
        if (rpgManager.isBusy) return;

        rpgManager.currentAction = "check";
        rpgManager.isChoosingTarget = true;
        textBox.text = "Choose an Enemy.";
        ShowBackButton(true); 
    }

    public void ShowBackButton(bool isShow)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.name == "Bck")
            {
                child.gameObject.SetActive(isShow);
            }
            else if (ActiveButtons.Contains(child.gameObject))
            {
                child.gameObject.SetActive(!isShow);
            }
        }
    }
}
