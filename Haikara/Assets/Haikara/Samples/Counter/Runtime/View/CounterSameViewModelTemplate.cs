using Haikara.Runtime;
using Haikara.Runtime.Bindable;
using Haikara.Runtime.View;
using Haikara.Sample.Counter;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Haikara.Samples.Counter.Runtime.View
{
    [HaikaraUI]
    public partial class CounterSameViewModelTemplate : HaikaraViewBaseWithViewModel<CounterViewModel>
    {
        private static readonly BindableProperty<Label> CountProperty = BindableProperty<Label>.Create(
            bindingId: PropertyPath.FromName(nameof(Label.text)),
            dataSourcePath: PropertyPath.FromName(nameof(CounterViewModel.Label)),
            elementNameInfo: ElementNames.TemplateInfoLabel
        );
    }
}