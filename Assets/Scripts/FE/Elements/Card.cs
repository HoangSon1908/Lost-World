using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
{
    public static Card Instance;

    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI topAnswer;
    [SerializeField] private TextMeshProUGUI bottomAnswer;

    [Header("AnimationCardIn")]
    // Khai báo list thẻ phụ
    [SerializeField] private List<RectTransform> cardList = new List<RectTransform>();
    // Khoảng delay giữa các thẻ bài
    [SerializeField] float delayBetweenCards = 0.3f;
    [SerializeField] private GameObject ListCard;

    [Header("Stats")]
    public RulingDays rulingDays;
    private float yPos;
    private RectTransform rect;
    private Vector2 offset;
    private bool isDraggingUp = false;
    private bool isDraggingDown = false;
    private bool isLocked = false;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AnimationCardIn()
    {
        // Di chuyển từng thẻ bài vào màn hình với delay
        for (int i = 0; i < cardList.Count; i++)
        {
            RectTransform card = cardList[i];

            // Đặt vị trí ban đầu của thẻ bài nằm ngoài màn hình
            card.anchoredPosition = new Vector2(-Screen.width, 0);

            // Tạo chuỗi hoạt ảnh
            Sequence sequence = DOTween.Sequence();

            // Đặt delay cho mỗi thẻ bài
            sequence.SetDelay(i * delayBetweenCards);

            // Phát âm thanh cho mỗi thẻ sau khi delay tương ứng cho từng thẻ
            sequence.AppendCallback(() =>
            {
                // Kiểm tra xem âm thanh có đang phát không
                if (SoundManager.Instance != null)
                {
                    SoundManager.Instance.PlaySFX(SoundManager.SFXType.CardSwipe);
                }
            });

            // Sau đó di chuyển thẻ bài về vị trí X = 0
            sequence.Append(card.DOAnchorPosX(0, 0.5f).SetEase(Ease.OutBack, 0.8f));

            // Gọi ResetCard() sau khi hoạt ảnh của thẻ cuối cùng kết thúc
            if (i == cardList.Count - 1)
            {
                sequence.OnComplete(() =>
                {
                    ListCard.SetActive(false);
                    Data.instance.MakeDecision();
                });
            }
        }
    }





    public void OnPointerDown(PointerEventData eventData)
    {
        // Convert screen position to local position within the rect
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect.root as RectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);

        // Calculate offset from the local point within the rect
        offset = rect.anchoredPosition - localPoint;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Convert screen position to local position within the rect
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect.root as RectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);

        rect.anchoredPosition = new Vector2(0, localPoint.y + offset.y);
        yPos = localPoint.y + offset.y;
        CalculateFadeText();

        if (isLocked) return;

        
        
        if (yPos > 0 && !isDraggingUp) 
        {
            isDraggingUp = true;
            isDraggingDown = false;
            isLocked = true;
            Invoke(nameof(UnlockPreview), 0.25f);
            
            Choice choice = Data.instance.CurrentChoice;

            if (TabData.instance.seeTheFuture) {
                StatManager.instance.ClearBuffSeeTheFuture(
                    choice.militaryEffect2,
                    choice.publicEsteem2,
                    choice.economy2,
                    choice.spiritualityEffect2
                );
                StatManager.instance.ApplyBuffSeeTheFuture(
                    choice.militaryEffect1,
                    choice.publicEsteem1,
                    choice.economy1,
                    choice.spiritualityEffect1
                );
            }
            else
            {
                StatManager.instance.ClearPreviewStatChange(
                    choice.militaryEffect2,
                    choice.publicEsteem2,
                    choice.economy2,
                    choice.spiritualityEffect2
                );
                StatManager.instance.PreviewStatChange(
                    choice.militaryEffect1,
                    choice.publicEsteem1,
                    choice.economy1,
                    choice.spiritualityEffect1
                );
            }
        }
        else if (yPos < 0 && !isDraggingDown) 
        {

            isDraggingUp = false;
            isDraggingDown = true;
            isLocked = true;
            Invoke(nameof(UnlockPreview), 0.25f);

            Choice choice = Data.instance.CurrentChoice;

            if (TabData.instance.seeTheFuture)
            {
                StatManager.instance.ClearBuffSeeTheFuture(
                    choice.militaryEffect1,
                    choice.publicEsteem1,
                    choice.economy1,
                    choice.spiritualityEffect1
                );
                StatManager.instance.ApplyBuffSeeTheFuture(
                    choice.militaryEffect2,
                    choice.publicEsteem2,
                    choice.economy2,
                    choice.spiritualityEffect2
                );
            }
            else
                {
                StatManager.instance.ClearPreviewStatChange(
                    choice.militaryEffect1,
                    choice.publicEsteem1,
                    choice.economy1,
                    choice.spiritualityEffect1
                );
                StatManager.instance.PreviewStatChange(
                    choice.militaryEffect2,
                    choice.publicEsteem2,
                    choice.economy2,
                    choice.spiritualityEffect2
                );
            }
        }
            
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (yPos > 150)
        {
            SoundManager.Instance.PlaySFX(SoundManager.SFXType.CardSwipe);
            if (Data.instance.Gameover)
            {
                rect.DOAnchorPosY(900, .5f).OnComplete(() => {
                    RulingDays.instance.GameOver();
                    LoadingScreen.Instance.GameOver();
                });
                return;
            }
            rect.DOAnchorPosY(900, .5f).OnComplete(() => 
        {
            Choice choice = Data.instance.CurrentChoice;
                GameManager.Instance.ApplySingleEffect(
                    choice.militaryEffect1,
                    choice.publicEsteem1,
                    choice.economy1,
                    choice.spiritualityEffect1
                );
                StatManager.instance.ApplyStatChanges();
                GameManager.Instance.AddDaysAfterDecision(choice.rulingDays1);
            //Set rect to the bottom of the screen
            rect.anchoredPosition = new Vector2(0, -Screen.height);

            ResetCard();
            GameManager.Instance.CheckGameOver();
            Data.instance.CheckAndCreateBuff();
            Data.instance.MakeDecision();
        });
        }
        else if (yPos < -150)
        {
            SoundManager.Instance.PlaySFX(SoundManager.SFXType.CardSwipe);
            if (Data.instance.Gameover)
            {
                rect.DOAnchorPosY(-800, .5f);
                RulingDays.instance.GameOver();
                LoadingScreen.Instance.GameOver();
                return;
            }
            rect.DOAnchorPosY(-800, .5f).OnComplete(() => 
            {
                    Choice choice = Data.instance.CurrentChoice;
                    GameManager.Instance.ApplySingleEffect(
                        choice.militaryEffect2,
                        choice.publicEsteem2,
                        choice.economy2,
                        choice.spiritualityEffect2
                    );
                    StatManager.instance.ApplyStatChanges();
                    GameManager.Instance.AddDaysAfterDecision(choice.rulingDays2);
                rect.anchoredPosition = new Vector2(0, Screen.height);

                ResetCard();
                GameManager.Instance.CheckGameOver();
                Data.instance.MakeDecision();
            });
        }
        else
        {
            ResetCard();
        }
        isDraggingUp = false;
        isDraggingDown = false;
    }

    //Tinh toan va ap dung do trong suot cua text
    private void CalculateFadeText()
    {
        float alpha = Mathf.Abs(yPos / 150f);
        alpha = Mathf.Clamp01(alpha);

        if (yPos > 0f)
            FadeAnswerText(topAnswer, alpha);

        if (yPos < 0f)        
            FadeAnswerText(bottomAnswer, alpha);
        
    }

    //Dua card ve cac gia tri ban dau
    private void ResetCard()
    {
        // Tạo chuỗi hoạt ảnh
        Sequence sequence = DOTween.Sequence();

        sequence.Append(rect.DOAnchorPosY(0, 0.5f).SetEase(Ease.OutBack, 1f)).OnComplete(() =>
        {
            if (TabData.instance.seeTheFuture)
                StatManager.instance.HideAllTriangle();

            StatManager.instance.HideAllDots();
        });
        FadeAnswerText(topAnswer, 0);
        FadeAnswerText(bottomAnswer, 0);
    }

    //Thay doi do trong suot cua text
    private void FadeAnswerText(TextMeshProUGUI text, float alpha)
    {
        Color currentColor = topAnswer.color;
        text.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
    }

    private void UnlockPreview() {
        isLocked = false;
    }
}
