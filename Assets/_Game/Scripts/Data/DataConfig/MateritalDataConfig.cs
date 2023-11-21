using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MateritalDataConfig", menuName = "ScriptableObject/MateritalDataConfig")]
public class MateritalDataConfig : ScriptableObject
{
    public List<MaterialData> materialDatas = new List<MaterialData>();
    public MaterialData GetMaterialData(int colorID) {
        return materialDatas.Find(e => e.colorID == colorID);
    }
}
[System.Serializable]
public class MaterialData {
    public int colorID;
    public Material material;
}
