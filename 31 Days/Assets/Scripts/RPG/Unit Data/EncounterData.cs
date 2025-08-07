using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Encounter", menuName = "RPG/Encounter")]
public class Encounter : ScriptableObject
{
    public List<EnemyData> enemyPool;
    public int enemyCount;

    //public bool isRepeatable = true;
    //public bool isStoryEncounter = false;

    [TextArea]
    public string introText;

    // Future additions:
    // public List<Item> rewardItems;
    // public int expReward;
}