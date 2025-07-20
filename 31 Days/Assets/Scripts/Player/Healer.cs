using UnityEngine;

public class Healer : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        DuskManager.instance.HealParty();
    }
}
