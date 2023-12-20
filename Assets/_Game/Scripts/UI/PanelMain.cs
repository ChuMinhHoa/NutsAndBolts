using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SDK;
using UIAnimation;
using DG.Tweening;
using System;
public class PanelMain : UIPanel
{
    [Header("IMAGE")]
    [SerializeField] Image imgFill;
    [Header("BUTTON")]
    [SerializeField] Button btnHome;
    [SerializeField] Button btnRePlay;
    [SerializeField] Button btnUndo;
    [SerializeField] Button btnAddBulong;

    [Header("TEXT")]
    [SerializeField] TextMeshProUGUI txtCurrentLevel;
    [SerializeField] TextMeshProUGUI txtState;
    [SerializeField] TextMeshProUGUI txtTicket;
    [SerializeField] TextMeshProUGUI txtCoin;
    [SerializeField] TextMeshProUGUI txtTimeCooldown;
    [SerializeField] TextMeshProUGUI txtTimeCooldownNumber;
    [SerializeField] Text txtCountUndo;

    [Header("Object")]
    [SerializeField] GameObject objADSUndo;
    [SerializeField] GameObject objADSBulong;
    [SerializeField] GameObject objCountUndo;
    [SerializeField] GameObject objCoinReplay;
    [SerializeField] GameObject objAdsReplay;
    [SerializeField] GameObject objNoticeADS;

    [Header("Rect")]
    [SerializeField] RectTransform rectNoticeTimeCooldown;

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
        ChangeCoin();
        ChangeTicket();
      
        EventManager.AddListener(EventName.ChangeLevel.ToString(), ChangeTextLevel);
        EventManager.AddListener(EventName.ChangeState.ToString(), ChangeTextState);
        EventManager.AddListener(EventName.CheckAbleOfButtonUndo.ToString(), CheckAbleUndo);
        EventManager.AddListener(EventName.ChangeTicket.ToString(), ChangeTicket);
        EventManager.AddListener(EventName.ChangeCoin.ToString(), ChangeCoin);
        
    }

    void ChangeCoin() { 
        txtCoin.text = ProfileManager.Instance.playerData.playerResource.coin.ToString();
        objCoinReplay.SetActive(ProfileManager.Instance.playerData.playerResource.IsEnoughCoin(10));
        objAdsReplay.SetActive(!objCoinReplay.activeSelf);
    }
    void ChangeTicket() { txtTicket.text = ProfileManager.Instance.playerData.playerResource.ticket.ToString(); }

    void ChangeTextLevel() {
        txtCurrentLevel.text = "LEVEL " + (ProfileManager.Instance.playerData.playerResource.GetLevel() + 1).ToString();
        btnAddBulong.interactable = true;
        objADSUndo.SetActive(false);
        objCountUndo.SetActive(true);
        txtCountUndo.text = "1";
    }

    void ChangeTextState() {
        txtState.text = "STATE " + (ProfileManager.Instance.playerData.playerResource.GetState()).ToString();
        btnAddBulong.interactable = true;
        txtCountUndo.text = "1";
        objADSUndo.SetActive(false);
        objCountUndo.SetActive(true);
    }

    void CheckAbleUndo()
    {
        DOVirtual.DelayedCall(1f, () => {
            btnUndo.interactable = GameManager.Instance.gamePlayController.CheckCanUndo();
            objADSUndo.SetActive(GameManager.Instance.gamePlayController.CheckShowObjUndo());
            objCountUndo.SetActive(!objADSUndo.activeSelf);
        });
    }

    void Replay()
    {
        GameManager.Instance.audioManager.PlaySound(SoundId.UIClick);
        UIAnimationController.BtnAnimZoomBasic(btnRePlay.transform, 0.25f, () =>
        {
            if (ProfileManager.Instance.playerData.playerResource.IsEnoughCoin(10))
            {
                ProfileManager.Instance.playerData.playerResource.ConsumeCoin(10);
                UIManager.instance.ShowPanelLoading(ReplaySuccess);
            }
            else
            {
                if (ProfileManager.Instance.playerData.playerResource.ticket > 0)
                {
                    UIManager.instance.ShowPanelConfirmUsingTicket(ReplaySuccess, ShowAdsReplay);
                }
                else { ShowAdsReplay(); }
            }
        });
    }

    void ShowAdsReplay() {
        if (ProfileManager.Instance.playerData.playerResource.isCheatADS) { ReplaySuccess(); }
        else
        {
            AdsManager.Instance.ShowInterstitial(null, null);
            ReplaySuccess();
        }
    }

    void ReplaySuccess() {
        GameManager.Instance.questManager.AddProgress(QuestType.WatchADS, 1);
        GameManager.Instance.gamePlayController.ReplayLevel();
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
                if (ProfileManager.Instance.playerData.playerResource.ticket > 0)
                {
                    UIManager.instance.ShowPanelConfirmUsingTicket(OnUndoADSSuccess, UndoCallBackPanelUsingticket);
                }
                else {
                  
                }
            }
            else
            {
                GameManager.Instance.gamePlayController.Undo(false);
                ProfileManager.Instance.playerData.questDataSave.AddProgress(1, QuestType.UsingUndo);
            }
            txtCountUndo.text = GameManager.Instance.gamePlayController.GetCountUndo().ToString();
        });
    }

    void UndoCallBackPanelUsingticket() {
        if (ProfileManager.Instance.playerData.playerResource.IsCheatADS())
        {
            OnUndoADSSuccess();
        }
        else
            AdsManager.Instance.ShowRewardVideo(WatchVideoRewardType.Undo.ToString(), OnUndoADSSuccess, OnUndoADSFail);
    }

    void OnUndoADSSuccess() {
        GameManager.Instance.questManager.AddProgress(QuestType.WatchADS, 1);
        GameManager.Instance.questManager.AddProgress(QuestType.UsingUndo, 1);
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
            if (ProfileManager.Instance.playerData.playerResource.ticket > 0)
            {
                UIManager.instance.ShowPanelConfirmUsingTicket(WatchAdsAddBulongDone, AddBulongCallBackPanelUsingticket);
            }
            else
            {
                AddBulongCallBackPanelUsingticket();
            }
        });
    }
    void AddBulongCallBackPanelUsingticket()
    {
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
        GameManager.Instance.questManager.AddProgress(QuestType.WatchADS, 1);
        GameManager.Instance.questManager.AddProgress(QuestType.UsingAddBulong, 1);
        GameManager.Instance.gamePlayController.AddBulongToLine();
        btnAddBulong.interactable = GameManager.Instance.gamePlayController.CheckCanAddBulong();
    }

    void Home() {
        UIAnimationController.BtnAnimZoomBasic(btnHome.transform, 0.5f, () =>
        {
            CancelInvoke("DisAbleTextNotice");
            GameManager.Instance.audioManager.PlaySound(SoundId.UIClick);
            UIManager.instance.ClosePanelMain();
            UIManager.instance.ShowPanelHome();
        });
    }

    Vector2 vectorRectTimeDefault;
    [SerializeField] float widthTimeNotice;
    public void ShowNoticeADS() {
        vectorRectTimeDefault.x = 0;
        vectorRectTimeDefault.y = rectNoticeTimeCooldown.sizeDelta.y;
        rectNoticeTimeCooldown.sizeDelta = vectorRectTimeDefault;
        vectorRectTimeDefault.x = widthTimeNotice;
        objNoticeADS.SetActive(true);
        rectNoticeTimeCooldown.DOSizeDelta(vectorRectTimeDefault, .5f);
        
        Invoke("DisAbleTextNotice", 3f);
    }

    public void DisAbleNotice() { objNoticeADS.SetActive(false); }

    void DisAbleTextNotice()
    {
        vectorRectTimeDefault.x = 0;
        vectorRectTimeDefault.y = rectNoticeTimeCooldown.sizeDelta.y;
        rectNoticeTimeCooldown.DOSizeDelta(vectorRectTimeDefault, .25f);
    }

    public void ChangeTextNoticeTime(float timeRemain) {
        txtTimeCooldown.text = "Ads show in "+ (int)timeRemain + " seconds!";
        txtTimeCooldownNumber.text = ((int)timeRemain).ToString();
        imgFill.fillAmount = 1f - timeRemain / 10f;
    }
}
