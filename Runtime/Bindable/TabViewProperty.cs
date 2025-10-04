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
    public class TabViewProperty<TViewModelBase> : BindablePropertyBase<TabView>, ISubViewProperty
        where TViewModelBase : ViewModelBase
    {
        private class TabContentViewInfo
        {
            public IHaikaraView View { get; }
            public PropertyPath LabelDataSourcePath { get; }

            public TabContentViewInfo(IHaikaraView view, PropertyPath labelDataSourcePath)
            {
                View = view;
                LabelDataSourcePath = labelDataSourcePath;
            }
        }

        public override ElementNameInfo ElementNameInfo { get; }
        private List<TabContentViewInfo> TabContentViewInfoList { get; }
        private readonly Dictionary<string, Tab> _guidToTab = new();

        private TabViewProperty(
            List<TabContentViewInfo> tabContentViewInfoList,
            ElementNameInfo elementNameInfo
        )
        {
            TabContentViewInfoList = tabContentViewInfoList;
            ElementNameInfo = elementNameInfo;
        }

        public override async void FindElementAndSetBinding(VisualElement elementRoot)
        {
            Elements = ElementNameInfo.Find<TabView>(elementRoot);
            foreach (var element in Elements)
            {
                if (element == null)
                {
                    continue;
                }

                foreach (var tabContentViewInfo in TabContentViewInfoList)
                {
                    var view = tabContentViewInfo.View;
                    var tabGuid = view.GetGuid();
                    if (!_guidToTab.TryGetValue(tabGuid, out var tab))
                    {
                        var tabContentAsset =
                            await RuntimeUICatalog.Instance.UxmlUICollection.LoadOrIncrementUIAssetAsync(tabGuid);

                        if (tabContentAsset == null)
                        {
                            throw new Exception();
                        }

                        var newTab = new Tab();
                        newTab.Add(tabContentAsset.Instantiate());

                        tab = newTab;
                        _guidToTab.Add(tabGuid, tab);
                        element.Add(newTab);
                    }

                    view.LinkElements(tab);

                    tab.SetBinding(
                        bindingId: PropertyPath.FromName(nameof(Tab.label)),
                        binding: new DataBinding()
                        {
                            dataSource = DataSource,
                            dataSourcePath = tabContentViewInfo.LabelDataSourcePath
                        }
                    );
                }
            }
        }

        public IEnumerable<IHaikaraView> GetViewInstances()
        {
            return TabContentViewInfoList.Select(tabContentViewInfo => tabContentViewInfo.View);
        }

        public static TabViewProperty<TViewModelBase> Create(
            IEnumerable<(string viewGuid, PropertyPath labelDataSourcePath)> tabContentViewInfoList,
            ElementNameInfo elementNameInfo
        )
        {
            var tabContentViews = new List<TabContentViewInfo>();
            foreach (var tabContentViewInfo in tabContentViewInfoList)
            {
                var viewGuid = tabContentViewInfo.viewGuid;
                var view = ViewProvider.Instance.CreateView(viewGuid);
                if (view == null)
                {
                    UnityEngine.Debug.Log($"view is null: {viewGuid}");
                    continue;
                }

                tabContentViews.Add(new TabContentViewInfo(view, tabContentViewInfo.labelDataSourcePath));
            }

            return new TabViewProperty<TViewModelBase>(
                tabContentViewInfoList: tabContentViews,
                elementNameInfo: elementNameInfo
            );
        }
    }
}