using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorLevelSave : SaveBase
{
    public List<BulongSave> bulongSaves = new List<BulongSave>();
    public LevelData levelData;
    public override void LoadData()
    {
        SetStringSave("ColorLevelSave");
        string jsonData = GetJsonData();
        if (!string.IsNullOrEmpty(jsonData))
        {
            ColorLevelSave data = JsonUtility.FromJson<ColorLevelSave>(jsonData);
            bulongSaves = data.bulongSaves;
            levelData = data.levelData;
        }
        else { 
            IsMarkChangeData();
            SaveData();
        }
    }

    public void SaveColorLevel(List<BuLong> listBulong, LevelData levelData) {
        bulongSaves.Clear();
        this.levelData = levelData;
        for (int i = 0; i < listBulong.Count; i++) {
            if (listBulong[i].gameObject.activeSelf) SaveBulong(listBulong[i]);
        }
        IsMarkChangeData();
        SaveData();
    }

    void SaveBulong(BuLong bulong) { 
        BulongSave newBulongSave = new BulongSave();
        List<OcVit> listOcVit = bulong.ListOcVit;
        for (int i = 0; i < listOcVit.Count; i++) {
            newBulongSave.AddColorID(listOcVit[i].colorID);
        }
        bulongSaves.Add(newBulongSave);
    }

    public List<BulongSave> GetBulongSave() {
        return bulongSaves;
    }

    public List<int> GetColorForBulong(int bulongIndex) { 
        List<int> colorIDs = new List<int>();
        if (bulongIndex >= bulongSaves.Count) { return null; }
        colorIDs = bulongSaves[bulongIndex].colorIDs;
        return colorIDs;
    }
}

[System.Serializable]
public class BulongSave {
    public List<int> colorIDs = new List<int>();
    public void AddColorID(int colorID) { colorIDs.Add(colorID); }
}