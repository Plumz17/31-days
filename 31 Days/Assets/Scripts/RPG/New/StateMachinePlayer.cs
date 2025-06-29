using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StateMachinePlayer : MonoBehaviour
{
    private StateMachineBattle BSM;
    public BasePlayer player;

    public enum TurnState
    {
        PROCESSING,
        ADDTOLIST,
        WAITING,
        SELECTING,
        ACTION,
        DEAD
    }

    public TurnState currentState;

    // for the ProgressBar
    private float cur_cooldown = 0f;
    private float max_cooldown = 5f;
    public Image ProgressBar;
    public GameObject Selector;
    //ienumerator
    public GameObject EnemyToAttack;
    private bool actionStarted = false;
    private Vector3 startPosition;
    private float animspeed = 2f; // speed of the animation
    private bool alive = true; // to check if the player is alive

    void Start()
    {
        startPosition = transform.position;
        cur_cooldown = Random.Range(0, 2.5f); // Random cooldown for testing
        Selector.SetActive(false);
        BSM = GameObject.Find("BattleManager").GetComponent<StateMachineBattle>();
        
        // Initialize player if not assigned in inspector
        if (player == null)
        {
            player = GetComponent<BasePlayer>();
            if (player == null)
            {
                player = gameObject.AddComponent<BasePlayer>();
                Debug.LogWarning("BasePlayer not assigned in inspector for " + gameObject.name + ". Added component.");
            }
        }
        
        // Initialize name if empty
        if (string.IsNullOrEmpty(player.theName))
        {
            player.theName = gameObject.name;
        }
        
        // Initialize player stats
        currentState = TurnState.PROCESSING;
    }


    void Update()
    {
        switch (currentState)
        {
            case (TurnState.PROCESSING):
                UpgradeProgressBar();
                break;
            case (TurnState.ADDTOLIST):
                BSM.PlayerToManage.Add(this.gameObject);
                currentState = TurnState.WAITING;
                break;
            case (TurnState.WAITING):
                break;
            case (TurnState.SELECTING):
                break;
            case (TurnState.ACTION):
                StartCoroutine(TimeForAction());
                break;
            case (TurnState.DEAD):
                if (!alive)
                {
                    return; // If already dead, do nothing
                }
                else
                {
                    //change tag

                    //not attackable

                    //not manageable

                    //deactivate selector

                    //reset gui

                    //remove from perform list

                    //change color / add animation

                    //reset input

                    alive = false;
                }
                break;
        }
    }

    void UpgradeProgressBar()
    {
        cur_cooldown = cur_cooldown + Time.deltaTime;
        float calc_cooldown = cur_cooldown / max_cooldown;
        ProgressBar.transform.localScale = new Vector3(
            Mathf.Clamp(calc_cooldown, 0, 1),
            ProgressBar.transform.localScale.y,
            ProgressBar.transform.localScale.z
        );

        if (cur_cooldown >= max_cooldown)
        {
            currentState = TurnState.ADDTOLIST;
        }
    }
    private IEnumerator TimeForAction()
    {
        if (actionStarted)
        {
            yield break;
        }

        actionStarted = true;

        // Animate the enemy near the player to attack
        Vector3 enemyPosition = new Vector3(EnemyToAttack.transform.position.x + 1.5f, EnemyToAttack.transform.position.y, EnemyToAttack.transform.position.z);
        while (!MoveTowardsTarget(enemyPosition))
        {
            yield return null;
        }

        // Wait a bit
        yield return new WaitForSeconds(0.5f);

        // Do damage

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

    public void TakeDamage(float getDamageAmount)
    {
        // Add null check for safety
        if (player == null)
        {
            Debug.LogError("Player component is null in TakeDamage!");
            return;
        }
        
        player.curHP -= getDamageAmount;
        if (player.curHP <= 0)
        {
            currentState = TurnState.DEAD;
            Debug.Log(player.theName + " has died.");
        }
        Debug.Log(player.theName + " took " + getDamageAmount + " damage. Current HP: " + player.curHP);
    }
}