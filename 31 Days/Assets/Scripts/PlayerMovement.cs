using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] private float moveSpeed;
    [SerializeField] private InputActionReference move;
    private float movement;
    private bool facingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        move.action.Enable();
    }

    private void Update()
    {
        movement = move.action.ReadValue<float>();
        anim.SetBool("isMoving", movement != 0);

        if ((movement > 0 && !facingRight) || (movement < 0 && facingRight))
        {
            Flip();
        }
        
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movement * moveSpeed, rb.linearVelocityY);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
