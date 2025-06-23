using UnityEngine;

public class Creepy : MonoBehaviour
{
    public AudioClip sceneSpecificMusic;
    private AudioClip prevMusic;

    private bool didChangeMusic = false;

    void Start()
    {
        if (BGMusic.instance != null)
        {
            prevMusic = BGMusic.instance.GetClip();

            if (sceneSpecificMusic != null && sceneSpecificMusic != prevMusic)
            {
                BGMusic.instance.PlayMusic(sceneSpecificMusic);
                didChangeMusic = true;
            }
        }
    }

    void OnDestroy()
    {
        if (BGMusic.instance != null && didChangeMusic)
        {
            BGMusic.instance.PlayMusic(prevMusic);
        }
    }
}
