using System;
using System.Collections;
using TMPro;
using UnityEngine;

public enum BattleState {START, ENEMYTURN, PLAYERTURN, WIN, LOSS}

public class RPGManager : MonoBehaviour
{
    public BattleState currentState;
    public Unit playerUnit;
    public Unit enemyUnit;
    public PlayerUI playerUI;
    public TMP_Text textBox;

    private bool isChoosingTarget = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = BattleState.START;
        SetupBattle();
    }

    private void SetupBattle() //Setup Battle
    {
        playerUI.UpdateUI(playerUnit);

        currentState = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    private void PlayerTurn()
    {
        textBox.text = "Player's Turn";
    }

    public void OnAttackButton()
    {
        isChoosingTarget = true;
        textBox.text = "Choose a target.";
    }

    public bool CanSelectTarget()
    {
        return isChoosingTarget && currentState == BattleState.PLAYERTURN;
    }

    public void OnEnemySelected(Unit selectedEnemy)
    {
        if (!CanSelectTarget()) return;

        isChoosingTarget = false;
        StartCoroutine(AttackEnemy(selectedEnemy));
    }


    IEnumerator AttackEnemy(Unit target)
    {
        target.TakeDamage(playerUnit.damage);
        textBox.text = target.Name + " took " + playerUnit.damage + " Damage";

        yield return new WaitForSeconds(1f);

        if (target.IsDead())
        {
            currentState = BattleState.WIN;
            EndBattle();
        }
        else
        {
            currentState = BattleState.ENEMYTURN;
            StartCoroutine(AttackPlayer());
        }
    }

    IEnumerator AttackPlayer()
    {
        playerUnit.TakeDamage(enemyUnit.damage);
        textBox.text = enemyUnit.Name + " deals " + enemyUnit.damage + " Damage";

        yield return new WaitForSeconds(1f);

        playerUI.UpdateUI(playerUnit);

        if (playerUnit.IsDead())
        {
            currentState = BattleState.LOSS;
            EndBattle();
        }
        else
        {
            currentState = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    private void EndBattle()
    {
        if (currentState == BattleState.WIN)
        {
            textBox.text = "You Won!";
        }
        else
        {
            textBox.text = "You Lost :(";
        }
    }    
}
