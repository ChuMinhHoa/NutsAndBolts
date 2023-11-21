using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

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
    [SerializeField] bool isDone;

    public void SetBulongID(int valueID) { bulongID = valueID; }
    public int GetBulongID() { return bulongID; }
    public void SetLineIndex(int lineIndex) { this.lineIndex = lineIndex; }
    public int GetLineIndex() { return lineIndex; }
    public void SetCountOcVit(int countOcVit) {
        this.countTotalOcVit = countOcVit;
        countScaleUp = (int)(countTotalOcVit / 2);
    }

    public void InitScaleup() {
        isDone = false;
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
            int colorID = GamePlayController.Instance.GetColor(ocVitCount / 2);
            MaterialData materialData = GamePlayController.Instance.GetMaterialData(colorID);
            ocVit.InitData(vectorSpawnOcvit, materialData);
        }
        GamePlayController.Instance.ResetSameColor();
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
    public void ReSetOcVit() {
        for (int i = 0; i < listOcVit.Count; i++)
        {
            Destroy(listOcVit[i].gameObject);
        }
        listOcVit.Clear();
    }

    #endregion

    public Transform GetPointInOut() { return pointOut; }

    private void OnMouseUp()
    {
        if (isDone || GamePlayController.Instance.detectSelect)
            return;
        if (!GamePlayController.Instance.onChoosed)
        {
            if (listOcVit.Count == 0)
                return;
            GamePlayController.Instance.OnChooseCurrentOcVit(listOcVit[listOcVit.Count - 1], this);
        }
        else
            GamePlayController.Instance.OnChooseOtherBulong(this, false);
    }

    public void ChooseSameBulong(float speed, UnityAction actionCallBack) {
        vectorSpawnOcvit = vetorOffsetDe + vectorSpaceOcVit * (listOcVit.Count - 1);
        pointIn.localPosition = vectorSpawnOcvit;
        listOcVit[listOcVit.Count - 1].ChooseIn(pointIn, speed, actionCallBack);
    }

    public void ChooseOtherBulong(OcVit ocVit, float speed, UnityAction actionCallBack) {
        listOcVit.Add(ocVit);
        isDone = CheckIsDone();
        if (isDone)
        {
            GamePlayController.Instance.OnDoneBulong();
        }
        ocVit.transform.parent = trsOcVitParent;
        ocVit.ChooseOut(pointOut, speed, ()=> {
            vectorSpawnOcvit = vetorOffsetDe + vectorSpaceOcVit * (listOcVit.Count - 1);
            pointIn.localPosition = vectorSpawnOcvit;
            ocVit.ChooseIn(pointIn, speed, actionCallBack);
        });
    }

    bool CheckIsDone() {
        if (listOcVit.Count < totalOcVit)
            return false;

        for (int i = 1; i < listOcVit.Count; i++)
        {
            if (!listOcVit[0].IsSameColor(listOcVit[i].GetColor()))
                return false;
        }
        
        return true;
    }

    public bool IsFull() { return (listOcVit.Count == totalOcVit && listOcVit.Count > 0); }
    public bool IsCanJoin(OcVit ocvit) {
        if (listOcVit.Count == 0)
            return true;
        return listOcVit[listOcVit.Count - 1].IsSameColor(ocvit.GetColor());
    }
    public OcVit IsSameColorWithCurrentOcvit(OcVit ocVit) {
        if (listOcVit.Count == 0)
            return null;
        if (listOcVit[listOcVit.Count - 1].IsSameColor(ocVit.GetColor()))
            return listOcVit[listOcVit.Count - 1];
        return null;
    }
    public void RemoveOcvit(OcVit ocvit)
    {
        listOcVit.Remove(ocvit);
    }
}
