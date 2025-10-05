using Haikara.Runtime;
using Haikara.Samples.Common.Runtime.Dialog;
using Haikara.Sample.Counter;
using Haikara.Samples.Common.Runtime;
using Haikara.Samples.Common.Runtime.Dialog.View;
using CounterViewInstaller = Haikara.Samples.Counter.Runtime.ViewInstaller;
using CommonViewInstaller = Haikara.Samples.Common.Runtime.ViewInstaller;
using SampleBackdrop = Haikara.Samples.Common.Runtime.Overlay.SampleBackdrop;

namespace Haikara.Samples.Counter.Runtime
{
    public class CounterSampleHaikaraManager : SampleCommonHaikaraManager
    {
        protected override void PreprocessInitialize(HaikaraUIContext uiContext)
        {
            SampleDialogProvider.Instance.RegisterDialog<SampleDialogViewmodel>(SampleDialog.UxmlGuid);
            SampleDialogProvider.Instance.Initialize(uiContext,
                SampleDefaultDialogRoot.UxmlGuid,
                SampleBackdrop.UxmlGuid
            );
        }

        protected override async void Initialize(HaikaraUIContext uiContext)
        {
            CounterViewInstaller.Install();
            CommonViewInstaller.Install();
            var counter = new View.Counter();
            await counter.LoadAndAddToAsync(uiDocument.rootVisualElement);
            counter.SetDataSource(new CounterViewModel(new CountModel()));
        }
    }
}