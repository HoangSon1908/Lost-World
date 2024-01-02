using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessButton : TabButton
{
    protected override void OnButtonClick()
    {
        base.OnButtonClick();

        ShowTab(UITab.Process);
    }
}
