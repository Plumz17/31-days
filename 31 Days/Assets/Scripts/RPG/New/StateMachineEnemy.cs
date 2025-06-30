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
        float calc_damage = enemy.curATK + BSM.PerformList[0].choosenAttack.attackDamage;
        PlayerToAttack.GetComponent<StateMachinePlayer>().TakeDamage(calc_damage);
    }
}