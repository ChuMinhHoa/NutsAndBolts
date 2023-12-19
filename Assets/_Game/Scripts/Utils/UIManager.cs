using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;
using UIAnimation;

public class UIManager : MonoBehaviour {
    public static UIManager instance;
    [SerializeField] Transform mainCanvas;
    Dictionary<UIPanelType, GameObject> listPanel = new Dictionary<UIPanelType, GameObject>();
    public bool isHasPopupOnScene = false;

    public bool isHasTutorial = false;
    public bool isHasShopPanel = false;
    public bool isHasPanelIgnoreTutorial = false;
    [SerializeField] RectTransform myRect;
    [SerializeField] GameObject ingameDebugConsole;

    public Camera mainCamera;

    // Start is called before the first frame update
    void Awake() {
        instance = this;
    }
    private void Start()
    {
        //uiPooling.FirstPooling();
        //StartCoroutine(WaitToSpawnUIPool());
        //ingameDebugConsole.SetActive(ProfileManager.Instance.IsShowDebug());
    }

    public bool isDoneFirstLoadPanel = false;
    bool isFirstLoad;
    public void FirstLoadPanel() {
        //LoadSceneManager.Instance.AddTotalProgress(30);

        isDoneFirstLoadPanel = true;
    }

    public void CloseAllPopUp(bool isFirstLoad = false) {
        isHasPopupOnScene = false;
        isHasShopPanel = false;
        foreach (KeyValuePair<UIPanelType, GameObject> panel in listPanel)
        {
            panel.Value.gameObject.SetActive(false);
        }
    }
    public bool IsHavePopUpOnScene() { return isHasPopupOnScene; }
    public bool IsHaveTutorial() { return isHasTutorial; }
    public void RegisterPanel(UIPanelType type, GameObject obj)
    {
        GameObject go = null;
        if (!listPanel.TryGetValue(type, out go))
        {
            //Debug.Log("RegisterPanel " + type.ToString());
            listPanel.Add(type, obj);
        }
        obj.SetActive(false);
    }
    public bool IsHavePanel(UIPanelType type) {
        GameObject panel = null;
        return listPanel.TryGetValue(type, out panel);
    }
    public GameObject GetPanel(UIPanelType type) {
        GameObject panel = null;
        if (!listPanel.TryGetValue(type, out panel)) {
            switch (type) {
                case UIPanelType.PanelHome:
                    panel = Instantiate(Resources.Load("UI/PanelHome") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelSetting:
                    panel = Instantiate(Resources.Load("UI/PanelSetting") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelMain:
                    panel = Instantiate(Resources.Load("UI/PanelMain") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelLoading:
                    panel = Instantiate(Resources.Load("UI/PanelLoading") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelWinGame:
                    panel = Instantiate(Resources.Load("UI/PanelWinGame") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelDailyQuest:
                    panel = Instantiate(Resources.Load("UI/PanelDailyQuest") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelLevelRace:
                    panel = Instantiate(Resources.Load("UI/PanelLevelRace") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelReward:
                    panel = Instantiate(Resources.Load("UI/PanelReward") as GameObject, mainCanvas);
                    break;
                case UIPanelType.PanelConfirmUsingTicket:
                    panel = Instantiate(Resources.Load("UI/PanelConfirmUsingTicket") as GameObject, mainCanvas);
                    break;
            }
            if (panel) panel.SetActive(true);
            return panel;
        }
        return listPanel[type];
    }

    public void ShowPanelBase()
    {
        //GameObject go = GetPanel(UIPanelType.PanelBase);
        //go.SetActive(true);
    }

    public void ClosePanelBase() {
        //GameObject go = GetPanel(UIPanelType.PanelBase);
        //go.SetActive(false);
    }

    public void ShowPanelHome() {
        isHasPopupOnScene = true;
        GameObject go = GetPanel(UIPanelType.PanelHome);
        go.SetActive(true);
    }
    public void ClosePanelHome()
    {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelHome);
        go.SetActive(false);
    }

    public void ShowPanelSetting() {
        isHasPopupOnScene = true;
        GameObject go = GetPanel(UIPanelType.PanelSetting);
        go.SetActive(true);
    }
    public void ClosepPanelSetting() {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelSetting);
        go.SetActive(false);
    }

    public void ShowPanelMain() {
        GameObject go = GetPanel(UIPanelType.PanelMain);
        go.SetActive(true);
    }
    public void ClosePanelMain() {
        GameObject go = GetPanel(UIPanelType.PanelMain);
        go.SetActive(false);
    }

    public void ShowPanelLoading(UnityAction actionCallback) {
        isHasPopupOnScene = true;
        GameObject go = GetPanel(UIPanelType.PanelLoading);
        PanelLoading panelLoading = go.GetComponent<PanelLoading>();
        panelLoading.SetActionCallBack(actionCallback);
        panelLoading.LoadIn();
        go.SetActive(true);
    }

    public void ClosePanelLoading() {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelLoading);
        go.SetActive(false);
    }

    public void ShowPanelWinGame(UnityAction actionDone) {
        GameManager.Instance.audioManager.PlaySound(SoundId.DoneLevel);
        isHasPopupOnScene = true;
        Debug.Log("Show Panel win game");
        GameObject go = GetPanel(UIPanelType.PanelWinGame);
        go.SetActive(true);
        go.GetComponent<PanelWinGame>().SetActionCallBack(actionDone);
    }
    public void ClosePanelWinGame() {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelWinGame);
        go.SetActive(false);
    }

    public void ShowPanelDailyQuest() {
        isHasPopupOnScene = true;
        GameObject go = GetPanel(UIPanelType.PanelDailyQuest);
        go.SetActive(true);
    }

    public void ClosePanelDailyQuest()
    {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelDailyQuest);
        go.SetActive(false);
    }

    public void ShowPanelReward(ItemType itemType, float amount)
    {
        isHasPopupOnScene = true;
        GameObject go = GetPanel(UIPanelType.PanelReward);
        go.SetActive(true);
        go.GetComponent<PanelReward>().InitData(itemType, amount);
    }

    public void ClosePanelReward()
    {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelReward);
        go.SetActive(false);
    }

    public void ShowPanelLevelRace()
    {
        isHasPopupOnScene = true;
        GameObject go = GetPanel(UIPanelType.PanelLevelRace);
        go.SetActive(true);
    }
    public void ClosePanelLevelRace()
    {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelLevelRace);
        go.SetActive(false);
    }

    public void ShowPanelConfirmUsingTicket(UnityAction actionConfirmCallBack, UnityAction actionCancelCallback)
    {
        isHasPopupOnScene = true;
        GameObject go = GetPanel(UIPanelType.PanelConfirmUsingTicket);
        go.SetActive(true);
        go.GetComponent<PanelConfirmUsingTicket>().InitData(actionConfirmCallBack, actionCancelCallback);
    }
    public void ClosePanelConfirmUsingTicket()
    {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelConfirmUsingTicket);
        go.SetActive(false);
    }
}
