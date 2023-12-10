using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public PlayerResourceSave playerResource;
    public LevelRaceSave levelRaceSave;
    //public SaveBase saveBase;

    public void LoadData()
    {
        //saveBase.LoadData();
        playerResource.LoadData();
        levelRaceSave.LoadData();
    }

    public void SaveData()
    {
        //saveBase.SaveData();
    }

    public void Update()
    {
        //saveBase.Update();
    }
}
