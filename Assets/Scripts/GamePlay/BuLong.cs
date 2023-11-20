using System;
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
    [SerializeField] BoxCollider myCollider; //y = 2.8f * i + 1.6f; Pivot.y = y/2;
    [SerializeField] List<Transform> objs;
    [SerializeField] Transform objScaleUp;
    [SerializeField] Transform trsScaleParent;
    [SerializeField] Vector3 vetorOffset;
    [SerializeField] Vector3 vetorOffsetDe;
    float colliderY;

    [Header("==============ListOcVit==============")]
    public int bulongID;
    [SerializeField] Transform trsOcVitParent;
    [SerializeField] OcVit objOcVitPref;
    [SerializeField] List<OcVit> listOcVit;
    [SerializeField] Vector3 vectorSpaceOcVit;
    [SerializeField] int totalOcVit;

    [Header("==============ControllMove==============")]
    [SerializeField] Transform pointOut; //1.6f+2.8f*i+vectorSpaceOcVit.y*2;
    [SerializeField] Transform pointIn; 

    public void SetBulongID(int valueID) { bulongID = valueID; }
    public int GetBulongID() { return bulongID; }
    public void SetLineIndex(int lineIndex) { this.lineIndex = lineIndex; }
    public int GetLineIndex() { return lineIndex; }
    public void SetCountOcVit(int countOcVit) {
        this.countTotalOcVit = countOcVit;
        countScaleUp = (int)(countTotalOcVit / 2);
    }

    public void InitScaleup() {
        colliderY = 1.6f;
        Vector3 vectorCenterCollider = myCollider.center;
        Vector3 vectorSizerCollider = myCollider.size;
        Vector3 vectorPointInOut = Vector3.zero;
        for (int i = 0; i < countScaleUp; i++)
        {
            if (i >= objs.Count)
                GetObjScale().localPosition = vetorOffset * i + vetorOffsetDe;
            colliderY += 2.8f;
        }

        vectorCenterCollider.y = colliderY / 2;
        myCollider.center = vectorCenterCollider;
        vectorSizerCollider.y = colliderY;
        myCollider.size = vectorSizerCollider;

        vectorPointInOut.y = colliderY + vectorSpaceOcVit.y * 2f;
        pointOut.localPosition = vectorPointInOut;

        for (int i = countScaleUp; i < objs.Count; i++)
            objs[i].gameObject.SetActive(false);
    }

    Transform GetObjScale() {
        for (int i = 0; i < objs.Count; i++)
        {
            if (!objs[i].gameObject.activeSelf)
            {
                objs[i].gameObject.SetActive(true);
                return objs[i];
            }
        }
        Transform temp = Instantiate(objScaleUp, trsScaleParent);
        temp.gameObject.SetActive(true);
        objs.Add(temp);
        return temp;
    }

    #region Oc Vit
    public void SetTotalOcVit(int ocVitCount) { totalOcVit = ocVitCount; }
    public void SpawnOcVit(int ocVitCount)
    {
        totalOcVit = ocVitCount;
        for (int i = 0; i < ocVitCount; i++)
        {
            vectorSpawnOcvit = vetorOffsetDe + vectorSpaceOcVit * i;
            OcVit ocVit = GetOcVit();
            MaterialColor mColor = GamePlayController.Instance.GetColor();
            ocVit.InitData(vectorSpawnOcvit, mColor);
        }
    }
    Vector3 vectorSpawnOcvit;
    OcVit GetOcVit()
    {
        for (int i = 0; i < listOcVit.Count; i++)
        {
            if (!listOcVit[i].gameObject.activeSelf)
            {
                listOcVit[i].gameObject.SetActive(true);
                return listOcVit[i];
            }
        }
        OcVit ocVit = Instantiate(objOcVitPref, trsOcVitParent);
        listOcVit.Add(ocVit);
        return ocVit;
    }

    #endregion

    public Transform GetPointInOut() { return pointOut; }

    private void OnMouseUp()
    {
        if (!GamePlayController.Instance.onChoosed)
        {
            if (listOcVit.Count == 0)
                return;
            GamePlayController.Instance.OnChooseCurrentOcVit(listOcVit[listOcVit.Count - 1], this);
        }
        else
            GamePlayController.Instance.OnChooseOtherBulong(this);
    }

    public void ChooseSameBulong() {
        vectorSpawnOcvit = vetorOffsetDe + vectorSpaceOcVit * (listOcVit.Count - 1);
        pointIn.localPosition = vectorSpawnOcvit;
        listOcVit[listOcVit.Count - 1].ChooseIn(pointIn);
    }

    public void ChooseOtherBulong(OcVit ocVit) {
        listOcVit.Add(ocVit);
        ocVit.transform.parent = trsOcVitParent;
        ocVit.ChooseOut(pointOut, ()=> {
            vectorSpawnOcvit = vetorOffsetDe + vectorSpaceOcVit * (listOcVit.Count - 1);
            pointIn.localPosition = vectorSpawnOcvit;
            ocVit.ChooseIn(pointIn);
        });
    
    }

    public bool IsFull() { return (listOcVit.Count == totalOcVit && listOcVit.Count > 0); }
    public bool IsCanJoin(OcVit ocvit) {
        if (listOcVit.Count == 0)
            return true;
        Debug.Log(listOcVit[listOcVit.Count - 1].GetColor());
        Debug.Log(ocvit.GetColor());
        return listOcVit[listOcVit.Count - 1].IsSameColor(ocvit.GetColor());
    }

    public void RemoveOcvit(OcVit ocvit)
    {
        listOcVit.Remove(ocvit);
    }
}
