using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public PlayerResourceSave playerResource;
    //public SaveBase saveBase;

    public void LoadData()
    {
        //saveBase.LoadData();
        playerResource.LoadData();
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
