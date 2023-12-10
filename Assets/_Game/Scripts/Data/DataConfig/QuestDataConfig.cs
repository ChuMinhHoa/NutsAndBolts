using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestDataConfig", menuName = "ScriptableObject/QuestDataConfig")]
public class QuestDataConfig : ScriptableObject 
{
    public List<QuestData> questData = new List<QuestData>();
    public List<DailyReward> dailyRewards = new List<DailyReward>();
#if UNITY_EDITOR
    private void OnEnable()
    {
        for (int i = 0; i < questData.Count; i++)
        {
            questData[i].questName = GetNameQuest(questData[i].questType);
        }
    }
#endif
    string GetNameQuest(QuestType qType) {
        switch (qType)
        {
            case QuestType.UsingUndo:
                return "Using Undo";
            case QuestType.UsingAddBulong:
                return "Using Add Bulong";
            case QuestType.CompleteLevel:
                return "Complete Level";
            case QuestType.CompleteLevelSecret:
                return "Complete Level Secret";
            case QuestType.FinishRace:
                return "Finish Race";
            case QuestType.Ranking:
                return "Earn rank";  
            case QuestType.WatchADS:
                return "Watch ADS";
            default:
                return "";
        }
    }
}

[System.Serializable]
public class QuestData {
    public string questName;
    public int questRequirebase;
    public QuestType questType;
    public int questStarEarn;
}

[System.Serializable]
public class DailyReward {
    public ItemType itemType;
    public float amount;
    public int pointGet;
}

public enum QuestType
{
    UsingUndo,
    UsingAddBulong,
    CompleteLevel,
    CompleteLevelSecret,
    FinishRace,
    Ranking,
    WatchADS
}