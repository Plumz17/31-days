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
    public int speed => data.speed;

    //Character/Enemy Exclusive Data
    public Sprite Icon => isPlayer ? ((CharacterData)data).icon : null;

    // Type flags
    public bool isPlayer => data is CharacterData;

    private void Awake()
    {
        if (data == null) return;

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

    public void SetStats(int hp, int will)
    {
        currentHP = Mathf.Clamp(hp, 0, maxHP);
        currentWILL = Mathf.Clamp(will, 0, maxWILL);
    }

    public void HealCharacter() //For Healing in Battle (maxHP and maxWILL in this is for in battle, the one in CharData is in the overworld)
    {
        currentHP = maxHP;
        currentWILL = maxWILL;
    }
}
