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

        if (isPlayer)
        {
            // Load runtime stats from CharacterData
            CharacterData cData = (CharacterData)data;
            currentHP = cData.currentHP <= 0 ? maxHP : cData.currentHP;
            currentWILL = cData.currentWILL <= 0 ? maxWILL : cData.currentWILL;
        }
        else
        {
            currentHP = maxHP;
            currentWILL = maxWILL;
        }
    }

    public void TakeDamage(int amount)
    {
        currentHP = Mathf.Max(currentHP - amount, 0);
    }

    public bool IsDead() => currentHP <= 0;
}
