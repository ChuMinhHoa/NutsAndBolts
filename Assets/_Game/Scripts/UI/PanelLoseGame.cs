using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UIAnimation;
public class PanelLoseGame : UIPanel
{
    [SerializeField] TextMeshProUGUI txtLevel;
    [SerializeField] Button btnReplay;
    [SerializeField] RectTransform popupRect;
    public override void Awake()
    {
        panelType = UIPanelType.PanelLoseGame;
        base.Awake();
        btnReplay.onClick.AddListener(Replay);
    }
    private void OnEnable()
    {
        txtLevel.text = "LEVEL " + (ProfileManager.Instance.playerData.playerResource.playerLevel + 1).ToString();
        UIAnimationController.PanelPopUpBasic(popupRect, .25f, false);
        InvokeRepeating("AnimButtonReplay", 0f, 3f);
    }
    void AnimButtonReplay() {
        UIAnimationController.PanelPopUpBasic(btnReplay.transform, .25f);
    }
    void Replay() {
        GameManager.Instance.audioManager.PlaySound(SoundId.UIClick);
        CancelInvoke("StartStarEffect");
        UIAnimationController.BtnAnimZoomBasic(btnReplay.transform, 0.25f, () =>
        {
            GameManager.Instance.gamePlayController.ReplayLevel();
        });
    }
}
