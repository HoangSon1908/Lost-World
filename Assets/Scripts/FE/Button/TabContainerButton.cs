using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TabContainerButton : BaseButton, IDragHandler, IEndDragHandler
{
    private float difference;
    public void OnDrag(PointerEventData eventData)
    {
        difference = eventData.pressPosition.x - eventData.position.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (difference > 0)
        {
            UIManager.Instance.GetPanel(UIPanelType.TabButton).Show();
        }
    }

    protected override void OnButtonClick()
    {
        base.OnButtonClick();

        UIManager.Instance.GetPanel(UIPanelType.TabButton).Show();
    }
}
