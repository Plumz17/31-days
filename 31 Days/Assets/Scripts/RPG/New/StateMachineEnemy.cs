using System.Collections;
using UnityEngine;

public class StateMachineEnemy : MonoBehaviour
{
    private StateMachineBattle BSM;
    public BaseEnemy enemy;

    public enum TurnState
    {
        PROCESSING,
        CHOOSEACTION,
        WAITING,
        ACTION,
        DEAD
    }

    public TurnState currentState;

    // for the ProgressBar
    private float cur_cooldown = 0f;
    private readonly float max_cooldown = 5f;

    private Vector3 startPosition;
    public GameObject Selector;
    private bool actionStarted = false;
    public GameObject PlayerToAttack;
    private bool alive = true;
    private readonly float animspeed = 2f; // speed of the animation
                                           // Use this for initialization
    void Start()
    {
        alive = true;
        Selector.SetActive(false);
        currentState = TurnState.PROCESSING;
        BSM = GameObject.Find("BattleManager").GetComponent<StateMachineBattle>();
        startPosition = transform.position;

        // Try to get the BaseEnemy component
        // Initialize enemy if not assigned in inspector
        if (enemy == null)
        {
            enemy = GetComponent<BaseEnemy>();
            if (enemy == null)
            {
                enemy = gameObject.AddComponent<BaseEnemy>();
                Debug.LogWarning("BaseEnemy not assigned in inspector for " + gameObject.name + ". Added component.");
            }
        }
        // Initialize name if empty
        if (string.IsNullOrEmpty(enemy.theName))
        {
            enemy.theName = gameObject.name;
        }

        if (BSM == null)
        {
            Debug.LogError("BattleManager not found! Make sure there's a GameObject named 'BattleManager' with StateMachineBattle component.");
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case TurnState.PROCESSING:
                UpgradeProgressBar();
                break;
            case TurnState.CHOOSEACTION:
                ChooseAction();
                currentState = TurnState.WAITING;
                break;
            case TurnState.WAITING:
                break;
            case TurnState.ACTION:
                currentState = TurnState.WAITING;
                StartCoroutine(TimeForAction());
                break;
            case TurnState.DEAD:
                if (!alive)
                {
                    return; // If already processed death, do nothing
                }
                else
                {
                    // Change tag to indicate death
                    this.gameObject.tag = "RPGEnemyDead";
                    
                    // Remove from battle lists
                    BSM.EnemysInBattle.Remove(this.gameObject);
                    
                    // Deactivate selector
                    Selector.SetActive(false);
                    
                    // Remove from perform list
                    for (int i = BSM.PerformList.Count - 1; i >= 0; i--)
                    {
                        if (BSM.PerformList[i].AttackersGameObject == this.gameObject)
                        {
                            BSM.PerformList.RemoveAt(i);
                        }
                    }
                    
                    // Visual feedback - change color to gray
                    SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.color = Color.gray;
                    }
                    else
                    {
                        // Try to find SpriteRenderer on child objects
                        SpriteRenderer childRenderer = GetComponentInChildren<SpriteRenderer>();
                        if (childRenderer != null)
                        {
                            childRenderer.color = Color.gray;
                        }
                    }
                    
                    // Mark as dead
                    alive = false;
                    
                    Debug.Log($"[ENEMY DEATH] {enemy.theName} has been removed from battle!");
                    
                    // Optionally disable the GameObject or play death animation
                    // gameObject.SetActive(false);
                }
                break;
        }
    }

    void UpgradeProgressBar()
    {
        // NEW: Check if this character can fill their bar
        if (!BSM.CanFillBar(this.gameObject))
        {
            // If this character can't fill their bar, don't update the progress
            return;
        }

        cur_cooldown += Time.deltaTime;

        if (cur_cooldown >= max_cooldown)
        {
            currentState = TurnState.CHOOSEACTION;
        }
    }

    void ChooseAction()
    {
        // Add null checks
        if (BSM == null || BSM.PlayersInBattle == null || BSM.PlayersInBattle.Count == 0)
        {
            Debug.LogError("No players available to attack!");
            return;
        }

        if (enemy == null)
        {
            Debug.LogError("Enemy component is null!");
            return;
        }

        HandleTurn myAttack = new HandleTurn
        {
            Attacker = enemy.theName,
            Type = "Enemy",
            AttackersGameObject = this.gameObject,
            AttackersTarget = BSM.PlayersInBattle[Random.Range(0, BSM.PlayersInBattle.Count)]
        };

        int num = Random.Range(0, enemy.attacks.Length);
        myAttack.choosenAttack = enemy.attacks[num];
        Debug.Log(this.gameObject.name + " chose " + myAttack.choosenAttack.attackName);

        BSM.CollectActions(myAttack);
    }

    private IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            yield break;
        }

        actionStarted = true;

        // Animate the enemy near the player to attack
        Vector3 targetPosition = new Vector3(PlayerToAttack.transform.position.x - 1.5f, PlayerToAttack.transform.position.y, PlayerToAttack.transform.position.z);
        while (!MoveTowardsTarget(targetPosition))
        {
            yield return null;
        }

        // Wait a bit
        yield return new WaitForSeconds(0.5f);

        // Do damage
        DoDamage();
        // Animate back to start position
        Vector3 firstPosition = startPosition;
        while (MoveTowardsStart(firstPosition))
        {
            yield return null;
        }

        // Remove this performer from the list in BSM
        BSM.PerformList.RemoveAt(0);

        // Reset BSM -> WAIT
        BSM.battleStates = StateMachineBattle.PerformAction.WAIT;

        // NEW: Notify BSM that action is completed
        BSM.OnActionCompleted();

        actionStarted = false;
        // Reset cooldown
        cur_cooldown = 0f;
        currentState = TurnState.PROCESSING;
    }

    private bool MoveTowardsTarget(Vector3 targetPosition)
    {
        return targetPosition != (transform.position = Vector3.MoveTowards(transform.position, targetPosition, animspeed * Time.deltaTime));
    }
    private bool MoveTowardsStart(Vector3 targetPosition)
    {
        return targetPosition != (transform.position = Vector3.MoveTowards(transform.position, targetPosition, animspeed * Time.deltaTime));
    }
    void DoDamage()
    {
        float calc_damage = enemy.curATK; // Start with base attack
        
        // Check if there's a specific attack/skill chosen
        if (BSM.PerformList.Count > 0 && BSM.PerformList[0].choosenAttack != null)
        {
            calc_damage += BSM.PerformList[0].choosenAttack.attackDamage;
            Debug.Log($"[ENEMY ATTACK] {enemy.theName} using skill: {BSM.PerformList[0].choosenAttack.attackName}");
            Debug.Log($"[ENEMY ATTACK] Base ATK: {enemy.curATK}, Skill damage: {BSM.PerformList[0].choosenAttack.attackDamage}, Total: {calc_damage}");
        }
        else
        {
            Debug.Log($"[ENEMY ATTACK] {enemy.theName} using basic attack with damage: {calc_damage}");
        }
        
        // Apply damage using the player's TakeDamage method (which includes defense calculation)
        StateMachinePlayer playerComponent = PlayerToAttack.GetComponent<StateMachinePlayer>();
        if (playerComponent != null)
        {
            Debug.Log($"[DAMAGE BEFORE DEFENSE] {calc_damage} damage going to {playerComponent.player.theName}");
            playerComponent.TakeDamage(calc_damage);
        }
        else
        {
            Debug.LogError("StateMachinePlayer component not found on target!");
        }
    }
    
    // NEW: Add TakeDamage method for enemies similar to players
    public void TakeDamage(float getDamageAmount)
    {
        // Add null check for safety
        if (enemy == null)
        {
            Debug.LogError("Enemy component is null in TakeDamage!");
            return;
        }

        Debug.Log($"[DEFENSE DEBUG] {enemy.theName} taking damage:");
        Debug.Log($"[DEFENSE DEBUG] - Incoming damage: {getDamageAmount}");
        Debug.Log($"[DEFENSE DEBUG] - Current defense: {enemy.curDEF}");

        // Apply defense calculation - Damage taken = Atk - Current Def
        float damageAfterDefense = getDamageAmount - enemy.curDEF;
        
        // Ensure damage doesn't go below 1 (minimum damage)
        damageAfterDefense = Mathf.Max(damageAfterDefense, 1f);
        
        Debug.Log($"[DEFENSE DEBUG] - Final damage after defense: {damageAfterDefense}");
        Debug.Log($"[DEFENSE DEBUG] - HP before: {enemy.curHP}");

        enemy.curHP -= damageAfterDefense;
        if (enemy.curHP <= 0)
        {
            enemy.curHP = 0; // Ensure HP doesn't go negative
            currentState = TurnState.DEAD;
            Debug.Log(enemy.theName + " has died.");
        }
        
        Debug.Log($"[DEFENSE DEBUG] - HP after: {enemy.curHP}");
        Debug.Log($"[DEFENSE RESULT] {enemy.theName} took {damageAfterDefense} damage (reduced from {getDamageAmount})");
    }
}