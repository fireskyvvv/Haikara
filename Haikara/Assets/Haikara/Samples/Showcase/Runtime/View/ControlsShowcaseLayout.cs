using System;
using Haikara.Runtime;
using Haikara.Runtime.Bindable;
using Haikara.Runtime.View;
using Unity.Properties;
using UnityEngine.UIElements;

namespace Haikara.Samples.Showcase.Runtime.View
{
    [HaikaraUI]
    public partial class
        ControlsShowcaseLayout : SubViewModelProvidableViewBase<ShowcaseViewModel,
        ControlsShowcaseViewModel>
    {
        private static readonly BindableProperty<Label> LabelTextProperty =
            BindableProperty<Label>.Create(
                bindingId: PropertyPath.FromName(nameof(Label.text)),
                dataSourcePath: PropertyPath.FromName(nameof(ControlsShowcaseViewModel.Label)),
                elementNameInfo: ElementNames.Label
            );

        private static readonly BindableProperty<Toggle> ToggleValueProperty =
            BindableProperty<Toggle>.Create(
                bindingId: PropertyPath.FromName(nameof(Toggle.value)),
                dataSourcePath: PropertyPath.FromName(nameof(ControlsShowcaseViewModel.ToggleValue)),
                elementNameInfo: ElementNames.Toggle,
                bindingMode: BindingMode.TwoWay
            );

        private static readonly BindableProperty<ToggleButtonGroup> ToggleButtonGroupValueProperty =
            BindableProperty<ToggleButtonGroup>.Create(
                bindingId: PropertyPath.FromName(nameof(ToggleButtonGroup.value)),
                dataSourcePath: PropertyPath.FromName(nameof(ControlsShowcaseViewModel.ToggleButtonGroupState)),
                elementNameInfo: ElementNames.ToggleButtonGroup,
                bindingMode: BindingMode.TwoWay
            );

        private static readonly BindableProperty<TextField> TextFieldValueProperty =
            BindableProperty<TextField>.Create(
                bindingId: PropertyPath.FromName(nameof(TextField.value)),
                dataSourcePath: PropertyPath.FromName(nameof(ControlsShowcaseViewModel.TextFieldValue)),
                elementNameInfo: ElementNames.TextField,
                BindingMode.TwoWay
            );

        private static readonly BindableProperty<Slider> SliderValueProperty = BindableProperty<Slider>.Create(
            bindingId: PropertyPath.FromName(nameof(Slider.value)),
            dataSourcePath: PropertyPath.FromName(nameof(ControlsShowcaseViewModel.SliderValue)),
            elementNameInfo: ElementNames.Slider,
            BindingMode.TwoWay
        );

        private static readonly BindableProperty<SliderInt> SliderIntValueProperty = BindableProperty<SliderInt>.Create(
            bindingId: PropertyPath.FromName(nameof(SliderInt.value)),
            dataSourcePath: PropertyPath.FromName(nameof(ControlsShowcaseViewModel.SliderIntValue)),
            elementNameInfo: ElementNames.SliderInt,
            BindingMode.TwoWay
        );

        private static readonly BindableProperty<MinMaxSlider> MinMaxSliderValueProperty =
            BindableProperty<MinMaxSlider>.Create(
                bindingId: PropertyPath.FromName(nameof(MinMaxSlider.value)),
                dataSourcePath: PropertyPath.FromName(nameof(ControlsShowcaseViewModel.MinMaxSliderValue)),
                elementNameInfo: ElementNames.MinMaxSlider,
                BindingMode.TwoWay
            );

        private static readonly BindableProperty<Label> MinMaxSliderLabelProperty = BindableProperty<Label>.Create(
            bindingId: PropertyPath.FromName(nameof(Label.text)),
            dataSourcePath: PropertyPath.FromName(nameof(ControlsShowcaseViewModel.MinMaxSliderInfo)),
            elementNameInfo: ElementNames.MinMaxSliderInfo
        );

        private static readonly BindableProperty<Button> ProgressBarStartButton =
            BindableProperty<Button>.Create(
                bindingId: PropertyPath.FromName(nameof(Button.enabledSelf)),
                dataSourcePath: PropertyPath.FromName(nameof(ControlsShowcaseViewModel.ProgressStartButtonIsEnabled)),
                elementNameInfo: ElementNames.ButtonProgressStart
            );

        private static readonly BindableProperty<ProgressBar> ProgressBarTitleProperty =
            BindableProperty<ProgressBar>.Create(
                bindingId: PropertyPath.FromName(nameof(ProgressBar.title)),
                dataSourcePath: PropertyPath.FromName(nameof(ControlsShowcaseViewModel.ProgressBarTitle)),
                elementNameInfo: ElementNames.ProgressBar
            );

        private static readonly BindableProperty<ProgressBar> ProgressBarValueProperty =
            BindableProperty<ProgressBar>.Create(
                bindingId: PropertyPath.FromName(nameof(ProgressBar.value)),
                dataSourcePath: PropertyPath.FromName(nameof(ControlsShowcaseViewModel.ProgressBarValue)),
                elementNameInfo: ElementNames.ProgressBar
            );

        private static readonly BindableProperty<DropdownField> DropdownValueProperty =
            BindableProperty<DropdownField>.Create(
                bindingId: PropertyPath.FromName(nameof(DropdownField.index)),
                dataSourcePath: PropertyPath.FromName(nameof(ControlsShowcaseViewModel.DropdownIndex)),
                elementNameInfo: ElementNames.Dropdown,
                BindingMode.TwoWay
            );

        private static readonly BindableProperty<DropdownField> DropdownChoicesProperty =
            BindableProperty<DropdownField>.Create(
                bindingId: PropertyPath.FromName(nameof(DropdownField.choices)),
                dataSourcePath: PropertyPath.FromName(nameof(ControlsShowcaseViewModel.DropdownChoices)),
                elementNameInfo: ElementNames.Dropdown
            );

        /// <summary>
        /// If the type for an EnumField is not specified in the UI Builder (or UXML),
        /// it is necessary to call EnumField.Init().
        /// EnumFieldValueProperty is a BindableProperty that executes EnumField.Init() to handle this.
        /// </summary>
        private static readonly EnumFieldValueProperty EnumFieldValueProperty =
            EnumFieldValueProperty.Create(
                defaultValue: ControlShowCaseModel.SampleEnum.Midori,
                dataSourcePath: PropertyPath.FromName(nameof(ControlsShowcaseViewModel.EnumFieldValue)),
                elementNameInfo: ElementNames.EnumField,
                BindingMode.TwoWay,
                includeObsoleteValues: true
            );

        /*
        /// <summary>
        /// If the type for an EnumField is already specified in the UI Builder (or UXML),
        /// it is not necessary to call EnumField.Init().
        /// In this case, a standard BindableProperty can be used.
        /// </summary>
        private static readonly BindableProperty<EnumField> EnumFieldValueProperty =
            BindableProperty<EnumField>.Create(
                bindingId: PropertyPath.FromName(nameof(EnumField.value)),
                dataSourcePath: PropertyPath.FromName(nameof(ControlsShowcaseViewModel.EnumFieldValue)),
                elementNameInfo: ElementNames.EnumField,
                BindingMode.TwoWay
            );
        */

        private static readonly BindableProperty<RadioButton> RadioButtonValueProperty =
            BindableProperty<RadioButton>.Create(
                bindingId: PropertyPath.FromName(nameof(RadioButton.value)),
                dataSourcePath: PropertyPath.FromName(nameof(ControlsShowcaseViewModel.RadioButtonValue)),
                elementNameInfo: ElementNames.RadioButton,
                BindingMode.TwoWay
            );

        private static readonly BindableProperty<RadioButtonGroup> RadioButtonGroupValueProperty =
            BindableProperty<RadioButtonGroup>.Create(
                bindingId: PropertyPath.FromName(nameof(RadioButtonGroup.value)),
                dataSourcePath: PropertyPath.FromName(nameof(ControlsShowcaseViewModel.RadioButtonGroupValue)),
                elementNameInfo: ElementNames.RadioButtonGroup,
                BindingMode.TwoWay
            );

        private static readonly BindableProperty<RadioButtonGroup> RadioButtonGroupChoicesProperty =
            BindableProperty<RadioButtonGroup>.Create(
                bindingId: PropertyPath.FromName(nameof(RadioButtonGroup.choices)),
                dataSourcePath: PropertyPath.FromName(nameof(ControlsShowcaseViewModel.RadioButtonGroupChoices)),
                elementNameInfo: ElementNames.RadioButtonGroup
            );

        [ClickCommand(ElementNames.Button)]
        private void OnClick(EventBase evt)
        {
            ViewModel?.OutputLog();
        }

        [ClickCommand(ElementNames.ButtonProgressStart)]
        private void OnClickProgressStart(EventBase evt)
        {
            ViewModel?.StartProgress();
        }

        protected override ControlsShowcaseViewModel ProvideSubViewModel(ShowcaseViewModel parentViewModel)
        {
            return parentViewModel.Controls;
        }
    }
}