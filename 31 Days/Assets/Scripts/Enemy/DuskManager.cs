using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DuskManager : MonoBehaviour
{
    [SerializeField] private List<string> duskSceneNames = new List<string> { "Dusk Classroom", "Dusk Zone", "RPG Mockup" };
    [SerializeField] private List<CharacterData> partyData = new List<CharacterData>();
    public static DuskManager instance;

    private HashSet<string> defeatedEnemies = new HashSet<string>(); //List without dupes and no order

    public Encounter currentEncounter;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (this == null) return;

        if (!duskSceneNames.Contains(scene.name))
        {
            defeatedEnemies.Clear();
            Destroy(gameObject);
        }
    }

    public List<CharacterData> LoadPartyData()
    {
        return partyData;
    }

    public void SavePartyData(List<Unit> newPartyUnits)
    {
        partyData.Clear();
        for (int i = 0; i < newPartyUnits.Count; i++)
        {
            int updatedHP = newPartyUnits[i].currentHP;
            int updatedWILL = newPartyUnits[i].currentWILL;
            partyData.Add((CharacterData)newPartyUnits[i].data);
            partyData[i].currentHP = updatedHP;
            partyData[i].currentWILL = updatedWILL; // Note: currentHP in CharacterData and Unit are different
        }
    }

    public void MarkEnemyAsDefeated(string id)
    {
        defeatedEnemies.Add(id);
    }

    public bool IsEnemyDefeated(string id)
    {
        return defeatedEnemies.Contains(id);
    }

    public void StartEncounter(Encounter encounter, string id)
    {
        MarkEnemyAsDefeated(id);
        currentEncounter = encounter;
        Time.timeScale = 1f;
        LevelLoader.Instance.LoadNextLevel(15, Vector3.zero);
    }

    public List<EnemyData> GetEnemyData()
    {
        return currentEncounter != null ? currentEncounter.enemies : new List<EnemyData>();
    }
}

