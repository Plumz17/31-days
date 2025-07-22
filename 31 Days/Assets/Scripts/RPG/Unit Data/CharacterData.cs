using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "RPG/PlayerData")]
public class CharacterData : UnitData
{
    public Sprite icon;
    public Skill skill;
    public int savedHP;
    public int savedWILL;

    private void OnEnable() //This is just for in the overworld
    {
        if (savedHP == 0) Heal();
        if (savedWILL == 0) Heal();
    }

    public void Heal()
    {
        savedHP = maxHP;
        savedWILL = maxWILL;
    }
}