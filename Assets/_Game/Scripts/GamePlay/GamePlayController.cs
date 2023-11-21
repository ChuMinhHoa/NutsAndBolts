using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
public class GamePlayController : MonoBehaviour
{
    public static GamePlayController Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    [SerializeField] LevelDataConfig levelDataConfig;
    [SerializeField] MateritalDataConfig materitalDataConfig;

    [SerializeField] Vector3 vectorSpace;
    [SerializeField] Vector3 vectorScale;
    [SerializeField] Vector3 vectorPositionSpawn;

    [SerializeField] List<int> countInLine = new List<int>();
    [SerializeField] int totalLine;
    [SerializeField] int countMaxInLine;
    [SerializeField] int totalCount;
    [SerializeField] int ocVitCount;
    [SerializeField] int bulongFreeCount;
    [SerializeField] int totalColor;
    [SerializeField] List<BuLong> buLongs;
    [SerializeField] BuLong bulongPref;
    [SerializeField] Transform parentSpawnBulong;

    [SerializeField] int currentLevel;
    [SerializeField] int state;

    [SerializeField] float speedOut;
    [SerializeField] float speedIn;
    [SerializeField] float speedOutSame;
    [SerializeField] float speedInSame;

    int countTemp;
    int countRemain;
    int indexLine;

    private void Start()
    {
        if (state == 1)
        {
            levelData = levelDataConfig.GetLevelData(currentLevel);
            SetDataLevel(levelData.TotalLine, levelData.TotalBulong, levelData.CountMaxInLine, levelData.TotalOcVit, levelData.BulongFree);
        }
        else
        {
            levelData = levelDataConfig.GetLevelEasy();
            SetDataLevel(levelData.TotalLine, levelData.TotalBulong, levelData.CountMaxInLine, levelData.TotalOcVit, levelData.BulongFree);
        }
    }
    bool isEven;
    Vector3 lastPostion;
    int lineOdd;
    int lineOddTemp;

    public void SetDataLevel(int totalLine, int totalCount, int countMaxInLine, int ocVitCount, int bulongFreeCount) {
        for (int i = 0; i < buLongs.Count; i++)
            buLongs[i].gameObject.SetActive(false);
        this.totalLine = totalLine;
        this.totalCount = totalCount;
        this.countMaxInLine = countMaxInLine;
        this.ocVitCount = ocVitCount;
        this.bulongFreeCount = bulongFreeCount;
        totalColor = totalCount - bulongFreeCount;
        ResetBulongOcVit();
        InitColor();
        InitBulong();
    }
    #region Bu Long
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
            SpawnBulong(i, countInLine[i], lineOdd);
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

        for (int i = 0; i < ocVitCount; i++)
        {
            for (int j = 0; j < buLongs.Count; j++)
            {
                if (!buLongs[j].isNull && buLongs[j].gameObject.activeSelf) buLongs[j].SetColorToOcVit(i);
            }
        }
    
    }
    
    void SetBulongNull() {
        for (int i = 0; i < bulongFreeCount; i++)
        {
            listBulongAble = buLongs.Where(e => e.gameObject.activeSelf && !e.isNull).ToList();
            bulongNullIndex = Random.Range(0, listBulongAble.Count);
            listBulongAble[bulongNullIndex].isNull = true;
        }
    }

    public bool IsCanAddBuLong() { return buLongs.Count == totalCount; }

    public void AddBulongToLine() {
        lineOddTemp = lineOdd;
        vectorPositionSpawn.z = 0;
        for (int i = countInLine.Count - 1; i >= 0; i--)
        {

            if (i == 0)
            {
                lineOddTemp = 0;
            }
            if (countInLine[i] < countMaxInLine)
            {
                countInLine[i]++;
                DeactiveBulongInLine(i);
                SpawnBulong(i, countInLine[i], lineOddTemp);
                break;
            }
            if (i % 2 != 0)
                lineOddTemp--;
        }
    }

    void DeactiveBulongInLine(int lineIndex) {
        for (int i = 0; i < buLongs.Count; i++)
        {
            if (buLongs[i].GetLineIndex() == lineIndex)
                buLongs[i].gameObject.SetActive(false);
        }
    }

    void SpawnBulong(int lineIndex, int totalBulongInLine, int lineOdd) {
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
            buLong.InitScaleup();
            lastPostion.x = 0;
        }
        else lastPostion.x = -vectorSpace.x + vectorScale.x / 2;
        for (int i = 1; i < totalBulongInLine / 2 + 1; i++)
        {
            vectorPositionSpawn.x = lastPostion.x + vectorSpace.x + vectorScale.x;
            BuLong newBuLong1 = GetBulong();
            newBuLong1.transform.position = vectorPositionSpawn;
            newBuLong1.SetLineIndex(lineIndex);
            newBuLong1.SetTotalOcVit(ocVitCount);
            newBuLong1.InitScaleup();
            lastPostion.x = vectorPositionSpawn.x;

            vectorPositionSpawn.x *= -1;
            BuLong newBuLong2 = GetBulong();
            newBuLong2.transform.position = vectorPositionSpawn;
            newBuLong2.SetLineIndex(lineIndex);
            newBuLong2.SetTotalOcVit(ocVitCount);
            newBuLong2.InitScaleup();
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

    #region Controll
    OcVit currentOcvit;
    BuLong currentBulong;
    OcVit ocVitCheck;
    public bool onChoosed;
    public bool detectSelect;
    public void OnChooseCurrentOcVit(OcVit ocVit, BuLong bulong) {
        currentBulong = bulong;
        currentOcvit = ocVit;
        onChoosed = true;
        currentOcvit.ChooseOut(bulong.GetPointInOut(), speedOut);
    }
    BuLong bulongOnChoose;
    public void OnChooseOtherBulong(BuLong bulong, bool isSame) {
        if (currentBulong == bulong)
            ChooseTheSameBulong(bulong);
        else {
            if (bulong.IsFull() || !bulong.IsCanJoin(currentOcvit))
            {
                ChooseTheSameBulong(currentBulong);
                SetDefault();
                return;
            }
            bulongOnChoose = bulong;
            currentBulong.RemoveOcvit(currentOcvit);
            detectSelect = true;
            if (isSame) bulong.ChooseOtherBulong(currentOcvit, speedInSame, OnMoveOcVitDone);
            else bulong.ChooseOtherBulong(currentOcvit, speedIn, OnMoveOcVitDone);
        }
    }

    void OnMoveOcVitDone() {
        ocVitCheck = currentBulong.IsSameColorWithCurrentOcvit(currentOcvit);
        if (ocVitCheck != null)
        {
            currentOcvit = ocVitCheck;
            if (bulongOnChoose.IsFull() || !bulongOnChoose.IsCanJoin(currentOcvit))
            {
                SetDefault();
                return;
            }
            currentOcvit.ChooseOut(currentBulong.GetPointInOut(), speedOutSame, ChooseBulongOnSameOcVit);

        }
        else SetDefault();
    }

    void ChooseBulongOnSameOcVit() { OnChooseOtherBulong(bulongOnChoose, true); }

    void SetDefault() {
        onChoosed = false;
        bulongOnChoose = null;
        ocVitCheck = null;
        currentBulong = null;
        currentOcvit = null;
        detectSelect = false;
    }

    void ChooseTheSameBulong(BuLong buLong) {
        buLong.ChooseSameBulong(speedInSame, SetDefault);
    }
    #endregion

    #region Random Color
    List<MaterialOnCount> colorRemaining = new List<MaterialOnCount>();
    List<MaterialOnCount> colorRemOnCount = new List<MaterialOnCount>();
    List<int> listColorIDs = new List<int>();
    void InitColor() {
        listColorIDs = materitalDataConfig.GetMaterialRandom(totalColor);
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

    #region Game Follow
    int currentBulongDoneCount = 0;
    public void OnDoneBulong() {
        currentBulongDoneCount++;
        if (currentBulongDoneCount == totalColor)
        {
            Debug.Log("Done level");
          
            DOVirtual.DelayedCall(.5f, () => {
                currentBulongDoneCount = 0;
                if (state == 1)
                {
                    currentLevel++;
                    state = 0;
                    levelData = levelDataConfig.GetLevelEasy();
                    SetDataLevel(levelData.TotalLine, levelData.TotalBulong, levelData.CountMaxInLine, levelData.TotalOcVit, levelData.BulongFree);
                }
                else
                {
                    state = 1;
                    levelData = levelDataConfig.GetLevelData(currentLevel);
                    SetDataLevel(levelData.TotalLine, levelData.TotalBulong, levelData.CountMaxInLine, levelData.TotalOcVit, levelData.BulongFree);
                }
               
               
            });
           
        }
    }

    public void StartLevel() {
        currentBulongDoneCount = 0;
    }
    #endregion

    public MaterialData GetMaterialData(int colorID) {
        return materitalDataConfig.GetMaterialData(colorID);
    }

    LevelData levelData;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddBulongToLine();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (state == 1)
            {
                levelData = levelDataConfig.GetLevelData(currentLevel);
                SetDataLevel(levelData.TotalLine, levelData.TotalBulong, levelData.CountMaxInLine, levelData.TotalOcVit, levelData.BulongFree);
            }
            else {
                levelData = levelDataConfig.GetLevelEasy();
                SetDataLevel(levelData.TotalLine, levelData.TotalBulong, levelData.CountMaxInLine, levelData.TotalOcVit, levelData.BulongFree);
            }
            
        }
    }
}
[System.Serializable]
public class MaterialOnCount {
    public int colorID;
    public int count;
}
