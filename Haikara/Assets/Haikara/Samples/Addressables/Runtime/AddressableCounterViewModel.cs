#if HAIKARA_IS_EXISTS_ADDRESSABLES
using Haikara.Runtime.ViewModel;
using Unity.Properties;

namespace Haikara.Samples.Addressables.Runtime
{
    public class AddressableCounterViewModel : ViewModelBase
    {
        private string _label;
        [CreateProperty] public string Label => _assetBundleCountModel.Label;

        private readonly AssetBundleCountModel _assetBundleCountModel;

        public AddressableCounterViewModel(AssetBundleCountModel assetBundleCountModel)
        {
            _assetBundleCountModel = assetBundleCountModel;
        }

        public void AddCount()
        {
            _assetBundleCountModel.currentCount++;
            OnPropertyChanged(nameof(Label));
            _assetBundleCountModel.DebugLogCurrentCount();
        }
    }
}
#endif