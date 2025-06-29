using System;
using NUnit.Framework;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public InputActions playerInput;
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;

    private void OnEnable() => playerInput.UI.Enable();
    private void OnDisable() => playerInput.UI.Disable();

    void Awake()
    {
        playerInput = new InputActions();
    }

    void Update()
    {
        if (playerInput.UI.Esc.triggered)
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Debug.Log("Test");
                Pause();
            }
        }
    }

    private void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        gameIsPaused = false;
    }

    void Pause()
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

    public void OnDeleteButtonClick()
    {
       SaveData.ResetToDefault();
    }
}
