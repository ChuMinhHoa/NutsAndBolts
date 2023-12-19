using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using UnityEngine.Events;
using SDK;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting;
public class GamePlayController : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [Header("VECTOR 3")]

    [SerializeField] Vector3 vectorSpace;
    [SerializeField] Vector3 vectorScale;
    [SerializeField] Vector3 vectorPositionSpawn;
    [SerializeField] Vector3 pointCameraOdd;
    [SerializeField] Vector3 pointCameraEven;
    Vector3 lastPostion;

    [Header("INT")]
    [SerializeField] List<int> countInLine = new List<int>();
    [SerializeField] int totalLine;
    [SerializeField] int countMaxInLine;
    [SerializeField] int totalCount;
    [SerializeField] int ocVitCount;
    [SerializeField] int bulongFreeCount;
    [SerializeField] int totalColor;
    [SerializeField] int currentLevel;
    [SerializeField] int state;
    int bulongOnActive;
    int lineOdd;
    int lineOddTemp;
    int countTemp;
    int countRemain;
    int indexLine;
    int currentCountBulongMaxInline;
    [SerializeField] int countUndoRemain;

    [Header("FLOAT")]
    [SerializeField] float speedOut;
    [SerializeField] float speedIn;

    [Header("OTHER")]
    public bool onChoosed;
    public bool detectSelect;
    public bool levelSecret;
    bool isEven;

    LevelData levelData;
    OcVit currentOcvit;
    BuLong currentBulong;
    OcVit ocVitBehind;

    [SerializeField] List<BuLong> buLongs = new List<BuLong>();
    [SerializeField] BuLong buLongAddedByWatch;
    [SerializeField] BuLong bulongPref;
    [SerializeField] Transform parentSpawnBulong;

    [Header("COLOR RANDOM")]
    List<MaterialOnCount> colorRemaining = new List<MaterialOnCount>();
    List<MaterialOnCount> colorRemOnCount = new List<MaterialOnCount>();
    List<int> listColorIDs = new List<int>();

    bool firstLoad = true;

    public void FirstOpenScene()
    {
        levelData = ProfileManager.Instance.playerData.colorLevelSave.levelData;
        currentLevel = ProfileManager.Instance.playerData.playerResource.GetLevel();
        state = ProfileManager.Instance.playerData.playerResource.GetState();
        countUndoRemain = 1;
        if (levelData.BulongFree != 0)
        {
            if (state == 1) levelSecret = (currentLevel + 1) > 3 && (currentLevel + 1) % 2 == 0;
            else levelSecret = false;
            SetDataLevel(levelData.TotalLine, levelData.TotalBulong, levelData.CountMaxInLine, levelData.TotalOcVit, levelData.BulongFree);
            firstLoad = false;
        }
        else
        {
            firstLoad = false;
            if (state == 1)
            {
                levelData = ProfileManager.Instance.dataConfig.GetLevelData(currentLevel);
                levelSecret = (currentLevel + 1) > 3 && (currentLevel + 1) % 2 == 0;
                SetDataLevel(levelData.TotalLine, levelData.TotalBulong, levelData.CountMaxInLine, levelData.TotalOcVit, levelData.BulongFree);
            }
            else
            {
                if (currentLevel < 3)
                    levelData = ProfileManager.Instance.dataConfig.GetLevel1Data();
                else
                    levelData = ProfileManager.Instance.dataConfig.GetLevelEasy();
                levelSecret = false;
                SetDataLevel(levelData.TotalLine, levelData.TotalBulong, levelData.CountMaxInLine, levelData.TotalOcVit, levelData.BulongFree);
            }
        }
        
    }


    public void SetDataLevel(int totalLine, int totalCount, int countMaxInLine, int ocVitCount, int bulongFreeCount) {
        for (int i = 0; i < buLongs.Count; i++)
            buLongs[i].gameObject.SetActive(false);
        this.totalLine = totalLine;
        this.totalCount = totalCount;
        this.countMaxInLine = countMaxInLine;
        this.ocVitCount = ocVitCount;
        this.bulongFreeCount = bulongFreeCount;
        totalColor = totalCount - bulongFreeCount;
        bulongOnActive = totalCount;
        ChangeCamera();
        ResetBulongOcVit();
        InitColor();
        InitBulong();
        ProfileManager.Instance.playerData.colorLevelSave.SaveColorLevel(buLongs, levelData);
    }
    void ChangeCamera() {
        GetCurrentMaxLine();
        if (totalLine < 3)
        {
            DOVirtual.Float(mainCamera.orthographicSize, 20, .25f, (value) => { mainCamera.orthographicSize = value; });
        }
        else if (totalLine >= 3 && totalLine <= 4) 
        {
            DOVirtual.Float(mainCamera.orthographicSize, 25, .25f, (value) => { mainCamera.orthographicSize = value; });
        }
        else {
            DOVirtual.Float(mainCamera.orthographicSize, 28, .25f, (value) => { mainCamera.orthographicSize = value; });
        }

        if (totalLine % 2 == 0)
        {
            mainCamera.transform.DOMove(pointCameraOdd, .25f);
        }
        else {
            mainCamera.transform.DOMove(pointCameraEven, .25f);
        }
    }
    void GetCurrentMaxLine() {
        currentCountBulongMaxInline = 0;
        for (int i = 0; i < totalLine; i++)
        {
            int countI = buLongs.FindAll(e => e.gameObject.activeSelf && e.GetLineIndex() == i).Count;
            if (countI > currentCountBulongMaxInline)
                currentCountBulongMaxInline = countI;
        }
    }
    #region ========================== Bu Long ==========================
    void ResetBulongOcVit() {
        for (int i = 0; i < buLongs.Count; i++)
        {
            buLongs[i].ReSetOcVit();
            buLongs[i].gameObject.SetActive(false);
        }
    }
    public void InitBulong() {
        countRemain = totalCount;
        countTemp = 3;
        countInLine.Clear();
        indexLine = 0;
        for (int i = 0; i < totalLine; i++)
            countInLine.Add(0);
        while (countRemain > 0)
        {
            if (indexLine == totalLine)
            {
                indexLine = 0;
                countTemp = 1;
            }
            countInLine[indexLine] += countTemp;
            countRemain -= countTemp;
            if (countRemain == 0)
                break;
            if (countRemain < 0)
            {
                countInLine[indexLine] += countRemain;
                break;
            }
            indexLine++;
        }
        if ((countInLine[countInLine.Count - 1] - countInLine[countInLine.Count - 2]) % 2 == 0 || countInLine[countInLine.Count - 1] == 0)
        {
            int countT = (countInLine[countInLine.Count - 1] - countInLine[countInLine.Count - 2]) / 2;
            countInLine[countInLine.Count - 2] += countT;
            countInLine[countInLine.Count - 1] -= countT;
        }
        lineOdd = 0;
        vectorPositionSpawn.z = 0;
        for (int i = 0; i < countInLine.Count; i++)
        {
            if (i % 2 != 0)
                lineOdd++;
            SpawnBulong(i, countInLine[i], lineOdd, false);
        }
        SetBulongNull();
        for (int i = 0; i < totalCount; i++)
        {
            buLongs[i].SpawnOcVit(ocVitCount);
        }
        SetColorToOcVit();
    }
    int bulongNullIndex;
    List<BuLong> listBulongAble = new List<BuLong>();

    void SetColorToOcVit() {
        if (firstLoad)
        {
            bulongSaves = ProfileManager.Instance.playerData.colorLevelSave.GetBulongSave();
            for (int i = 0; i < buLongs.Count; i++)
            {
                buLongs[i].SetColorSaveToOcVit(bulongSaves[i].colorIDs);
            }
        }
        else
        {
            for (int i = 0; i < ocVitCount; i++)
            {
                for (int j = 0; j < buLongs.Count; j++)
                {
                    if (!buLongs[j].isNull && buLongs[j].gameObject.activeSelf) buLongs[j].SetColorToOcVit(i);
                }
            }
        }
    }

    void SetBulongNull() {
        if (!firstLoad)
        {
            for (int i = 0; i < bulongFreeCount; i++)
            {
                listBulongAble = buLongs.Where(e => e.gameObject.activeSelf && !e.isNull).ToList();
                bulongNullIndex = Random.Range(0, listBulongAble.Count);
                listBulongAble[bulongNullIndex].isNull = true;
            }
        }
        else {
            bulongSaves = ProfileManager.Instance.playerData.colorLevelSave.GetBulongSave();
            listBulongAble = buLongs.Where(e => e.gameObject.activeSelf && !e.isNull).ToList();
            for (int i = 0; i < bulongSaves.Count; i++)
            {
                
                Debug.Log("color on bulong save: "+bulongSaves[i].colorIDs.Count);
                if (bulongSaves[i].colorIDs.Count == 0)
                {
                    listBulongAble[i].isNull = true;
                }
            }
        }
    }

    #region Add new bulong
    public void AddBulongToLine() {
        
        lineOddTemp = lineOdd;
        vectorPositionSpawn.z = 0;
        for (int i = countInLine.Count - 1; i >= 0; i--)
        {

            if (i == 0)
            {
                lineOddTemp = 0;
            }
            if (countInLine[i] < countMaxInLine || buLongAddedByWatch != null)
            {
                if (buLongAddedByWatch == null)
                {
                    bulongOnActive++;
                    SpawnNewBulong(i);
                }
                else {
                    if (buLongAddedByWatch.AddOneScaleUp()) buLongAddedByWatch = null;
                }
                break;
            }
            if (i % 2 != 0)
                lineOddTemp--;
        }
        ChangeCamera();
    }

    void SpawnNewBulong(int lineIndex) {
        countInLine[lineIndex]++;
        DeactiveBulongInLine(lineIndex);
        SpawnBulong(lineIndex, countInLine[lineIndex], lineOddTemp, true);
    }
    #endregion

    void DeactiveBulongInLine(int lineIndex) {
        for (int i = 0; i < buLongs.Count; i++)
        {
            if (buLongs[i].GetLineIndex() == lineIndex)
                buLongs[i].gameObject.SetActive(false);
        }
    }

    void SpawnBulong(int lineIndex, int totalBulongInLine, int lineOdd, bool ads) {
        if (lineIndex % 2 != 0)
        {
            vectorPositionSpawn.z = -vectorSpace.z * lineOdd;
        }
        else vectorPositionSpawn.z = vectorSpace.z * lineOdd;
        isEven = (totalBulongInLine % 2 == 0);
        vectorPositionSpawn.x = 0;

        if (!isEven)
        {
            vectorPositionSpawn.x = 0;
            BuLong buLong = GetBulong();
            buLong.transform.position = vectorPositionSpawn;
            buLong.SetLineIndex(lineIndex);
            buLong.SetTotalOcVit(ocVitCount);
            buLong.InitScaleup(levelSecret);
            lastPostion.x = 0;
        }
        else lastPostion.x = -vectorSpace.x; //+ vectorScale.x / 2;
        for (int i = 1; i < totalBulongInLine / 2 + 1; i++)
        {
            vectorPositionSpawn.x = lastPostion.x + vectorSpace.x + vectorScale.x;
            BuLong newBuLong1 = GetBulong();
            newBuLong1.transform.position = vectorPositionSpawn;
            newBuLong1.SetLineIndex(lineIndex);
            newBuLong1.SetTotalOcVit(ocVitCount);
            newBuLong1.InitScaleup(levelSecret);
            lastPostion.x = vectorPositionSpawn.x;

            
            vectorPositionSpawn.x *= -1;
            BuLong newBuLong2 = GetBulong();
            newBuLong2.transform.position = vectorPositionSpawn;
            newBuLong2.SetLineIndex(lineIndex);
            newBuLong2.SetTotalOcVit(ocVitCount);
            if (i == totalBulongInLine / 2)
            {
                newBuLong2.InitScaleup(levelSecret, ads);
                buLongAddedByWatch = ads ? newBuLong2 : null;
            }
            else
                newBuLong2.InitScaleup(levelSecret);

        }
    }

    BuLong GetBulong() {
        for (int i = 0; i < buLongs.Count; i++)
        {
            if (!buLongs[i].gameObject.activeSelf)
            {
                buLongs[i].gameObject.SetActive(true);
                return buLongs[i];
            }
        }
        BuLong buLong = Instantiate(bulongPref, parentSpawnBulong);
        buLongs.Add(buLong);
        buLong.SetBulongID(buLongs.Count);
        return buLong;
    }
    #endregion

    #region ========================== Controll ==========================
    public List<StepMoveOcVit> stepMoveOcVits = new List<StepMoveOcVit>();
    public List<OcVit> ocVitOnMove = new List<OcVit>();
    public void OnChooseCurrentOcVit(OcVit ocVit, BuLong bulong, UnityAction actionDone = null) {
        GameManager.Instance.audioManager.PlaySound(SoundId.ChooseBulong);
        currentBulong = bulong;
        currentOcvit = ocVit;
        onChoosed = true;
        //detectSelect = true;
        ocVitOnMove.Clear();
        ocVitOnMove.Add(ocVit);
        ocVitsTemp.Clear();
        GetOcVitSameColor();
        currentOcvit.ChooseOut(bulong.GetPointInOut().position, speedOut, ()=> {
            
            if (actionDone != null) actionDone();
        });
       
    }
    Vector3 vectorScaleOcvit = new Vector3(0, 2.35f/2f, 0);
    void GetOcVitSameColor() {
        OcVit ocVitTemp = currentBulong.GetOcVitBehind(ocVitOnMove.Count);
        if (ocVitTemp == null)
        {
            detectSelect = false;
            return;
        }
        if (ocVitTemp.IsSameColor(currentOcvit.GetColor()) && !ocVitTemp.IsOnSecretMode())
        {
            ocVitOnMove.Add(ocVitTemp);
            ocVitTemp.ChooseOut(currentBulong.GetPointInOut().position - vectorScaleOcvit * (ocVitOnMove.Count - 1), speedOut);
            GetOcVitSameColor();
        }
        else {
            detectSelect = false;
        }
    }

    BuLong bulongOnChoose;
    public void OnChooseOtherBulong(BuLong bulong) {
        if (currentBulong == bulong)
        {
            GameManager.Instance.audioManager.PlaySound(SoundId.Faild);
            NextStepMoveBack();
        }
        else
        {
            if (!bulong.IsCanJoin(ocVitOnMove[0]))
            {
                GameManager.Instance.audioManager.PlaySound(SoundId.Faild);
                bulong.ShowFailOtherColor();
                NextStepMoveBack();
                return;
            }

            if (bulong.IsFull())
            {
                GameManager.Instance.audioManager.PlaySound(SoundId.Faild);
                bulong.ShowFailFull();
                NextStepMoveBack();
                return;
            }
            detectSelect = true;
            AddOcvitToListMove(ocVitOnMove[0]);
            bulongOnChoose = bulong;
            currentBulong.RemoveOcvit(ocVitOnMove[0]);
            bulong.ChooseOtherBulong(ocVitOnMove[0], speedIn, NextStep, OnDoneBulong);
        }
    }

    void NextStep() {
        ocVitOnMove.Remove(ocVitOnMove[0]);
        detectSelect = true;
        if (ocVitOnMove.Count > 0)
        {
            if (bulongOnChoose.IsFull() || !bulongOnChoose.IsCanJoin(ocVitOnMove[0]))
            {
                NextStepMoveBack();
                return;
            }
            currentBulong.RemoveOcvit(ocVitOnMove[0]);
            AddOcvitToListMove(ocVitOnMove[0]);
            bulongOnChoose.ChooseOtherBulong(ocVitOnMove[0], speedIn, NextStep, OnDoneBulong);
        }
        else {
            AddStepMove(currentBulong, bulongOnChoose);
            ocVitBehind = currentBulong.GetOcVitBehind();
            if (ocVitBehind != null && ocVitBehind.IsOnSecretMode())
                ocVitBehind.OffSecretMode();
            SetDefault();
        } 
    }

    void NextStepMoveBack() {
        detectSelect = true;
        if (ocVitOnMove.Count > 0)
        {
            currentBulong.ChooseSameBulong(ocVitOnMove[ocVitOnMove.Count - 1], speedIn, null);
            ocVitOnMove.Remove(ocVitOnMove[ocVitOnMove.Count - 1]);
            NextStepMoveBack();
        }
        else SetDefault();
    }
    List<OcVit> ocVitsTemp = new List<OcVit>();
    void AddOcvitToListMove(OcVit ocVit) { ocVitsTemp.Add(ocVit); }
    public void AddStepMove(BuLong lastBulong, BuLong currentBulong) {
        StepMoveOcVit step = new StepMoveOcVit();
        for (int i = 0; i < ocVitsTemp.Count; i++)
            step.ocVit.Add(ocVitsTemp[i]);
        step.lastBulong = lastBulong;
        step.currentBulong = currentBulong;
        stepMoveOcVits.Add(step);
    }

    public void RemoveStepMove()
    {
        stepMoveOcVits.Remove(stepMoveOcVits[stepMoveOcVits.Count - 1]);
    }

    void SetDefault() {
        onChoosed = false;
        bulongOnChoose = null;
        currentBulong = null;
        currentOcvit = null;
        detectSelect = false;
        EventManager.TriggerEvent(EventName.CheckAbleOfButtonUndo.ToString());
    }
    #endregion

    #region ========================== Random Color ==========================
    List<BulongSave> bulongSaves = new List<BulongSave>();
    void InitColor() {
        listColorIDs = ProfileManager.Instance.dataConfig.GetRandomMaterial(totalColor);
        colorRemaining.Clear();
        for (int i = 0; i < totalColor; i++)
        {
            MaterialOnCount mOnCount = new MaterialOnCount();
            mOnCount.colorID = listColorIDs[i];
            mOnCount.count = ocVitCount;
            colorRemaining.Add(mOnCount);
        }
    }
    public int GetColorSwitch(int colorID, BuLong bulong) {
        for (int i = 0; i < buLongs.Count; i++)
        {
            if (bulong != buLongs[i])
            {
                if (!buLongs[i].CheckBulongHasColor(colorID))
                {
                    return buLongs[i].SwitchColorOcVit(colorID);
                }
            }
        }
        return -1;
    }
    public int GetColor() {
        int colorIndex = -1;
        colorRemOnCount = colorRemaining.Where(e => e.count > 0).ToList();
        colorIndex = Random.Range(0, colorRemOnCount.Count);
        return colorRemOnCount[colorIndex].colorID;
    }

    public bool ColorRemainCountCheckIsLastColor() {
        return colorRemOnCount.Count <= 1;
    }

    public void MinusColorRemain(int colorID) {
        colorRemaining.Find(e => e.colorID == colorID).count--;
    }
    #endregion

    #region ========================== Game Follow ==========================
    int currentBulongDoneCount = 0;
    public void OnDoneBulong() {
        GameManager.Instance.audioManager.PlaySound(SoundId.DoneBulong);
        currentBulongDoneCount++;
        DOVirtual.DelayedCall(.5f, () => {
            if (currentBulongDoneCount == totalColor)
            {
                Debug.Log("Level Up");
                currentBulongDoneCount = 0;
                stepMoveOcVits.Clear();
                countUndoRemain = 1;
                SetDefault();
                if (state == 1)
                {
                    Debug.Log("level: "+currentLevel);
                    if (levelSecret)
                        GameManager.Instance.questManager.AddProgress(QuestType.CompleteLevelSecret, 1);
                    GameManager.Instance.questManager.AddProgress(QuestType.CompleteLevel, 1);
                    levelSecret = false;
                    ProfileManager.Instance.playerData.playerResource.LevelUp(currentLevel + 1);
                    UIManager.instance.ShowPanelWinGame(ChangeLevel);
                }
                else
                {
                    GameManager.Instance.audioManager.PlaySound(SoundId.DoneLevel);
                    UIManager.instance.ShowPanelLoading(ChangeState);
                }
            }
        });
        
    }

    void ChangeState() {
        ProfileManager.Instance.playerData.playerResource.ChangeState(1);
        state = 1;
        levelData = ProfileManager.Instance.dataConfig.GetLevelData(currentLevel);
        levelSecret = ((currentLevel + 1) > 3 && (currentLevel + 1) % 2 == 0);
        SetDataLevel(levelData.TotalLine, levelData.TotalBulong, levelData.CountMaxInLine, levelData.TotalOcVit, levelData.BulongFree);
    }

    void ChangeLevel() {
        currentLevel++;
        state = 0;
        if (currentLevel < 3)
            levelData = ProfileManager.Instance.dataConfig.GetLevel1Data();
        else
            levelData = ProfileManager.Instance.dataConfig.GetLevelEasy();
        SetDataLevel(levelData.TotalLine, levelData.TotalBulong, levelData.CountMaxInLine, levelData.TotalOcVit, levelData.BulongFree);
        if (currentLevel % 3 == 0) AdsManager.Instance.ShowInterstitial(null, null);
    }

    #endregion

    public MaterialData GetMaterialData(int colorID) {
        return ProfileManager.Instance.dataConfig.GetMaterialData(colorID);
    }

    public void ReplayLevel() {
        currentBulongDoneCount = 0;
        firstLoad = true;
        levelData = ProfileManager.Instance.playerData.colorLevelSave.levelData;
        currentLevel = ProfileManager.Instance.playerData.playerResource.GetLevel();
        state = ProfileManager.Instance.playerData.playerResource.GetState();
        SetDataLevel(levelData.TotalLine, levelData.TotalBulong, levelData.CountMaxInLine, levelData.TotalOcVit, levelData.BulongFree);
        countUndoRemain = 1;
        SetDefault();
        firstLoad = false;
        stepMoveOcVits.Clear();
    }
    public bool CheckCanUndo() { return stepMoveOcVits.Count > 0; }
    public int GetCountUndo() { return countUndoRemain; }
    public bool CheckShowObjUndo() { return countUndoRemain == 0; }
    public bool CheckCanAddBulong() {
        if (buLongAddedByWatch == null)
            return (totalLine * countMaxInLine > bulongOnActive);
        else 
            return true;
    }

    #region ========================== Undo ==========================
    public bool onUndo;
    public void Undo(bool isADS = false) {
        if (countUndoRemain == 0 && !isADS)
        {
            EventManager.TriggerEvent(EventName.CheckAbleOfButtonUndo.ToString());
            return;
        }
        onUndo = true;
        if (currentOcvit != null)
        {
            NextStepMoveBack(StartMoveBack);
        }
        else
            StartMoveBack();
        if (!isADS) countUndoRemain--;
        else countUndoRemain += 2;
    }
    // call on Undo
    void NextStepMoveBack(UnityAction actionCallBack)
    {
        detectSelect = true;
        if (ocVitOnMove.Count > 0)
        {
            currentBulong.ChooseSameBulong(ocVitOnMove[ocVitOnMove.Count - 1], speedIn, null);
            ocVitOnMove.Remove(ocVitOnMove[ocVitOnMove.Count - 1]);
            NextStepMoveBack(actionCallBack);
        }
        else {
            SetDefault();
            actionCallBack();
        }
        
    }

    void StartMoveBack() {
        ocVitOnMove = stepMoveOcVits[stepMoveOcVits.Count - 1].ocVit;
        OnMoveBack(ocVitOnMove[ocVitOnMove.Count-1], stepMoveOcVits[stepMoveOcVits.Count - 1].currentBulong, stepMoveOcVits[stepMoveOcVits.Count - 1].lastBulong);
    }

    void OnMoveBack(OcVit ocVit, BuLong currentBulong, BuLong lastBulong) {
        OcVitUndoOut(ocVit, currentBulong, ()=> {
            currentBulong.RemoveOcvit(ocVit);
            ChooseOtherBulongOnMoveBack(ocVit, lastBulong);
            if (currentBulong.OnDoneMode())
            {
                currentBulong.addDone = false;
                currentBulongDoneCount--;
            }
            ocVitOnMove.Remove(ocVitOnMove[ocVitOnMove.Count - 1]);
            if (ocVitOnMove.Count > 0)
                OnMoveBack(ocVitOnMove[ocVitOnMove.Count - 1], stepMoveOcVits[stepMoveOcVits.Count - 1].currentBulong, stepMoveOcVits[stepMoveOcVits.Count - 1].lastBulong);
            else
            {
                RemoveStepMove();
            }
        });
    }
    void OcVitUndoOut(OcVit ocVit, BuLong bulong, UnityAction actionDone = null)
    {
        GameManager.Instance.audioManager.PlaySound(SoundId.ChooseBulong);
        onChoosed = true;
        //detectSelect = true;
        ocVit.ChooseOut(bulong.GetPointInOut().position, speedOut, () => {
            if (actionDone != null) actionDone();
        });

    }

    void ChooseOtherBulongOnMoveBack(OcVit ocVit, BuLong buLong) {
        detectSelect = true;
        buLong.ChooseOtherBulongOnUndo(ocVit, speedIn, ()=>
        {
            if (ocVitOnMove.Count == 0) {
                SetDefault();
                onUndo = false;
            }
        } );
        buLong.CheckBulongIsDone();
    }
    #endregion

    float currentTime;  
    private void Update()
    {
        if (currentTime > 60)
        {
            currentTime = 0;
            AdsManager.Instance.ShowInterstitial(null, null);
        }
        else {
            currentTime += Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            levelData = ProfileManager.Instance.dataConfig.GetLevelData(currentLevel);
            SetDataLevel(levelData.TotalLine, levelData.TotalBulong, levelData.CountMaxInLine, levelData.TotalOcVit, levelData.BulongFree);
        }
    }
}
[System.Serializable]
public class MaterialOnCount {
    public int colorID;
    public int count;
}

[System.Serializable]
public class StepMoveOcVit {
    public BuLong currentBulong;
    public List<OcVit> ocVit = new List<OcVit>();
    public BuLong lastBulong;
}
