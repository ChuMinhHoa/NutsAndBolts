using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelDataConfig", menuName = "ScriptableObject/LevelDataConfig")]
public class LevelDataConfig : ScriptableObject
{
    public TextAsset csv;
    public List<LevelData> levelDatas= new List<LevelData>();
    public LevelData levelDataEasy;
    private void OnEnable()
    {
        levelDatas.Clear();
        List<Dictionary<string, string>> datas = CSVReader.Read(csv);
        for (int i = 0; i < datas.Count; i++)
        {
            LevelData levelData = new LevelData();
            levelData.Level = int.Parse(datas[i]["Level"]);
            levelData.TotalLine = int.Parse(datas[i]["TotalLine"]);
            levelData.TotalBulong = int.Parse(datas[i]["TotalBulong"]);
            levelData.CountMaxInLine = int.Parse(datas[i]["CountMaxInLine"]);
            levelData.BulongFree = int.Parse(datas[i]["BulongFree"]);
            levelData.TotalOcVit = int.Parse(datas[i]["OcVitCount"]);
            levelDatas.Add(levelData);
        }
       
    }

    public LevelData GetLevelData(int level) {
        return levelDatas[level];
    }

    public LevelData GetLevelEasy() { return levelDataEasy; }
}

[System.Serializable]
public class LevelData {
    public int Level;
    public int TotalLine;
    public int TotalBulong;
    public int CountMaxInLine;
    public int BulongFree;
    public int TotalOcVit;
}
