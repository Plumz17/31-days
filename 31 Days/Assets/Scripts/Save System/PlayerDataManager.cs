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

    public void StartNewGame()
    {
        currentData = new PlayerData();

        currentData.playerConnections.Add(new ConnectionSaveData("Adachi", 0)); // Add all characters
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
    
    public void IncreaseConnection(string name)
    {
        var connection = currentData.playerConnections.Find(l => l.name == name);
        if (connection != null && connection.level < 5)
        {
            connection.level++;
        }
    }
}
