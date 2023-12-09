using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UIAnimation;
public enum LevelMode { 
    Current,
    NextLevel,
    LastLevel
}
public class LevelSlot : MonoBehaviour
{
    [SerializeField] Image imgBG;
    [SerializeField] TextMeshProUGUI txtLevel;
    [SerializeField] List<Image> lines;
    [SerializeField] List<Sprite> sprBG;
    [SerializeField] List<Sprite> sprLine;
    [SerializeField] Vector3 vectorCurrent = new Vector3(1.2f, 1.2f, 1.2f);
    [SerializeField] Vector3 vectorNomal = Vector3.one;
    public LevelMode levelMode;
    public int level;
    Sequence sequence;
    public void InitData(LevelMode levelMode, int level) {
        this.levelMode = levelMode;
        this.level = level;
        switch (levelMode)
        {
            case LevelMode.Current:
                CurrentLevelMode();
                break;
            case LevelMode.NextLevel:
                NextLevelMode();
                break;
            case LevelMode.LastLevel:
                LastLevelMode();
                break;
            default:
                break;
        }
        txtLevel.text = (level + 1).ToString();
    }

    void CurrentLevelMode() {
        imgBG.sprite = sprBG[1];
        imgBG.transform.localScale = vectorCurrent;
        sequence = UIAnimationController.BtnAnimZoomBasic(imgBG.transform, .5f, -1);
        ChangeLine(sprLine[1]);
    }
    void NextLevelMode() {
        imgBG.sprite = sprBG[2];
        imgBG.transform.localScale = vectorNomal;
        ChangeLine(sprLine[1]); 
    }
    void LastLevelMode() {
        imgBG.sprite = sprBG[0];
        imgBG.transform.localScale = vectorNomal;
        ChangeLine(sprLine[0]);
    }

    void ChangeLine(Sprite sprLine) {
        for (int i = 0; i < lines.Count; i++)
        {
            lines[i].sprite = sprLine;
        }
    }

    private void OnDisable()
    {
        sequence.Kill();
    }
}
