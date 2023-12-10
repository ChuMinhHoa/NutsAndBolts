using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIAnimation;

public class PanelSetting : UIPanel
{
    [SerializeField] Button btnClose;
    [SerializeField] Button btnSound;
    [SerializeField] Button btnVibration;
    [SerializeField] List<Sprite> sprButton;

    [SerializeField] ButtonSwitch btnSoundSwitch;
    [SerializeField] ButtonSwitch btnVibrationSwitch;

    bool onSound;
    bool onVibration;
    public override void Awake()
    {
        panelType = UIPanelType.PanelSetting;
        base.Awake();

        onSound = ProfileManager.Instance.playerData.playerResource.GetOnSound();
        onVibration = ProfileManager.Instance.playerData.playerResource.GetOnVibration();

        btnSound.image.sprite = onSound ? sprButton[0] : sprButton[1];
        btnVibration.image.sprite = onVibration ? sprButton[0] : sprButton[1];

        btnSound.onClick.AddListener(ChangeSoundStatus);
        btnVibration.onClick.AddListener(ChangeVibrationStatus);
        btnClose.onClick.AddListener(OnClosePanel);

        openAndCloseAnim.actionCallBackOnFirstLoad = () =>
        {
            btnSoundSwitch.SetIsOn(onSound);
            btnVibrationSwitch.SetIsOn(onVibration);
        };
    }

    void ChangeSoundStatus() {
        GameManager.Instance.audioManager.PlaySound(SoundId.UIClick);
        onSound = !onSound;
        btnSound.image.sprite = onSound ? sprButton[0] : sprButton[1];
        btnSoundSwitch.OnChangePoint();
        ProfileManager.Instance.playerData.playerResource.ChangeSoundStatus(onSound);
    }

    void ChangeVibrationStatus() {
        GameManager.Instance.audioManager.PlaySound(SoundId.UIClick);
        onVibration = !onVibration;
        btnVibration.image.sprite = onVibration ? sprButton[0] : sprButton[1];
        btnVibrationSwitch.OnChangePoint();
        ProfileManager.Instance.playerData.playerResource.ChangeVibrationStatus(onVibration);
    }

    void OnClosePanel() {
        UIAnimationController.BtnAnimZoomBasic(btnClose.transform, .25f, () =>
        {
            GameManager.Instance.audioManager.PlaySound(SoundId.UIClick);
            UIManager.instance.ClosepPanelSetting();
        });
    }
}
