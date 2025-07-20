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
    public bool isAttacking = false;

    public bool isChoosingTarget = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = BattleState.START;
        SetupBattle();
    }

    private void SetupBattle() //Setup Battle
    {
        playerGroup.SetupPartyUI(playerUnits);
        enemyGroup.SetupEnemies(enemyUnits);

        turnOrder.Clear();
        turnOrder.AddRange(playerUnits);
        turnOrder.AddRange(enemyUnits);
        turnOrder.Sort((a, b) => b.speed.CompareTo(a.speed));

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

        currentUnit = turnOrder[turnIndex];

        if (currentUnit.IsDead())
        {
            EndTurn();
            yield break;
        }

        if (currentUnit.isPlayer)
        {
            currentState = BattleState.PLAYERTURN;
            textBox.text = currentUnit.Name + "'s Turn. Choose an action.";
            isAttacking = false;
        }

        else
        {
            currentState = BattleState.ENEMYTURN;
            yield return StartCoroutine(EnemyAttack(currentUnit));
            EndTurn();
        }
    }

    public bool CanSelectTarget()
    {
        return isChoosingTarget && currentState == BattleState.PLAYERTURN && !isAttacking;
    }

    public void OnEnemySelected(Unit selectedEnemy)
    {
        if (!CanSelectTarget()) return;

        isChoosingTarget = false;
        isAttacking = true;
        StartCoroutine(PlayerAttack(currentUnit, selectedEnemy));
    }


    IEnumerator PlayerAttack(Unit attacker, Unit target)
    {
        target.TakeDamage(attacker.damage);
        textBox.text = target.Name + " took " + attacker.damage + " Damage, now it has " + target.currentHP + " HP";

        yield return new WaitForSeconds(1f);

        if (target.IsDead())
        {
            turnOrder.Remove(target);
            enemyUnits.Remove(target);
            enemyGroup.OnEnemyDeath(target);
            //enemyGroup.SetupEnemies(enemyUnits);
        }
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
        {
            playerUnits.Remove(target);
            turnOrder.Remove(target);
        }

        playerGroup.SetupPartyUI(playerUnits);
    }

    void EndTurn()
    {
        turnIndex = (turnIndex + 1) % turnOrder.Count;
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
            LevelLoader.Instance.LoadNextLevel(12, new Vector3(22.5f,1.875f,0));
        }
        else
        {
            textBox.text = "You Lost :(";
        }
    }    
}
