using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DuskManager : MonoBehaviour
{
    [SerializeField] private List<string> duskSceneNames = new List<string> { "Dusk Classroom", "Dusk Zone", "RPG Mockup" };
    [SerializeField] private List<CharacterSaveData> partyData = new List<CharacterSaveData>();
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

    public void SaveParty(List<Unit> playerUnits)
    {
        partyData.Clear();

        foreach (var unit in playerUnits)
        {
            if (unit.isPlayer)
            {
                partyData.Add(new CharacterSaveData
                {
                    characterID = unit.data.unitName,
                    currentHP = unit.currentHP,
                    currentWILL = unit.currentWILL
                });
            }
        }
    }

    public void LoadParty(List<Unit> playerUnits)
    {
        foreach (var unit in playerUnits)
        {
            if (!unit.isPlayer) continue;

            var savedData = partyData.Find(x => x.characterID == unit.data.unitName);
            if (savedData != null)
            {
                unit.ForceSetStats(savedData.currentHP, savedData.currentWILL);
            }
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

