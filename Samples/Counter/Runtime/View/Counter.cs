using Haikara.Runtime;
using Haikara.Runtime.Bindable;
using Haikara.Runtime.View;
using Haikara.Sample.Counter;
using Haikara.Samples.Common.Runtime.Dialog;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Haikara.Samples.Counter.Runtime.View
{
    [HaikaraUI(ReferenceMode = AssetReferenceMode.Resource)]
    public partial class Counter : HaikaraViewBaseWithViewModel<CounterViewModel>
    {
        private static readonly BindableProperty<Label> CountProperty = BindableProperty<Label>.Create(
            bindingId: PropertyPath.FromName(nameof(Label.text)),
            dataSourcePath: PropertyPath.FromName(nameof(CounterViewModel.Label)),
            elementNameInfo: ElementNames.CounterValue
        );

        private static readonly TemplateProperty<CounterSameViewModelTemplate> CounterSameViewModelTemplate =
            TemplateProperty<CounterSameViewModelTemplate>.Create(TemplateInfoList.SameVmTemplate);

        private static readonly TemplateProperty<CounterTemplate> TemplateViewProperty =
            TemplateProperty<CounterTemplate>.Create(TemplateInfoList.ChildVmTemplate);

        [ClickCommand(ElementNames.CounterButtonAdd)]
        private void OnClick(EventBase evt)
        {
            if (ViewModel == null)
            {
                return;
            }

            ViewModel.AddCount();

            SampleDialogProvider.Instance.ShowDialog(new SampleDialogViewmodel());
        }
    }
}