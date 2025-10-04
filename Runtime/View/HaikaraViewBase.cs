using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Haikara.Runtime.Bindable;
using Haikara.Runtime.Catalog;
using UnityEngine.UIElements;

namespace Haikara.Runtime.View
{
    public abstract class HaikaraViewBase : IHaikaraView
    {
        public virtual bool IgnoreTemplateContainerLayout => false;
        protected virtual bool NeedStopPropagation => false;

        public VisualElement? RootElement { get; protected set; }

        private object? _dataSource;

        public object? DataSource
        {
            get => _dataSource;
            protected set
            {
                var previousValue = _dataSource;
                var newValue = value;
                _dataSource = ValidateDataSource(newValue) ? newValue : null;
                OnDataSourceChanged(previousValue, newValue);
            }
        }

        protected HaikaraViewBase()
        {
            InitializeComponent();
        }

        public abstract bool ValidateDataSource(object? dataSource);

        public void SetDataSource(object? dataSource)
        {
            DataSource = dataSource;
        }

        private List<IHaikaraView> _subViews = new();
        public List<IElementProperty> ElementProperties { get; } = new();
        public List<ITemplateProperty> TemplateProperties { get; } = new();

        public virtual AssetReferenceMode AssetReferenceMode => AssetReferenceMode.Resource;

        protected void InitializeComponent()
        {
            InitializeComponentInternal();
        }

        protected virtual void InitializeComponentInternal()
        {
        }

        public virtual string GetGuid() => "";

        private async Task<VisualTreeAsset?> LoadViewAsync()
        {
            return await RuntimeUICatalog.Instance.LoadVisualTreeAssetAsync(this);
        }

        protected virtual void OnDataSourceChanged(object? previous, object? newValue)
        {
            foreach (var elementProperty in ElementProperties)
            {
                if (elementProperty is IBindableProperty bindableProperty)
                {
                    bindableProperty.DataSource = newValue;
                }
            }

            foreach (var subView in GetValidSubViews())
            {
                var subViewDataSource = newValue;
                if (subView is IViewModelProvidable viewModelProvidableSubView)
                {
                    subViewDataSource = viewModelProvidableSubView.ProvideSubViewModel(newValue);
                }

                subView.SetDataSource(subViewDataSource);
            }
        }

        public void AddSubView(IHaikaraView subView)
        {
            _subViews.Add(subView);
        }

        public async Task<VisualElement> LoadAndAddToAsync(VisualElement parent)
        {
            // Load the corresponding VisualTreeAsset.
            var loadedView = await LoadViewAsync();
            if (loadedView == null)
            {
                throw new Exception("Cant load view");
            }

            // Instantiate a VisualElement from the loaded VisualTreeAsset.
            VisualElement elementRoot;

            // If IgnoreTemplateContainerLayout is enabled,
            // use the first child of the instantiated TemplateContainer as the elementRoot.
            if (IgnoreTemplateContainerLayout)
            {
                var template = loadedView.Instantiate();
                elementRoot = template[0];
            }
            else
            {
                elementRoot = loadedView.Instantiate();
            }

            if (NeedStopPropagation)
            {
                elementRoot.AddManipulator(new Clickable(evt => { evt.StopPropagation(); }));
            }

            await OnElementLoaded(elementRoot);

            parent.Add(elementRoot);

            await OnElementAdded(elementRoot);

            // Link the elements within the instantiated VisualElement to their corresponding ElementProperties.
            LinkElements(elementRoot);

            return elementRoot;
        }

        protected virtual Task OnElementLoaded(VisualElement elementRoot)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnElementAdded(VisualElement elementRoot)
        {
            return Task.CompletedTask;
        }

        public void LinkElements(VisualElement elementRoot)
        {
            RootElement = elementRoot;
            ElementProperties.SortElementPropertiesByPriority();
            foreach (var elementProperty in ElementProperties)
            {
                elementProperty.FindElementAndSetBinding(elementRoot);
                if (elementProperty is IBindableProperty bindableProperty)
                {
                    // Find the target VisualElement for the BindableProperty within the elementRoot,
                    // then apply the binding and update its DataSource.
                    bindableProperty.DataSource = DataSource;

                    // If a property needs to be added as a SubView, propagate the DataSource and then add it as a SubView.
                    // e.g., TabViewProperty
                    if (bindableProperty is ISubViewProperty subViewProperty)
                    {
                        foreach (var subViewInstance in subViewProperty.GetViewInstances())
                        {
                            var subViewDataSource = DataSource;
                            if (subViewInstance is IViewModelProvidable viewModelProvidableSubView)
                            {
                                subViewDataSource = viewModelProvidableSubView.ProvideSubViewModel(DataSource);
                            }

                            subViewInstance.SetDataSource(subViewDataSource);
                            _subViews.Add(subViewInstance);
                        }
                    }
                }
            }

            foreach (var templateProperty in TemplateProperties)
            {
                foreach (var template in templateProperty.FindTemplates(elementRoot))
                {
                    var templateView = templateProperty.GetViewInstances().FirstOrDefault();
                    if (templateView == null)
                    {
                        continue;
                    }

                    templateView.LinkElements(template);
                    var subViewDataSource = DataSource;
                    if (templateView is IViewModelProvidable viewModelProvidable)
                    {
                        subViewDataSource = viewModelProvidable.ProvideSubViewModel(DataSource);
                    }

                    templateView.SetDataSource(subViewDataSource);
                    _subViews.Add(templateView);
                }
            }
        }

        public virtual void ReleaseView()
        {
            foreach (var subView in GetValidSubViews())
            {
                subView.ReleaseView();
            }

            RootElement?.RemoveFromHierarchy();
            RuntimeUICatalog.Instance.ReleaseVisualTreeAsset(this);
        }

        /// <summary>
        /// Returns only the valid SubViews.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<IHaikaraView> GetValidSubViews()
        {
            var hasInvalidSubView = false;
            foreach (var subView in _subViews)
            {
                if (subView == null)
                {
                    hasInvalidSubView = true;
                    continue;
                }

                yield return subView;
            }

            if (hasInvalidSubView)
            {
                _subViews = _subViews.Where(x => x != null).ToList();
            }
        }
    }
}