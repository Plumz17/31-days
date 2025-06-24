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
    public bool isFacingRight = true;
    private bool canMove = true;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

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
        spriteRenderer.flipX = !isFacingRight;
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }

    public bool GetFacingDirection()
    {
        return isFacingRight;
    }

    public void SetFacingDirection(bool facingRight)
    {
        isFacingRight = facingRight;
        spriteRenderer.flipX = !facingRight;
    }
    
    void OnDisable()
    {
        playerInput.Player.Disable();
    }
}
