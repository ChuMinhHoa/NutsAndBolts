using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UIAnimation;
public class PanelWinGame : UIPanel
{
    [SerializeField] Button btnNextLeve;
    [SerializeField] Transform amazingRect;
    [SerializeField] Transform popupRect;
    [SerializeField] Transform levelWrap;
    [SerializeField] TextMeshProUGUI txtPerfect;
    [SerializeField] TextMeshProUGUI txtLevel;
    [SerializeField] List<Transform> trsStars;
    UnityAction actionCallBack;
    public override void Awake()
    {
        panelType = UIPanelType.PanelWinGame;
        base.Awake();
        btnNextLeve.onClick.AddListener(NextLevel);
    }

    private void OnEnable()
    {
        txtLevel.text = "LEVEL " + (ProfileManager.Instance.playerData.playerResource.playerLevel+1).ToString();
        UIAnimationController.PanelPopUpBasic(popupRect, .25f, false, () =>
        {
            InvokeRepeating("StartStarEffect", 0f, 3f);
        });
    }
    private void StartStarEffect()
    {
        UIAnimationController.PanelPopUpBasic(amazingRect, .25f, () => {
            indexStar = 0;
            StartCoroutine(StarEffect());
            UIAnimationController.PanelPopUpBasic(levelWrap, .25f, () => {
                UIAnimationController.PanelPopUpBasic(btnNextLeve.transform, .25f);
            });
        });
    }
    int indexStar;
    IEnumerator StarEffect() {
        while (indexStar < trsStars.Count)
        {
            UIAnimationController.BtnAnimZoomBasic(trsStars[indexStar], .25f);
            indexStar++;
            yield return new WaitForSeconds(.1f);
        }
    }

    public void SetActionCallBack(UnityAction actionCallBack) { this.actionCallBack = actionCallBack; }

    void NextLevel() {
        CancelInvoke("StartStarEffect");
        StopCoroutine(StarEffect());
        UIAnimationController.BtnAnimZoomBasic(btnNextLeve.transform, 0.25f, () => {
            GameManager.Instance.audioManager.PlaySound(SoundId.UIClick);
            UIManager.instance.ClosePanelWinGame();
            UIManager.instance.ShowPanelLoading(actionCallBack);
        });
    }
}
