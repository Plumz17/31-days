
// Slash.cs
using UnityEngine;

public class Slash : BaseAttack
{
    void Awake()
    {
        attackName = "Slash";
        attackDescription = "A quick and powerful slash with a sharp blade.";
        attackDamage = 10f;
        attackCost = 0;
    }
}