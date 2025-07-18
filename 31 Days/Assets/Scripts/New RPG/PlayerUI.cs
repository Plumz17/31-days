using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Slider hpSlider;
    public Slider willSlider;
    public Image charPortrait;
    public TMP_Text textName;

    public void UpdateUI(Unit unit)
    {
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
        willSlider.maxValue = unit.maxWILL;
        willSlider.value = unit.currentWILL;
        charPortrait.sprite = unit.Icon;
        charPortrait.SetNativeSize();
        textName.text = unit.Name;
    }
}
