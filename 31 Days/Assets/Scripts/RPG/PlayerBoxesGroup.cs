using UnityEngine;
using System.Collections.Generic;

public class PlayerBoxesGroup : MonoBehaviour
{
    [Header("UI Boxes for Each Character")]
    [SerializeField] private List<GameObject> characterBoxes;
    [SerializeField] private List<CharacterData> playerDataList;

    public void SetupPartyUI(List<Unit> partyUnits)
    {
        playerDataList = DuskManager.instance.LoadPartyData();

        for (int i = 0; i < characterBoxes.Count; i++)
        {
            if (i < playerDataList.Count)
            {
                GameObject characterBox = characterBoxes[i];
                characterBox.SetActive(true);

                PlayerUI uiBox = characterBoxes[i].GetComponent<PlayerUI>();
                Unit unit = characterBox.GetComponent<Unit>();
                int loadedHP = playerDataList[i].savedHP;
                int loadedWILL = playerDataList[i].savedWILL;
                unit.Init(playerDataList[i]);
                unit.SetStats(loadedHP, loadedWILL);
                partyUnits.Add(unit);
                if (uiBox != null)
                {
                    uiBox.UpdateUI(partyUnits[i]);
                }
            }
            else
            {
                characterBoxes[i].SetActive(false);
            }
        }
    }

    public void UpdatePartyUI(List<Unit> partyUnits)
    {
        for (int i = 0; i < characterBoxes.Count; i++)
        {
            if (i < partyUnits.Count)
            {
                GameObject characterBox = characterBoxes[i];
                characterBox.SetActive(true);

                PlayerUI uiBox = characterBoxes[i].GetComponent<PlayerUI>();
                uiBox?.UpdateUI(partyUnits[i]);
            }
            else
            {
                characterBoxes[i].SetActive(false);
            }
        }
    }
}
