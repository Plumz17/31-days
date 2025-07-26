using UnityEngine;
using UnityEngine.Playables;

public class AutoPlayTimeline : MonoBehaviour
{
    void Start()
    {
        GetComponent<PlayableDirector>().Play();
    }
}