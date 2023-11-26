using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerResourceSave : SaveBase
{
    public bool isCheatADS;
    public int playerLevel;
    public int stateOfLevel;
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
        }
        else {
            playerLevel = 0;
            stateOfLevel = 0;
            isCheatADS = false;
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
}
