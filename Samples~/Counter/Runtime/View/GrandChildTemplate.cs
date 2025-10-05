using Haikara.Runtime;
using Haikara.Runtime.Bindable;
using Haikara.Runtime.View;
using Haikara.Sample.Counter;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Haikara.Samples.Counter.Runtime.View
{
    [HaikaraUI]
    public partial class GrandChildTemplate : HaikaraViewBaseWithViewModel<CounterGrandChildTemplateViewModel>,
        IViewModelProvidable
    {
        private static readonly BindableProperty<Label> CountProperty = BindableProperty<Label>.Create(
            bindingId: PropertyPath.FromName(nameof(Label.text)),
            dataSourcePath: PropertyPath.FromName(nameof(CounterGrandChildTemplateViewModel.GrandChildLabel)),
            elementNameInfo: ElementNames.GrandchildLabel
        );

        public object ProvideSubViewModel(object parentViewModel)
        {
            if (parentViewModel is CounterTemplateViewModel counterTemplateViewModel)
            {
                return counterTemplateViewModel.GrandChildViewModel;
            }

            return null;
        }
    }
}