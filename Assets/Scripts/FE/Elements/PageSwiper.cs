using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform contentTab;
    [SerializeField] private Transform dotContainer;
    [SerializeField] private float widthChildren;
    private float difference;
    private float index;
    private Image[] dots;

    private void Awake()
    {
        dots = dotContainer.GetComponentsInChildren<Image>();
    }

    public void SetIndex(int index) 
    {
        this.index = index;
        ChangeActiveDot();
    } 

    public void OnDrag(PointerEventData eventData)
    {
        difference = eventData.pressPosition.x - eventData.position.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float newPosition = 0;
        float currentPosX = contentTab.anchoredPosition.x;

        // Luot tu phai sang trai
        if (difference > 0)
        {
            //Neu den item cuoi cung thi dung
            if ((contentTab.rect.width - Mathf.Abs(currentPosX)) <= widthChildren)
                return;
            index++;
            newPosition = currentPosX - widthChildren;
        }
        
        //Luot tu trai sang phai
        if (difference < 0)
        {
            //Neu den item dau tien thi dung
            if (contentTab.anchoredPosition.x >= 0)
                return;
            index--;
            newPosition = currentPosX + widthChildren;
        }

        ChangeActiveDot();

        contentTab.DOAnchorPosX(newPosition, .15f);
    }

    private void ChangeActiveDot()
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].color = Color.gray;
            if (i == index)
                dots[i].color = Color.white;
        }
    }
}
