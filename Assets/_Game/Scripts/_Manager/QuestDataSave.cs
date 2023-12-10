using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class QuestDataSave : SaveBase
{
    public string timeOnQuest;
    public int starsEarned;
    public int rewardEarned;
    public List<QuestSave> questSaves = new List<QuestSave>();

    DateTime dateTimeOnQuest;
    public override void LoadData()
    {
        SetStringSave("QuestDataSave");
        string json = GetJsonData();
        if (!string.IsNullOrEmpty(json))
        {
            QuestDataSave data = JsonUtility.FromJson<QuestDataSave>(json);
            questSaves = data.questSaves;
            timeOnQuest = data.timeOnQuest;
            starsEarned = data.starsEarned;
            rewardEarned = data.rewardEarned;
            InitData();
        }
        else {
            ResetTime();
        }
    }

    void ResetTime() {
        timeOnQuest = DateTime.Now.ToString();
        questSaves.Clear();
        starsEarned = 0;
        rewardEarned = 0;
        IsMarkChangeData();
        SaveData();
    }

    public void InitData()
    {
        DateTime.TryParse(timeOnQuest, out dateTimeOnQuest);
        if (DateTime.Now.Day > dateTimeOnQuest.Day)
            ResetTime();
    }

    TimeSpan timeReturn;

    public double GetTimeCoolDown() {
        DateTime timeEndDay = DateTime.Today.AddDays(1);
        timeReturn = timeEndDay.Subtract(DateTime.Now);
        if (timeReturn > TimeSpan.Zero)
            return timeReturn.TotalSeconds;
        else
            return 0;
    }

    public int GetStarEarned() { return starsEarned; }

    public void GetReward() {
        rewardEarned++;
        IsMarkChangeData();
        SaveData();
    }

    public bool CheckCanEarnQuest(int questIndex) {
        return questIndex > rewardEarned;
    }
}

[System.Serializable]
public class QuestSave {
    public QuestType questType;
    public float progress;
}
