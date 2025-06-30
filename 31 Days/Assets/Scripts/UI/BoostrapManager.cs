using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapManager : MonoBehaviour
{
    void Awake()
    {
        // Unparent all children and mark them as DontDestroyOnLoad
        foreach (Transform child in transform)
        {
            child.SetParent(null); // Detach from BootstrapManager
            child.gameObject.name = child.name; // Optional: remove (Clone) or any suffix
            DontDestroyOnLoad(child.gameObject);
        }
    }

    void Start()
    {
        SceneManager.LoadScene("Classroom");
    }
}