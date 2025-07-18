using UnityEngine;

public class EnemyClickTarget : MonoBehaviour
{
    private Unit enemyUnit;
    public RPGManager manager;

    void Awake()
    {
        enemyUnit = GetComponent<Unit>();
        Debug.Log("Assigned unit to: " + gameObject.name);
    }

    public void OnEnemyClick()
    {
        if (manager.CanSelectTarget())
        {
            manager.OnEnemySelected(enemyUnit);
        }
    }
}
