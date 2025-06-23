using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusic : MonoBehaviour
{
    public static BGMusic instance;
    private AudioSource audioSource;

    [Header("Music Clips")]
    public AudioClip defaultMusic;
    public AudioClip battleMusic;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            audioSource = GetComponent<AudioSource>();
            if (defaultMusic != null)
            {
                PlayMusic(defaultMusic);
            }
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip == clip) return; // Avoid restarting the same music
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    public AudioClip GetClip()
    {
        return audioSource.clip;
    }
    
    public void PauseMusic()
    {
        audioSource.Pause();
    }

    public void ResumeMusic()
    {
        audioSource.UnPause();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }
}