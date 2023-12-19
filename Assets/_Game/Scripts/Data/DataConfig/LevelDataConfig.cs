using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "LevelDataConfig", menuName = "ScriptableObject/LevelDataConfig")]
public class LevelDataConfig : ScriptableObject
{
    public TextAsset csv;
    public List<LevelData> levelDatas= new List<LevelData>();
    public List<LevelData> levelDataEasys = new List<LevelData>();
    public LevelData level1Data;
    public int totalLevelEasy;
    private void OnEnable()
    {
        levelDatas.Clear();
        levelDataEasys.Clear();

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


        for (int i = 0; i < totalLevelEasy; i++)
        {
            LevelData levelData = new LevelData(); 
            levelData.Level = i;
            levelData.TotalLine = Random.Range(2, 4);
            levelData.CountMaxInLine = Random.Range(3, 4);
            levelData.TotalBulong = levelData.TotalLine * levelData.CountMaxInLine - 1;
            levelData.BulongFree = levelData.TotalBulong > 4 ? 2 : 1;
            levelData.TotalOcVit = 2;
            levelDataEasys.Add(levelData);
        }
    }

    public LevelData GetLevelData(int level) {
        return levelDatas[level];
    }

    public LevelData GetLevelEasy() { return levelDataEasys[Random.Range(0, levelDataEasys.Count)]; }
    public LevelData GetLevel1Data() { return level1Data; }
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
