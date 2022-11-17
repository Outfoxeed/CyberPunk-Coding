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
                if(IsCommandUiInChainedList(commandUiObject))
                    return;
                
                if (Next)
                {
                    CommandUiObject _next = Next;
                    while (_next.Next)
                    {
                        _next = _next.Next;
                    }
                    _next.Next = commandUiObject;
                    commandUiObject.Last = _next;
                }
                else
                {
                    Next = commandUiObject;
                    Next.Last = this;
                }

                SetNextCommandsAnchoredPosition();
            }
        }

        public Command[] GetCommands()
        {
            if (!Next) return Array.Empty<Command>();
            List<Command> commands = new();
            CommandUiObject _next = Next;
            while (_next)
            {
                commands.Add(_next.Command);
                _next = _next.Next;
            }

            return commands.ToArray();
        }
    }
}