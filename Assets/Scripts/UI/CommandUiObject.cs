using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CyberPunkCoding
{
    public class CommandUiObject : Grabable, IDropHandler
    {
        private CommandUiObject last;
        public CommandUiObject Last
        {
            get => last;
            set
            {
                last = value;
            }
        }
        private CommandUiObject next;
        public CommandUiObject Next
        {
            get => next;
            set
            {
                next = value;
            }
        }

        private bool original;
        
        public Command Command => command;
        [SerializeField] private Command command;
        [SerializeField] private Image blankImage;

        protected override void Awake()
        {
            base.Awake();
            original = true;
            Next = null;

            Grabable.OnGrabUpdate += grabbing =>
            {
                if (original)
                    return;
                blankImage.gameObject.SetActive(grabbing && !Next);
            };
        }

        private void SetNext(CommandUiObject commandUiObject)
        {
            if (Next)
            {
                Next.Last = null;
                Next.RectTransform.anchoredPosition += (RectTransform.sizeDelta.y * 1.1f) * Vector2.up;
                Next.SetNextCommandsAnchoredPosition();
            }
            Next = commandUiObject;

            if (Next)
            {
                Next.Last = this;
                SetNextCommandsAnchoredPosition();
            }
        }

        public virtual void OnDrop(PointerEventData eventData)
        {
            if (original)
                return;
            if (eventData.pointerDrag.TryGetComponent<CommandUiObject>(out CommandUiObject commandUiObject))
            {
                // Abort if it is not a CommandUiObject
                if (commandUiObject.GetType() != typeof(CommandUiObject))
                    return;
                
                // Abort if the command ui is in the chained-list
                if (IsCommandUiInChainedList(commandUiObject))
                    return;

                SetNext(commandUiObject);
            }
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
                Last.Next = null;
                Last = null;
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
            CommandUiObject _next = Next;
            while (_next)
            {
                _next.RectTransform.pivot = _next.Last.RectTransform.pivot;
                _next.RectTransform.anchorMin = _next.Last.RectTransform.anchorMin;
                _next.RectTransform.anchorMax = _next.Last.RectTransform.anchorMax;
                _next.RectTransform.anchoredPosition = _next.Last.RectTransform.anchoredPosition + _next.Last.RectTransform.sizeDelta.x * Vector2.right;
                _next = _next.Next;
            }
        }

        protected bool IsCommandUiInChainedList(CommandUiObject commandUiObject)
        {
            if (commandUiObject == this)
                return true;
            // Abort if the command is already in the chained-list
            CommandUiObject first = this;
            while (first.Last) first = first.Last;
            while (first)
            {
                if (first == commandUiObject)
                    return true;
                first = first.Next;
            }
            return false;
        }
    }
}