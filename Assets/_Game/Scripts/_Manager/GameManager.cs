using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public CameraManager cameraManager;
    public GamePlayController gamePlayController;

    protected override void Awake()
    {
        base.Awake();
        UIManager.instance.ShowPanelHome();
    }
    private void Start()
    {
        InitDataLevelFirstOpen();
    }

    public void InitDataLevelFirstOpen() {
        gamePlayController.FirstOpenScene();
    }


}
