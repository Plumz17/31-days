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
    public int skillDamage => data.skillDamage;
    public int speed => data.speed;

    //Character/Enemy Exclusive Data
    public Sprite Icon => isPlayer ? ((CharacterData)data).icon : null;
    public string enemyInfo => !isPlayer ? ((EnemyData)data).enemyInfo : null;

    // Type flags
    public bool isPlayer => data is CharacterData;

    // Battle flags
    public bool isDefending { get; private set; } = false;

    public void Init(UnitData newData)
    {
        data = newData;
        maxHP = data.maxHP;
        maxWILL = data.maxWILL;
        currentHP = maxHP;
        currentWILL = maxWILL;
        isDefending = false;
    }

    public int TakeDamage(int amount)
    {
        if (isDefending)
        {
            amount = Mathf.CeilToInt(amount * 0.5f);
        }
        currentHP = Mathf.Max(currentHP - amount, 0);
        return amount;
    }

    public void UseWILL(int amount)
    {
        currentWILL = Mathf.Max(currentWILL - amount, 0);
    }

    public void Defend()
    {
        isDefending = true;
    }

    public void ResetDefend()
    {
        isDefending = false;
    }

    public bool IsDead() => currentHP <= 0;

    public void SetStats(int hp, int will) //May be useless, check later
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
