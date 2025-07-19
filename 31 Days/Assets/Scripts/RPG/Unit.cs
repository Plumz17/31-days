using UnityEngine;

public class Unit : MonoBehaviour
{
    [Header("Data")]
    public UnitData data; // Can be CharacterData (player) or EnemyData (enemy)

    private RuntimeUnitData runtimeData;

    [Header("Runtime Stats")]
    public int currentHP { get; private set; }
    public int currentWILL { get; private set; }
    public int maxHP { get; private set; }
    public int maxWILL { get; private set; }

    // Properties
    public string Name => data.unitName;
    public int damage => data.damage;
    public int speed => data.speed;
    public bool isPlayer => data is CharacterData;
    public Sprite Icon => isPlayer ? ((CharacterData)data).icon : null;

    public void InitializeFromRuntimeData(RuntimeUnitData runtime)
    {
        data = runtime.baseData;
        runtimeData = runtime;

        maxHP = data.maxHP;
        maxWILL = data.maxWILL;

        currentHP = runtime.currentHP;
        currentWILL = runtime.currentWILL;
    }

    // Init for ENEMY units (temporary stats)
    public void InitializeFromData(EnemyData enemyData)
    {
        data = enemyData;

        maxHP = data.maxHP;
        maxWILL = data.maxWILL;

        currentHP = maxHP;
        currentWILL = maxWILL;
    }

    public void TakeDamage(int amount)
    {
        currentHP = Mathf.Max(currentHP - amount, 0);
    }

    public bool IsDead() => currentHP <= 0;
}
