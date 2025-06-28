using UnityEngine;
using System.Collections;

[System.Serializable]
public class BasePlayer : MonoBehaviour
{
    public string playerName;

    public float baseHP;
    public float curHP;

    public float baseMP;
    public float curMP;

    public int stamina;
    public int intellect;
    public int dexterity;
    public int agility;
}
