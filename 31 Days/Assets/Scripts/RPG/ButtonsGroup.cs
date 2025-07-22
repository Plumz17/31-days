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

        rpgManager.enemyGroup.SetEnemyHighlights(true);

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

    public void OnSkillButton()
    {
        if (rpgManager.isBusy) return;

        rpgManager.enemyGroup.SetEnemyHighlights(true);
        rpgManager.isChoosingTarget = true;
        textBox.text = "Choose a target.";
        rpgManager.currentAction = "skill";
        ShowBackButton(true);
    }

    public void OnCheckButton()
    {
        if (rpgManager.isBusy) return;

        rpgManager.enemyGroup.SetEnemyHighlights(true);
        rpgManager.currentAction = "check";
        rpgManager.isChoosingTarget = true;
        textBox.text = "Choose an Enemy.";
        ShowBackButton(true); 
    }

    public void OnFocusButton()
    {
        if (rpgManager.isBusy) return;

        int will = 30;

        rpgManager.isBusy = true;
        rpgManager.currentUnit.RestoreWILL(will);
        rpgManager.playerGroup.UpdatePartyUI(rpgManager.playerUnits);

        textBox.text = rpgManager.currentUnit.Name + " Focused! and gained +" + will + " WILL";

        StartCoroutine(EndTurnAfterDelay());
    }

    public void OnBackButton()
    {
        rpgManager.enemyGroup.SetEnemyHighlights(false);
        rpgManager.infoBox.gameObject.SetActive(false);
        rpgManager.isChoosingTarget = false;
        ShowBackButton(false);
        textBox.text = rpgManager.currentUnit.Name + "'s Turn. Choose an action.";
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
