using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Slider hpSlider;
    public Slider willSlider;

    public void UpdateUI(Unit unit)
    {
        hpSlider.maxValue = unit.maxHP;
        hpSlider.value = unit.currentHP;
        willSlider.maxValue = unit.maxWILL;
        willSlider.value = unit.currentWILL;
    }
}
