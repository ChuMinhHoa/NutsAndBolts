using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;

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

    public void ShowPanelLoadScene() {
        isHasPopupOnScene = true;
        GameObject go = GetPanel(UIPanelType.PanelLoading);
        go.SetActive(true);
    }
    public void ClosePanelLoadScene() {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelLoading);
        go.SetActive(false);
    }

    public void ShowPanelWinGame(UnityAction actionDone) {
        isHasPopupOnScene = true;
        GameObject go = GetPanel(UIPanelType.PanelWinGame);
        go.SetActive(true);
        go.GetComponent<PanelWinGame>().SetActionCallBack(actionDone);
    }
    public void ClosePanelWinGame() {
        isHasPopupOnScene = false;
        GameObject go = GetPanel(UIPanelType.PanelWinGame);
        go.SetActive(false);
    }
}
