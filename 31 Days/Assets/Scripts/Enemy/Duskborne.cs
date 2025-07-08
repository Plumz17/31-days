using UnityEngine;

public class Duskborne : MonoBehaviour
{
    public enum duskState { Idle, Follow }
    [SerializeField] private duskState currentState = duskState.Idle;
    [SerializeField] private Transform player;
    [SerializeField] float duskSpeed = 10f; 
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FollowPlayer()
    {
        Vector2 targetPosition = new Vector2(player.position.x, rb.position.y); 
        Vector2 newPos = Vector2.MoveTowards(rb.position, targetPosition, duskSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);  
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            currentState = duskState.Follow;
        }
    }

    void FixedUpdate()
    {        
        if (currentState == duskState.Follow && player != null)
        {
            FollowPlayer();
        }
    }
}
