using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System;

public enum MaterialColor { 
    Red,
    Green,
    Purple,
    Black,
    Ping,
    Grey
}
public class OcVit : MonoBehaviour
{
    [SerializeField] MaterialColor mColor;
    [SerializeField] Animator anim;
    [SerializeField] float speed;
    public void InitData(Vector3 pointSpawn, MaterialColor mColor) {
        this.mColor = mColor;
        transform.localPosition = pointSpawn;
    }

    public void ChooseOut(Transform pointOut, UnityAction actionCallBack = null) {
        anim.SetBool("RotateOut", true);
        transform.DOMove(pointOut.position, speed).OnComplete(()=> {
            anim.SetBool("RotateOut", false);
            if (actionCallBack != null)
                actionCallBack();
        });
    }

    public void ChooseIn(Transform pointIn, UnityAction actionCallBack = null) {
        anim.SetBool("RotateIn", true);
        transform.DOMove(pointIn.position, speed).OnComplete(() => {
            anim.SetBool("RotateIn", false);
            if (actionCallBack != null)
                actionCallBack();
        });
    }

    public bool IsSameColor(MaterialColor color)
    {
        return color == mColor;

    }

    public MaterialColor GetColor()
    {
        return mColor;
    }
}
