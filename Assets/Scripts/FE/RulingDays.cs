using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class RulingDays : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rulingDayText;
    [SerializeField] private TextMeshProUGUI currentYearText;
    [SerializeField] private TextMeshProUGUI top1RulingDayText;
    [SerializeField] private TextMeshProUGUI top2RulingDayText;
    [SerializeField] private TextMeshProUGUI top3RulingDayText;

    public List<int> topRulingDays = new List<int>();

    public static RulingDays instance;

    private int previousRulingDays = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        LoadTopRulingDays();
    }

    public void GameOver()
    {
        CheckAndUpdateTopRulingDays();
    }

    void CheckAndUpdateTopRulingDays()
    {
        int currentRulingDays = GameManager.Instance.rulingDays;
        topRulingDays.Add(currentRulingDays);
        topRulingDays.Sort((a, b) => b.CompareTo(a));

        if (topRulingDays.Count > 3)
        {
            topRulingDays.RemoveRange(3, topRulingDays.Count - 3);
        }

        SaveTopRulingDays();
    }

    void SaveTopRulingDays()
    {
        for (int i = 0; i < topRulingDays.Count; i++)
        {
            PlayerPrefs.SetInt($"TopRulingDay{i + 1}", topRulingDays[i]);
        }
        PlayerPrefs.Save();
    }

    void LoadTopRulingDays()
    {
        topRulingDays.Clear();

        for (int i = 1; i <= 3; i++)
        {
            int rulingDay = PlayerPrefs.GetInt($"TopRulingDay{i}", 0); // Use default -1 for empty slots
            if (rulingDay >0)
            {
                topRulingDays.Add(rulingDay);
            }
        }

        UpdateTopRulingDayText();
    }

    public void UpdateTopRulingDayText()
    {
        UpdateSingleTopText(top1RulingDayText, 0);
        UpdateSingleTopText(top2RulingDayText, 1);
        UpdateSingleTopText(top3RulingDayText, 2);
    }

    private void UpdateSingleTopText(TextMeshProUGUI textUI, int index)
    {
        if (textUI != null && topRulingDays.Count > index)
        {
            int years = topRulingDays[index] / 365;
            int days = topRulingDays[index] % 365;

            if (years > 0)
            {
                textUI.text = $"Cầm quyền trong vòng\n{years} năm {days} ngày";
            }
            else
            {
                textUI.text = $"Cầm quyền trong vòng\n{days} ngày";
            }
        }
        else
        {
            textUI.text = "N/A";
        }
    }


    public void UpdateYearsAndDaysUI(int targetDays)
    {
        int currentYears =GameManager.Instance.currentYears;
        int rulingYears = GameManager.Instance.rulingYears;

        DOTween.To(() => previousRulingDays, x => previousRulingDays = x, targetDays, 1f)
            .OnUpdate(() =>
            {
                // Update year text
                currentYearText.text = $"Năm {currentYears+rulingYears}";

                // Calculate the ruling years and display ruling days incrementally
                if (previousRulingDays > 365)
                    rulingDayText.text = $"{previousRulingDays / 365} năm và {previousRulingDays%365} ngày";
                else
                    rulingDayText.text = $"{previousRulingDays} ngày";
            })
            .OnComplete(() =>
            {
                previousRulingDays = targetDays; // Finalize the value
            });
    }
}
