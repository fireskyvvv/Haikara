using System;
using System.Collections.Generic;
using Haikara.Runtime.View;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Haikara.Runtime.Bindable
{
    public interface ITemplateProperty : ISubViewProperty
    {
        public TemplateInfo TemplateInfo { get; }
        public IEnumerable<TemplateContainer> FindTemplates(VisualElement elementRoot);
    }
}