using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Encounter", menuName = "RPG/Encounter")]
public class EncounterData : ScriptableObject
{
    public List<EnemyData> enemies;
}