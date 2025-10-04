using Haikara.Runtime;
using Haikara.Samples.Common.Runtime.Dialog;
using Haikara.Samples.Common.Runtime.Dialog.View;
using Haikara.Samples.Common.Runtime.Overlay;

namespace Haikara.Samples.Common.Runtime
{
    public abstract class SampleCommonHaikaraManager : HaikaraManager
    {
        protected override void PreprocessInitialize(HaikaraUIContext uiContext)
        {
            base.PreprocessInitialize(uiContext);
            SampleDialogProvider.Instance.RegisterDialog<SampleDialogViewmodel>(SampleDialog.UxmlGuid);
            SampleDialogProvider.Instance.Initialize(uiContext,
                SampleDefaultDialogRoot.UxmlGuid,
                SampleBackdrop.UxmlGuid
            );
        }
    }
}