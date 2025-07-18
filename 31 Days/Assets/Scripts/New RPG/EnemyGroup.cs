using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnemyGroup : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] private List<GameObject> enemyStations;

    public void SetupEnemies(EncounterData encounter, List<Unit> enemyUnits)
    {
        for (int i = 0; i < enemyStations.Count; i++)
        {
            if (i < encounter.enemies.Count)
            {
                GameObject enemy = enemyStations[i];
                enemy.SetActive(true);

                Debug.Log(enemy.name);
                Image img = enemyStations[i].transform.Find("Enemy")?.GetComponent<Image>();

                Unit unit = enemyUnits[i];
                unit.data = encounter.enemies[i];

                if (unit.data is EnemyData enemyData && img != null)
                {
                    Debug.Log("Image is Null");
                    img.sprite = enemyData.enemySprite;
                }
            }
            else
            {
                enemyStations[i].SetActive(false); // Hide unused slots
            }
        }
    }
}
