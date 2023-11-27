using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataConfig
{
    public LevelDataConfig levelDataConfig;
    public MateritalDataConfig materitalDataConfig;

    #region LevelDataConfig
    public LevelData GetLevelData(int level)
    {
        return levelDataConfig.GetLevelData(level);
    }
    public LevelData GetLevelEasy()
    {
        return levelDataConfig.GetLevelEasy();
    }

    public LevelData GetLevel1Data() { return levelDataConfig.GetLevel1Data(); }
    #endregion

    #region MaterialDataConfig
    public List<int> GetRandomMaterial(int totalColor) {
        return materitalDataConfig.GetMaterialRandom(totalColor);
    }
    public MaterialData GetMaterialData(int colorID)
    {
        return materitalDataConfig.GetMaterialData(colorID);
    }
    #endregion
}
