using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIPlayerRaceInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerNameTxt;
    [SerializeField] TextMeshProUGUI processTxt;
    [SerializeField] TextMeshProUGUI txtLevel;
    [SerializeField] Slider processSlider;
    [SerializeField] Color playerColor;
    private void OnEnable()
    {
        processSlider.value = 0;
    }
    public void SetUp(int process = 0, bool player = false, string name = "poco")
    {
        DOVirtual.Float(processSlider.value, (float)process / (float)ConstantValue.LEVEL_RACE_GOAL, .25f, (valuef) => {
            processSlider.value = valuef;
        });

        if (player )
        {
            playerNameTxt.text = "YOU";
            playerNameTxt.color = playerColor;
        }
        else
        {
            playerNameTxt.text = name;
            playerNameTxt.color = Color.white;
        }
        processTxt.text = $"({process}/{ConstantValue.LEVEL_RACE_GOAL})";
        txtLevel.text = process.ToString();
    }
}
