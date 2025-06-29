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
    //panel
    private PlayerPanelStats stats;
    public GameObject PlayerPanel;
    private Transform PlayerPanelSpacer;

    void Start()
    {
        PlayerPanelSpacer = GameObject.Find("UI").transform.Find("MainPanel").transform.Find("PlayerBarSpacer");
        
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

        // Create player panel AFTER player is initialized
        CreatePlayerPanel();

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
                    this.gameObject.tag = "RPGPlayerDead";
                    //not attackable
                    BSM.PlayersInBattle.Remove(this.gameObject); // Remove from the list of players in battle
                    //not manageable
                    BSM.PlayerToManage.Remove(this.gameObject); // Remove from the list of players to manage
                    //deactivate selector
                    Selector.SetActive(false);
                    //reset gui
                    BSM.AttackPanel.SetActive(false);
                    BSM.EnemySelectPanel.SetActive(false);
                    //remove from perform list
                    for (int i = 0; i < BSM.PerformList.Count; i++)
                    {
                        if (BSM.PerformList[i].AttackersGameObject == this.gameObject)
                        {
                            BSM.PerformList.Remove(BSM.PerformList[i]);
                        }
                    }
                    //change color / add animation - Fixed to find the SpriteRenderer on the child
                    Transform squareChild = transform.Find("Square");
                    if (squareChild != null)
                    {
                        SpriteRenderer spriteRenderer = squareChild.GetComponent<SpriteRenderer>();
                        if (spriteRenderer != null)
                        {
                            spriteRenderer.color = Color.gray; // Change color to gray
                        }
                        else
                        {
                            Debug.LogWarning("SpriteRenderer not found on Square child of " + gameObject.name);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Square child not found on " + gameObject.name);
                    }
                    //reset input
                    BSM.PlayerGUIState = StateMachineBattle.PlayerGUI.ACTIVATE;
                    alive = false;
                }
                break;
        }
    }

    void UpgradeProgressBar()
    {
        // Add null check to prevent NullReferenceException
        if (ProgressBar == null)
        {
            Debug.LogWarning("ProgressBar is null for " + gameObject.name + ". Skipping progress bar update.");
            return;
        }

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
            player.curHP = 0; // Ensure HP doesn't go negative
            currentState = TurnState.DEAD;
            Debug.Log(player.theName + " has died.");
        }
        Debug.Log(player.theName + " took " + getDamageAmount + " damage. Current HP: " + player.curHP);
        
        // Update the UI panel if it exists
        UpdatePlayerPanel();
    }

    void CreatePlayerPanel()
    {
        // Add null checks
        if (PlayerPanel == null)
        {
            Debug.LogError("PlayerPanel prefab is not assigned in the inspector for " + gameObject.name);
            return;
        }

        if (PlayerPanelSpacer == null)
        {
            Debug.LogError("PlayerPanelSpacer not found. Make sure UI structure exists.");
            return;
        }

        if (player == null)
        {
            Debug.LogError("Player component is null when creating panel for " + gameObject.name);
            return;
        }

        GameObject panelInstance = Instantiate(PlayerPanel) as GameObject;
        stats = panelInstance.GetComponent<PlayerPanelStats>();

        if (stats == null)
        {
            Debug.LogError("PlayerPanelStats component not found on PlayerPanel prefab.");
            return;
        }

        // Initialize the panel with player data
        UpdatePlayerPanelData();

        // Get the progress bar reference
        ProgressBar = stats.ProgressBar;
        
        // Set the parent
        panelInstance.transform.SetParent(PlayerPanelSpacer, false);
    }

    void UpdatePlayerPanelData()
    {
        if (stats != null && player != null)
        {
            stats.PlayerName.text = player.theName;
            stats.PlayerHP.text = "HP: " + player.curHP + "/" + player.baseHP;
            stats.PlayerMP.text = "MP: " + player.curMP + "/" + player.baseMP;
        }
    }

    void UpdatePlayerPanel()
    {
        UpdatePlayerPanelData();
    }
}