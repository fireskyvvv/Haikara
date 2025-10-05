using Haikara.Runtime;
using Haikara.Runtime.Bindable;
using Haikara.Runtime.View;
using Unity.Properties;

namespace Haikara.Samples.Showcase.Runtime.View
{
    [HaikaraUI]
    public partial class ShowcaseLayout : HaikaraViewBaseWithViewModel<ShowcaseViewModel>
    {
        private static readonly TabViewProperty<ShowcaseViewModel> TabProperty =
            TabViewProperty<ShowcaseViewModel>.Create(
                tabContentViewInfoList: new[]
                {
                    (
                        viewGuid: ControlsShowcaseLayout.UxmlGuid,
                        labelDataSourcePath: PropertyPath.Combine(
                            PropertyPath.FromName(nameof(ShowcaseViewModel.Controls)),
                            PropertyPath.FromName(nameof(ControlsShowcaseViewModel.TabLabel))
                        )
                    ),
                    (
                        viewGuid: ListViewShowcaseLayout.UxmlGuid,
                        labelDataSourcePath: PropertyPath.Combine(
                            PropertyPath.FromName(nameof(ShowcaseViewModel.ListView)),
                            PropertyPath.FromName(nameof(ListViewShowcaseViewModel.TabLabel))
                        )
                    )
                },
                elementNameInfo: ElementNames.RootTab
            );
    }
}