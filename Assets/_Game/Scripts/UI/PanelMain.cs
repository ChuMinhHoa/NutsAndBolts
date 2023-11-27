using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SDK;
using UIAnimation;
using DG.Tweening;
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

    int replayCount;

    public override void Awake()
    {
        panelType = UIPanelType.PanelMain;
        base.Awake();
        btnHome.onClick.AddListener(Home);
        btnRePlay.onClick.AddListener(Replay);
        btnUndo.onClick.AddListener(Undo);
        btnAddBulong.onClick.AddListener(AddBulong);
        ChangeTextLevel();
        ChangeTextState();
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
        DOVirtual.DelayedCall(1f, () => {
            btnUndo.interactable = GameManager.Instance.gamePlayController.CheckCanUndo();
            objADSUndo.SetActive(GameManager.Instance.gamePlayController.CheckShowObjUndo());
        });
    }

    void Replay()
    {
        GameManager.Instance.audioManager.PlaySound(SoundId.UIClick);
        UIAnimationController.BtnAnimZoomBasic(btnRePlay.transform, 0.25f, () =>
        {
           
            if (replayCount > 3)
            {
                AdsManager.Instance.ShowInterstitial(null, null);
                replayCount = 0;
            }
            else replayCount++;
            GameManager.Instance.gamePlayController.ReplayLevel();
        });
    }

    void Undo()
    {
        GameManager.Instance.audioManager.PlaySound(SoundId.UIClick);
        btnUndo.interactable = false;
        UIAnimationController.BtnAnimZoomBasic(btnUndo.transform, 0.25f, () =>
        {
            if (!GameManager.Instance.gamePlayController.CheckCanUndo())
                return;
            if (objADSUndo.activeSelf)
            {
                if (ProfileManager.Instance.playerData.playerResource.IsCheatADS())
                {
                    OnUndoADSSuccess();
                }
                else
                    AdsManager.Instance.ShowRewardVideo(WatchVideoRewardType.Undo.ToString(), OnUndoADSSuccess, OnUndoADSFail);
            }
            else
                GameManager.Instance.gamePlayController.Undo(false);
        });
    }

    void OnUndoADSSuccess() {
        GameManager.Instance.gamePlayController.Undo(true);
    }

    void OnUndoADSFail() {
        CheckAbleUndo();
    }

    void AddBulong() {
        btnAddBulong.interactable = false;
        GameManager.Instance.audioManager.PlaySound(SoundId.UIClick);
        if (GameManager.Instance.gamePlayController.onUndo) return;
        UIAnimationController.BtnAnimZoomBasic(btnAddBulong.transform, 0.25f, () =>
        {
            if (!GameManager.Instance.gamePlayController.CheckCanAddBulong() )
                return;
            if (ProfileManager.Instance.playerData.playerResource.IsCheatADS())
            {
                WatchAdsAddBulongDone();
            }
            else
            {
                AdsManager.Instance.ShowRewardVideo(WatchVideoRewardType.AddBulong.ToString(), WatchAdsAddBulongDone);
            }
            btnAddBulong.interactable = true;
        });
    }

    public void WatchAdsAddBulongDone() {
        GameManager.Instance.gamePlayController.AddBulongToLine();
        btnAddBulong.interactable = GameManager.Instance.gamePlayController.CheckCanAddBulong();
    }

    void Home() {
        UIAnimationController.BtnAnimZoomBasic(btnHome.transform, 0.25f, () =>
        {
            GameManager.Instance.audioManager.PlaySound(SoundId.UIClick);
            UIManager.instance.ClosePanelMain();
            UIManager.instance.ShowPanelHome();
        });
        
    }
}
