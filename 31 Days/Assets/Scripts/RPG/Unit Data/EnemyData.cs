using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Enemy", menuName = "RPG/EnemyData")]
public class EnemyData : UnitData
{
    public int rarity;
    public Sprite enemySprite;
    public Sprite enemyInfoBox;
}