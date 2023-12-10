using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UIAnimation;

public class PanelDailyQuest : UIPanel
{
    [SerializeField] List<PointClaimReward> pointClaimRewards = new List<PointClaimReward>();
    [SerializeField] List<DailyReward> dailyReward = new List<DailyReward>();
    [SerializeField] Vector2 vectorScale;
    [SerializeField] Vector2 vectorScaleOffset;
    [SerializeField] Slider progressDailyReward;
    [SerializeField] Transform trsPointStarTemp;
    [SerializeField] Button btnExit;
    [SerializeField] TextMeshProUGUI txtTimeCoolDown;
    double timeRemain;
    int starEarned;

    public override void Awake()
    {
        panelType = UIPanelType.PanelDailyQuest;
        base.Awake();
    }

    private void OnEnable()
    {
        StartCoroutine(WaitToEndOfFrame());
    }

    IEnumerator WaitToEndOfFrame() {
        yield return new WaitForEndOfFrame();
        starEarned = ProfileManager.Instance.playerData.questDataSave.GetStarEarned();
        dailyReward = ProfileManager.Instance.dataConfig.questDataConfig.dailyRewards;
        progressDailyReward.maxValue = dailyReward[pointClaimRewards.Count - 1].pointGet;
        for (int i = 0; i < pointClaimRewards.Count; i++)
        {
            progressDailyReward.value = dailyReward[i].pointGet;
            bool canGetReward = starEarned >= dailyReward[i].pointGet && ProfileManager.Instance.playerData.questDataSave.CheckCanEarnQuest(i + 1);
            pointClaimRewards[i].InitData(vectorScale + vectorScaleOffset * i, trsPointStarTemp.position, (int)dailyReward[i].amount, dailyReward[i].pointGet, dailyReward[i].itemType, canGetReward);
        }
        progressDailyReward.value = starEarned;
        btnExit.onClick.AddListener(ClosePanel);
    }

    private void Update()
    {
        timeRemain = ProfileManager.Instance.playerData.questDataSave.GetTimeCoolDown();
        txtTimeCoolDown.text = "Complete tasks in <color=#FF5D5D>" + TimeUtil.TimeToString((float)timeRemain)+"</color>";
    }

    void ClosePanel() {
        UIAnimationController.BtnAnimZoomBasic(btnExit.transform, .25f, ()=> {
            GameManager.Instance.audioManager.PlaySound(SoundId.UIClick);
            StopCoroutine(WaitToEndOfFrame());
            openAndCloseAnim.OnClose(()=> { UIManager.instance.ClosePanelDailyQuest(); });
        });
    }
}
