[System.Serializable]
public class ConnectionSaveData
{
    public string name;
    public int level;    
    
    public ConnectionSaveData(string name, int level)
    {
        this.name = name;
        this.level = level;
    }
}