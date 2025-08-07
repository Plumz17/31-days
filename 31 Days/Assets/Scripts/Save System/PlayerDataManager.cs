using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager instance;
    public PlayerData currentData;
    public bool loadedFromSave = false;
    public CharacterData protagCharacterData;

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

        currentData.playerConnections.Add(new ConnectionSaveData("Bagas", 0));
        currentData.playerConnections.Add(new ConnectionSaveData("Cass", 0));
        currentData.playerConnections.Add(new ConnectionSaveData("Klara", 0));

        AddPartyMember(protagCharacterData);

        //Reset calendar to day 1
        CalenderAndObjectiveManager.instance.LoadFromPlayerData(PlayerDataManager.instance.currentData);
    }

    public void IncreaseConnection(string name)
    {
        var connection = currentData.playerConnections.Find(l => l.name == name);
        if (connection != null && connection.level < 5)
        {
            connection.level++;
        }
    }

    public void AddPartyMember(CharacterData newMember)
    {
        if (!PlayerDataManager.instance.currentData.partyMembers.Contains(newMember))
        {
            PlayerDataManager.instance.currentData.partyMembers.Add(newMember);
        }
        else
        {
            Debug.Log("Member already exist");
        }
    }
}
