using UnityEngine;
using System.Collections;

public class EnemySelectButton : MonoBehaviour {

    public GameObject EnemyPrefab;

    public void SelectEnemy()
    {
        GameObject.Find("BattleManager").GetComponent <StateMachineBattle>().Input2(EnemyPrefab);//save input enemy prefsab
    }
}