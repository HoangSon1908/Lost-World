using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButton : BaseButton
{
    protected override void OnButtonClick()
    {
        base.OnButtonClick();
        SoundManager.Instance.SaveVolume();

        UIManager.Instance.GetPanel(UIPanelType.Tab).Hide();
        UIManager.Instance.GetPanel(UIPanelType.TabButton).Hide();
    }
}
