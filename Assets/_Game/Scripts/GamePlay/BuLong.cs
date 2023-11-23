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
    [SerializeField] ParticleSystem myEffect;


    public void SetBulongID(int valueID) { bulongID = valueID; }
    public int GetBulongID() { return bulongID; }
    public void SetLineIndex(int lineIndex) { this.lineIndex = lineIndex; }
    public int GetLineIndex() { return lineIndex; }

    public void InitScaleup() {
        for (int i = 0; i < objs.Count; i++)
        {
            objs[i].gameObject.SetActive(false);
        }
        myEffect.gameObject.SetActive(false);
        isDone = false;
        colliderY = 1.6f;
        Vector3 vectorCenterCollider = myCollider.center;
        Vector3 vectorSizerCollider = myCollider.size;
        Vector3 vectorPointInOut = Vector3.zero;
        for (int i = 0; i < countScaleUp; i++)
        {
            GetObjScale().localPosition = vetorOffset * i + vetorOffsetDe;
            colliderY += vetorOffset.y;
        }
        colliderY += .2f;
        vectorCenterCollider.y = colliderY / 2;
        myCollider.center = vectorCenterCollider;
        vectorSizerCollider.y = colliderY;
        myCollider.size = vectorSizerCollider;

        vectorPointInOut.y = colliderY + vectorSpaceOcVit.y * 2f;
        pointOut.localPosition = vectorPointInOut;

        for (int i = countScaleUp; i < objs.Count; i++)
            objs[i].gameObject.SetActive(false);
        ParticleSystem.ShapeModule shapeModule = myEffect.shape;
        shapeModule.length = colliderY;
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
    public void SetTotalOcVit(int ocVitCount) {
        totalOcVit = ocVitCount;
        countMaxSameColor = totalOcVit / 2 ;
        countScaleUp = (totalOcVit / 2);
    }
    public void SpawnOcVit(int ocVitCount)
    {
        if (isNull)
            return;
        for (int i = 0; i < ocVitCount; i++)
        {
            vectorSpawnOcvit = vetorOffsetDe + vectorSpaceOcVit * i;
            OcVit ocVit = GetOcVit();
            ocVit.InitData(vectorSpawnOcvit);
        }
    }
    int countSameColor;
    int countMaxSameColor;
    bool isLastColor;
    public void SetColorToOcVit(int ocVitIndex) {
        int colorID = GameManager.Instance.gamePlayController.GetColor();
        MaterialData materialData;
        if (ocVitIndex > 0)
        {
            if (listOcVit[ocVitIndex - 1].IsSameColor(colorID))
            {
                countSameColor++;
                isLastColor = GameManager.Instance.gamePlayController.ColorRemainCountCheckIsLastColor();
                if (countSameColor >= countMaxSameColor)
                {
                    if (!isLastColor)
                    {
                        SetColorToOcVit(ocVitIndex);
                        return;
                    }
                    else {
                        colorID = GameManager.Instance.gamePlayController.GetColorSwitch(colorID, this);
                        Debug.Log("Color ID switch: " + colorID);
                        materialData = GameManager.Instance.gamePlayController.GetMaterialData(colorID);
                        listOcVit[ocVitIndex].InitMaterial(materialData);
                        return;
                    }
                }
            }
            else
                countSameColor = 0;
        }
        else countSameColor = 0;
        GameManager.Instance.gamePlayController.MinusColorRemain(colorID);
        materialData = GameManager.Instance.gamePlayController.GetMaterialData(colorID);
        listOcVit[ocVitIndex].InitMaterial(materialData);
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
        isNull = false;
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
        if (isDone || GameManager.Instance.gamePlayController.detectSelect)
            return;
        if (!GameManager.Instance.gamePlayController.onChoosed)
        {
            if (listOcVit.Count == 0)
                return;
            GameManager.Instance.gamePlayController.OnChooseCurrentOcVit(listOcVit[listOcVit.Count - 1], this);
        }
        else
        {
            GameManager.Instance.gamePlayController.OnChooseOtherBulong(this, false);
        }
    }

    public void ChooseSameBulong(float speed, UnityAction actionCallBack) {
        vectorSpawnOcvit = vetorOffsetDe + vectorSpaceOcVit * (listOcVit.Count - 1);
        pointIn.localPosition = vectorSpawnOcvit;
        listOcVit[listOcVit.Count - 1].ChooseIn(pointIn, speed, actionCallBack);
    }

    public void ChooseOtherBulong(OcVit ocVit, float speed, UnityAction actionCallBack, UnityAction actionDoneCallBack = null) {
        listOcVit.Add(ocVit);
        isDone = CheckIsDone();
        if (isDone)
            myEffect.gameObject.SetActive(true);
        ocVit.transform.parent = trsOcVitParent;
        ocVit.ChooseOut(pointOut, speed, ()=> {
            vectorSpawnOcvit = vetorOffsetDe + vectorSpaceOcVit * (listOcVit.Count - 1);
            pointIn.localPosition = vectorSpawnOcvit;
            ocVit.ChooseIn(pointIn, speed,()=>{
                actionCallBack();
                if (isDone && actionDoneCallBack != null)
                    actionDoneCallBack();
            });
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
    public bool CheckBulongHasColor(int colorID) {
        for (int i = 0; i < listOcVit.Count; i++)
        {
            if (colorID == listOcVit[i].GetColor())
                return true;
        }
        return false;
    }

    public int SwitchColorOcVit(int colorID)
    {
        return listOcVit[listOcVit.Count - 1].SwitchColorID(colorID);
    }

    public bool CheckBulongIsDone()
    {
        if (isDone)
        {
            isDone = CheckIsDone();
            myEffect.gameObject.SetActive(isDone);
            return isDone;
        }
        return true;
    }
}
