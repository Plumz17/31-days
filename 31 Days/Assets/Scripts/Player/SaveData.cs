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

        LoadPrevScene();
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

        Save();
    }

    public static void SavePrevScene()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            PlayerDataManager.instance.currentData.currentScene = SceneManager.GetActiveScene().name;
            PlayerDataManager.instance.currentData.currentPosition = player.transform.position;
        }
    }

    public static void LoadPrevScene()
    {
        PlayerDataManager.instance.loadedFromSave = true;
        string sceneToLoad = PlayerDataManager.instance.currentData.currentScene;
        SceneManager.LoadScene(sceneToLoad);
    }
}
