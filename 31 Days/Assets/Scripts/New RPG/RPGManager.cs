using System;
using System.Collections;
using TMPro;
using UnityEngine;
using System.Collections.Generic;


public enum BattleState {START, ENEMYTURN, PLAYERTURN, WIN, LOSS}

public class RPGManager : MonoBehaviour
{
    public BattleState currentState;
    public List<Unit> playerUnits;
    public List<Unit> enemyUnits;
    public List<Unit> turnOrder = new List<Unit>();
    public int turnIndex = 0;

    private Unit currentUnit;
    public PlayerBoxesGroup playerGroup;
    public EnemyGroup enemyGroup;
    public TMP_Text textBox;

    public EncounterData encounter;

    private bool isChoosingTarget = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = BattleState.START;
        SetupBattle();
    }

    private void SetupBattle() //Setup Battle
    {
        playerGroup.SetupPartyUI(playerUnits);
        enemyGroup.SetupEnemies(encounter, enemyUnits);

        turnOrder.Clear();
        turnOrder.AddRange(playerUnits);
        turnOrder.AddRange(enemyUnits);
        turnIndex = 0;

        StartCoroutine(NextTurn());
    }

    IEnumerator NextTurn()
    {
        yield return new WaitForSeconds(1f);

        if (IsBattleOver())
        {
            EndBattle();
            yield break;
        }

        do
        {
            currentUnit = turnOrder[turnIndex];
            turnIndex = (turnIndex + 1) % turnOrder.Count;
        } while (currentUnit.IsDead());

        if (currentUnit.isPlayer)
        {
            currentState = BattleState.PLAYERTURN;
            textBox.text = currentUnit.Name + "'s Turn. Choose an action.";
        }

        else
        {
            currentState = BattleState.ENEMYTURN;
            yield return StartCoroutine(EnemyAttack(currentUnit));
            EndTurn();
        }
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
        StartCoroutine(PlayerAttack(currentUnit, selectedEnemy));
    }


    IEnumerator PlayerAttack(Unit attacker, Unit target)
    {
        Debug.Log("Player Attack()");
        Debug.Log(target.name);
        target.TakeDamage(attacker.damage);
        textBox.text = target.Name + " took " + attacker.damage + " Damage";
        Debug.Log("Take Damage Successful");

        yield return new WaitForSeconds(1f);

        if (target.IsDead())
            enemyUnits.Remove(target);

        EndTurn();
    }

    IEnumerator EnemyAttack(Unit enemy)
    {
        var validTargets = playerUnits.FindAll(p => !p.IsDead());
        if (validTargets.Count == 0) yield break;

        Unit target = validTargets[UnityEngine.Random.Range(0, validTargets.Count)];
        target.TakeDamage(enemy.damage);

        textBox.text = enemy.Name + " attacks " + target.Name + " for " + enemy.damage + " damage.";

        yield return new WaitForSeconds(1f);

        if (target.IsDead())
            playerUnits.Remove(target);

        playerGroup.SetupPartyUI(playerUnits);
    }

    void EndTurn()
    {
        StartCoroutine(NextTurn());
    }

    bool IsBattleOver()
    {
        if (playerUnits.Count == 0)
        {
            currentState = BattleState.LOSS;
            return true;
        }

        if (enemyUnits.Count == 0)
        {
            currentState = BattleState.WIN;
            return true;
        }

        return false;
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

        LevelLoader.Instance.LoadNextLevelFromButton(12);
    }    
}
