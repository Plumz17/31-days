using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Data")]
    public UnitData data; // Can be CharacterData (player) or EnemyData (enemy)

    [Header("Runtime Stats")]
    public int currentHP { get; private set; }
    public int currentWILL { get; private set; }
    public int maxHP { get; private set; }
    public int maxWILL { get; private set; }

    // Properties from data
    public string Name => data.unitName;
    public int damage => data.damage;

    // Type flags
    public bool isPlayer => data is CharacterData;

    private void Start()
    {
        if (data == null) return;

        maxHP = data.maxHP;
        maxWILL = data.maxWILL;
        currentHP = maxHP;
        currentWILL = maxWILL;
        Debug.Log(maxHP);
    }

    public void TakeDamage(int amount)
    {
        currentHP = Mathf.Max(currentHP - amount, 0);
    }

    public bool IsDead() => currentHP <= 0;
}
