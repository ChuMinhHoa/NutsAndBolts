using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelRaceSave : SaveBase
{
    public List<RacersData> racerDatas;
    public string raceEndMark;
    public bool raceStarted;
    public bool firstRace;
    RacersData playerData;
    public override void LoadData()
    {
        SetStringSave("LevelRaceSave");
        base.LoadData();
        string strJsonData = GetJsonData();
        if (!string.IsNullOrEmpty(strJsonData))
        {
            LevelRaceSave data = JsonUtility.FromJson<LevelRaceSave>(strJsonData);
            racerDatas = data.racerDatas;
            raceStarted = data.raceStarted;
            raceEndMark = data.raceEndMark;
            firstRace = data.firstRace;
        }
        else
        {
            raceStarted = false;
            firstRace = true;
            InitOpponent();
            IsMarkChangeData();
            SaveData();
        }
        playerData = GetPlayer();
        IsMarkChangeData();
        SaveData();
    }

    void InitOpponent()
    {
        racerDatas = new List<RacersData>();
        RacersData player = new RacersData();
        player.player = true;
        player.racerName = "YOU";
        player.levelReached = 0;
        racerDatas.Add(player);

        for (int i = 0; i < 4; i++)
        {
            RacersData opponent = new RacersData();
            opponent.racerName = ConstantValue.defaultName[UnityEngine.Random.Range(0, ConstantValue.defaultName.Count)];
            opponent.levelReached = 0;
            racerDatas.Add(opponent);
        }
    }

    public void StartRace()
    {
        InitOpponent();
        playerData = GetPlayer();
        raceStarted = true;
        DateTime endTime = DateTime.Now.AddHours(1);
        raceEndMark = endTime.ToString();
        IsMarkChangeData();
        SaveData();
    }

    public RacersData GetPlayer()
    {
        for (int i = 0; i < racerDatas.Count; i++)
        {
            if (racerDatas[i].player == true) { return racerDatas[i]; }
        }
        return null;
    }

    public List<RacersData> GetRacerList()
    {
        racerDatas.Sort((x, y) => y.levelReached.CompareTo(x.levelReached));
        InitDataToShow();
        racerDatas.Sort((x, y) => y.levelReached.CompareTo(x.levelReached));
        return racerDatas;
    }

    public void PlayerFinishLevel()
    {
        if (CheckRaceEnd()) return;
        if (playerData != null)
        {
            playerData.levelReached++;
            if(playerData.levelReached >= 30) {
                playerData.levelReached = 30;
            }
        }
        IsMarkChangeData();
        SaveData();
    }

    public bool IsWon()
    {
        racerDatas.Sort((x, y) => y.levelReached.CompareTo(x.levelReached));
        for (int i = 0; i < racerDatas.Count; i++)
        {
            if (racerDatas[i].player)
            {
                return i < 3;
            }
        }
        return false;
    }

    public void GetReward()
    {
        IsMarkChangeData();
        SaveData();
    }

    public bool CheckRaceEnd()
    {
        if (!String.IsNullOrEmpty(raceEndMark))
        {
            DateTime momment = DateTime.Now;
            DateTime boostEndTime = DateTime.Parse(raceEndMark);
            TimeSpan span = boostEndTime.Subtract(momment);
            if (span.TotalSeconds > 0) return false;
            return true;
        }
        return true;
    }

    public float GetRemainTime()
    {
        if (!String.IsNullOrEmpty(raceEndMark))
        {
            DateTime momment = DateTime.Now;
            DateTime boostEndTime = DateTime.Parse(raceEndMark);
            TimeSpan span = boostEndTime.Subtract(momment);
            return (float)span.TotalSeconds;
        }
        return 0f;
    }

    public void EndRace()
    {
        raceStarted = false;
    }

    public void InitDataToShow()
    {
        if (CheckRaceEnd()) return;
        float passedTime = (3600 - GetRemainTime()) / 60;
        int delta = 0;
        for (int i = 0; i < racerDatas.Count; i++)
        {
            if (!racerDatas[i].player) 
            {
                racerDatas[i].levelReached = (int)(passedTime / (1.5 + delta * 1));
                if(racerDatas[i].levelReached >= 30)
                {
                    racerDatas[i].levelReached = 30;
                }
                delta++;
            }
        }
        IsMarkChangeData();
        SaveData();
    }
}

[System.Serializable]
public class RacersData
{
    public bool player;
    public string racerName;
    public int levelReached;
}

