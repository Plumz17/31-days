using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{   
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;
    
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

    private void PlayClickSFX()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(clickSound);
        }
    }
}
