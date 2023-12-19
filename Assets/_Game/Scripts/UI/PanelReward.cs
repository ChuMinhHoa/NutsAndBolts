using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIAnimation;
using TMPro;
using DG.Tweening;

public class PanelReward : UIPanel
{
    [SerializeField] Transform wrap;
    [SerializeField] Button btnClaim;
    [SerializeField] TextMeshProUGUI txtAmount;
    [SerializeField] Image imgIconItem;
    Sequence mainSequence;
    public override void Awake()
    {
        panelType = UIPanelType.PanelReward;
        base.Awake();
        btnClaim.onClick.AddListener(Claim);
    }

    private void OnEnable()
    {
        mainSequence = DOTween.Sequence();
        mainSequence = UIAnimationController.PopupBigZoom(wrap, .25f, false);
    }

    public void InitData(ItemType itemType, float amount) { 
        imgIconItem.sprite = ProfileManager.Instance.dataConfig.spriteDataConfig.GetSpriteItemIcon(itemType);
        txtAmount.text = "+" + amount.ToString();
    }

    void Claim() {
        UIAnimationController.BtnAnimZoomBasic(btnClaim.transform, 0.25f, () =>
        {
            UIManager.instance.ClosePanelReward();
        });
    }
}
