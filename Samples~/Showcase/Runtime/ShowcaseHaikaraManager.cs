using Haikara.Runtime;
using Haikara.Samples.Common.Runtime;

namespace Haikara.Samples.Showcase.Runtime
{
    public class ShowcaseHaikaraManager : SampleCommonHaikaraManager
    {
        protected override async void Initialize(HaikaraUIContext uiContext)
        {
            Common.Runtime.ViewInstaller.Install();
            ViewInstaller.Install();

            var root = new View.ShowcaseLayout();
            await root.LoadAndAddToAsync(uiDocument.rootVisualElement);
            root.SetDataSource(new ShowcaseViewModel());
        }
    }
}