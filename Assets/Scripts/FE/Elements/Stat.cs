using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class Stat : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI percentText;
    [SerializeField] private Image iconImage;

    [Header("Propeties")]
    [SerializeField] private float fillDuration = 1f;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color increaseColor = Color.green;
    [SerializeField] private Color decreaseColor = Color.red;
    [SerializeField] private Ease effect;
    private float previousValue;

    public void UpdateStatBar(float currentValue, float maxValue)
    {
        float targetFillAmount = currentValue / maxValue; 

        if (currentValue > previousValue)
        {
            iconImage.color = increaseColor;
        }
        else if (currentValue < previousValue)
        {
            iconImage.color = decreaseColor;
        }

        iconImage.DOFillAmount(targetFillAmount, fillDuration)
            .OnComplete(() => iconImage.color = normalColor);  

        if (percentText != null)
        {
            percentText.text = Mathf.RoundToInt(currentValue).ToString() + "%";
        }
        previousValue = currentValue;
    }
}
