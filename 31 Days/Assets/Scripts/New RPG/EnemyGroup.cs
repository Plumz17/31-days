using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnemyGroup : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] private List<GameObject> enemies;

    public void SetupEnemies(List<Unit> enemyUnits)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (i < enemyUnits.Count)
            {
                GameObject enemy = enemies[i];
                enemy.SetActive(true);

                Image img = enemy.transform.Find("Enemy")?.GetComponent<Image>();
                Unit unit = enemyUnits[i];
                if (unit.data is EnemyData enemyData && img != null)
                {
                    img.sprite = enemyData.enemySprite;
                }
            }
            else
            {
                enemies[i].SetActive(false); // Hide unused slots
            }
        }
    }

    public void OnEnemyDeath(Unit deadEnemy)
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            GameObject enemyObj = enemies[i];
            if (!enemyObj.activeSelf) continue;

            Unit enemyUnit = enemyObj.GetComponent<Unit>();
            if (enemyUnit == deadEnemy)
            {
                enemyObj.SetActive(false);
                break;
            }
        }
    }
}
