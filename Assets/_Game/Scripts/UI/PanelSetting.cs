using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelSetting : UIPanel
{
    [SerializeField] Button btnClose;
    [SerializeField] Button btnSound;
    [SerializeField] Button btnVibration;
    [SerializeField] List<Sprite> sprButton;

    bool onSound;
    bool onVibration;
    public override void Awake()
    {
        panelType = UIPanelType.PanelSetting;
        base.Awake();

        // onSound = ProfileManager.Instace.playerData.playerResource.GetSoundStatus
        // onVibration = ProfileManager.Instace.playerData.playerResource.GetVibrationStatus
        btnSound.image.sprite = onSound ? sprButton[0] : sprButton[1];
        btnSound.image.sprite = onSound ? sprButton[0] : sprButton[1];

        btnSound.onClick.AddListener(ChangeSoundStatus);
        btnVibration.onClick.AddListener(ChangeVibrationStatus);
        btnClose.onClick.AddListener(OnClosePanel);
    }

    void ChangeSoundStatus() {
        onSound = !onSound;
        btnSound.image.sprite = onSound ? sprButton[0] : sprButton[1];
        //ProfileManager.Instace.playerData.playerResource.ChangeSoundStatus(onSound)
    }

    void ChangeVibrationStatus() {
        onVibration = !onVibration;
        btnSound.image.sprite = onSound ? sprButton[0] : sprButton[1];
        //ProfileManager.Instace.playerData.playerResource.ChangeVibrationStatus(onVibration)
    }

    void OnClosePanel() {
        UIManager.instance.ClosepPanelSetting();
    }
}
