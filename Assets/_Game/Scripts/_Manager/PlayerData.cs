using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public SaveBase saveBase;

    public void LoadData()
    {
        saveBase.LoadData();
    }

    public void SaveData()
    {
        saveBase.SaveData();
    }

    public void Update()
    {
        saveBase.Update();
    }
}
