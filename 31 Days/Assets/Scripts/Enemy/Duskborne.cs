using UnityEngine;

public class Duskborne : MonoBehaviour
{
    public enum duskState { Wander, Follow }
    [SerializeField] private string enemyID;
    [SerializeField] private Encounter encounterData;
    [SerializeField] private duskState currentState = duskState.Wander;
    [SerializeField] private Transform player;
    [SerializeField] float duskSpeed = 10f; 
    [SerializeField] private float wanderSpeed = 2f;
    [SerializeField] private float wanderChangeInterval = 2f;

    private float wanderTimer;
    private float wanderDirection = 0; // -1 = left, 1 = right, 0 = idle
    private Rigidbody2D rb;
    private PlayerHiding playerHiding;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHiding = player.GetComponent<PlayerHiding>();
    }

    void Start()
    {
        if (DuskManager.instance != null && DuskManager.instance.IsEnemyDefeated(enemyID))
        {
            gameObject.SetActive(false);
        }
    }

    void FollowPlayer()
    {
        Vector2 playerPos = new Vector2(player.position.x, rb.position.y); 
        Vector2 newPos = Vector2.MoveTowards(rb.position, playerPos, duskSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);  
    }

    public void SetFollowState()
    {
        if (!playerHiding.IsHiding())
        {
            currentState = duskState.Follow;
        }
    }

    public void OnPlayerHit()
    {
        Time.timeScale = 0f;
        DuskManager.instance.StartEncounter(encounterData, enemyID);
    }

    private void Wander()
    {
        wanderTimer -= Time.fixedDeltaTime;

        if (wanderTimer <= 0f)
        {
            wanderTimer = wanderChangeInterval;
            wanderDirection = Random.Range(-1f, 1f);
        }

        Vector2 move = new Vector2(wanderDirection * wanderSpeed * Time.fixedDeltaTime, 0);
        rb.MovePosition(rb.position + move);
    }


    void FixedUpdate()
    {
        if (player == null) return;        
        switch (currentState)
        {
            case duskState.Follow:
                if (playerHiding.IsHiding())
                {
                    currentState = duskState.Wander;
                }
                else
                {
                    FollowPlayer();
                }
                break;

            case duskState.Wander:
                Wander();
                break;
        }
    }
}
