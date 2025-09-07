using UnityEngine;

public class MusicChanger : MonoBehaviour
{
    public AudioClip sceneMusic;
    private AudioClip prevMusic;
    private float prevTime;

    void Start()
    {
        if (AudioManager.instance != null)
        {
            prevMusic = AudioManager.instance.GetClip();

            if (prevMusic != null && AudioManager.instance.GetTime() > 0f)
            {
                prevTime = AudioManager.instance.GetTime();
            }
            else
            {
                prevTime = 0f;
            }

            if (sceneMusic != null && sceneMusic != prevMusic)
            {
                AudioManager.instance.CrossfadeMusic(sceneMusic, prevTime, false);
            }
        }
    }
}
