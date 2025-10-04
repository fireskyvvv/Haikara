using System;
using System.Collections.Generic;
using System.Linq;
using Haikara.Runtime.View;
using UnityEngine.UIElements;

namespace Haikara.Runtime.Bindable
{
    public class TemplateProperty<TView> : ITemplateProperty 
        where TView : IHaikaraView
    {
        public TemplateInfo TemplateInfo { get; }
        private TemplateProperty(TemplateInfo templateInfo)
        {
            TemplateInfo = templateInfo; 
        }

        public IEnumerable<TemplateContainer> FindTemplates(VisualElement elementRoot)
        {
            return elementRoot
                .Query<TemplateContainer>()
                .Build()
                .Where(x => x.templateId == TemplateInfo.TemplateId);
        }

        public static TemplateProperty<TView> Create(TemplateInfo templateInfo)
        {
            return new TemplateProperty<TView>(templateInfo: templateInfo);
        }
        

        public IEnumerable<IHaikaraView> GetViewInstances()
        {
            var templateView = ViewProvider.Instance.CreateView(TemplateInfo.ViewGuid);
            if (templateView is not null)
            {
                yield return templateView;
            }
        }
    }
}