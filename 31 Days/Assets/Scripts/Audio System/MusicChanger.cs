using UnityEngine;

public class MusicChanger : MonoBehaviour
{
    public AudioClip sceneMusic;
    private AudioClip prevMusic;
    private float prevTime;
    //private bool didChangeMusic = false;

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
                //didChangeMusic = true;
            }
        }
    }

    // void OnDestroy()
    // {
    //     if (AudioManager.instance != null && didChangeMusic)
    //     {
    //         Debug.Log("Reverting to " + prevMusic?.name);
    //         AudioManager.instance.CrossfadeMusic(prevMusic, prevTime, true);
    //     }
    // }
}
