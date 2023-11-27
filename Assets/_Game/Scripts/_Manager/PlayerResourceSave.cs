using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerResourceSave : SaveBase
{
    public bool isCheatADS;
    public int playerLevel;
    public int stateOfLevel;

    [Header("===================SETTING===================")]
    public bool soundOn;
    public bool vibrationOn;
    public override void LoadData()
    {
        SetStringSave("PlayerResourceSave");
        base.LoadData();
        string strJsonData = GetJsonData();
        if (!string.IsNullOrEmpty(strJsonData))
        {
            PlayerResourceSave data = JsonUtility.FromJson<PlayerResourceSave>(strJsonData);
            playerLevel = data.playerLevel;
            stateOfLevel = data.stateOfLevel;
            isCheatADS = data.isCheatADS;
            soundOn = data.soundOn;
            vibrationOn = data.vibrationOn;
        }
        else {
            playerLevel = 0;
            stateOfLevel = 0;
            isCheatADS = false;
            ChangeSoundStatus(true);
            ChangeVibrationStatus(true);
            IsMarkChangeData();
            SaveData();
        }
    }

    public bool IsCheatADS() { return isCheatADS; }

    public void LevelUp(int level) {
        playerLevel = level;
        stateOfLevel = 0;
        EventManager.TriggerEvent(EventName.ChangeLevel.ToString());
        EventManager.TriggerEvent(EventName.ChangeState.ToString());
        IsMarkChangeData();
        SaveData();
    }

    public void ChangeState(int state) {
        stateOfLevel = state;
        EventManager.TriggerEvent(EventName.ChangeState.ToString());
        IsMarkChangeData();
        SaveData();
    }

    public int GetState() {
        return stateOfLevel;
    }

    public int GetLevel() {
        return playerLevel;
    }

    public void ChangeSoundStatus(bool onSound)
    {
        soundOn = onSound;
        EventManager.TriggerEvent(EventName.ChangeSoundStatus.ToString(), soundOn);
        IsMarkChangeData();
        SaveData();
    }

    public void ChangeVibrationStatus(bool onVibration)
    {
        vibrationOn = onVibration;
        IsMarkChangeData();
        SaveData();
    }

    public bool GetOnSound() { return soundOn; }

    public bool GetOnVibration() { return vibrationOn; }
}
