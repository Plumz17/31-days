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

    public void SavePartyData(List<Unit> partyUnits)
    {
        partyData.Clear();
        for (int i = 0; i < partyUnits.Count; i++)
        {
            Debug.Log("Before: " + partyUnits[i].currentHP);
            int currentHP = partyUnits[i].currentHP;
            int currentWILL = partyUnits[i].currentWILL;
            partyData.Add((CharacterData)partyUnits[i].data);
            partyUnits[i].data.currentHP = currentHP;
            partyUnits[i].data.currentHP = currentWILL;
            Debug.Log("After: " + partyData[i].currentHP);
        }
        Debug.Log("Party Saved!");
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

