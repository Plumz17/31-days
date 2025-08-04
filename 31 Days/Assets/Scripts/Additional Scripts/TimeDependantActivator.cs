using UnityEngine;

public class TimeDependantActivator : MonoBehaviour
{
    public CalenderAndObjectiveManager.timeOfDay activateAt = CalenderAndObjectiveManager.timeOfDay.Night;

    void Start()
    {
        UpdateActiveState();
    }

    void UpdateActiveState()
    {
        if (CalenderAndObjectiveManager.instance != null)
        {
            gameObject.SetActive(CalenderAndObjectiveManager.instance.currentTimeOfDay == activateAt);
        }
    }

    // Optional: expose method to call when time changes
    public void Refresh()
    {
        UpdateActiveState();
    }
}