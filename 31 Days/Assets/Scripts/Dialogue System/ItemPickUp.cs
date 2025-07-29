using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class ItemPickUp : MonoBehaviour
{
    private PlayerMovement player;
    public ExclamationMark exclamationMark;
    public Canvas itemCanvas;
    private bool playerIsClose = false;

    private InputActions inputActions;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        exclamationMark = GetComponentInChildren<ExclamationMark>(true);

        inputActions = new InputActions();
        inputActions.Player.Interact.performed += OnInteract;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (!playerIsClose)
            return;

        if (!itemCanvas.gameObject.activeInHierarchy)
        {
            player.SetCanMove(false);
            exclamationMark?.SetVisible(false);
            itemCanvas.gameObject.SetActive(true);
        }
        else
        {
            player.SetCanMove(true);
            exclamationMark?.SetVisible(true);
            itemCanvas.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerIsClose = true;
        exclamationMark?.SetIsClose(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerIsClose = false;
        exclamationMark?.SetIsClose(false);
    }

    public void ForceOpenItem()
    {
        if (!itemCanvas.gameObject.activeInHierarchy)
        {
            player.SetCanMove(false);
            exclamationMark?.SetVisible(false);
            itemCanvas.gameObject.SetActive(true);
        }
    }
}
