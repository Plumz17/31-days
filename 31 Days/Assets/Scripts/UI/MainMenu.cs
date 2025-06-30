using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayMusic(AudioManager.instance.mainMenuMusic);
        }
    }

    public void OnStartButtonClick()
    {
        AudioManager.instance.CrossfadeMusic(AudioManager.instance.defaultMusic, 0, false);
        SceneManager.LoadScene("Classroom");
    }
}
