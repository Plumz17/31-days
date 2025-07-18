using UnityEngine;

[System.Serializable]
public class BaseEnemy : MonoBehaviour
{
    public string theName;

    public float baseHP;
    public float curHP;

    public float baseMP;
    public float curMP;

    public float baseATK;
    public float curATK;

    public float baseDEF;
    public float curDEF;

    // Changed from string[] to BaseAttack[] to match the system
    public BaseAttack[] attacks;

    public enum EnemyType
    {
        Easy,
        Medium,
        Hard
    }
    public EnemyType enemyType = EnemyType.Easy;
}