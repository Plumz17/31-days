// ShadowlessSlash.cs
using UnityEngine;

public class ShadowlessSlash : BaseAttack
{
    void Awake()
    {
        attackName = "Shadowless Slash";
        attackDescription = "Slash through your enemies ten times at a lightning speed, leaving no shadow behind.";
        attackDamage = 1000f;
        attackCost = 20f;
    }
}