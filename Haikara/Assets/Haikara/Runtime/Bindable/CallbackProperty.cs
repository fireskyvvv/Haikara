using System;
using System.Collections.Generic;
using Haikara.Runtime.View;
using UnityEngine.UIElements;

namespace Haikara.Runtime.Bindable
{
    public class CallbackProperty<T, TEventType> : IElementProperty
        where T : VisualElement
        where TEventType : EventBase<TEventType>, new()
    {
        public List<T> Elements { get; set; } = new();
        public ElementNameInfo ElementNameInfo { get; }
        public Action<TEventType> Callback { get; }

        private CallbackProperty(ElementNameInfo elementNameInfo, Action<TEventType> callback)
        {
            ElementNameInfo = elementNameInfo;
            Callback = callback;
        }

        public static CallbackProperty<T, TEventType> Create(ElementNameInfo elementNameInfo,
            Action<TEventType> callback)
        {
            return new CallbackProperty<T, TEventType>(
                elementNameInfo: elementNameInfo,
                callback: callback
            );
        }

        public void FindElementAndSetBinding(VisualElement elementRoot)
        {
            Elements = ElementNameInfo.Find<T>(elementRoot);

            foreach (var element in Elements)
            {
                if (typeof(TEventType) == typeof(KeyUpEvent) || typeof(TEventType) == typeof(KeyDownEvent))
                {
                    element.focusable = true;
                }

                element.UnregisterCallback<TEventType>(Invoke);
                element.RegisterCallback<TEventType>(Invoke);
            }
        }

        private void Invoke(TEventType eventType)
        {
            Callback.Invoke(eventType);
        }
    }
}