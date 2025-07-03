using TMPro;
using UnityEngine;

public class CalenderManager : MonoBehaviour
{
    public static CalenderManager instance;

    [Header("References")]
    public TMP_Text dateText;
    public TMP_Text dayText;
    public TMP_Text timeOfDayText;
    public GameObject calendarUIPanel;

    public int currentMonth = 8;
    public int currentDay = 4;
    public enum timeOfDay { Pagi = 0, Siang = 1, Sore = 2, Malam = 3, Dusk = -1 }
    public timeOfDay currentTimeOfDay = timeOfDay.Pagi;
    public int totalDaysPassed = 0;

    private int daysInMonth = 31;
    private string[] dayNames = { "Sen", "Sel", "Rab", "Kam", "Jum", "Sab", "Mng" };


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
        dateText.text = $"{currentDay}/{currentMonth}";          // e.g. 8/4
        dayText.text = dayNames[totalDaysPassed % 7];               // e.g. Monday
        timeOfDayText.text = currentTimeOfDay.ToString();         // e.g. Morning
    }

    public void SaveToPlayerData(PlayerData data)
    {
        data.day = currentDay;
        data.month = currentMonth;
        data.time = (int)currentTimeOfDay;
        data.totalDaysPassed = totalDaysPassed;
    }

    public void LoadFromPlayerData(PlayerData data)
    {
        currentDay = data.day;
        currentMonth = data.month;
        currentTimeOfDay = (timeOfDay)data.time;
        totalDaysPassed = data.totalDaysPassed;
    }
    
    public void SetCalendarUIActive(bool isActive)
    {
        if (calendarUIPanel != null)
        {
            calendarUIPanel.SetActive(isActive);
        }
    }
}
