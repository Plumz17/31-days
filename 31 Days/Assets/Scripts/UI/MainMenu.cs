using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnNewGameButtonClick()
    {
        SaveData.ResetToDefault();
        SaveData.Load();
    }

    public void OnContinueButtonClick()
    {
        SaveData.Load();
    }

    public void OnExitButtonClick()
    {
        Application.Quit();
    }
}
