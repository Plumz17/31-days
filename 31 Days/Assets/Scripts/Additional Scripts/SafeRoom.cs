using System;
using Unity.VisualScripting;
using UnityEngine;

public class SafeRoom : MonoBehaviour
{    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Duskborne"))
        {
            DuskAvoid(collision.gameObject);
        }
    }

    private void DuskAvoid(GameObject enemy)
    {
        Duskborne dusk = enemy.GetComponent<Duskborne>();
        if (dusk != null)
        {
            dusk.AvoidSafeRoom(transform.position);
        }
    }
}
