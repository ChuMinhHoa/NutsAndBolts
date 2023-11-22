using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelHome : UIPanel
{
    [SerializeField] Button btnPlay;
    [SerializeField] Button btnSetting;
    public override void Awake()
    {
        panelType = UIPanelType.PanelHome;
        base.Awake();
        btnPlay.onClick.AddListener(PlayGame);
        btnSetting.onClick.AddListener(ShowPanelSetting);
    }

    void PlayGame() {
        UIManager.instance.ClosePanelHome();
        UIManager.instance.ShowPanelMain();
    }

    void ShowPanelSetting() {
        UIManager.instance.ShowPanelSetting();
    }
}
