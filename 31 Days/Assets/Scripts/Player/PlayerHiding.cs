using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHiding : MonoBehaviour
{
    [SerializeField] private GameObject playerSprite;
    private PlayerMovement playerMovement;

    private InputActions inputActions;
    private bool isNearLocker = false;
    private bool isHiding = false;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        inputActions = new InputActions();
        inputActions.Player.Interact.performed += HandleInteract;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Interact.performed -= HandleInteract;
        inputActions.Disable();
    }

    private void HandleInteract(InputAction.CallbackContext context)
    {
        if (!isNearLocker) return;

        ToggleHide();
    }

    public void SetNearLocker(bool value)
    {
        isNearLocker = value;
    }

    private void ToggleHide()
    {
        isHiding = !isHiding;

        playerSprite.SetActive(!isHiding);
        playerMovement.SetCanMove(!isHiding);
    }

    public bool IsHiding()
    {
        return isHiding;
    }
}