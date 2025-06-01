using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    InputActions playerInput;

    private Rigidbody2D rb;
    private Animator anim;
    private float movement;
    private bool isFacingRight = true;
    private bool canMove = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        playerInput = new InputActions();
        playerInput.Player.Enable();
    }

    private void Update()
    {
        if (!canMove)
        {
            movement = 0;
            anim.SetBool("isMoving", false);
            return;
        }

        movement = playerInput.Player.Move.ReadValue<float>();
        anim.SetBool("isMoving", movement != 0);

        if ((movement > 0 && !isFacingRight) || (movement < 0 && isFacingRight))
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
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }
}
