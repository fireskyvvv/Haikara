using System;
using Haikara.Runtime.View;

namespace Haikara.Runtime.Bindable
{
    [AttributeUsage(validOn: AttributeTargets.Method)]
    public class CallbackAttribute : Attribute
    {
        public string TargetElementName { get; }
        public Type EventType { get; }
        public int ElementIndex { get; set; } = -1;
        public ElementNameInfo.ElementFindType FindType { get; set; } = ElementNameInfo.ElementFindType.First;

        public CallbackAttribute(string targetElementName, Type eventType)
        {
            TargetElementName = targetElementName;
            EventType = eventType;
        }
    }
}