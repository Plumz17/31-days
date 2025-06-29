using System.Data.Common;
using System.IO;
using UnityEngine;

public static class SaveData
{
    private static string path = Application.persistentDataPath + "/playerdata.json";

    public static void Save()
    {
        string json = JsonUtility.ToJson(PlayerDataManager.instance.currentData);
        File.WriteAllText(path, json);
        Debug.Log("Saved in: " + path);
    }

    public static void Load()
    {
        if (!File.Exists(path))
        {
            Debug.LogWarning("No save file found.");
            PlayerDataManager.instance.currentData = new PlayerData();
            return;
        }

        string json = File.ReadAllText(path);
        PlayerDataManager.instance.currentData = JsonUtility.FromJson<PlayerData>(json);
        return;
    }

    public static void ResetToDefault()
    {
        PlayerDataManager.instance.currentData = new PlayerData();
        Debug.Log("Player data reset to default.");

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file deleted at: " + path);
        }
    }
}
