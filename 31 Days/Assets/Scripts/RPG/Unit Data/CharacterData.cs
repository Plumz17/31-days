using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "RPG/PlayerData")]
public class CharacterData : UnitData
{
    public Sprite icon;
    public int currentHP;
    public int currentWILL;
}