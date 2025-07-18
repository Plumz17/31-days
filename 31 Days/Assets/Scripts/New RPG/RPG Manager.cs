using System;
using UnityEngine;

public enum BattleState {START, ENEMYTURN, PLAYERTURN, WIN, LOSS}

public class RPGManager : MonoBehaviour
{
    public BattleState currentState;
    public Unit playerUnit;
    public Unit enemyUnit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = BattleState.START;
        SetupBattle();
    }

    private void SetupBattle() //Setup Battle
    {
        throw new NotImplementedException();
    }
}
