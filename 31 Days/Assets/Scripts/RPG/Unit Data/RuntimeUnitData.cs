[System.Serializable]
public class RuntimeUnitData
{
    public CharacterData baseData;
    public int currentHP;
    public int currentWILL;

    public RuntimeUnitData(CharacterData data)
    {
        baseData = data;
        currentHP = data.maxHP;
        currentWILL = data.maxWILL;
    }
}
