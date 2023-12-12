using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerRaceInfo : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerNameTxt;
    [SerializeField] TextMeshProUGUI processTxt;
    [SerializeField] Slider processSlider;
    [SerializeField] Color playerColor;
    
    public void SetUp(int process = 0, bool player = false, string name = "poco")
    {
        processSlider.value = (float)process / (float)ConstantValue.LEVEL_RACE_GOAL;
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
        processTxt.text = $"{process}/{ConstantValue.LEVEL_RACE_GOAL}";
    }
}
