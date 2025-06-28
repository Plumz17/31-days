using UnityEngine;
using System.Collections;

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
    public enum EnemyType
    {
        Easy,
        Medium,
        Hard
    }
    
    public EnemyType enemyType = EnemyType.Easy;
}