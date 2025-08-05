using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

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

        int will = 10;
        rpgManager.currentUnit.RestoreWILL(will);

        rpgManager.currentUnit.Defend();
        rpgManager.isBusy = true;

        rpgManager.PlaySFX(rpgManager.defendSFX);
        textBox.text = rpgManager.currentUnit.Name + " Defended! and gained + " + will + " WILL";;

        StartCoroutine(EndTurnAfterDelay());
    }

    public void OnRunButton()
    {
        float chance = Random.value;
        if (chance <= (0.1f / rpgManager.playerUnits.Count))
        {
            textBox.text = "Successfully fled";
            rpgManager.EndBattle(true);
        }
        else
        {
            textBox.text = "There's no opening, Failed to flee!";
            StartCoroutine(EndTurnAfterDelay());
        }
    }

    private IEnumerator EndTurnAfterDelay()
    {
        yield return new WaitForSeconds(rpgManager.waitingTime);
        rpgManager.EndTurn(); // Ends the player's turn and starts the next one
    }

    public void OnSkillButton()
    {
        if (rpgManager.isBusy) return;

        if (rpgManager.currentUnit.skill.skillType == "attack")
        {
            rpgManager.enemyGroup.SetEnemyHighlights(true);
            rpgManager.isChoosingTarget = true;
            textBox.text = "Choose an enemy target.";
            rpgManager.currentAction = "skill";
            ShowBackButton(true);
        }
        else if (rpgManager.currentUnit.skill.skillType == "heal")
        {
            rpgManager.currentAction = "skill";
            rpgManager.isBusy = true;
            StartCoroutine(rpgManager.PlayerAttack(rpgManager.currentUnit, null, "skill"));
        }
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

        rpgManager.isBusy = true;
        int will = 30;
        rpgManager.currentUnit.RestoreWILL(will);
        rpgManager.playerGroup.UpdatePartyUI(rpgManager.playerUnits);

        rpgManager.PlaySFX(rpgManager.focusSFX);
        textBox.text = rpgManager.currentUnit.Name + " Focused! and gained +" + will + " WILL";

        StartCoroutine(EndTurnAfterDelay());
    }

    public void OnBackButton()
    {
        if (rpgManager.isBusy) return;

        rpgManager.PlaySFX(rpgManager.backSFX);
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
    
    public void SetButtonHighlights(bool enable = true)
    {
        Button button;

        foreach (GameObject buttons in ActiveButtons)
        {
            button = buttons.GetComponent<Button>();
            if (button == null) continue;

            Color softGray = new Color32(200, 200, 200, 255);

            ColorBlock colors = button.colors;
            colors.highlightedColor = enable ? Color.whiteSmoke : Color.white;
            colors.pressedColor = enable ? softGray : Color.white;
            button.colors = colors;
        }
    }
}
