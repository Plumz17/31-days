using UnityEngine;
using UnityEngine.EventSystems; // Make sure to include this namespace

public class PersistentEventSystem : MonoBehaviour
{
    public static PersistentEventSystem instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {

            Destroy(gameObject);
        }
    }
}