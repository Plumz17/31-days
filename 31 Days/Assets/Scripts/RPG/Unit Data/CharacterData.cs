using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "RPG/PlayerData")]
public class CharacterData : UnitData
{
    public Sprite icon;
    public int currentHP;
    public int currentWILL;

    private void OnEnable() //This is just for in the overworld
    {
        if (currentHP == 0) Heal();
        if (currentWILL == 0) Heal();
    }

    public void Heal()
    {
        currentHP = maxHP;
        currentWILL = maxWILL;
    }
}