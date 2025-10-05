using Haikara.Runtime;
using Haikara.Runtime.Bindable;
using Haikara.Runtime.View;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Haikara.Samples.Showcase.Runtime.View
{
    [HaikaraUI(ReferenceMode = AssetReferenceMode.Resource)]
    public partial class ListViewItemLayout : HaikaraViewBaseWithViewModel<ListViewShowcaseItemViewModel>
    {
        private static readonly BindableProperty<Label> InfoLabelProperty = BindableProperty<Label>.Create(
            bindingId: PropertyPath.FromName(nameof(Label.text)),
            dataSourcePath: PropertyPath.FromName(nameof(ListViewShowcaseItemViewModel.Label)),
            elementNameInfo: ElementNames.InfoLabel
        );
    }
}