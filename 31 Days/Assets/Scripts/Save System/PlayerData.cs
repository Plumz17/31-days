using System.Collections.Generic;
using System.Data;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public int day = 0;
    public int time = 0; //0: morning, 1: afternoon, 2: night, -1: dusk

    public int hp = 100;
    public int will = 50;

    public int xp = 0;
    public int level = 1;
    public int money = 0;

    public string prevScene = "Room";
    public Vector3 prevPosition = new Vector3(-17, 2.125f, 0);


    // Social Links
    public List<ConnectionSaveData> playerConnections = new List<ConnectionSaveData>();

    // You can also add flags or quest progress here
    //public List<string> unlockedEvents = new List<string>();
}
