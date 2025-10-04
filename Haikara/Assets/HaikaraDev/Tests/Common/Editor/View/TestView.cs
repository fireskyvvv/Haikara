using Haikara.Runtime;
using Haikara.Runtime.Bindable;
using Haikara.Runtime.View;
using HaikaraDev.Tests.Common.Editor.ViewModel;
using Unity.Properties;
using UnityEngine.UIElements;

namespace HaikaraDev.Tests.Common.Editor.View
{
    [HaikaraUI]
    public partial class TestView : HaikaraViewBaseWithViewModel<TestViewModel>
    {
        private static readonly BindableProperty<Label> LabelTextProperty =
            BindableProperty<Label>.Create(
                bindingId: PropertyPath.FromName(nameof(Label.text)),
                dataSourcePath: PropertyPath.FromName(nameof(TestViewModel.Label)),
                elementNameInfo: "test-view__label"
            );

        private static readonly BindableProperty<Toggle> ToggleValueProperty =
            BindableProperty<Toggle>.Create(
                bindingId: PropertyPath.FromName(nameof(Toggle.value)),
                dataSourcePath: PropertyPath.FromName(nameof(TestViewModel.Toggle)),
                elementNameInfo: "test-view__toggle",
                BindingMode.TwoWay
            );

        private static readonly BindableProperty<TextField> TextFieldValueProperty =
            BindableProperty<TextField>.Create(
                bindingId: PropertyPath.FromName(nameof(TextField.value)),
                dataSourcePath: PropertyPath.FromName(nameof(TestViewModel.TextField)),
                elementNameInfo: "test-view__text-field",
                BindingMode.ToSource
            );

        private static readonly TemplateProperty<Editor.View.TestSubView> SubViewProperty =
            TemplateProperty<Editor.View.TestSubView>.Create(
                new TemplateInfo(
                    elementName: "test-view-sub",
                    viewGuid: "4f74df35d43446d479b3145387941d86",
                    templateId: "TestSubView"
                )
            );

        [ClickCommand("test-view__button")]
        private void OnClick(EventBase evt)
        {
            ViewModel?.AddClickCount();
        }
    }
}