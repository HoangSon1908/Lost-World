using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabButtonPanel : BasePanel
{
    [SerializeField] private RectTransform tabButtonContainer;
    [SerializeField] private float duration = .1f;
    [SerializeField] private Ease effect;
    private float widthContainer;
    protected override void Awake()
    {
        base.Awake();

        UIManager.Instance.ResgisterPanel(UIPanelType.TabButton, this);
        widthContainer = tabButtonContainer.rect.width;
    }

    public override void Hide()
    {
        base.Hide();
        tabButtonContainer.DOAnchorPosX(widthContainer, duration)
            .SetEase(effect)
            .OnComplete(() =>
            {
                Deactive();
            });
    }

    public override void Show()
    {
        base.Show();
        tabButtonContainer.DOAnchorPosX(0, duration)
            .SetEase(effect);
    }
}
