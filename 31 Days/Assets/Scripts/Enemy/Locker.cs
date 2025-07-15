using UnityEngine;

public class Locker : MonoBehaviour
{
    private PlayerHiding playerHiding;
    public ExclamationMark exclamationMark;

    void Awake()
    {
        exclamationMark = GetComponentInChildren<ExclamationMark>(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerHiding = collision.GetComponent<PlayerHiding>();
        if (playerHiding != null)
        {
            playerHiding.SetNearLocker(true);
            exclamationMark?.SetIsClose(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (playerHiding != null)
        {
            playerHiding.SetNearLocker(false);
            exclamationMark?.SetIsClose(false);
        }
    }
}