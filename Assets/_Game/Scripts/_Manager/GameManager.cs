using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;
public class GameManager : Singleton<GameManager>
{
    //public CameraManager cameraManager;
    public GamePlayController gamePlayController;
    public NiceVibrationsDemoManager vibrationManager;
    public AudioManager audioManager;
    public QuestManager questManager;

    protected override void Awake()
    {
        base.Awake();
        UIManager.instance.ShowPanelHome();
    }
    private void Start()
    {
        InitDataLevelFirstOpen();
        audioManager.PlaySound(SoundId.BGSound);
    }

    public void InitDataLevelFirstOpen() {
        gamePlayController.FirstOpenScene();
    }

    #region RewardManager
    public void ClaimReward(ItemType itemType, float amount) {
        ProfileManager.Instance.playerData.questDataSave.GetReward();

        switch (itemType)
        {
            case ItemType.NoAds:
                break;
            case ItemType.Coin:
                ProfileManager.Instance.playerData.playerResource.AddCoin((int)amount);
                break;
            case ItemType.Ticket:
                ProfileManager.Instance.playerData.playerResource.AddTicket((int)amount);
                break;
            default:
                break;
        }
        UIManager.instance.ShowPanelReward(itemType, amount);
    }
    #endregion
}
