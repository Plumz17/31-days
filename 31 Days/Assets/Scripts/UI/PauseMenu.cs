using System;
using NUnit.Framework;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    public InputActions playerInput;
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public Button saveButton;
    public GameObject controlsGuide;
    public GameObject buttonContainer;
    private bool isControlUp = false;
    [SerializeField] private AudioClip cancelSound;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioSource audioSource;

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
        if (SceneManager.GetActiveScene().name == "Main Menu") return;
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

    public void OnResumeButtonClick()
    {
        PlayCancelSFX();
        Resume();
    }

    public void OnMenuButtonClick()
    {
        PlayClickSFX();
        Time.timeScale = 1;
        SceneManager.LoadScene(11);
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
        CalenderManager.instance.AdvanceTimeBlock();
    }

    public void SetCanPause(bool set)
    {
        saveButton.gameObject.SetActive(set);
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
