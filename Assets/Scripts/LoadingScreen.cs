using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Image loadingImage;
    [SerializeField] private Button actionButton;
    [SerializeField] private TextMeshProUGUI currentYearText;

    public static LoadingScreen Instance;

    public int startingYear = 1000;

    [Header("UI Elements for Sound Manager")]
    [SerializeField] private Slider BGSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private Toggle BGMuteToggle;
    [SerializeField] private Toggle SFXToggle;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DOTween.PauseAll();
        actionButton.onClick.AddListener(OnActionClick);

        UpdateSoundManager();
    }

    private void UpdateSoundManager()
    {
        SoundManager.Instance.backgroundMusicSlider = BGSlider;
        SoundManager.Instance.sfxSlider = SFXSlider;
        SoundManager.Instance.backgroundMusicMuteToggle = BGMuteToggle;
        SoundManager.Instance.sfxMuteToggle = SFXToggle;

        SoundManager.Instance.SetupSoundManager();
    }

    public void AnimateYearText(int toYear)
    {
        DOTween.To(() => startingYear, x => {
            startingYear = x;
            currentYearText.text = $"Năm\n {startingYear} trước công nguyên";
        }, toYear, 0.5f) 
        .SetEase(Ease.Linear);
    }

    void OnActionClick()
    {
        StartGame();
    }

    void StartGame()
    {
        FadeOutLoadingImage();
    }

    public void GameOver()
    {
        loadingImage.gameObject.SetActive(true);
        FadeInLoadingImage();
    }

    void FadeOutLoadingImage()
    {
        // Create a DOTween Sequence
        Sequence fadeSequence = DOTween.Sequence();

        // Get all Graphic components (including Image, Text, TextMeshProUGUI) under the loadingImage
        Graphic[] graphics = loadingImage.GetComponentsInChildren<Graphic>();

        // Fade out each graphic component and add it to the sequence
        foreach (Graphic graphic in graphics)
        {
            fadeSequence.Join(graphic.DOFade(0, 0.5f).SetUpdate(true)); // Using Join to run all fades in parallel
        }

        // After all fade animations complete, run the following actions
        fadeSequence.OnComplete(() =>
        {
            DOTween.PlayAll();
            Card.Instance.AnimationCardIn();
            RulingDays.instance.UpdateYearsAndDaysUI(GameManager.Instance.rulingDays);
            loadingImage.gameObject.SetActive(false);
        });

        // Play the sequence
        fadeSequence.Play();
    }


    void FadeInLoadingImage()
    {
        Sequence fadeInSequence = DOTween.Sequence();

        // Get all Graphic components (including Image, Text, TextMeshProUGUI) under the loadingImage
        Graphic[] graphics = loadingImage.GetComponentsInChildren<Graphic>();

        // Fade in each graphic component
        foreach (Graphic graphic in graphics)
        {
            fadeInSequence.Join(graphic.DOFade(1, 0.5f).SetUpdate(true));
        }

        fadeInSequence.OnComplete(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });

        fadeInSequence.Play();
    }
}
