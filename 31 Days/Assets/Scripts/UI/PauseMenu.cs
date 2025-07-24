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
        if (playerInput.UI.Esc.triggered && !isControlUp)
        {
            if (gameIsPaused)
            {
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
        SaveData.Load();
    }

    public void OnSaveButtonClick()
    {
        SaveData.Save();
    }

    public void OnResumeButtonClick()
    {
        Resume();
    }

    public void OnMenuButtonClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(11);
    }

    public void OnControlsButtonClick()
    {
        if (!isControlUp)
        {
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
}
