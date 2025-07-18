using UnityEngine;
using System.Collections.Generic;

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

                SpriteRenderer sr = enemy.GetComponentInChildren<SpriteRenderer>();
                Unit unit = enemyUnits[i];
                if (unit.data is EnemyData enemyData && sr != null)
                {
                    sr.sprite = enemyData.enemySprite;
                }
            }
            else
            {
                enemies[i].SetActive(false); // Hide unused slots
            }
        }
    }
}
