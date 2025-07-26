using UnityEngine;

public class Escapable : MonoBehaviour
{
    private InputActions input;

    private void Awake()
    {
        input = new InputActions();
    }

    private void OnEnable()
    {
        input.UI.Enable();
    }

    private void OnDisable()
    {
        input.UI.Disable();
    }

    void Update()
    {
        if (input.UI.Esc.triggered && gameObject.activeSelf)
        {
            PauseMenu.instance.CloseControlsGuide();
        }
    }
}