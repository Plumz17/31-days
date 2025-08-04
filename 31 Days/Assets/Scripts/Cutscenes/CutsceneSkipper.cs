using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class CutsceneSkipper : MonoBehaviour
{
    private PlayableDirector director;
    private InputActions inputActions;

    void Awake()
    {
        director = GetComponent<PlayableDirector>();

        inputActions = new InputActions();
        inputActions.UI.Esc.performed += OnSkip;
    }

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    private void OnSkip(InputAction.CallbackContext context)
    {
        SkipCutscene();
    }

    public void SkipCutscene()
    {
        if (director == null) return;

        director.time = director.duration;
        director.Evaluate();
        director.Stop();
    }
}
