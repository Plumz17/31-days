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

    public void StartNewGame() //Run this when starting a new game
    {
        currentData = new PlayerData();

        currentData.playerConnections.Add(new ConnectionSaveData("Adachi", 0)); // Add all characters

        //Reset calendar to day 1
        CalenderManager.instance.LoadFromPlayerData(PlayerDataManager.instance.currentData);
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
