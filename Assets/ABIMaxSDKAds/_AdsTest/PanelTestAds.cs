using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using SDK;
using TMPro;

public class PanelTestAds : MonoBehaviour
{
    [SerializeField] Button rewardAdsBtn;
    [SerializeField] GameObject rewardItem;

    [SerializeField] TextMeshProUGUI interCounterTxt;
    public float interCooldown = 30;
    public float interCounter = 30;

    private void Start()
    {
        rewardAdsBtn.onClick.AddListener(WatchRewardAds);
    }

    void WatchRewardAds()
    {
        AdsManager.Instance.ShowRewardVideo("RewardTest", GetRewardItem);
    }

    void GetRewardItem()
    {
        rewardItem.SetActive(true);
        rewardAdsBtn.interactable = false;
        DOVirtual.DelayedCall(5f, ResetRewardItem);
    }

    void ResetRewardItem()
    {
        rewardItem.SetActive(false);
        rewardAdsBtn.interactable = true;
    }

    // Update is called once per frame
    void Update()
    {
        interCounterTxt.text = ((int)interCounter).ToString();
        interCounter -= Time.deltaTime;
        if (interCounter < 0)
        {
            interCounter = interCooldown;
            AdsManager.Instance.ShowInterstitial(null, null);
        }
    }
}
