using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIAnimation;
using UnityEngine;
using UnityEngine.UI;

public class PanelLevelRace : UIPanel
{
    [SerializeField] Button continueBtn;
    [SerializeField] List<UIPlayerRaceInfo> uIPlayerRaceInfos;
    [SerializeField] TextMeshProUGUI remainTimeTxt;
    [SerializeField] float remainTime;
    public override void Awake()
    {
        panelType = UIPanelType.PanelLevelRace;
        base.Awake();
        continueBtn.onClick.AddListener(PlayGame);
    }

    private void OnEnable()
    {
        remainTime = 60 * 60;
        SetUpList();
    }

    void SetUpList()
    {
        for (int i = 0; i < uIPlayerRaceInfos.Count; i++)
        {
            if(i == 0) 
                uIPlayerRaceInfos[i].SetUp(30 - i * 3, true);
            else
                uIPlayerRaceInfos[i].SetUp(30 - i * 3, false, $"Mech {i}");
        }   
    }

    // Update is called once per frame
    void Update()
    {
        remainTimeTxt.text = TimeUtil.TimeToString(remainTime, TimeFommat.Symbol);
        remainTime -= Time.deltaTime;
    }

    void PlayGame()
    {
        UIAnimationController.BtnAnimZoomBasic(continueBtn.transform, .25f, () => {
            GameManager.Instance.audioManager.PlaySound(SoundId.UIClick);
            UIManager.instance.ShowPanelLoading(() => {
                UIManager.instance.ShowPanelMain();
                UIManager.instance.ClosePanelLevelRace();
            });
        });
    }
}
