using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    private InputActions playerInput;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private CalenderAndObjectiveManager calendar;

    private float movementInput;
    private bool isFacingRight = true;
    private bool isMovable = true;

    [Header("Footstep Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private AudioClip enemyClip;
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
        calendar = CalenderAndObjectiveManager.instance;
    }

    private void Update()
    {
        if (!isMovable)
        {
            StopMovement();
            return;
        }

        HandleInput();
        HandleAnimation();
        HandleFacing();
        HandleFootsteps();
    }

    private void HandleFacing()
    {
        if ((movementInput > 0 && !isFacingRight) || (movementInput < 0 && isFacingRight))
            Flip();
    }

    private void HandleAnimation()
    {
        anim.SetBool("isMoving", movementInput != 0);
    }

    private void HandleInput()
    {
        movementInput = playerInput.Player.Move.ReadValue<float>();
    }

    private void StopMovement()
    {
        movementInput = 0;
        anim.SetBool("isMoving", false);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movementInput * moveSpeed, rb.linearVelocityY);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        spriteRenderer.flipX = !isFacingRight;
    }

    public void SetCanMove(bool value)
    {
        isMovable = value;
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
        if (Mathf.Abs(movementInput) > 0.1f) //if is Moving
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

    private void PlaySFX(AudioClip clip)
    {
        if (audioSource == null) return;
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(clip);
    }

    public void PlayFootstep() => PlaySFX(footstepClip);
    public void PlayEnemyEncounterSFX() => PlaySFX(enemyClip);

    private void OnShowCalendar(InputAction.CallbackContext context)
    {
        if (calendar != null)
        {
            calendar.SetCalendarUI();
        }
    }
    
    public void WalkToPosition(Vector2 targetPosition, float stopDistance = 0.1f)
    {
        StartCoroutine(WalkCoroutine(targetPosition, stopDistance));
    }

    private IEnumerator WalkCoroutine(Vector2 targetPosition, float stopDistance)
    {
        Vector2 finalTarget = targetPosition;

        if (stopDistance < 0)
        {
            float direction = Mathf.Sign(targetPosition.x - transform.position.x);
            finalTarget.x += -stopDistance * direction;
            stopDistance = 0.05f; // safety threshold
        }

        isMovable = false;

        while (Mathf.Abs(transform.position.x - finalTarget.x) > stopDistance)
        {
            float moveDir = Mathf.Sign(finalTarget.x - transform.position.x);
            movementInput = moveDir;

            HandleAnimation();
            HandleFacing();

            yield return null;
        }

        StopMovement();
        rb.linearVelocity = Vector2.zero;
        isMovable = true;
    }
}
