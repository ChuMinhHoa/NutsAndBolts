using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PanelLoading : MonoBehaviour
{
    [SerializeField] Transform imgMove;
    [SerializeField] Transform pointStand;
    [SerializeField] Transform pointCenter;
    [SerializeField] Transform pointMove;

    private void OnEnable()
    {
        imgMove.position = pointStand.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            imgMove.DOMove(pointCenter.position, 1f);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            imgMove.DOMove(pointMove.position, 1f);
        }
    }
}
