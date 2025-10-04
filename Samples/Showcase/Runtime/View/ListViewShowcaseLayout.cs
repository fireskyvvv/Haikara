using System;
using Haikara.Runtime;
using Haikara.Runtime.Bindable;
using Haikara.Runtime.View;
using Unity.Properties;

namespace Haikara.Samples.Showcase.Runtime.View
{
    [HaikaraUI]
    public partial class
        ListViewShowcaseLayout : SubViewModelProvidableViewBase<ShowcaseViewModel, ListViewShowcaseViewModel>
    {
        private static readonly ListViewProperty<ListViewShowcaseItemViewModel> ListDataSourceProperty =
            ListViewProperty<ListViewShowcaseItemViewModel>.Create(
                itemViewId: ListViewItemLayout.UxmlGuid,
                makeItemSource: MakeItem,
                itemsSourcePath: PropertyPath.FromName(nameof(ListViewShowcaseViewModel.Current)),
                elementNameInfo: ElementNames.ListView
            );
        
        protected override ListViewShowcaseViewModel ProvideSubViewModel(ShowcaseViewModel parentViewModel)
        {
            return parentViewModel.ListView;
        }
        
        private static ListViewShowcaseItemViewModel MakeItem(int index)
        {
            return new ListViewShowcaseItemViewModel(label: Guid.NewGuid().ToString());
        }
    }
}