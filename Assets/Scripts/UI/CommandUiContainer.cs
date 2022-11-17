using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace CyberPunkCoding
{
    public class CommandUiContainer : CommandUiObject
    {
        protected override void Awake()
        {
            base.Awake();
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

        public override void OnPointerDown(PointerEventData eventData)
        {
        }

        public override void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag.TryGetComponent(out CommandUiObject commandUiObject))
            {
                if (!next)
                {
                    next = commandUiObject;
                    next.last = this;
                }
                else
                {
                    next.last = commandUiObject;
                    commandUiObject.next = next;
                    next = commandUiObject;
                    next.last = this;
                }
                SetNextCommandsAnchoredPosition();
            }
        }

        public Command[] GetCommands()
        {
            if (!next) return Array.Empty<Command>();
            List<Command> commands = new();
            CommandUiObject _next = next;
            while (_next)
            {
                commands.Add(_next.Command);
                _next = _next.next;
            }
            return commands.ToArray();
        }
    }
}