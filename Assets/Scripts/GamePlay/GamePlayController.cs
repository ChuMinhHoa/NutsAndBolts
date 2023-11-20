using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayController : MonoBehaviour
{
    public static GamePlayController Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    [SerializeField] LevelDataConfig levelDataConfig;

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

    int countTemp;
    int countRemain;
    int indexLine;

  

    private void Start()
    {
        InitBulong();
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
        InitColor();
        InitBulong();
    }
    #region Bu Long
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
        if ((countInLine[countInLine.Count - 1] - countInLine[countInLine.Count - 2]) % 2 == 0 || countInLine[countInLine.Count - 1]==0)
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

        for (int i = 0; i < buLongs.Count - bulongFreeCount; i++)
        {
            //SpawnOcVit(buLongs[i]);
            buLongs[i].SpawnOcVit(ocVitCount);
        }

        for (int i = buLongs.Count - bulongFreeCount; i < buLongs.Count; i++)
        {
            buLongs[i].SetTotalOcVit(ocVitCount);
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
            buLong.SetCountOcVit(ocVitCount);
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
            newBuLong1.SetCountOcVit(ocVitCount);
            newBuLong1.InitScaleup();
            lastPostion.x = vectorPositionSpawn.x;

            vectorPositionSpawn.x *= -1;
            BuLong newBuLong2 = GetBulong();
            newBuLong2.transform.position = vectorPositionSpawn;
            newBuLong2.SetLineIndex(lineIndex);
            newBuLong2.SetCountOcVit(ocVitCount);
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
    public bool onChoosed;
    public void OnChooseCurrentOcVit(OcVit ocVit, BuLong bulong) {
        currentBulong = bulong;
        currentOcvit = ocVit;
        onChoosed = true;
        currentOcvit.ChooseOut(bulong.GetPointInOut());
    }
    public void OnChooseOtherBulong(BuLong bulong) {
        if (currentBulong == bulong)
            ChooseTheSameBulong(bulong);
        else {
            if (bulong.IsFull() || !bulong.IsCanJoin(currentOcvit))
            {
                ChooseTheSameBulong(currentBulong);
                SetDefault();
                return;
            }
            Debug.Log("Choose Other");
            currentBulong.RemoveOcvit(currentOcvit);
            bulong.ChooseOtherBulong(currentOcvit);
            SetDefault();
        }
    }

    void SetDefault() {
        onChoosed = false;

        currentBulong = null;
        currentOcvit = null;
    }

    void ChooseTheSameBulong(BuLong buLong) {
        buLong.ChooseSameBulong();
        onChoosed = false;
        currentBulong = null;
        currentOcvit = null;
    }
    public void OnOutChoose() {
       
    }
    #endregion
    List<int> colorRemaining = new List<int>();
    void InitColor() {
        colorRemaining.Clear();
        for (int i = 0; i < totalColor; i++)
            colorRemaining.Add(ocVitCount);
    }
    public MaterialColor GetColor() {
        int colorIndex = -1;
        while (colorIndex == -1) {
            colorIndex= Random.Range(0, colorRemaining.Count);
            if (colorRemaining[colorIndex] == 0)
                colorIndex = -1;
        }
        colorRemaining[colorIndex]--;
        return (MaterialColor)colorIndex;
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
