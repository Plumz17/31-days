using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
    public Skill skill => isPlayer ? ((CharacterData)data).skill : null;
    public Sprite enemyInfoBox => !isPlayer ? ((EnemyData)data).enemyInfoBox : null;

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

    public void RestoreWILL(int amount)
    {
        currentWILL = Mathf.Min(currentWILL + amount, 100);
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

    public void HealSkill(int percentage)
    {
        float healAmount = percentage * maxHP / 100;
        currentHP += (int)healAmount;
        if (currentHP > maxHP)
            currentHP = maxHP;
    }
    
    public IEnumerator FlickerAlpha(int flickerCount = 3, float flickerSpeed = 0.1f)
    {
        Image image = transform.Find("Enemy")?.GetComponent<Image>();

        Color originalColor = image.color;

        for (int i = 0; i < flickerCount; i++)
        {
            // Set alpha to 0 (invisible)
            image.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
            yield return new WaitForSeconds(flickerSpeed);

            // Set alpha back to original (visible)
            image.color = originalColor;
            yield return new WaitForSeconds(flickerSpeed);
        }
    }

}
