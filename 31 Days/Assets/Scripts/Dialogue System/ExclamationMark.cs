using UnityEngine;
using System;

public class ExclamationMark : MonoBehaviour
{
    [SerializeField] float floatStrength = 0.2f;
    private float originalY;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void Awake() // Set up components
    {
        originalY = transform.position.y;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update() //Handle Bobbing effect
    {
        transform.position = new Vector3(transform.position.x,
            originalY + ((float)Math.Sin(Time.time) * floatStrength),
            transform.position.z);
    }

    public void SetVisible(bool visible) // Show or hide the exclamation mark
    {
        if (spriteRenderer != null)
            spriteRenderer.enabled = visible;
    }

    public void SetIsClose(bool isClose) // Change Exclamation Mark to arrow and vice versa
    {
        if (animator != null)
        {
            animator.SetBool("isClose", isClose);
        }
    }
}
