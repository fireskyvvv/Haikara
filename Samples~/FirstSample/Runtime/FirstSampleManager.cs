using Haikara.Runtime;
using Haikara.Samples.Common.Runtime;

namespace Haikara.Samples.FirstSample.Runtime
{
    public class FirstSampleManager : SampleCommonHaikaraManager
    {
        protected override async void Initialize(HaikaraUIContext uiContext)
        {
            var view = new View.FirstSample();
            await view.LoadAndAddToAsync(uiDocument.rootVisualElement);
            view.SetDataSource(new FirstSampleViewModel());
        }
    }
}