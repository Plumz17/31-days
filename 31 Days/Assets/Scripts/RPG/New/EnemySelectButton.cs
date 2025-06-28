using UnityEngine;
using System.Collections;

public class EnemySelectButton : MonoBehaviour {

    public GameObject EnemyPrefab;

    public void SelectEnemy()
    {
        GameObject.Find("BattleManager").GetComponent < StateMachineBattle> ();//save input enemy prefsab
    }
}