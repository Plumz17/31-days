using UnityEngine;
using System.Collections.Generic;
using TMPro;

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
    private HandleTurn PlayerChoice;

    public GameObject EnemyButton; // Prefab for enemy buttons
    public Transform Spacer;

    public GameObject AttackPanel;
    public GameObject EnemySelectPanel;
    // Use this for initialization
    void Start()
    {
        battleStates = PerformAction.WAIT;
        EnemysInBattle.AddRange(GameObject.FindGameObjectsWithTag("RPGEnemy"));
        PlayersInBattle.AddRange(GameObject.FindGameObjectsWithTag("RPGPlayer"));
        PlayerGUIState = PlayerGUI.ACTIVATE;

        AttackPanel.SetActive(false);
        EnemySelectPanel.SetActive(false);

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
                        for (int i = 0; i < PlayersInBattle.Count; i++)
                        {
                            if (PerformList[0].AttackersTarget == PlayersInBattle[i])
                            {
                                ESM.PlayerToAttack = PerformList[0].AttackersTarget;
                                ESM.currentState = StateMachineEnemy.TurnState.ACTION;
                                break;
                            }
                            else
                            {
                                PerformList[0].AttackersTarget = PlayersInBattle[Random.Range(0, PlayersInBattle.Count)];
                                ESM.PlayerToAttack = PerformList[0].AttackersTarget;
                                ESM.currentState = StateMachineEnemy.TurnState.ACTION;
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("StateMachineEnemy component not found on " + performer.name);
                    }
                }

                if (PerformList[0].Type == "Player")
                {
                    StateMachinePlayer HSM = performer.GetComponent<StateMachinePlayer>();
                    HSM.EnemyToAttack = PerformList[0].AttackersTarget;
                    HSM.currentState = StateMachinePlayer.TurnState.ACTION;
                }

                battleStates = PerformAction.PERFORMACTION;
                break;

            case PerformAction.PERFORMACTION:
                break;
        }

        switch (PlayerGUIState)
        {
            case PlayerGUI.ACTIVATE:
                if (PlayerToManage.Count > 0)
                {
                    PlayerToManage[0].transform.Find("Selector").gameObject.SetActive(true);
                    PlayerChoice = new HandleTurn();

                    AttackPanel.SetActive(true);
                    PlayerGUIState = PlayerGUI.WAITING;
                }
                break;
            case PlayerGUI.WAITING:
                // Activate player GUI logic here
                break;
            case PlayerGUI.DONE:
                PlayerInputDone();
                // Activate player GUI logic here
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

            TMP_Text buttonText = textTransform.GetComponent<TMP_Text>();
            if (buttonText == null)
            {
                Debug.LogError("Text component not found on Text GameObject in EnemyButton prefab.");
                Destroy(newButton); // Clean up the button we just created
                continue;
            }

            buttonText.text = cur_enemy.enemy.theName;
            button.EnemyPrefab = enemy;

            // Set the parent to organize the UI
            // Set the parent to organize the UI
            newButton.transform.SetParent(Spacer, false);
        }
    }

    public void Input1()
    {
        if (PlayerToManage.Count > 0)
        {
            PlayerChoice.Attacker = PlayerToManage[0].name;
            PlayerChoice.AttackersGameObject = PlayerToManage[0];
            PlayerChoice.Type = "Player";

            AttackPanel.SetActive(false);
            EnemySelectPanel.SetActive(true);
        }
    }
    public void Input2(GameObject chosenEnemy)
    {
        PlayerChoice.AttackersTarget = chosenEnemy;
        PlayerGUIState = PlayerGUI.DONE;
    }

    void PlayerInputDone()
    {
        PerformList.Add(PlayerChoice);
        EnemySelectPanel.SetActive(false);
        PlayerToManage[0].transform.Find("Selector").gameObject.SetActive(false);
        PlayerToManage.RemoveAt(0);
        PlayerGUIState = PlayerGUI.ACTIVATE;
    }
}