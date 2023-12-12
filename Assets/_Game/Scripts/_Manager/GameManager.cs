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
        UIManager.instance.ShowPanelDailyQuest();
    }
    #endregion
}
