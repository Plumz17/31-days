using UnityEngine;
using System.Collections;

[System.Serializable]
public class HandleTurn
{
    public string Attacker; // name of attacker
    public string Type;
    public GameObject AttackersGameObject; // who attacks
    public GameObject AttackersTarget; // who is going to be attacked

    // which attack is performed (can be added later)
    public BaseAttack choosenAttack; // attack performed
}
