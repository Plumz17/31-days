using UnityEngine;
using System.Collections;

public class Slash : BaseAttack
{
    public Slash()
    {
        attackName = "Slash";
        attackDescription = "A quick and powerful slash with a sharp blade.";
        attackDamage = 10f;
        attackCost = 0;
    }
}