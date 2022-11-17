using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CyberPunkCoding
{
    public class Trash : Grabable, IDropHandler
    {
        [SerializeField] private float destroyAnimationDuration = 0.2f;
        [SerializeField] private Ease destroyAnimationEase;
        
        public override void OnPointerDown(PointerEventData eventData)
        {
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
        }

        public override void OnDrag(PointerEventData eventData)
        {
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag.TryGetComponent(out CommandUiObject commandUi))
            {
                while (commandUi)
                {
                    // commandUi.RectTransform.anchorMin = Vector2.one * 0.5f;
                    // commandUi.RectTransform.anchorMax = Vector2.one * 0.5f;
                    // commandUi.RectTransform.anchoredPosition *= 0.5f;
                    commandUi.transform.DOScale(Vector3.zero, 0.2f).SetEase(destroyAnimationEase).onComplete += () => Destroy(commandUi.gameObject);
                    commandUi = commandUi.Next;
                }
            }
        }
    }
}