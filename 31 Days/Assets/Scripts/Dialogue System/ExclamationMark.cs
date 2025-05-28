using UnityEngine;
using System;

public class ExclamationMark : MonoBehaviour
{
    [SerializeField] float floatStrength = 0.2f;
    private float originalY;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void Awake()
    {
        originalY = transform.position.y;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x,
            originalY + ((float)Math.Sin(Time.time) * floatStrength),
            transform.position.z);
    }

    public void SetVisible(bool visible)
    {
        if (spriteRenderer != null)
            spriteRenderer.enabled = visible;
    }

    public void SetIsClose(bool isClose)
    {
        if (animator != null)
        {
            animator.SetBool("isClose", isClose);
        }
    }
}
