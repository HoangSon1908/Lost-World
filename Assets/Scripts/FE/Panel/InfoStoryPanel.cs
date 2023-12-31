using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoStoryPanel : BasePanel
{
    [SerializeField] private Ease scaleTransition;
    [SerializeField] private float duration = .1f;
    protected override void Awake()
    {
        base.Awake();

        UIManager.Instance.ResgisterPanel(UIPanelType.InfoStory, this);

        transform.localScale = Vector3.zero;
    }

    public override void Hide()
    {
        base.Hide();
        canvasGroup.DOFade(0f, duration);
        transform.DOScale(Vector3.zero, duration)
            .SetEase(scaleTransition)
            .OnComplete(()=> 
            {
                Deactive();
            });
    }

    public override void Show()
    {
        base.Show();

        canvasGroup.DOFade(1f, duration);
        transform.DOScale(Vector3.one, duration)
            .SetEase(scaleTransition);
    }
}
