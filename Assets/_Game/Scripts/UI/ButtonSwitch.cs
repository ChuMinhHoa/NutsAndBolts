using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ButtonSwitch : MonoBehaviour
{
    [SerializeField] Transform pointOff;
    [SerializeField] Transform pointOn;
    [SerializeField] Transform pointMove;
    bool isOn;
    public void SetIsOn(bool isOn) { 
        this.isOn = isOn;
        if (!isOn) pointMove.DOMove(pointOff.position, .1f);
        else pointMove.DOMove(pointOn.position, .1f);
    }
    public void OnChangePoint() {
        isOn = !isOn;
        if (!isOn) pointMove.DOMove(pointOff.position, .1f);
        else pointMove.DOMove(pointOn.position, .1f);
    }
}
