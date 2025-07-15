using UnityEngine;
using UnityEngine.UI;

public abstract class BaseNode : ScriptableObject
{
    public string characterName;
    public Sprite characterPortrait;
    public bool levelUpFlag;
    public int advanceTimeFlag;
}
