using System;
using System.Collections.Generic;
using System.Linq;
using Haikara.Runtime.Catalog;
using Haikara.Runtime.View;
using Haikara.Runtime.ViewModel;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Haikara.Runtime.Bindable
{
    public abstract class TreeViewEntryViewModel<T> : ViewModelBase where T : TreeViewEntryViewModel<T>
    {
        public List<T> Children = new();

        public TreeViewItemData<T> CreateTreeViewItemData(ref int id)
        {
            var children = new List<TreeViewItemData<T>>(Children.Count);
            foreach (var child in Children)
            {
                children.Add(child.CreateTreeViewItemData(ref id));
            }
            
            return new TreeViewItemData<T>(id++, (T)this, children);   
        }
    }
    
    public class TreeViewProperty<TTreeViewEntryViewModel> : CollectionViewProperty<TreeView> where TTreeViewEntryViewModel : TreeViewEntryViewModel<TTreeViewEntryViewModel>
    {
        public delegate IList<TTreeViewEntryViewModel> GetTreeViewRootItemDelegate(object? dataSource);

        public string ItemViewId { get; }
        private readonly GetTreeViewRootItemDelegate _getTreeViewRootItemDelegate;
        private readonly Dictionary<int, IHaikaraView> _views = new();
        private VisualTreeAsset? _viewAsset;

        private TreeViewProperty(
            string itemViewId,
            GetTreeViewRootItemDelegate getTreeViewRootItemDelegate,
            PropertyPath itemsSourcePath,
            ElementNameInfo elementNameInfo,
            BindingMode bindingMode,
            BindingUpdateTrigger updateTrigger
        ) : base(itemsSourcePath, elementNameInfo, bindingMode, updateTrigger)
        {
            ItemViewId = itemViewId;
            _getTreeViewRootItemDelegate = getTreeViewRootItemDelegate;
        }
        
        protected override async void SetBinding(TreeView element)
        {
            if (_viewAsset == null)
            {
                _viewAsset = await RuntimeUICatalog.Instance.UxmlUICollection.LoadOrIncrementUIAssetAsync(ItemViewId);
            }
            
            element.makeItem = () =>
            {
                UnityEngine.Debug.Log("Make item");

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
                var itemData = element.GetItemDataForIndex<TTreeViewEntryViewModel>(index);
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
                view.SetDataSource(itemData);
            };
        }

        protected override void ElementDataSourceChanging(TreeView element, object? previous, object? newValue)
        {
            var id = 0;
            var rootItems = new List<TreeViewItemData<TTreeViewEntryViewModel>>();
            foreach (var treeViewEntryViewModel in _getTreeViewRootItemDelegate.Invoke(DataSource))
            {
                var parent = treeViewEntryViewModel.CreateTreeViewItemData(ref id);
                rootItems.Add(parent);
            }

            element.SetRootItems(rootItems);
            UnityEngine.Debug.Log(rootItems.Count);
            base.ElementDataSourceChanging(element, previous, newValue);
        }
        
        public static TreeViewProperty<TTreeViewEntryViewModel> Create(
            string itemViewId,
            GetTreeViewRootItemDelegate getTreeViewRootItemDelegate,
            PropertyPath itemsSourcePath,
            ElementNameInfo elementNameInfo,
            BindingMode bindingMode = BindingMode.ToTarget,
            BindingUpdateTrigger updateTrigger = BindingUpdateTrigger.OnSourceChanged
        )
        {
            return new TreeViewProperty<TTreeViewEntryViewModel>(
                itemViewId: itemViewId,
                getTreeViewRootItemDelegate: getTreeViewRootItemDelegate,
                itemsSourcePath: itemsSourcePath,
                elementNameInfo: elementNameInfo,
                bindingMode: bindingMode,
                updateTrigger: updateTrigger
            );
        }
    }
}