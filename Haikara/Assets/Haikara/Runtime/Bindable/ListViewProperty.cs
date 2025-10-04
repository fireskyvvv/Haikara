using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haikara.Runtime.Catalog;
using Haikara.Runtime.View;
using Haikara.Runtime.ViewModel;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Haikara.Runtime.Bindable
{
    public class ListViewProperty<TItemSourceViewModel> : CollectionViewProperty<ListView>
        where TItemSourceViewModel : ViewModelBase
    {
        public delegate TItemSourceViewModel MakeItemSourceDelegate(int index);

        public string ItemViewId { get; }
        private readonly MakeItemSourceDelegate _makeItemSource;
        private readonly Dictionary<int, IHaikaraView> _views = new();
        private VisualTreeAsset? _viewAsset;

        private ListViewProperty(
            string itemViewId,
            MakeItemSourceDelegate makeItemSource,
            PropertyPath itemsSourcePath,
            ElementNameInfo elementNameInfo,
            BindingMode bindingMode,
            BindingUpdateTrigger updateTrigger
        ) : base(itemsSourcePath, elementNameInfo, bindingMode, updateTrigger)
        {
            ItemViewId = itemViewId;
            _makeItemSource = makeItemSource;
        }

        protected override async void SetBinding(ListView element)
        {
            if (_viewAsset == null)
            {
                _viewAsset = await RuntimeUICatalog.Instance.UxmlUICollection.LoadOrIncrementUIAssetAsync(ItemViewId);
            }

            element.makeItem = ()=>
            {
                var viewAsset = _viewAsset; 
                if (viewAsset == null)
                {
                    throw new Exception();
                }

                var item = viewAsset.Instantiate();
                return item;
            };

            element.bindItem = (item, index) =>
            {
                element.itemsSource[index] ??= _makeItemSource.Invoke(index);
                if (!_views.TryGetValue(index, out var view))
                {
                    view = ViewProvider.Instance.CreateView(ItemViewId);
                    if (view == null)
                    {
                        throw new Exception();
                    }

                    _views.Add(index, view);
                }

                view.LinkElements(item);
                view.SetDataSource(element.itemsSource[index]);
            };
        }

        public static ListViewProperty<TItemSourceViewModel> Create(
            string itemViewId,
            MakeItemSourceDelegate makeItemSource,
            PropertyPath itemsSourcePath,
            ElementNameInfo elementNameInfo,
            BindingMode bindingMode = BindingMode.ToTarget,
            BindingUpdateTrigger updateTrigger = BindingUpdateTrigger.OnSourceChanged
        )
        {
            return new ListViewProperty<TItemSourceViewModel>(
                itemViewId: itemViewId,
                makeItemSource: makeItemSource,
                itemsSourcePath: itemsSourcePath,
                elementNameInfo: elementNameInfo,
                bindingMode: bindingMode,
                updateTrigger: updateTrigger
            );
        }
    }
}