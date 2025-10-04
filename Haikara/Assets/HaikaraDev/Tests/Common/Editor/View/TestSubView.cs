using Haikara.Runtime;
using Haikara.Runtime.Bindable;
using Haikara.Runtime.View;
using HaikaraDev.Tests.Common.Editor.ViewModel;
using Unity.Properties;
using UnityEngine.UIElements;

namespace HaikaraDev.Tests.Common.Editor.View
{
    [HaikaraUI]
    public partial class TestSubView : HaikaraViewBaseWithViewModel<TestSubViewModel>, IViewModelProvidable
    {
        private static readonly BindableProperty<Label> LabelTextProperty =
            BindableProperty<Label>.Create(
                bindingId: PropertyPath.FromName(nameof(Label.text)),
                dataSourcePath: PropertyPath.FromName(nameof(TestSubViewModel.Label)),
                elementNameInfo: "test-sub__label"
            );
        
        public object ProvideSubViewModel(object parentViewModel)
        {
            if (parentViewModel is TestViewModel editModeTestViewModel)
            {
                return editModeTestViewModel.SubViewModel;
            }

            return null;
        }
    }
}