using System.Collections.Generic;
using System.Data;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public int day = 4;
    public int month = 8;
    public int time = 0;
    public int totalDaysPassed = 0;

    public int hp = 100;
    public int will = 50;

    public int xp = 0;
    public int level = 1;
    public int money = 0;

    public string prevScene = "Cutscene";
    public Vector3 prevPosition = new Vector3(-17, 2.125f, 0);


    // Social Links
    public List<ConnectionSaveData> playerConnections = new List<ConnectionSaveData>();
    public List<CharacterData> partyMembers = new List<CharacterData>();

    // You can also add flags or quest progress here
    //public List<string> unlockedEvents = new List<string>();
}
