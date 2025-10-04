using System;
using System.Threading.Tasks;

namespace Haikara.Runtime.Catalog.UILoader
{
    public abstract class CustomUILoader<TAsset> : IUILoader<TAsset, CustomUIInfo<TAsset>>, IDisposable
        where TAsset : UnityEngine.Object
    {
        public abstract TAsset? Load(CustomUIInfo<TAsset> uiAssetInfo);
        public abstract Task<TAsset?> LoadAsync(CustomUIInfo<TAsset> uiAssetInfo);
        public abstract Task ReleaseAsync(CustomUIInfo<TAsset> uiAssetInfo, TAsset asset);

        public virtual void Dispose()
        {
        }
    }
    
}