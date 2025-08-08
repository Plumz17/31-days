using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    public InputActions playerInput;
    public bool canPause = true;
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public Button saveButton;
    public GameObject controlsGuide;
    public GameObject buttonContainer;
    private bool isControlUp = false;
    [SerializeField] private AudioClip cancelSound;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioSource audioSource;
    public static event Action OnPauseMenuClosed;


    private void OnEnable() => playerInput.UI.Enable();
    //private void OnDisable() => playerInput.UI.Disable();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        playerInput = new InputActions();
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (!canPause) return;
        if (SceneManager.GetActiveScene().name == "Main Menu" || SceneManager.GetActiveScene().name.Contains("Cutscene")) return;
        if (SceneManager.GetActiveScene().name.Contains("RPG")) return;
        if (playerInput.UI.Esc.triggered && !isControlUp)
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
        if (!isControlUp)
        {
            PlayClickSFX();
            controlsGuide.SetActive(true);
            isControlUp = true;
        }
    }

    public void CloseControlsGuide()
    {
        controlsGuide.SetActive(false);
        isControlUp = false;
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
