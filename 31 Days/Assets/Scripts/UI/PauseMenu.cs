using System;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance {get; private set;}

    private InputActions playerInput;
    private bool canPause = true;
    private bool gameIsPaused = false;

    [Header("References")]
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Button saveButton;
    [SerializeField] private GameObject controlsGuide;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip cancelSound;
    [SerializeField] private AudioClip clickSound;

    [Header("Unpausable Scene Keywords")]
    [SerializeField] private List<string> blockedScenes = new List<string> {"Main Menu", "Cutscene", "RPG", "End Cutscene"};

    private AudioSource audioSource;
    private bool isControlActive = false;
    public static event Action OnPauseMenuClosed;
    private string sceneName;

    private void Awake()
    {
        HandleSingeton();
        playerInput = new InputActions();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        playerInput.UI.Enable();
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnDisable()
    {
        playerInput.UI.Disable();
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene current, Scene next)
    {
        sceneName = next.name;
        Debug.Log("Scene changed to: " + sceneName);
        Debug.Log("Blocked: " + blockedScenes.Contains(sceneName));
    }

    void HandleSingeton()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (!canPause) return;

        // Exact matches
        if (blockedScenes.Contains(sceneName)) return;

        if (playerInput.UI.Esc.triggered && !isControlActive)
        {
            if (gameIsPaused)
            {
                PlayCancelSFX();
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        OnPauseMenuClosed?.Invoke();
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        gameIsPaused = true;
    }

    public void OnLoadButtonClick()
    {
        PlayClickSFX();
        SaveData.Load();
    }

    public void OnSaveButtonClick()
    {
        PlayClickSFX();
        SaveData.Save();
    }

    public void OnDebugButtonClick()
    {
        Debug.Log(PlayerDataManager.instance.currentData.objective);
    }

    public void OnResumeButtonClick()
    {
        PlayCancelSFX();
        Resume();
    }

    public void OnMenuButtonClick()
    {
        OnPauseMenuClosed?.Invoke();
        pauseMenuUI.SetActive(false);
        PlayClickSFX();
        Time.timeScale = 1;
        SceneManager.LoadScene(11);
        gameIsPaused = false;
    }

    public void OnControlsButtonClick()
    {
        if (!isControlActive)
        {
            PlayClickSFX();
            controlsGuide.SetActive(true);
            isControlActive = true;
        }
    }

    public void CloseControlsGuide()
    {
        controlsGuide.SetActive(false);
        isControlActive = false;
    }

    public void OnAdvanceButtonClick()
    {
        CalenderAndObjectiveManager.instance.AdvanceTimeBlock();
    }

    public void SetCanSave(bool set)
    {
        saveButton.gameObject.SetActive(set);
    }

    public void SetCanPause(bool set)
    {
        canPause = set;
    }

    private void PlayClickSFX()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(clickSound);
        }
    }

    public void PlayCancelSFX()
    {
        if (audioSource != null && cancelSound != null)
        {
            audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(cancelSound);
        }
    }
}
