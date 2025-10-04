using System;
using Haikara.Runtime.Bindable;

namespace Haikara.Runtime.View
{
    public readonly struct TemplateInfo
    {
        public ElementName ElementName { get; }
        public string TemplateId { get; }
        public string ViewGuid { get; }

        public TemplateInfo(ElementName elementName, string viewGuid, string templateId)
        {
            TemplateId = templateId;
            ElementName = elementName;
            ViewGuid = viewGuid;
        }
    }
}