using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UIAnimation;
using DG.Tweening;
public class PanelHome : UIPanel
{
    [SerializeField] Button btnPlay;
    [SerializeField] Button btnSetting;
    [SerializeField] Button btnDailyQuest;
    [SerializeField] List<LevelSlot> levelSlots = new List<LevelSlot>();
    [SerializeField] LevelSlot levelSlotPref;
    [SerializeField] Transform trsSpawnSlot;
    [SerializeField] ScrollRect scroll;
    [SerializeField] GameObject objNoticeQuest;
    int currentLevel;
    int maxLevel;
    int currentIndexOnMaxLevelSitution;
    public override void Awake()
    {
        panelType = UIPanelType.PanelHome;
        base.Awake();
        btnPlay.onClick.AddListener(PlayGame);
        btnSetting.onClick.AddListener(ShowPanelSetting);
        btnDailyQuest.onClick.AddListener(ShowPanelDailyReward);
    }

    private void OnEnable()
    {
        InitData();
        objNoticeQuest.SetActive(ProfileManager.Instance.playerData.questDataSave.CheckShowNoticeQuest());
    }

    public void InitData() {
        maxLevel = ProfileManager.Instance.dataConfig.levelDataConfig.levelDatas.Count;
        currentLevel = ProfileManager.Instance.playerData.playerResource.playerLevel;
        if (levelSlots.Count < 50)
        {
            SpawnLevel();
        }
        else {
            ReloadData();
        }
        if (gameObject.activeSelf)
            StartCoroutine(AnimScroll());
    }

    void SpawnLevel() {
        int levelStartSpawn = currentLevel > 3 ? (currentLevel - 2) : currentLevel;
        for (int i = 0; i < 50; i++)
        {
           
            LevelSlot newLevelSlot = Instantiate(levelSlotPref, trsSpawnSlot);

            if (levelStartSpawn < currentLevel) newLevelSlot.InitData(LevelMode.LastLevel, levelStartSpawn);
            if (levelStartSpawn > currentLevel) newLevelSlot.InitData(LevelMode.NextLevel, levelStartSpawn);
            if (levelStartSpawn == currentLevel)
            {
                Debug.Log("Current level: " + currentLevel +" "+ levelStartSpawn);
                newLevelSlot.InitData(LevelMode.Current, levelStartSpawn);
            }
            levelStartSpawn++;
            levelSlots.Add(newLevelSlot);

        }
    }

    void DisableSlot() {
        for (int i = currentIndexOnMaxLevelSitution; i < levelSlots.Count; i++)
        {
            levelSlots[i].gameObject.SetActive(false);
        }
    }

    void ReloadData() {
        int levelStartSpawn = 0;
        if (currentLevel > levelSlots[levelSlots.Count - 1].level - 2)
            levelStartSpawn = levelSlots[levelSlots.Count - 1].level - 2;
        else
            levelStartSpawn = levelSlots[0].level;

        for (int i = 0; i < levelSlots.Count; i++)
        {
            levelSlots[i].gameObject.SetActive(true);
            if (levelStartSpawn < currentLevel) 
                levelSlots[i].InitData(LevelMode.LastLevel, levelStartSpawn);
            if (levelStartSpawn > currentLevel) 
                levelSlots[i].InitData(LevelMode.NextLevel, levelStartSpawn);
            if (levelStartSpawn == currentLevel)
                levelSlots[i].InitData(LevelMode.Current, levelStartSpawn);
            levelStartSpawn++;
            if (levelStartSpawn > maxLevel)
            {
                currentIndexOnMaxLevelSitution = i + 1;
                DisableSlot();
                break;
            }
        }
    }

    IEnumerator AnimScroll() {
        yield return new WaitForSeconds(.25f);
        ScrollToCurrent();
    }
    float indexLevelCurrent = 0;
    void ScrollToCurrent() {
        indexLevelCurrent = 0;
        for (int i = 0; i < levelSlots.Count; i++)
        {
            if (levelSlots[i].level == currentLevel)
            {
                indexLevelCurrent = i;
                break;
            }
        }
        DOVirtual.Float(scroll.verticalNormalizedPosition, (indexLevelCurrent / (float)levelSlots.Count), .25f, (value) => {
            scroll.verticalNormalizedPosition = value;
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DOVirtual.Float(scroll.verticalNormalizedPosition, (indexLevelCurrent / (float)levelSlots.Count), .25f, (value) => {
                scroll.verticalNormalizedPosition = value;
            });
        }
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

    void ShowPanelDailyReward() {
        
        UIAnimationController.BtnAnimZoomBasic(btnDailyQuest.transform, .25f,()=> {
            GameManager.Instance.audioManager.PlaySound(SoundId.UIClick);
            UIManager.instance.ShowPanelDailyQuest();
        });
    }
}
