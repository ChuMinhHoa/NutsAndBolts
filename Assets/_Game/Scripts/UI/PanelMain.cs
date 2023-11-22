using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelMain : UIPanel
{



    public override void Awake()
    {
        panelType = UIPanelType.PanelMain;
        base.Awake();
    }
}
