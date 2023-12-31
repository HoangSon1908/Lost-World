using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetStoryButton : BaseButton
{
    protected override void OnButtonClick()
    {
        base.OnButtonClick();

        UIManager.Instance.GetPanel(UIPanelType.InfoStory).Show();
    }
}
