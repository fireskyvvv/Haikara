using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haikara.Runtime.Bindable;
using UnityEngine.UIElements;

namespace Haikara.Runtime.View
{
    public interface IHaikaraView : IHaikaraUI
    {
        /// <summary>
        /// In some cases, the TemplateContainer is not needed when building a UI with a VisualElement as the root.
        /// If this flag is true, the first child (index 0) of the instantiated TemplateContainer will be used as this view's root,
        /// and the container itself will be discarded.
        /// </summary>
        public bool IgnoreTemplateContainerLayout { get; }

        public VisualElement? RootElement { get; }
        public object? DataSource { get; }
        public void SetDataSource(object? dataSource);

        public List<IElementProperty> ElementProperties { get; }
        public List<ITemplateProperty> TemplateProperties { get; }
        public Task<VisualElement> LoadAndAddToAsync(VisualElement parent);

        public bool ValidateDataSource(object? dataSource);

        //public void SetDataSource(object? dataSource);
        public void LinkElements(VisualElement elementRoot);
        public void AddSubView(IHaikaraView subView);
        public void ReleaseView();
    }
}