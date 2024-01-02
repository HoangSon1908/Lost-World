using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingButton : TabButton
{
    protected override void OnButtonClick()
    {
        base.OnButtonClick();

        ShowTab(UITab.Setting);
    }
}
