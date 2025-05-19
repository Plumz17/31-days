using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed;
    private float movement;
    [SerializeField] private InputActionReference move;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>(); 
        move.action.Enable();
    }

    private void Update() {
        movement = move.action.ReadValue<float>();
    }

    private void FixedUpdate() {
        rb.linearVelocity = new Vector2(movement * moveSpeed, rb.linearVelocityY);
    }
}
