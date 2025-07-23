using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    public InputActions playerInput;
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    private bool canPause = true;
    public Button saveButton;

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

        if (playerInput.UI.Esc.triggered)
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

    public void OnAdvanceButtonClick()
    {
        CalenderManager.instance.AdvanceTimeBlock();
    }

    public void SetCanPause(bool set)
    {
        canPause = set;
    }
}
