using UnityEngine;
using System.Collections;

public class AttackButton : MonoBehaviour
{
    public BaseAttack skillAttack; // Reference to the BaseAttack scriptable object

    public void CastSkill()
    {
        GameObject.Find("BattleManager").GetComponent<StateMachineBattle>().Input4(skillAttack);
    }
}
