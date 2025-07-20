using UnityEngine;

public class Heal : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        DuskManager.instance.HealParty();
    }
}
