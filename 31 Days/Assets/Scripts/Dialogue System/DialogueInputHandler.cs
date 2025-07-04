using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class DialogueInputHandler : MonoBehaviour
{
    private InputActions input;
    public event Action OnSubmit;
    public event Action<int> OnNavigate;

    private int currentOptionIndex = 0;
    private int optionCount = 0;
    private bool isInChoiceMode = false;

    public int CurrentOptionIndex => currentOptionIndex;

    private void Awake()
    {
        input = new InputActions();
        input.UI.Enable();

        input.UI.Submit.performed += ctx => OnSubmit?.Invoke();
        input.UI.Navigate.performed += ctx => OnNavigateInput(ctx.ReadValue<Vector2>());
    }

    private void OnDestroy()
    {
        input.UI.Submit.performed -= ctx => OnSubmit?.Invoke();
        input.UI.Navigate.performed -= ctx => OnNavigateInput(ctx.ReadValue<Vector2>());
    }

    public void EnableChoiceMode(int count)
    {
        optionCount = count;
        isInChoiceMode = true;
        currentOptionIndex = 0;
        OnNavigate?.Invoke(currentOptionIndex);
    }

    public void DisableChoiceMode()
    {
        isInChoiceMode = false;
    }

    private void OnNavigateInput(Vector2 inputVec)
    {
        if (!isInChoiceMode) return;

        Debug.Log("Test");

        if (inputVec.y < 0)
        {
            currentOptionIndex = (currentOptionIndex + 1) % optionCount;
            OnNavigate?.Invoke(currentOptionIndex);
        }
        else if (inputVec.y > 0)
        {
            currentOptionIndex = (currentOptionIndex - 1 + optionCount) % optionCount;
            OnNavigate?.Invoke(currentOptionIndex);
        }
    }
}
