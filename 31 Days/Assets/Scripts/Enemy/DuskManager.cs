using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DuskManager : MonoBehaviour
{
    [SerializeField] private List<string> duskSceneNames = new List<string> {"Dusk Zone", "RPG"};
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
            PauseMenu.instance.SetCanPause(true);
            defeatedEnemies.Clear();
            Destroy(gameObject);
        }
        else
        {
            partyData.Clear();
            foreach (var charData in PlayerDataManager.instance.currentData.partyMembers)
            {
                partyData.Add(Instantiate(charData));
            }
            PauseMenu.instance.SetCanPause(false);
        }
    }


    public List<CharacterData> LoadPartyData()
    {
        Debug.Log(partyData.Count);
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
            Debug.Log("Healed " + partyData[i].unitName);
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

