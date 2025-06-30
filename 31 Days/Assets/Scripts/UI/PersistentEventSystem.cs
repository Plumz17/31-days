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
            Debug.Log("Persistent Event System created and set to DontDestroyOnLoad.");
        }
        else if (instance != this)
        {
            // If another Event System already exists and is persistent, destroy this one.
            Debug.LogWarning("Duplicate Event System detected. Destroying new one.");
            Destroy(gameObject);
        }
    }
}