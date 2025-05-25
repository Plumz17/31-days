using UnityEngine;
using UnityEngine.InputSystem;

public class NPC : MonoBehaviour
{
    [SerializeField] private InputActionReference interact; //Note to self: There might be a better way to do this... (To Access the Player's Movement)
    private bool playerIsClose;

    void Start()
    {
        interact.action.Enable();
    }
    
    void Update()
    {
        if (playerIsClose && interact.action.triggered)
        {
            Debug.Log("Hello!");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerIsClose = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerIsClose = false;
        }
    }
}
