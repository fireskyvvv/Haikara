#if HAIKARA_IS_EXISTS_ADDRESSABLES
namespace Haikara.Samples.Addressables.Runtime
{
    public class AssetBundleCountModel
    {
        public int currentCount = 0;
        public string Label => $"CurrentCount:{currentCount}";

        public void DebugLogCurrentCount()
        {
            UnityEngine.Debug.Log(currentCount);
        }
    }
}
#endif