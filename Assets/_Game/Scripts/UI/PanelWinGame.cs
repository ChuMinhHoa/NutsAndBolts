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
    UnityAction actionCallBack;
    public override void Awake()
    {
        panelType = UIPanelType.PanelWinGame;
        base.Awake();
        btnNextLeve.onClick.AddListener(NextLevel);
    }

    private void OnEnable()
    {
        UIAnimationController.PanelPopUpBasic(amazingRect, 0.5f, false);
        UIAnimationController.PopupBigZoom(btnNextLeve.transform, 0.25f, false, () => {
            UIAnimationController.PopupBigZoomLoop(btnNextLeve.transform, 2f);
        });
    }

    public void SetActionCallBack(UnityAction actionCallBack) { this.actionCallBack = actionCallBack; }

    void NextLevel() {
        UIAnimationController.BtnAnimZoomBasic(btnNextLeve.transform, 0.25f, () => {
            GameManager.Instance.audioManager.PlaySound(SoundId.UIClick);
            if (actionCallBack != null)
                actionCallBack();
            UIManager.instance.ClosePanelWinGame();
        });
    }
}
