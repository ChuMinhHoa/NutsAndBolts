using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using System;

public class PanelLoading : UIPanel
{
    [SerializeField] Image imgColor;
    [SerializeField] Transform imgMove;
    [SerializeField] Transform pointStand;
    [SerializeField] Transform pointCenter;
    [SerializeField] Transform pointMove;
    [SerializeField] CanvasGroup cGroup;
    [SerializeField] AnimationCurve myCurveMove;
    [SerializeField] AnimationCurve myCurveAlpha;
    UnityAction actionCallBack;
    public override void Awake()
    {
        panelType = UIPanelType.PanelLoading;
        base.Awake();   
    }
    private void OnEnable()
    {
        imgMove.position = pointStand.position;
        cGroup.alpha = 1f;
        float r = UnityEngine.Random.Range(0, 255);
        float g = UnityEngine.Random.Range(0, 255);
        float b = UnityEngine.Random.Range(0, 255);
        float a = UnityEngine.Random.Range(0, 255);
        //imgColor.color = new Color(r, g, b, a);
    }

    public void LoadIn() {
        imgMove.DOMove(pointCenter.position, 1f).OnComplete(()=> {
            actionCallBack();
            DOVirtual.DelayedCall(1f, LoadOut);
        }).SetEase(myCurveMove);
    }
    public void LoadOut() {

        cGroup.DOFade(0, 1f).SetEase(myCurveAlpha);
        imgMove.DOMove(pointMove.position, 1f).OnComplete(() =>
        {
            UIManager.instance.ClosePanelLoading();
        }).SetEase(myCurveMove);
    }

    public void SetActionCallBack(UnityAction actionCallback)
    {
        this.actionCallBack = actionCallback;
    }
}
