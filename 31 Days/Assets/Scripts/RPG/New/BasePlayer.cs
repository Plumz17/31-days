using UnityEngine;
using System.Collections;

[System.Serializable]
public class BasePlayer : MonoBehaviour
{
    public string theName;
    
    public float baseHP;
    public float curHP;

    public float baseMP;
    public float curMP;

    public float baseATK;
    public float curATK;
    
    public float baseDEF;
    public float curDEF;
    public int level;
    public int stamina;
    public int intellect;
    public int dexterity;
    public int agility;
}
