using UnityEngine;

public class DialogueSingleton : MonoBehaviour
{
    public static DialogueSingleton Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
