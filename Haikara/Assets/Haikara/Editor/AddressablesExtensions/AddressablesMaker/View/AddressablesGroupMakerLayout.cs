#if HAIKARA_IS_EXISTS_ADDRESSABLES
using Haikara.Runtime;
using Haikara.Runtime.Bindable;
using Haikara.Runtime.View;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Haikara.Editor.AddressablesExtensions.AddressablesMaker.View
{
    [HaikaraUI(ReferenceMode = AssetReferenceMode.AssetPath)]
    internal partial class AddressablesGroupMakerLayout : HaikaraViewBaseWithViewModel<HaikaraAddressablesGroupMakerViewModel>
    {
        private static readonly BindableProperty<Label> LabelAddressableGroupNamesProperty =
            BindableProperty<Label>.Create(
                bindingId: nameof(Label.text),
                dataSourcePath: PropertyPath.FromName(
                    nameof(HaikaraAddressablesGroupMakerViewModel.LabelAddressableGroupNames)
                ),
                elementNameInfo: ElementNames.AddressableGroupNameLabel
            );

        private static readonly BindableProperty<Button> LabelRunBuildButtonProperty =
            BindableProperty<Button>.Create(
                bindingId: nameof(Button.text),
                dataSourcePath: PropertyPath.FromName(
                    nameof(HaikaraAddressablesGroupMakerViewModel.LabelRunBuildButton)
                ),
                elementNameInfo: ElementNames.ButtonRunBuild
            );

        private static readonly BindableProperty<Label> LabelWindowInfoProperty =
            BindableProperty<Label>.Create(
                bindingId: nameof(Label.text),
                dataSourcePath: PropertyPath.FromName(
                    nameof(HaikaraAddressablesGroupMakerViewModel.LabelWindowInfo)
                ),
                elementNameInfo: ElementNames.LabelWindowInfo
            );

        private static readonly BindableProperty<TextField> UxmlGroupNameProperty =
            BindableProperty<TextField>.Create(
                bindingId: nameof(TextField.value),
                dataSourcePath: PropertyPath.FromName(
                    nameof(HaikaraAddressablesGroupMakerViewModel.UxmlGroupName)
                ),
                elementNameInfo: ElementNames.AddressableGroupNameTextFieldUxmlGroup,
                BindingMode.TwoWay
            );

        private static readonly BindableProperty<TextField> UssGroupNameProperty =
            BindableProperty<TextField>.Create(
                bindingId: nameof(TextField.value),
                dataSourcePath: PropertyPath.FromName(
                    nameof(HaikaraAddressablesGroupMakerViewModel.UssGroupName)
                ),
                elementNameInfo: ElementNames.AddressableGroupNameTextFieldUssGroup,
                BindingMode.TwoWay
            );

        private static readonly BindableProperty<Button> CanBuildProperty =
            BindableProperty<Button>.Create(
                bindingId: nameof(Button.enabledSelf),
                dataSourcePath: PropertyPath.FromName(
                    nameof(HaikaraAddressablesGroupMakerViewModel.CanRunBuild)
                ),
                elementNameInfo: ElementNames.ButtonRunBuild
            );

        private static readonly BindableProperty<Label> ValidationInfoProperty =
            BindableProperty<Label>.Create(
                bindingId: nameof(Label.text),
                dataSourcePath: PropertyPath.FromName(
                    nameof(HaikaraAddressablesGroupMakerViewModel.ValidationInfo)
                ),
                elementNameInfo: ElementNames.ValidationInfoText
            );

        private static readonly BindableProperty<Label> ShowValidationInfoProperty =
            BindableProperty<Label>.Create(
                bindingId: nameof(Label.visible),
                dataSourcePath: PropertyPath.FromName(
                    nameof(HaikaraAddressablesGroupMakerViewModel.ShowValidationInfo)
                ),
                elementNameInfo: ElementNames.ValidationInfoText
            );

        [ClickCommand(ElementNames.ButtonRunBuild)]
        private void OnClickBuild(EventBase _)
        {
            AddressablesGroupMaker.Build();
        }
    }
}

#endif