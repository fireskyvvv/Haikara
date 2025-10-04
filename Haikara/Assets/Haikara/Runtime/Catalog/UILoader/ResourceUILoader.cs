using System.Threading.Tasks;

namespace Haikara.Runtime.Catalog.UILoader
{
    public sealed class ResourceUILoader<TAsset> : IUILoader<TAsset, ResourceUIInfo<TAsset>>
        where TAsset : UnityEngine.Object
    {
        public TAsset? Load(ResourceUIInfo<TAsset> uiAssetInfo)
        {
            return uiAssetInfo.Asset;
        }

        public Task<TAsset?> LoadAsync(ResourceUIInfo<TAsset> uiAssetInfo)
        {
            return Task.FromResult(uiAssetInfo.Asset);
        }

        public Task ReleaseAsync(ResourceUIInfo<TAsset> uiAssetInfo, TAsset asset)
        {
            // do not nothing
            // Since it is an asset stored within a ScriptableObject, Destroy is unnecessary.
            return Task.CompletedTask;
        }
    }
}