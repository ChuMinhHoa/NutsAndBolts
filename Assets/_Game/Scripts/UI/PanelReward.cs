using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIAnimation;

public class PanelReward : UIPanel
{
    public override void Awake()
    {
        panelType = UIPanelType.PanelReward;
        base.Awake();
    }
}
