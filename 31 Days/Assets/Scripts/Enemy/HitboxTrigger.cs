using UnityEngine;
using UnityEngine.SceneManagement;

public class HitboxTrigger : MonoBehaviour
{
    private Duskborne duskborne;

    private void Awake()
    {
        duskborne = GetComponentInParent<Duskborne>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Trigger Entered!");
            PlayerHiding playerHiding = other.GetComponent<PlayerHiding>();

            if (playerHiding != null && !playerHiding.IsHiding())
            {
                DuskManager.instance.currentLocation = other.transform.position;
                DuskManager.instance.currentScene = SceneManager.GetActiveScene().buildIndex;
                other.GetComponent<PlayerMovement>().PlayEnemyEncounterSFX();
                duskborne.OnPlayerHit();
            }
        }
    }
}
