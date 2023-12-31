using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabButton : BaseButton
{
    protected override void OnButtonClick()
    {
        base.OnButtonClick();
    }

    protected void ShowTab(UITab tabType)
    {
        BasePanel panel = UIManager.Instance.GetPanel(UIPanelType.Tab);
        panel.Show();

        if (panel is TabPanel tabPanel)
        {
            tabPanel.ShowTab(tabType);
        }
    }
}
