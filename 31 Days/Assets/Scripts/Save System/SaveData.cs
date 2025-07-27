using System.Data.Common;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SaveData
{
    private static string path = Application.persistentDataPath + "/playerdata.json";

    public static void Save()
    {
        SavePrevScene();

        CalenderManager.instance.SaveToPlayerData(PlayerDataManager.instance.currentData); //Save Calendar State
        CalenderManager.instance.UpdateCalenderUI();

        string json = JsonUtility.ToJson(PlayerDataManager.instance.currentData);
        File.WriteAllText(path, json);
        
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

        StoryManager.instance.LoadCutscenesFromPlayerData();

        LoadPrevScene();

        CalenderManager.instance.LoadFromPlayerData(PlayerDataManager.instance.currentData); //Save Calendar State
        CalenderManager.instance.UpdateCalenderUI();
    }

    public static void ResetToDefault()
    {
        PlayerDataManager.instance.StartNewGame();
        Debug.Log("Player data reset to default.");

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save file deleted at: " + path);
        }

        Save();
    }

    public static void SavePrevScene()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            PlayerDataManager.instance.currentData.prevScene = SceneManager.GetActiveScene().name;
            PlayerDataManager.instance.currentData.prevPosition = player.transform.position;
        }
    }

    public static void LoadPrevScene()
    {
        PlayerDataManager.instance.loadedFromSave = true;
        string sceneToLoad = PlayerDataManager.instance.currentData.prevScene;
        SceneManager.LoadScene(sceneToLoad);
    }
}
