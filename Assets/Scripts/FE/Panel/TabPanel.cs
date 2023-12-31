using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TabPanel : BasePanel
{
    [Header("Elements")]
    [SerializeField] private RectTransform containerTab;
    [SerializeField] private RectTransform tabContainer;
    private PageSwiper pageSwiper;

    [Header("Properties")]
    [SerializeField] private float duration = .1f;
    [SerializeField] private Ease scaleTtransition;

    protected override void Awake()
    {
        base.Awake();

        UIManager.Instance.ResgisterPanel(UIPanelType.Tab, this);
        canvasGroup = GetComponent<CanvasGroup>();
        pageSwiper = GetComponentInChildren<PageSwiper>();

        containerTab.localScale = Vector3.zero;
    }

    public override void Hide()
    {
        base.Hide();
        canvasGroup.DOFade(0, duration);
        containerTab.DOScale(Vector3.zero, duration)
            .SetEase(scaleTtransition)
            .OnComplete(() =>
            {
                Deactive();
            });
    }

    public override void Show()
    {
        base.Show();
        canvasGroup.DOFade(1, duration);
        containerTab.DOScale(Vector3.one, duration)
            .SetEase(scaleTtransition);
    }

    public void ShowTab(UITab tabType)
    {
        pageSwiper.SetIndex((int)tabType);
        switch (tabType)
        {
            case UITab.Setting:
                tabContainer.DOAnchorPosX(0, 0);
                break;

            case UITab.Process:
                tabContainer.DOAnchorPosX(-391, 0);
                break;

            case UITab.Effect:
                tabContainer.DOAnchorPosX(-782, 0);
                break;
        }
    }
}
