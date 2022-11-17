using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CyberPunkCoding
{
    public class CommandUiObject : Grabable, IDropHandler
    {
        public CommandUiObject last;
        public CommandUiObject next;

        private bool original;
        
        public Command Command => command;
        [SerializeField] private Command command;

        protected override void Awake()
        {
            base.Awake();
            original = true;
        }

        private void SetNext(CommandUiObject commandUiObject)
        {
            if (next)
            {
                next.last = null;
                next.RectTransform.anchoredPosition += (RectTransform.sizeDelta.y * 1.1f) * Vector2.up;
            }
            next = commandUiObject;

            if (next)
            {
                next.last = this;
                SetNextCommandsAnchoredPosition();
            }
        }

        public virtual void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag.TryGetComponent<CommandUiObject>(out CommandUiObject commandUiObject))
                SetNext(commandUiObject);
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (original)
            {
                Instantiate(this, transform.position, Quaternion.identity, transform.parent).OnPointerExit(eventData);
                original = false;
            }
            if (last)
            {
                last.next = null;
                last = null;
            }
            base.OnBeginDrag(eventData);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            SetNextCommandsAnchoredPosition();
        }

        protected void SetNextCommandsAnchoredPosition()
        {
            CommandUiObject _next = next;
            while (_next)
            {
                _next.RectTransform.pivot = _next.last.RectTransform.pivot;
                _next.RectTransform.anchorMin = _next.last.RectTransform.anchorMin;
                _next.RectTransform.anchorMax = _next.last.RectTransform.anchorMax;
                _next.RectTransform.anchoredPosition = _next.last.RectTransform.anchoredPosition + _next.last.RectTransform.sizeDelta.x * Vector2.right;
                _next = _next.next;
            }
        }
    }
}