using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

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
    private CalenderManager calendar;

    [Header("Footstep Settings")]
    public AudioSource audioSource;
    public AudioClip footstepClip;
    public float stepDelay = 0.4f; // Delay between steps
    private float stepTimer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        playerInput = new InputActions();
        playerInput.Player.Enable();

        playerInput.Player.Show.performed += OnShowCalendar;
    }

    private void Start()
    {
        calendar = CalenderManager.instance;
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
        HandleFootsteps();
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

    private void HandleFootsteps()
    {
        if (Mathf.Abs(movement) > 0.1f) //if is Moving
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = stepDelay;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    private void PlayFootstep()
    {
        if (footstepClip != null && audioSource != null)
            {
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.PlayOneShot(footstepClip);
            }
    }

    private void OnShowCalendar(InputAction.CallbackContext context)
    {
        if (calendar != null)
        {
            calendar.SetCalendarUI();
        }
    }
}
