using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StatManager : MonoBehaviour
{
    private Color defaultColor = Color.white;
    private Color positiveEffectColor = Color.green;
    private Color negativeEffectColor = Color.red;
    public Stat stat1Bar;
    public Stat stat2Bar;
    public Stat stat3Bar;
    public Stat stat4Bar;
    public Image dot1;
    public Image dot2;
    public Image dot3;
    public Image dot4;
    public Image tri1;
    public Image tri2;
    public Image tri3;
    public Image tri4;
    public static StatManager instance { get; private set; }

    void Awake() 
    {
        if (instance == null) 
        {
            instance = this;
        } 
        else 
        {
            Destroy(gameObject);
        }
    }

    void Start() {
        ApplyStatChanges();
    }

    public void ApplyBuffSeeTheFuture(int change1, int change2, int change3, int change4) {
        ShowTrianglePreview(tri1, change1);
        ShowTrianglePreview(tri2, change2);
        ShowTrianglePreview(tri3, change3);
        ShowTrianglePreview(tri4, change4);
    }

    public void ClearBuffSeeTheFuture(int change1, int change2, int change3, int change4) 
    {
        HideTrianglePreview(tri1, change1);
        HideTrianglePreview(tri2, change2);
        HideTrianglePreview(tri3, change3);
        HideTrianglePreview(tri4, change4);
    }

    public void PreviewStatChange(int change1, int change2, int change3, int change4) 
    {
        ShowDotPreview(dot1, change1);
        ShowDotPreview(dot2, change2);
        ShowDotPreview(dot3, change3);
        ShowDotPreview(dot4, change4);
    }

    public void ClearPreviewStatChange(int change1, int change2, int change3, int change4)  
    {
        HideDotPreview(dot1, change1);
        HideDotPreview(dot2, change2);
        HideDotPreview(dot3, change3);
        HideDotPreview(dot4, change4);
    }
    public void ApplyStatChanges() {
        stat1Bar.UpdateStatBar(GameManager.Instance.militaryPower, GameManager.Instance.maxStat);
        stat2Bar.UpdateStatBar(GameManager.Instance.publicEsteem, GameManager.Instance.maxStat);
        stat3Bar.UpdateStatBar(GameManager.Instance.economy, GameManager.Instance.maxStat);
        stat4Bar.UpdateStatBar(GameManager.Instance.spirituality, GameManager.Instance.maxStat);
    }

    private void ShowTrianglePreview(Image triangle, int statEffect) {
        if (statEffect > 0) 
        {
            triangle.DOFade(1f, 0.5f);
            if (statEffect == 15) 
            {
                triangle.GetComponent<RectTransform>().DOScale(1.5f, 0.5f);
            }
        }
        else if (statEffect < 0)
        {
            triangle.DOFade(1f, 0.5f);
            triangle.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, 180), 0.5f);
            if (statEffect == -15) 
            {
                triangle.GetComponent<RectTransform>().DOScale(1.5f, 0.5f);
            }
        }
    }

    private void HideTrianglePreview(Image triangle, int statEffect) 
    {
        if (statEffect!= 0 )
            triangle.DOFade(0f, 0.3f);
        triangle.GetComponent<RectTransform>().DORotate(new Vector3(0, 0, 0), 0.5f);
        triangle.GetComponent<RectTransform>().DOScale(1, 0.3f);
    }

    private void ShowDotPreview(Image dot, int statEffect) 
    {
        if (statEffect != 0)
        {
            if (statEffect == 15 || statEffect == -15)
            {
                dot.GetComponent<RectTransform>().DOScale(1.5f, 0.5f);
            }
            dot.DOFade(1f, 0.5f);
        }
    }

    private void HideDotPreview(Image dot, int statEffect) 
    {
        if (statEffect != 0)
            dot.DOFade(0f, 0.3f);
        dot.GetComponent<RectTransform>().DOScale(1f, 0.5f);
    }

    public void HideAllDots() 
    {
        dot1.DOFade(0f, 0.3f);
        dot2.DOFade(0f, 0.3f);
        dot3.DOFade(0f, 0.3f);
        dot4.DOFade(0f, 0.3f);
    }

    public void HideAllTriangle() 
    {
        tri1.DOFade(0f, 0.3f);
        tri2.DOFade(0f, 0.3f);
        tri3.DOFade(0f, 0.3f);
        tri4.DOFade(0f, 0.3f);
    }
}
