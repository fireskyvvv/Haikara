using Haikara.Runtime;
using Haikara.Runtime.Bindable;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Haikara.Samples.Common.Runtime.Dialog.View
{
    [HaikaraUI]
    public partial class SampleDialog : SampleDialogViewBase<SampleDialogViewmodel>
    {
        public override bool IgnoreTemplateContainerLayout => true;

        private static readonly BindableProperty<Label> TextProperty = BindableProperty<Label>.Create(
            bindingId: PropertyPath.FromName(nameof(Label.text)),
            dataSourcePath: PropertyPath.FromName(nameof(SampleDialogViewmodel.Text)),
            elementNameInfo: ElementNames.Text
        );
    }
}