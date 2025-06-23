using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class ScreenChange : MonoBehaviour
{
    [Header("Transition")]
    [SerializeField] private Transform positionToGo;
    [SerializeField] private Animator anim;
    [SerializeField] private float transitionTime = 0.2f;

    [Header("Cinemachine Confiner Switch")]
    [SerializeField] private CinemachineCamera roomACam;
    [SerializeField] private CinemachineCamera roomBCam;
    
    private bool isTransitioning = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isTransitioning && collision.CompareTag("Player"))
        {
            StartCoroutine(ChangeScreen(collision.gameObject));
        }
    }
    

    IEnumerator ChangeScreen(GameObject player)
    {
        isTransitioning = true;

        if (anim != null)
            anim.SetBool("Start", true);

        yield return new WaitForSeconds(transitionTime);

        player.transform.position = positionToGo.position;

        yield return new WaitForSeconds(transitionTime);

        if (anim != null)
            anim.SetBool("Start", false);

        isTransitioning = false;
    }
}
