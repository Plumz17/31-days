using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class StateMachineBattle : MonoBehaviour
{
    public enum PerformAction
    {
        WAIT,
        TAKEACTION,
        PERFORMACTION
    }

    public PerformAction battleStates;

    // Use this for initialization
    public List<HandleTurn> PerformList = new List<HandleTurn>();

    public List<GameObject> PlayersInBattle = new List<GameObject>();
    public List<GameObject> EnemysInBattle = new List<GameObject>();

    public enum PlayerGUI
    {
        ACTIVATE,
        WAITING,
        INPUT1,
        INPUT2,
        DONE
    }

    public PlayerGUI PlayerGUIState;

    public List<GameObject> PlayerToManage = new List<GameObject>();
    private HandleTurn PlayerAction;

    public GameObject EnemyButton; // Prefab for enemy buttons
    public Transform Spacer;
    // Use this for initialization
    void Start()
    {
        battleStates = PerformAction.WAIT;
        EnemysInBattle.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        PlayersInBattle.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        EnemyButtons();
    }


    // Update is called once per frame
    void Update()
    {
        switch (battleStates)
        {
            case PerformAction.WAIT:
                if (PerformList.Count > 0)
                {
                    battleStates = PerformAction.TAKEACTION;
                }
                break;

            case PerformAction.TAKEACTION:
                // Use the stored GameObject reference instead of searching by name
                GameObject performer = PerformList[0].AttackersGameObject;

                // Add null check for safety
                if (performer == null)
                {
                    Debug.LogError("Performer GameObject is null!");
                    PerformList.RemoveAt(0); // Remove the invalid action
                    battleStates = PerformAction.WAIT;
                    break;
                }

                if (PerformList[0].Type == "Enemy")
                {
                    StateMachineEnemy ESM = performer.GetComponent<StateMachineEnemy>();
                    if (ESM != null)
                    {
                        ESM.PlayerToAttack = PerformList[0].AttackersTarget;
                        ESM.currentState = StateMachineEnemy.TurnState.ACTION;
                    }
                    else
                    {
                        Debug.LogError("StateMachineEnemy component not found on " + performer.name);
                    }
                }

                if (PerformList[0].Type == "Player")
                {
                    // Player logic here
                }

                battleStates = PerformAction.PERFORMACTION;
                break;

            case PerformAction.PERFORMACTION:
                break;
        }
    }
    public void CollectActions(HandleTurn input)
    {
        PerformList.Add(input);
    }

    void EnemyButtons()
    {
        foreach (GameObject enemy in EnemysInBattle)
        {
            GameObject newButton = Instantiate(EnemyButton) as GameObject;
            EnemySelectButton button = newButton.GetComponent<EnemySelectButton>();

            if (button == null)
            {
                Debug.LogError("EnemySelectButton component not found on EnemyButton prefab.");
                Destroy(newButton);
                continue;
            }

            StateMachineEnemy cur_enemy = enemy.GetComponent<StateMachineEnemy>();
            // Fixed null check logic
            if (cur_enemy == null)
            {
                Debug.LogError("StateMachineEnemy component not found on: " + enemy.name);
                Destroy(newButton); // Clean up the button we just created
                continue;
            }
            
            if (cur_enemy.enemy == null)
            {
                Debug.LogError("BaseEnemy component is null on: " + enemy.name);
                Destroy(newButton); // Clean up the button we just created
                continue;
            }

            Transform textTransform = newButton.transform.Find("Text");
            if (textTransform == null)
            {
                Debug.LogError("Text child not found in EnemyButton prefab.");
                Destroy(newButton); // Clean up the button we just created
                continue;
            }

            Text buttonText = textTransform.GetComponent<Text>();
            if (buttonText == null)
            {
                Debug.LogError("Text component not found on Text GameObject in EnemyButton prefab.");
                Destroy(newButton); // Clean up the button we just created
                continue;
            }

            buttonText.text = cur_enemy.enemy.enemyName;
            button.EnemyPrefab = enemy;
            
            // Set the parent to organize the UI
            newButton.transform.SetParent(Spacer, false);
        }
    }
}
