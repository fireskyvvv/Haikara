using Haikara.Runtime;
using Haikara.Runtime.Bindable;
using Haikara.Runtime.View;
using Haikara.Runtime.ViewModel;
using UnityEngine.UIElements;

namespace Haikara.Samples.Common.Runtime.Overlay
{
    [HaikaraUI]
    public partial class SampleBackdrop : HaikaraViewBaseWithViewModel<ViewModelBase>
    {
        public override bool IgnoreTemplateContainerLayout => true;

        [ClickCommand(ElementNames.OverlayRoot)]
        private void OnClick(EventBase evt)
        {
            ReleaseView();
        }
    }
}