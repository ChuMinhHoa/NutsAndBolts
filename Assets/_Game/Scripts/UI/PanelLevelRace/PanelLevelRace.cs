using SDK;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIAnimation;
using UnityEngine;
using UnityEngine.UI;

public class PanelLevelRace : UIPanel
{
    [SerializeField] GameObject StartRaceObj;
    [SerializeField] GameObject RaceJoinedObj;
    [SerializeField] GameObject joinRaceByAds;
    [SerializeField] Button joinBtn;
    [SerializeField] Button continueBtn;
    [SerializeField] Button rewardBtn;
    [SerializeField] Button btnCloseRace;
    [SerializeField] List<UIPlayerRaceInfo> uIPlayerRaceInfos;
    [SerializeField] TextMeshProUGUI desTxt;
    [SerializeField] TextMeshProUGUI remainTimeTxt;
    [SerializeField] float remainTime;
    List<RacersData> racers;
    bool finished;
    string raceDesciption = "Beat 30 levels before others to win the epic price";
    string raceWonDescription = "You won";
    string raceLoseDescription = "Better luck next time";
    public override void Awake()
    {
        panelType = UIPanelType.PanelLevelRace;
        base.Awake();
        joinBtn.onClick.AddListener(JoinRace);
        continueBtn.onClick.AddListener(Continue);
        rewardBtn.onClick.AddListener(GetReward);
        btnCloseRace.onClick.AddListener(ClosePanel);
    }

    private void OnEnable()
    {
        if (ProfileManager.Instance.playerData.levelRaceSave.raceStarted)
        {
            SetUpList();
            remainTime = ProfileManager.Instance.playerData.levelRaceSave.GetRemainTime();
            if (remainTime > 0)
            {
                finished = false;
                desTxt.text = raceDesciption;
                continueBtn.gameObject.SetActive(true);
                rewardBtn.gameObject.SetActive(false);
            }
            else
            {
                GameOver();
            }
        }
        else
        {
            SetUpRaceInfo();
        }

    }

    void SetUpRaceInfo()
    {
        StartRaceObj.SetActive(true);
        RaceJoinedObj.SetActive(false);
        joinRaceByAds.SetActive(!ProfileManager.Instance.playerData.levelRaceSave.firstRace);
    }

    void SetUpList()
    {
        StartRaceObj.SetActive(false);
        RaceJoinedObj.SetActive(true);
        racers = ProfileManager.Instance.playerData.levelRaceSave.GetRacerList();
        indexSlot = 0;
        StartCoroutine(CallSetUp());
    }
    int indexSlot;
    IEnumerator CallSetUp() { 
        while (indexSlot< racers.Count) { 
            yield return new WaitForSeconds(.15f);
            uIPlayerRaceInfos[indexSlot].SetUp(racers[indexSlot].levelReached, racers[indexSlot].player, racers[indexSlot].racerName);
            indexSlot++;
        }
    }

    void Update()
    {
        if (!finished)
        {
            remainTimeTxt.text = "End in: <color=#FF6E3D>" + TimeUtil.TimeToString(remainTime) + "</color>";
            remainTime -= Time.deltaTime;
        }
        if (remainTime < 0 && !finished)
        {
            GameOver();
        }
    }

    void JoinRace()
    {
        if (ProfileManager.Instance.playerData.playerResource.isCheatADS)
        {
            JoinRaceSuccess();
        }
        else
        {
            AdsManager.Instance.ShowRewardVideo(WatchVideoRewardType.JoinRace.ToString(), JoinRaceSuccess);
        }
    }

    void JoinRaceSuccess() {
        ProfileManager.Instance.playerData.levelRaceSave.StartRace();
        GameManager.Instance.questManager.AddProgress(QuestType.WatchADS, 1);
        desTxt.text = raceDesciption;
        SetUpList();
        continueBtn.gameObject.SetActive(true);
        rewardBtn.gameObject.SetActive(false);
        remainTime = ProfileManager.Instance.playerData.levelRaceSave.GetRemainTime();
        finished = false;
    }

    void GameOver()
    {
        finished = true;
        remainTimeTxt.text = ConstantValue.STR_BLANK;
        if (ProfileManager.Instance.playerData.levelRaceSave.IsWon())
        {
            continueBtn.gameObject.SetActive(false);
            rewardBtn.gameObject.SetActive(true);
            desTxt.text = raceWonDescription;
        }
        else
        {
            continueBtn.gameObject.SetActive(true);
            rewardBtn.gameObject.SetActive(false);
            desTxt.text = raceLoseDescription;
        }
    }

    void Continue()
    {
        StopCoroutine(CallSetUp());
        openAndCloseAnim.OnClose(() => {
            if (remainTime > 0)
            {
                //UIAnimationController.BtnAnimZoomBasic(continueBtn.transform, .25f, () => {
                //    GameManager.Instance.audioManager.PlaySound(SoundId.UIClick);
                //    UIManager.instance.ShowPanelLoading(() => {
                //        UIManager.instance.ClosePanelLevelRace();
                //    });
                //});
                UIManager.instance.ClosePanelLevelRace();
            }
            else
            {
                EndRace();
                SetUpRaceInfo();
            }
        });
    }

    void GetReward()
    {
        ProfileManager.Instance.playerData.levelRaceSave.GetReward();
        EndRace();
    }

    void EndRace()
    {
        ProfileManager.Instance.playerData.levelRaceSave.EndRace();
        SetUpRaceInfo();
    }

    void ClosePanel() {
        StopCoroutine(CallSetUp());
        UIManager.instance.ClosePanelLevelRace();
    }
}
