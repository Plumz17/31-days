using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnemyGroup : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] private List<GameObject> enemyStations;
    [SerializeField] private List<EnemyData> enemyDataList;

    public void SetupEnemies(List<Unit> enemyUnits)
    {
        enemyDataList = DuskManager.instance.GetEnemyData();

        for (int i = 0; i < enemyStations.Count; i++)
        {
            if (i < enemyDataList.Count)
            {
                GameObject enemyStation = enemyStations[i];
                enemyStation.SetActive(true);

                Image img = enemyStation.transform.Find("Enemy")?.GetComponent<Image>();
                Unit unit = enemyStation.GetComponent<Unit>();
                unit.Init(enemyDataList[i]);
                
                enemyUnits.Add(unit); //Doesnt add properly
                if (img != null)
                {
                    img.sprite = enemyDataList[i].enemySprite;
                    img.SetNativeSize();
                }
            }
            else
            {
                enemyStations[i].SetActive(false); // Hide unused slots
            }
        }
        
    }

    public void OnEnemyDeath(Unit deadEnemy)
    {
        for (int i = 0; i < enemyStations.Count; i++)
        {
            GameObject enemyObj = enemyStations[i];
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
