using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuLong : MonoBehaviour
{
    [SerializeField] int lineIndex;
    [SerializeField] int countTotalOcVit;
    [SerializeField] int countScaleUp;
    public bool isNull;
    [Header("==============ScaleUp==============")]
    [SerializeField] List<Transform> objs;
    [SerializeField] Transform objScaleUp;
    [SerializeField] Transform trsScaleParent;
    [SerializeField] Vector3 vetorOffset;
    [SerializeField] Vector3 vetorOffsetDe;

    [Header("==============OcVit==============")]
    [SerializeField] Transform trsOcVitParent;
    [SerializeField] OcVit objOcVitPref;
    [SerializeField] Vector3 vectorSpaceOcVit;
    private void Start()
    {
        objs.Add(objScaleUp);
    }
    public void SetLineIndex(int lineIndex) { this.lineIndex = lineIndex; }
    public int GetLineIndex() { return lineIndex; }
    public void SetCountOcVit(int countOcVit) {
        this.countTotalOcVit = countOcVit;
        countScaleUp = (int)(countTotalOcVit / 2);
    }

    public void InitScaleup() {
        for (int i = 0; i < countScaleUp; i++)
        {
            if (i >= objs.Count)
            {
                Transform temp = Instantiate(objScaleUp, trsScaleParent);
                temp.transform.localPosition = vetorOffset * i + vetorOffsetDe;
                objs.Add(temp);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetCountOcVit(4);
            InitScaleup();
        }
    }
}
