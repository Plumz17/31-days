using UnityEngine;

[System.Serializable]
public class BaseAttack : MonoBehaviour
{
    public string attackName;//name
    public string attackDescription;//description
    public float attackDamage;//Base Damage 15, mellee lvl 10 stamina 35 = basedmg + stamina + lvl = 60
    public float attackCost;//ManaCost
}