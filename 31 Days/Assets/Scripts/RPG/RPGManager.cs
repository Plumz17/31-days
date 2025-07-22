using System;
using System.Collections;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public enum BattleState {START, ENEMYTURN, PLAYERTURN, WIN, LOSS}


public class RPGManager : MonoBehaviour
{
    public BattleState currentState;
    public List<Unit> playerUnits;
    public List<Unit> enemyUnits;
    public List<Unit> turnOrder = new List<Unit>();
    public int turnIndex = 0;
    public string currentAction = "";

    public Unit currentUnit;
    public PlayerBoxesGroup playerGroup;
    public EnemyGroup enemyGroup;
    public ButtonsGroup buttonsGroup;
    public TMP_Text textBox;
    public GameObject infoBox;
    public float waitingTime = 1f;
    public bool isBusy = false;

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
        yield return new WaitForSeconds(waitingTime);
        buttonsGroup.SetButtonHighlights(true);

        if (IsBattleOver())
        {
            EndBattle();
            yield break;
        }

        currentUnit = turnOrder[turnIndex];
        currentUnit.ResetDefend();

        if (currentUnit.IsDead())
        {
            EndTurn();
            yield break;
        }

        if (currentUnit.isPlayer)
        {
            currentState = BattleState.PLAYERTURN;
            textBox.text = currentUnit.Name + "'s Turn. Choose an action.";
            isBusy = false;
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
        return isChoosingTarget && currentState == BattleState.PLAYERTURN && !isBusy;
    }

    public void OnEnemySelected(Unit selectedEnemy)
    {
        if (!CanSelectTarget()) return;

        enemyGroup.SetEnemyHighlights(false);

        if (currentAction == "attack" || currentAction == "skill")
        {
            isChoosingTarget = false;
            isBusy = true;
            StartCoroutine(PlayerAttack(currentUnit, selectedEnemy, currentAction));
        }

        else if (currentAction == "check")
        {
            textBox.text = $"{selectedEnemy.Name} HP: {selectedEnemy.currentHP}/{selectedEnemy.maxHP}";
            infoBox.gameObject.SetActive(true);
            Image image = infoBox.GetComponentInChildren<Image>();
            image.sprite = selectedEnemy.enemyInfoBox;
            image.SetNativeSize();
        }
    }

    IEnumerator PlayerAttack(Unit attacker, Unit target, string currentAction)
    {
        buttonsGroup.SetButtonHighlights(false);
        int damage = 0;
        int skillCost = 50;

        if (currentAction == "attack")
            damage = attacker.damage;
        else if (currentAction == "skill")
        {
            if (attacker.currentWILL >= skillCost)
            {
                damage = attacker.skillDamage;
                attacker.UseWILL(skillCost);
            }
            else
            {
                textBox.text = $"{attacker.Name} tried to use a skill but didn’t have enough WILL!";
                yield return new WaitForSeconds(waitingTime);
                EndTurn();
                yield break;
            }
        }

        target.TakeDamage(damage);
        
        textBox.text = target.Name + " took " + damage + " Damage, now it has " + target.currentHP + " HP";
        playerGroup.UpdatePartyUI(playerUnits);
        buttonsGroup.ShowBackButton(false);

        yield return new WaitForSeconds(waitingTime);

        if (target.IsDead())
        {
            textBox.text = target.Name + " died"; 
            turnOrder.Remove(target);
            enemyUnits.Remove(target);
            enemyGroup.OnEnemyDeath(target);
            //enemyGroup.SetupEnemies(enemyUnits);
        }
        EndTurn();
    }
    

    IEnumerator EnemyAttack(Unit enemy)
    {
        buttonsGroup.SetButtonHighlights(false);
        var validTargets = playerUnits.FindAll(p => !p.IsDead());
        if (validTargets.Count == 0) yield break;

        Unit target = validTargets[UnityEngine.Random.Range(0, validTargets.Count)];

        textBox.text = enemy.Name + " attacks " + target.Name + " for " + target.TakeDamage(enemy.damage) + " damage.";

        yield return new WaitForSeconds(waitingTime);

        if (target.IsDead())
        {
            textBox.text = target.Name + " died"; 
            playerUnits.Remove(target);
            turnOrder.Remove(target);
        }

        playerGroup.UpdatePartyUI(playerUnits);
    }

    public void EndTurn()
    {
        turnIndex = (turnIndex + 1) % turnOrder.Count;

        currentAction = ""; //reset current action

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

    public void EndBattle(bool didPlayerFlee = false)
    {
        DuskManager.instance.SavePartyData(playerUnits);

        if (didPlayerFlee)
        {
            textBox.text = "You ran!";
        }
        else if (currentState == BattleState.WIN)
        {
            textBox.text = "You Won!";
        }
        else
        {
            textBox.text = "You Lost :(";
        }
        LevelLoader.Instance.LoadNextLevel(12, DuskManager.instance.currentLocation);
    }    
}
