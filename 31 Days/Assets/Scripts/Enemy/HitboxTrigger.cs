using UnityEngine;

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
            duskborne.OnPlayerHit();
        }
    }
}
