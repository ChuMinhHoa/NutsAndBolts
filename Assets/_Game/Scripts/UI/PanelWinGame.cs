using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelWinGame : UIPanel
{
    [SerializeField] Button btnNextLeve;
    UnityAction actionCallBack;
    public override void Awake()
    {
        panelType = UIPanelType.PanelWinGame;
        base.Awake();
        btnNextLeve.onClick.AddListener(NextLevel);
    }

    public void SetActionCallBack(UnityAction actionCallBack) { this.actionCallBack = actionCallBack; }

    void NextLevel() {
        if (actionCallBack != null)
            actionCallBack();
        UIManager.instance.ClosePanelWinGame();
    }
}
