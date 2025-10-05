using Haikara.Runtime;
using Haikara.Runtime.Bindable;
using Haikara.Runtime.View;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Haikara.Samples.FirstSample.Runtime.View
{
    [HaikaraUI]
    public partial class FirstSample : HaikaraViewBaseWithViewModel<FirstSampleViewModel>
    {
        private static readonly BindableProperty<Label> LabelProperty =
            BindableProperty<Label>.Create(
                bindingId: PropertyPath.FromName(nameof(Label.text)),
                dataSourcePath: PropertyPath.FromName(nameof(FirstSampleViewModel.Label)),
                elementNameInfo: ElementNames.FirstSampleLabel
            );
    }
}