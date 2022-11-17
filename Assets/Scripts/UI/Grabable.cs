using System;
using CyberPunkCoding;
using UnityEngine;
using UnityEngine.EventSystems;

public class Grabable : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private static Canvas canvas;
    public static Canvas Canvas
    {
        get
        {
            if (!canvas) canvas = FindObjectOfType<Canvas>();
            return canvas;
        }
    }

    public RectTransform RectTransform => rectTransform;
    private RectTransform rectTransform;
    
    private CanvasGroup canvasGroup;
    protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        RectTransform.anchoredPosition += eventData.delta / Canvas.scaleFactor;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
    }
}