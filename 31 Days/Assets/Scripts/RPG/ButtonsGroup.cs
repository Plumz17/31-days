using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsGroup : MonoBehaviour
{
    public RPGManager rpgManager;
    public TMP_Text textBox;

    public void OnAttackButton()
    {
        if (rpgManager.isAttacking) return;

        rpgManager.isChoosingTarget = true;
        textBox.text = "Choose a target.";
    }

    public void OnRunButton()
    {
        rpgManager.EndBattle(true);
    }

}
