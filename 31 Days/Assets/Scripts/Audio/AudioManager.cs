using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private AudioSource audioSource;

    [Header("Music Clips")]
    public AudioClip mainMenuMusic;
    public AudioClip defaultMusic;
    public AudioClip doorMusic;
    [SerializeField] public float fadeTime = 0.4f;
    [SerializeField] public float fullVolume = 0.5f;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            audioSource = GetComponent<AudioSource>();
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

    public float GetTime()
    {
        return audioSource.time;
    }

    public void SetTime(float newTime)
    {
        audioSource.time = newTime;
    }

    public static IEnumerator FadeOutEnumerator(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            yield return null;
        }


        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    public static IEnumerator FadeInEnumerator(AudioSource audioSource, float FadeTime, float fullVolume)
    {
        audioSource.volume = 0f;
        audioSource.Play();

        while (audioSource.volume < fullVolume)
        {
            audioSource.volume += Time.deltaTime / FadeTime;
            yield return null;
        }

        audioSource.volume = fullVolume;
    }

    public void CrossfadeMusic(AudioClip newClip, float prevTime, bool saveTime)
    {
        StartCoroutine(CrossfadeCoroutine(newClip, prevTime, saveTime));
    }

    private IEnumerator CrossfadeCoroutine(AudioClip newClip, float prevTime, bool saveTime)
    {
        yield return StartCoroutine(FadeOutEnumerator(audioSource, fadeTime));
        PlayMusic(newClip);
        if (saveTime)
            SetTime(prevTime);
        yield return StartCoroutine(FadeInEnumerator(audioSource, fadeTime, fullVolume));
    }
}