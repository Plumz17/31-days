using UnityEngine;
using System.Collections.Generic;

public class PlayerBoxesGroup : MonoBehaviour
{
    [Header("UI Boxes for Each Character")]
    [SerializeField] private List<GameObject> characterBoxes;

    public void SetupPartyUI(List<Unit> activePartyMembers)
    {
        for (int i = 0; i < characterBoxes.Count; i++)
        {
            if (i < PlayerDataManager.instance.currentData.partyStats.Count)
            {
                characterBoxes[i].SetActive(true);
                activePartyMembers[i].data = PlayerDataManager.instance.currentData.partyStats[i];

                PlayerUI uiBox = characterBoxes[i].GetComponent<PlayerUI>();
                if (uiBox != null)
                {
                    uiBox.UpdateUI(activePartyMembers[i]);
                }
            }
            else
            {
                characterBoxes[i].SetActive(false);
            }
        }
    }
}
