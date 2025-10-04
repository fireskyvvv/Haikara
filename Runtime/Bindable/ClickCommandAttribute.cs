using System;
using Haikara.Runtime.View;

namespace Haikara.Runtime.Bindable
{
    [AttributeUsage(validOn: AttributeTargets.Method)]
    public class ClickCommandAttribute : Attribute
    {
        public string TargetElementName { get; }
        public int ElementIndex { get; set; } = -1;
        public ElementNameInfo.ElementFindType FindType { get; set; } = ElementNameInfo.ElementFindType.First;

        public ClickCommandAttribute(string targetElementName)
        {
            TargetElementName = targetElementName;
        }
    }
}