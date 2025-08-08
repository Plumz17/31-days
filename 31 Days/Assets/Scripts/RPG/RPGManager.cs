using System;
using System.Collections;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


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
    public AudioSource sfxSource;
    public AudioClip attackSFX;
    public AudioClip healSFX;
    public AudioClip focusSFX;
    public AudioClip defendSFX;
    public AudioClip fleeSFX;
    public AudioClip backSFX;
    public float defaultVolume = 1;


    public bool isChoosingTarget = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = BattleState.START;
        SetupBattle();
    }

    public void PlaySFX(AudioClip sfx)
    {
        sfxSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        sfxSource.PlayOneShot(sfx);
    }

    private void SetupBattle() //Setup Battle
    {
        sfxSource.volume = defaultVolume;
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

        // if (currentUnit.IsDead())
        // {
        //     EndTurn();
        //     yield break;
        // }

        if (currentUnit.isPlayer)
        {
            buttonsGroup.ShowBackButton(false);
            currentState = BattleState.PLAYERTURN;
            textBox.text = currentUnit.Name + "'s Turn. Choose an action.";
            isBusy = false;
        }

        else
        {
            buttonsGroup.ShowBackButton(true);
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

    public IEnumerator PlayerAttack(Unit attacker, Unit target, string currentAction)
    {
        buttonsGroup.SetButtonHighlights(false);
        int skillCost = attacker.skill.willCost;

        if (currentAction == "attack")
        {
            PlaySFX(attackSFX);
            int damage = attacker.damage;
            textBox.text = $"{attacker.Name} attacked!";
            StartCoroutine(target.FlickerAlpha());
            yield return new WaitForSeconds(waitingTime);
            target.TakeDamage(damage);
            textBox.text = $"{target.Name} took {damage} damage, now it has {target.currentHP} HP.";
            HandleEnemyDeath(target);
            yield return new WaitForSeconds(waitingTime);
        }

        else if (currentAction == "skill")
        {
            if (attacker.currentWILL < skillCost)
            {
                textBox.text = $"{attacker.Name} tried to use {attacker.skill.skillName}, but didn’t have enough WILL!";
                yield return new WaitForSeconds(waitingTime);
                EndTurn();
                yield break;
            }

            attacker.UseWILL(skillCost);
            playerGroup.UpdatePartyUI(playerUnits);

            if (attacker.skill.skillType == "attack")
            {
                PlaySFX(attackSFX);
                int damage = attacker.skill.skillAmount;
                textBox.text = $"{attacker.Name} used {attacker.skill.skillName}!";
                StartCoroutine(target.FlickerAlpha());
                yield return new WaitForSeconds(waitingTime);
                target.TakeDamage(damage);
                textBox.text = $"{target.Name} took {damage} damage, now it has {target.currentHP} HP.";
                HandleEnemyDeath(target);
                yield return new WaitForSeconds(waitingTime);
            }
            else if (attacker.skill.skillType == "heal")
            {
                PlaySFX(healSFX);
                int healPercentage = attacker.skill.skillAmount;
                textBox.text = $"{attacker.Name} used {attacker.skill.skillName} and healed the party!";
                yield return new WaitForSeconds(waitingTime);

                foreach (var ally in playerUnits)
                {
                    if (!ally.IsDead())
                    {
                        ally.HealSkill(healPercentage);
                    }
                }
                playerGroup.UpdatePartyUI(playerUnits);
            }
            else if (attacker.skill.skillType == "spread")
            {
                PlaySFX(attackSFX);
                int damage = attacker.skill.skillAmount;
                textBox.text = $"{attacker.Name} used {attacker.skill.skillName}!";

                yield return new WaitForSeconds(waitingTime);

                foreach (var enemy in enemyUnits.ToArray()) // Copy list to avoid modification during iteration
                {
                    if (!enemy.IsDead())
                    {
                        StartCoroutine(enemy.FlickerAlpha());
                        yield return new WaitForSeconds(0.1f);
                        enemy.TakeDamage(damage);
                        textBox.text = $"{enemy.Name} took {damage} damage.";
                        HandleEnemyDeath(enemy);
                        yield return new WaitForSeconds(0.1f);
                    }
                }
            }
        }

        yield return new WaitForSeconds(waitingTime);
        EndTurn();
    }


    IEnumerator EnemyAttack(Unit enemy)
    {
        buttonsGroup.SetButtonHighlights(false);
        var validTargets = playerUnits.FindAll(p => !p.IsDead());
        if (validTargets.Count == 0) yield break;

        Unit target = validTargets[UnityEngine.Random.Range(0, validTargets.Count)];

        textBox.text = enemy.Name + " attacks " + target.Name + " for " + target.TakeDamage(enemy.damage) + " damage.";
        PlaySFX(attackSFX);
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

    void HandleEnemyDeath(Unit target)
    {
        if (target.IsDead())
        {
            textBox.text = target.Name + " died";
            turnOrder.Remove(target);
            enemyUnits.Remove(target);
            enemyGroup.OnEnemyDeath(target);
        }
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

        if (DuskManager.instance.currentEncounter.name == "First Encounter")
            LevelLoader.Instance.LoadNextLevel(14, DuskManager.instance.currentLocation);

        if (didPlayerFlee)
        {
            textBox.text = "You ran!";
            PlaySFX(fleeSFX);
        }
        else if (currentState == BattleState.WIN)
        {
            textBox.text = "You Won!";
            PlaySFX(fleeSFX);
        }
        else
        {
            textBox.text = "You Lost :(";
            Time.timeScale = 1;
            LevelLoader.Instance.LoadNextLevel(DuskManager.instance.currentScene, DuskManager.instance.defaultLocation);
        }
        DuskManager.instance.SavePartyData(playerUnits);
        LevelLoader.Instance.LoadNextLevel(DuskManager.instance.currentScene, DuskManager.instance.currentLocation);
    }    

}
