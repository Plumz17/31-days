using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class DuskManager : MonoBehaviour
{
    [SerializeField] private List<string> duskSceneNames = new List<string> {"Dusk Zone", "RPG", "Dusk Classroom"};
    [SerializeField] private List<CharacterData> partyData = new List<CharacterData>();
    public static DuskManager instance;

    private HashSet<string> defeatedEnemies = new HashSet<string>();
    public Vector2 currentLocation = new Vector2();

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
            PauseMenu.instance.SetCanSave(true);
            CalenderAndObjectiveManager.instance.UpdateCalenderUI();
            defeatedEnemies.Clear();
            Destroy(gameObject);
        }
        else
        {
            partyData.Clear();
            foreach (var charData in PlayerDataManager.instance.currentData.partyMembers)
            {
                Debug.Log(charData.name);
            }

            foreach (var charData in PlayerDataManager.instance.currentData.partyMembers)
            {
                partyData.Add(Instantiate(charData));
            }
            PauseMenu.instance.SetCanSave(false);
            CalenderAndObjectiveManager.instance.timeOfDayText.text = "Dusk";
        }
    }
    
    public bool IsInDuskZone()
    {
        return duskSceneNames.Contains(SceneManager.GetActiveScene().name);
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
            partyData[i].savedHP = updatedHP;
            partyData[i].savedWILL = updatedWILL; // Note: currentHP in CharacterData and Unit are different
        }
    }

    public void HealParty()
    {
        for (int i = 0; i < partyData.Count; i++)
        {
            partyData[i].savedHP = partyData[i].maxHP;
            partyData[i].savedWILL = partyData[i].maxWILL;
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
        if (currentEncounter == null || currentEncounter.enemyPool.Count == 0)
            return new List<EnemyData>();

        List<EnemyData> selectedEnemies = new List<EnemyData>();
        List<EnemyData> pool = currentEncounter.enemyPool;
        int count = currentEncounter.enemyCount;

        List<EnemyData> weightedPool = new List<EnemyData>();
        foreach (var enemy in pool)
        {
            int weight = enemy.rarity;
            for (int i = 0; i < weight; i++)
            {
                weightedPool.Add(enemy);
            }
        }

        for (int i = 0; i < count; i++)
        {
            if (weightedPool.Count == 0) break;
            EnemyData chosen = weightedPool[Random.Range(0, weightedPool.Count)];
            selectedEnemies.Add(chosen);
        }

        return selectedEnemies;
    }
}

