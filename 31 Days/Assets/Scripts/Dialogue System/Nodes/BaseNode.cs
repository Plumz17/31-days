using UnityEngine;
using UnityEngine.UI;

public abstract class BaseNode : ScriptableObject
{
    public string characterName;
    public Sprite characterPortrait;
    public bool levelUpFlag;
    public int advanceTimeFlag;
    public bool saveDataFlag;
    public bool turnFlag;
    public string objectiveFlag;
    public CharacterData newMemberFlag;
    public TransitionData transitionFlag;
    public bool onlyPlayedOnce = true;
}
