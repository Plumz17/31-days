using UnityEngine;

public class Unit : MonoBehaviour
{
    public string Name;
    public int maxHP, currentHP;
    public int maxWILL = 100;
    public int currentWILL;
    public int damage;
    public bool isPlayer;

    public void TakeDamage(int amount)
    {
        currentHP = Mathf.Max(currentHP - amount, 0);
    }

    public bool IsDead() => currentHP <= 0;
}
