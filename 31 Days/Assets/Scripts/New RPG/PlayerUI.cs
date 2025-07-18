using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Slider hpSlider;
    public Slider willSlider;
    public TMP_Text nameUI;
    public Image PortraitUI;

    public void UpdateUI(Unit unit)
    {
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
        willSlider.maxValue = unit.maxWILL;
        willSlider.value = unit.currentWILL;
        nameUI.text = unit.Name;
        PortraitUI.sprite = unit.Icon;
        PortraitUI.SetNativeSize();
    }
}
