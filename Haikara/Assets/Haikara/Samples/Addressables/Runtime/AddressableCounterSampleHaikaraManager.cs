#if HAIKARA_IS_EXISTS_ADDRESSABLES
using Haikara.Runtime;
using Haikara.Runtime.AddressablesExtension;
using Haikara.Runtime.Catalog;
using Haikara.Samples.Addressables.Runtime.View;
using Haikara.Samples.Common.Runtime;
using UnityEngine.UIElements;
using AddressableSampleViewInstaller = Haikara.Samples.Addressables.Runtime.ViewInstaller;
using CommonViewInstaller = Haikara.Samples.Common.Runtime.ViewInstaller;

namespace Haikara.Samples.Addressables.Runtime
{
    public class AddressableCounterSampleHaikaraManager : SampleCommonHaikaraManager
    {
        protected override async void Initialize(HaikaraUIContext uiContext)
        {
            AddressableSampleViewInstaller.Install();
            CommonViewInstaller.Install();

            // Both the VisualTreeAsset and the StyleSheet must be registered with the CustomUILoader.
            RuntimeUICatalog.Instance.UxmlUICollection.RegisterCustomUILoader(
                new AddressablesUILoader<VisualTreeAsset>()
            );

            RuntimeUICatalog.Instance.UssUICollection.RegisterCustomUILoader(
                new AddressablesUILoader<StyleSheet>()
            );

            var counter = new AddressableCounter();
            await counter.LoadAndAddToAsync(uiDocument.rootVisualElement);
            counter.SetDataSource(new AddressableCounterViewModel(new AssetBundleCountModel()));
        }
    }
}
#endif