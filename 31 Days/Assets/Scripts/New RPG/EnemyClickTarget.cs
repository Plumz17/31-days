using UnityEngine;

public class EnemyClickTarget : MonoBehaviour
{
    private Unit enemyUnit;
    public RPGManager manager;

    void Awake()
    {
        enemyUnit = GetComponent<Unit>();
    }

    public void OnEnemyClick()
    {
        if (manager.CanSelectTarget())
        {
            manager.OnEnemySelected(enemyUnit);
        }
    }
}
