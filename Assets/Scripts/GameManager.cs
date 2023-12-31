using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Time")]

    public int StartingTime = 1000;
    public int rulingYears;
    public int rulingDays;
    public int currentYears;
    public int currentDays;

    [Header("Stats")]
    public int publicEsteem;
    public int militaryPower;
    public int economy;
    public int spirituality;
    public int maxStat;
    private int[] Stat;

    public bool isChecked;

    private const string PlayerPrefsDayKey = "CurrentDays";
    private const string PlayerPrefsYearKey = "CurrentYear";

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

            isChecked = false;
        currentYears = PlayerPrefs.GetInt(PlayerPrefsYearKey, StartingTime);
        currentDays = PlayerPrefs.GetInt(PlayerPrefsDayKey);

        rulingDays = Random.Range(11, 51);
        rulingYears = 0;
    }

    private void Start()
    {
        LoadingScreen.Instance.AnimateYearText(currentYears);
    }

    public void ApplySingleEffect(int change1, int change2, int change3, int change4)
    {
        militaryPower = Mathf.Clamp(militaryPower + change1, 0, maxStat);
        publicEsteem = Mathf.Clamp(publicEsteem + change2, 0, maxStat);
        economy = Mathf.Clamp(economy + change3, 0, maxStat);
        spirituality = Mathf.Clamp(spirituality + change4, 0, maxStat);
    }

    public void CheckGameOver()
    {
        if (isChecked)
        {
            return;
        }

        // Preload stat, buffs, keys, and buff types
        Stat = new int[] { militaryPower, publicEsteem, economy, spirituality };
        bool[] buffs = new bool[]
        {
        Data.instance.hasMilitaryBuff,
        Data.instance.hasPublicEsteemBuff,
        Data.instance.hasEconomyBuff,
        Data.instance.hasSpiritualityBuff
        };
        string[] buffKeys = new string[]
        {
        Data.instance.hasMilitaryBuffKey,
        Data.instance.hasPublicEsteemBuffKey,
        Data.instance.hasEconomyBuffKey,
        Data.instance.hasSpiritualityBuffKey
        };

        int DieCardIndex = 0;
        bool gameOverTriggered = false;

        for (int i = 0; i < Stat.Length; i++)
        {
            int statValue = Stat[i];

            // Check if stat is at critical level
            if (statValue == maxStat || statValue == 0)
            {
                // Handle buff removal if necessary
                if (buffs[i])
                {
                    RemoveBuff(buffKeys[i]);
                    ResetStat(i);
                }

                // If stat is maxed out, increment DieCardIndex
                if (statValue == maxStat)
                {
                    DieCardIndex += 4;
                }

                UpdateCurrentDaysAndYears();
                Debug.Log("Game Over!!");

                // Kill the character based on DieCardIndex
                Choice dieCharacter = Data.instance.FindChoiceInDieCard(DieCardIndex);
                Data.instance.KillPlayer(dieCharacter);

                isChecked = true;
                gameOverTriggered = true;
                break; // Exit early since game is over
            }
            DieCardIndex++;
        }

        // Save progress only if game over was triggered
        if (gameOverTriggered)
        {
            PlayerPrefs.SetInt(PlayerPrefsDayKey, currentDays);
            PlayerPrefs.SetInt(PlayerPrefsYearKey, currentYears);
            PlayerPrefs.Save();
        }
    }

    private void RemoveBuff(string buffKey)
    {
        PlayerPrefs.SetInt(buffKey, 0); // Remove buff
        PlayerPrefs.Save();
    }

    private void ResetStat(int index)
    {
        switch (index)
        {
            case 0:
                militaryPower = 50;
                Data.instance.SetUpBuff(index, BuffType.military);
                break;
            case 1:
                publicEsteem = 50;
                Data.instance.SetUpBuff(index, BuffType.publicEsteem);
                break;
            case 2:
                economy = 50;
                Data.instance.SetUpBuff(index, BuffType.economy);
                break;
            case 3:
                spirituality = 50;
                Data.instance.SetUpBuff(index, BuffType.spirituality);
                break;
        }
    }

    private void UpdateCurrentDaysAndYears()
    {
        currentDays = currentDays + rulingDays - (rulingYears * 365);
        currentYears += rulingYears;
    }



    public void ResetElementStats()
    {
        publicEsteem = 50;
        militaryPower = 50;
        economy = 50;
        spirituality = 50;
    }

    public void AddDaysAfterDecision(int day)
    {
        rulingDays += day;
        int Days = currentDays + rulingDays;

        // Nếu rulingDays vượt quá 365
        if (Days >= 365)
        {
            rulingYears = Days / 365;
        }

        RulingDays.instance.UpdateYearsAndDaysUI(rulingDays);
    }
}