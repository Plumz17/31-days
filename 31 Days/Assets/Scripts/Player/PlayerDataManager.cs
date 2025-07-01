using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager instance;
    public PlayerData currentData;
    public bool loadedFromSave = false;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        currentData = new PlayerData();
    }

    public void AdvanceTime(int timePassed) //Number of time of day that pass
    {
        currentData.time = currentData.time + timePassed;
        if (currentData.time > 2)
        {
            int dayPassed = (currentData.time / 3);
            currentData.time = currentData.time % 3;
            currentData.day += dayPassed;
        }

    }
}
