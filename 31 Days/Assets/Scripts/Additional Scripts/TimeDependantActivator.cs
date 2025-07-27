using UnityEngine;

public class TimeDependantActivator : MonoBehaviour
{
    public CalenderManager.timeOfDay activateAt = CalenderManager.timeOfDay.Malam;

    void Start()
    {
        UpdateActiveState();
    }

    void UpdateActiveState()
    {
        if (CalenderManager.instance != null)
        {
            gameObject.SetActive(CalenderManager.instance.currentTimeOfDay == activateAt);
        }
    }

    // Optional: expose method to call when time changes
    public void Refresh()
    {
        UpdateActiveState();
    }
}