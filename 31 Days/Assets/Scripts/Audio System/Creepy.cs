using UnityEngine;

public class Creepy : MonoBehaviour
{
    private AudioClip sceneSpecificMusic;
    private AudioClip prevMusic;
    private float prevTime;
    private bool didChangeMusic = false;

    void Start()
    {
        if (AudioManager.instance != null)
        {
            sceneSpecificMusic = AudioManager.instance.doorMusic;
            prevMusic = AudioManager.instance.GetClip();
            prevTime = AudioManager.instance.GetTime();

            if (sceneSpecificMusic != null && sceneSpecificMusic != prevMusic)
            {
                AudioManager.instance.CrossfadeMusic(sceneSpecificMusic, prevTime, false);
                didChangeMusic = true;
            }
        }
    }

    void OnDestroy()
    {
        if (AudioManager.instance != null && didChangeMusic)
        {
            AudioManager.instance.CrossfadeMusic(prevMusic, prevTime, true);
        }
    }
}
