using UnityEngine;
using System.Collections;

public class EnemySelectButton : MonoBehaviour
{

    public GameObject EnemyPrefab;
    private bool showSelector;

    // Start is called before the first frame update

    public void SelectEnemy()
    {
        GameObject.Find("BattleManager").GetComponent<StateMachineBattle>().Input2(EnemyPrefab);//save input enemy prefsab
    }

    public void HideSelector()
    {
        EnemyPrefab.transform.Find("Selector").gameObject.SetActive(false);
    }

    public void ShowSelector()
    {
        EnemyPrefab.transform.Find("Selector").gameObject.SetActive(true);
    }
}