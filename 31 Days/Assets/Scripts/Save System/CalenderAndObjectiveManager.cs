using TMPro;
using UnityEngine;

public class CalenderAndObjectiveManager : MonoBehaviour
{
    public static CalenderAndObjectiveManager instance;

    [Header("References")]
    [SerializeField] private TMP_Text dateText;
    [SerializeField] private TMP_Text dayText;
    public TMP_Text timeOfDayText;
    [SerializeField] private GameObject UIPanel;
    [SerializeField] private TMP_Text objectiveText;

    public int currentMonth = 8;
    public int currentDay = 4;
    public enum timeOfDay { Morn = 0, Noon = 1, Even = 2, Night = 3}
    public timeOfDay currentTimeOfDay = timeOfDay.Morn;
    private int totalDaysPassed = 0;
    private bool calIsActive = true;

    public string currentObjective;

    private int daysInMonth = 31;
    private string[] dayNames = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AdvanceTimeBlock(int blockAdvanced = 1) // To Move Morning to Afternoon, etc (block = Morning, afternoon, etc.)
    {
        int totalBlocks = (int)currentTimeOfDay + blockAdvanced;
        int dayPassed = totalBlocks / 4;
        int newBlock = totalBlocks % 4;
        currentTimeOfDay = (timeOfDay)newBlock;
        AdvanceDay(dayPassed);

        GameObject[] objs = GameObject.FindGameObjectsWithTag("Time Dependant");
        foreach (GameObject obj in objs)
        {
            TimeDependantActivator activator = obj.GetComponent<TimeDependantActivator>();
            if (activator != null)
                activator.Refresh();
        }
    }

    public void AdvanceDay(int days = 1)
    {
        for (int i = 0; i < days; i++)
        {
            currentDay++;
            totalDaysPassed++;

            if (currentMonth < 1 || currentMonth > 12)
                currentMonth = 1;

            if (currentDay > daysInMonth)
            {
                currentDay = 1;
                currentMonth++;

                if (currentMonth > 12)
                    currentMonth = 1;
            }
        }
        UpdateCalenderUI();
    }

    public void UpdateCalenderUI()
    {
        dateText.text = $"{currentMonth}/{currentDay}";          // e.g. 8/4
        dayText.text = dayNames[totalDaysPassed % 7];               // e.g. Monday
        timeOfDayText.text = currentTimeOfDay.ToString();         // e.g. Morning
    }

    public void UpdateObjectiveUI()
    {
        objectiveText.text = currentObjective;
    }

    public void SaveToPlayerData(PlayerData data)
    {
        data.day = currentDay;
        data.month = currentMonth;
        data.time = (int)currentTimeOfDay;
        data.totalDaysPassed = totalDaysPassed;
        data.objective = currentObjective;
    }

    public void LoadFromPlayerData(PlayerData data)
    {
        currentDay = data.day;
        currentMonth = data.month;
        currentTimeOfDay = (timeOfDay)data.time;
        totalDaysPassed = data.totalDaysPassed;
        currentObjective = data.objective;
    }
    
    public void SetCalendarUI()
    {
        if (UIPanel != null)
        {
            calIsActive = !calIsActive;
            UIPanel.SetActive(calIsActive);
        }
    }
}
