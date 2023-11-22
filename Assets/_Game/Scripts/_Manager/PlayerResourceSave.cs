using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlayerResourceSave : SaveBase
{
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
        }
        else {
            playerLevel = 0;
            stateOfLevel = 0;
            IsMarkChangeData();
            SaveData();
        }
    }

    public void LevelUp(int level) {
        playerLevel = level;
        stateOfLevel = 0;
        IsMarkChangeData();
        SaveData();
    }

    public void ChangeState(int state) {
        stateOfLevel = state;
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
