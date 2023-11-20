using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OcVit : MonoBehaviour
{
    [SerializeField] Animator anim;
    public void InitData(Vector3 pointSpawn) {
        transform.localPosition = pointSpawn;
    }

    public void ChooseOut(Transform pointOut) {
        Debug.Log("OnChooseOut");
        anim.SetBool("RotateOut", true);
        transform.DOMove(pointOut.position, .25f).OnComplete(()=> {
            anim.SetBool("RotateOut", false);
        });
    }
}
