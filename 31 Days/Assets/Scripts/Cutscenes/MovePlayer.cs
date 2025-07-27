using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public Transform deskTarget;

    public void StartIntroWalk()
    {
        StartCoroutine(playerMovement.WalkTo(deskTarget.position, OnArrivedAtDesk));
    }

    void OnArrivedAtDesk()
    {

    }
}
