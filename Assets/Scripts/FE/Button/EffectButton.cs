using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectButton : TabButton
{
    protected override void OnButtonClick()
    {
        base.OnButtonClick();

        ShowTab(UITab.Effect);
    }
}
