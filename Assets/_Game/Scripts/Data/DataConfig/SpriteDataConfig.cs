using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SpriteDataConfig", menuName = "ScriptableObject/SpriteDataConfig")]
public class SpriteDataConfig : ScriptableObject
{
    public List<Sprite> sprBackGrounds;
    public Sprite GetSpriteByName(string strName, List<Sprite> listSprite) {
        for (int i = 0; i < listSprite.Count; i++)
        {
            if (listSprite[i].name == strName)
                return listSprite[i];
        }
        return null;
    }

    public Sprite GetBackGround() {
        int indexRandom = Random.Range(0, sprBackGrounds.Count);
        return sprBackGrounds[indexRandom];
    }
}
