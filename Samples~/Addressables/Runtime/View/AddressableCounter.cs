#if HAIKARA_IS_EXISTS_ADDRESSABLES
using Haikara.Runtime;
using Haikara.Runtime.Bindable;
using Haikara.Runtime.View;
using Haikara.Samples.Common.Runtime.Dialog;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Haikara.Samples.Addressables.Runtime.View
{
    [HaikaraUI(ReferenceMode = AssetReferenceMode.Custom)]
    public partial class AddressableCounter : HaikaraViewBaseWithViewModel<AddressableCounterViewModel>
    {
        private static readonly BindableProperty<Label> CountProperty = BindableProperty<Label>.Create(
            bindingId: PropertyPath.FromName(nameof(Label.text)),
            dataSourcePath: PropertyPath.FromName(nameof(AddressableCounterViewModel.Label)),
            elementNameInfo: ElementNames.CounterValue
        );

        [ClickCommand(ElementNames.CounterButtonAdd)]
        private void OnClickAdd(EventBase evt)
        {
            if (ViewModel == null)
            {
                return;
            }

            ViewModel.AddCount();
        }

        [ClickCommand(ElementNames.CounterButtonShowDialog)]
        private void OnClickShowDialog(EventBase evt)
        {
            SampleDialogProvider.Instance.ShowDialog(new SampleDialogViewmodel());
        }
    }
}
#endif