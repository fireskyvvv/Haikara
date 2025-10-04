using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Haikara.Runtime.Catalog.UILoader
{
    internal interface IUILoader<TAsset, in TUIAssetInfo> where TAsset : Object where TUIAssetInfo : UIAssetInfo<TAsset>
    {
        public TAsset? Load(TUIAssetInfo uiAssetInfo);
        public Task<TAsset?> LoadAsync(TUIAssetInfo uiAssetInfo);

        public Task ReleaseAsync(TUIAssetInfo uiAssetInfo, TAsset asset);
    }
}