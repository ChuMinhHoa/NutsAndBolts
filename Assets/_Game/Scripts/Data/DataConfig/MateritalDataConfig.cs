using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[CreateAssetMenu(fileName = "MateritalDataConfig", menuName = "ScriptableObject/MateritalDataConfig")]
public class MateritalDataConfig : ScriptableObject
{
    public List<MaterialData> materialDatas = new List<MaterialData>();
    public MaterialData GetMaterialData(int colorID) {
        return materialDatas.Find(e => e.colorID == colorID);
    }
    List<MaterialData> materialDatasTemp = new List<MaterialData>();
    List<int> listID = new List<int>();
    public List<int> GetMaterialRandom(int totalColor) {
        listID.Clear();
        int randomID = -1;
        int randomIndex;
        for (int i = 0; i < totalColor; i++)
        {
            materialDatasTemp = materialDatas.Where(e => !listID.Contains(e.colorID)).ToList();
            randomIndex = Random.Range(0, materialDatasTemp.Count);
            randomID = materialDatasTemp[randomIndex].colorID;
            listID.Add(randomID);
        }
        return listID;
    }
}
[System.Serializable]
public class MaterialData {
    public int colorID;
    public Material material;
}
