using Haikara.Runtime;
using Haikara.Runtime.Bindable;
using Haikara.Runtime.View;
using Haikara.Sample.Counter;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Haikara.Samples.Counter.Runtime.View
{
    [HaikaraUI]
    public partial class CounterTemplate : HaikaraViewBaseWithViewModel<CounterTemplateViewModel>,
        IViewModelProvidable
    {
        private static readonly BindableProperty<Label> CountProperty = BindableProperty<Label>.Create(
            bindingId: PropertyPath.FromName(nameof(Label.text)),
            dataSourcePath: PropertyPath.FromName(nameof(CounterTemplateViewModel.TemplateLabel)),
            elementNameInfo: ElementNames.TemplateInfoLabel
        );

        private static readonly TemplateProperty<GrandChildTemplate> GrandChildTemplateProperty =
            TemplateProperty<GrandChildTemplate>.Create(templateInfo: TemplateInfoList.GrandChildTemplate);

        public object ProvideSubViewModel(object parentViewModel)
        {
            if (parentViewModel is CounterViewModel counterViewModel)
            {
                return counterViewModel.TemplateViewModel;
            }

            return null;
        }
    }
}