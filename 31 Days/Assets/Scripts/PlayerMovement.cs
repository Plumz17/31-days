using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float movement;
    private Animator anim;
    [SerializeField] private float moveSpeed;
    [SerializeField] private InputActionReference move;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        move.action.Enable();
    }

    private void Update()
    {
        movement = move.action.ReadValue<float>();

        if (movement != 0)
        {
            Debug.Log("Moved!");
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movement * moveSpeed, rb.linearVelocityY);
    }
}
