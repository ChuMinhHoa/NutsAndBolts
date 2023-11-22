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
    MaterialData materialData;
    Material[] materials = new Material[1];
    bool materialInited;
    public void InitData(Vector3 pointSpawn)
    {
        myEffect.Stop();
        transform.localPosition = pointSpawn;
    }

    public void InitMaterial(MaterialData materialData) {
        this.materialData = materialData;
        materials[0] = materialData.material;
        meshRenderer.materials = materials;
        materialInited = true;
    }

    public void ChooseOut(Transform pointOut, float speed, UnityAction actionCallBack = null) {
        anim.SetBool("RotateOut", true);
        transform.DOMove(pointOut.position, speed).OnComplete(()=> {
            anim.SetBool("RotateOut", false);
            if (actionCallBack != null)
                actionCallBack();
        });
    }

    public void ChooseIn(Transform pointIn, float speed, UnityAction actionCallBack = null) {
        anim.SetBool("RotateIn", true);
        transform.DOMove(pointIn.position, speed).OnComplete(() => {
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
        InitMaterial(materialData);
        return currentColorID;
    }
}
