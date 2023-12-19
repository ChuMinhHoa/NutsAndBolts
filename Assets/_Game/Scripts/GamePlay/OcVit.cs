using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System;

public class OcVit : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] ParticleSystem myEffect;
    [SerializeField] GameObject objSecret;
    [SerializeField] OutlinePack outLinePack;
    public bool isSecret;
    MaterialData materialData;
    Material[] materials = new Material[1];
    Color colorOutline;
    public int colorID;
    public void InitData(Vector3 pointSpawn, bool isSecret = false)
    {
        myEffect.Stop();
        transform.localPosition = pointSpawn;
        objSecret.SetActive(isSecret);
        this.isSecret = isSecret;
    }

    public void InitMaterial(MaterialData materialData, int colorID) {
        this.colorID = colorID;
        this.materialData = materialData;
        materials[0] = materialData.material;
        meshRenderer.materials = materials;
        colorOutline = materialData.colorOutline;
        outLinePack.OutlineColor = colorOutline;
    }

    public void ChooseOut(Vector3 pointOut, float speed, UnityAction actionCallBack = null) {
        
        anim.SetBool("RotateOut", true);
        transform.DOMove(pointOut, speed).OnComplete(()=> {
            outLinePack.enabled = true;
            anim.SetBool("RotateOut", false);
            if (actionCallBack != null)
                actionCallBack();
        });
    }
    public void DoJump(Transform pointJump, float speed, UnityAction actionCallBack = null) {
        anim.SetBool("RotateIn", true);
        transform.DOJump(pointJump.position, 5, 1, speed).OnComplete(() =>
        {
            anim.SetBool("RotateIn", false);
            if (actionCallBack != null)
                actionCallBack();
        });
    }

    public void ChooseIn(Transform pointIn, float speed, UnityAction actionCallBack = null) {

        anim.SetBool("RotateIn", true);
        transform.DOMove(pointIn.position, speed).OnComplete(() => {
            outLinePack.enabled = false;
            anim.SetBool("RotateIn", false);
            myEffect.Play();
            if (actionCallBack != null)
                actionCallBack();
        });
    }

    public bool IsSameColor(int colorID)
    {
        return colorID == materialData.colorID;

    }

    public int GetColor()
    {
        return materialData.colorID;
    }

    public int SwitchColorID(int colorID)
    {
        int currentColorID = materialData.colorID;
        materialData = GameManager.Instance.gamePlayController.GetMaterialData(colorID);
        InitMaterial(materialData, colorID);
        return currentColorID;
    }

    public void OffSecretMode() { 
        objSecret.SetActive(false);
        isSecret = false;
    }
    public bool IsOnSecretMode() { return isSecret; }
}
