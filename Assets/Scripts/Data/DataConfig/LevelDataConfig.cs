using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelDataConfig", menuName = "ScriptableObject/LevelDataConfig")]
public class LevelDataConfig : ScriptableObject
{
    public TextAsset csv;
    public List<LevelData> levelDatas= new List<LevelData>();
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
            levelDatas.Add(levelData);
        }
       
    }

    public LevelData GetLevelData(int level) {
        return levelDatas[level];
    }
}

[System.Serializable]
public class LevelData {
    public int Level;
    public int TotalLine;
    public int TotalBulong;
    public int CountMaxInLine;
    public int BulongFree;
}
