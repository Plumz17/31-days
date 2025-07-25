using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "RPG/Skill")]
public class Skill : ScriptableObject
{
    public string skillName;
    [Tooltip("How many damage delt or HP healed")]
    public int skillAmount;
    public int willCost;
    [Tooltip("Either heal or attack")]
    public string skillType;
}
