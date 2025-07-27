using UnityEngine;
using System.Collections;

public class MovePlayerCutscene : MonoBehaviour
{
    public PlayerMovement player;
    public Transform targetTransform;
    public ItemPickUp itemToOpen; 
    public float stopDistance = 0.1f;

    private void Start()
    {
        StartCoroutine(StartCutscene());
    }

    private IEnumerator StartCutscene()
    {
        yield return new WaitForSeconds(1f); // Optional delay

        if (targetTransform != null)
        {
            player.WalkToPosition(targetTransform.position, stopDistance);
        }
        else
        {
            Debug.LogWarning("Target Transform is not assigned.");
            yield break;
        }

        // Wait until player can move again (cutscene done)
        while (Vector2.Distance(player.transform.position, targetTransform.position) > stopDistance)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        // Trigger item interaction
        if (itemToOpen != null)
        {
            itemToOpen.ForceOpenItem();
        }

        SaveData.Save();
        // Do next cutscene action, e.g. dialogue or animation
    }
}
