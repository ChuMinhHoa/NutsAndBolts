using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIAnimation;
using UnityEngine.Events;
using TMPro;

public class PanelConfirmUsingTicket : UIPanel
{
    UnityAction actionCallbackConfirm;
    UnityAction actionCallbackCancel;
    [SerializeField] Button btnComfirm;
    [SerializeField] Button btnCancel;
    [SerializeField] TextMeshProUGUI txtCurrentTicket;

    public override void Awake()
    {
        panelType = UIPanelType.PanelConfirmUsingTicket;
        base.Awake();
        btnComfirm.onClick.AddListener(Confirm);
        btnCancel.onClick.AddListener(Cancel);
    }

    public void InitData(UnityAction actionCallbackConfirm, UnityAction actionCalbackCancel) { 
        this.actionCallbackConfirm = actionCallbackConfirm;
        this.actionCallbackCancel = actionCalbackCancel;
        txtCurrentTicket.text = ProfileManager.Instance.playerData.playerResource.ticket.ToString();
    }

    void Confirm() {
        UIAnimationController.BtnAnimZoomBasic(btnComfirm.transform, .25f, ()=>{
            ProfileManager.Instance.playerData.playerResource.ConsumeTicket(1);
            actionCallbackConfirm();
            UIManager.instance.ClosePanelConfirmUsingTicket();
        });
    }

    void Cancel() {
        UIAnimationController.BtnAnimZoomBasic(btnCancel.transform, .25f, () => {
            actionCallbackCancel();
            UIManager.instance.ClosePanelConfirmUsingTicket();
        }); 
    }
}
