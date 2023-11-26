using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SDK;

public class PanelMain : UIPanel
{
    [Header("BUTTON")]
    [SerializeField] Button btnHome;
    [SerializeField] Button btnRePlay;
    [SerializeField] Button btnUndo;
    [SerializeField] Button btnAddBulong;

    [Header("TEXT")]
    [SerializeField] TextMeshProUGUI txtCurrentLevel;
    [SerializeField] TextMeshProUGUI txtState;

    [Header("Object")]
    [SerializeField] GameObject objADSUndo;
    [SerializeField] GameObject objADSBulong;

    public override void Awake()
    {
        panelType = UIPanelType.PanelMain;
        base.Awake();
        btnHome.onClick.AddListener(Home);
        btnRePlay.onClick.AddListener(Replay);
        btnUndo.onClick.AddListener(Undo);
        btnAddBulong.onClick.AddListener(AddBulong);
        ChangeTextLevel();
        EventManager.AddListener(EventName.ChangeLevel.ToString(), ChangeTextLevel);
        EventManager.AddListener(EventName.ChangeState.ToString(), ChangeTextState);
        EventManager.AddListener(EventName.CheckAbleOfButtonUndo.ToString(), CheckAbleUndo);
    }

    void ChangeTextLevel() {
        txtCurrentLevel.text = "LEVEL " + (ProfileManager.Instance.playerData.playerResource.GetLevel() + 1).ToString();
        btnAddBulong.interactable = true;
    }

    void ChangeTextState() {
        txtState.text = "STATE " + ProfileManager.Instance.playerData.playerResource.GetState().ToString();
        btnAddBulong.interactable = true;
    }

    void CheckAbleUndo()
    {
        btnUndo.interactable = true;
    }

    void Replay()
    {
        GameManager.Instance.gamePlayController.ReplayLevel();
    }

    void Undo() {
        if (!GameManager.Instance.gamePlayController.CheckCanUndo())
            return;
        btnUndo.interactable = false;
        if (objADSUndo.activeSelf)
        {
            if (ProfileManager.Instance.playerData.playerResource.IsCheatADS())
            {
                OnUndoADSSuccess();
            }
            else
                AdsManager.Instance.ShowRewardVideo(WatchVideoRewardType.Undo.ToString(), OnUndoADSSuccess, OnUndoADSFail);
        }else
            GameManager.Instance.gamePlayController.Undo(ActionCallBackUndo, false);
    }

    void OnUndoADSSuccess() {
        GameManager.Instance.gamePlayController.Undo(ActionCallBackUndo, true);
    }

    void OnUndoADSFail() {
        Debug.Log("false");
        btnUndo.interactable = true;
        ActionCallBackUndo();
    }

    void ActionCallBackUndo() {
        objADSUndo.SetActive(GameManager.Instance.gamePlayController.CheckShowObjUndo());
    }

    void AddBulong() {
        if (!GameManager.Instance.gamePlayController.CheckCanAddBulong())
            return;
        if (ProfileManager.Instance.playerData.playerResource.IsCheatADS())
        {
            WatchAdsAddBulongDone();
        }
        else
        {
            AdsManager.Instance.ShowRewardVideo(WatchVideoRewardType.AddBulong.ToString(), WatchAdsAddBulongDone);
        }
    }

    public void WatchAdsAddBulongDone() {
        GameManager.Instance.gamePlayController.AddBulongToLine();
        btnAddBulong.interactable = GameManager.Instance.gamePlayController.CheckCanAddBulong();
    }

    void Home() {
        UIManager.instance.ClosePanelMain();
        UIManager.instance.ShowPanelHome();
    }
}
