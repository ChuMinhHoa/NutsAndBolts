using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UIAnimation;
public class PanelHome : UIPanel
{
    [SerializeField] Button btnPlay;
    [SerializeField] Button btnSetting;
    [SerializeField] TextMeshProUGUI txtCurrentLevel;
    public override void Awake()
    {
        panelType = UIPanelType.PanelHome;
        base.Awake();
        btnPlay.onClick.AddListener(PlayGame);
        btnSetting.onClick.AddListener(ShowPanelSetting);
    }

    private void OnEnable()
    {
        txtCurrentLevel.text = "LEVEL " + (ProfileManager.Instance.playerData.playerResource.playerLevel + 1).ToString();
    }

    void PlayGame() {
        UIAnimationController.BtnAnimZoomBasic(btnPlay.transform, .25f, ()=> {
            GameManager.Instance.audioManager.PlaySound(SoundId.UIClick);
            UIManager.instance.ShowPanelLoading(()=> {
                UIManager.instance.ShowPanelMain();
                UIManager.instance.ClosePanelHome();
            });
        });
    }



    void ShowPanelSetting() {
        UIAnimationController.BtnAnimZoomBasic(btnSetting.transform, .25f);
        GameManager.Instance.audioManager.PlaySound(SoundId.UIClick);
        UIManager.instance.ShowPanelSetting();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UIManager.instance.ShowPanelLevelRace();
        }
    }
}
