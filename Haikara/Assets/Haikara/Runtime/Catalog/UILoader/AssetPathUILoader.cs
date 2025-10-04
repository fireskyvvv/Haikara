using System.Threading.Tasks;

namespace Haikara.Runtime.Catalog.UILoader
{
    public sealed class AssetPathUILoader<TAsset> : IUILoader<TAsset, AssetPathUIInfo<TAsset>>
        where TAsset : UnityEngine.Object
    {
        public TAsset? Load(AssetPathUIInfo<TAsset> uiAssetInfo)
        {
#if UNITY_EDITOR

            var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<TAsset>(uiAssetInfo.AssetPath);
            if (asset == null)
            {
                return null;
            }

            return asset;
#else
            UnityEngine.Debug.LogError(
                Haikara.Runtime.Util.HaikaraLogUtil.GetMessage(
                    $"{nameof(AssetReferenceMode)}.{AssetReferenceMode.AssetPath} is EditorOnly. Can not load UI."
                )
            );

            return null;
#endif
        }
        
        public Task<TAsset?> LoadAsync(AssetPathUIInfo<TAsset> uiAssetInfo)
        {
            return Task.FromResult(Load(uiAssetInfo));
        }

        public Task ReleaseAsync(AssetPathUIInfo<TAsset> uiAssetInfo, TAsset asset)
        {
            UnityEngine.Object.DestroyImmediate(asset);
            return Task.CompletedTask;
        }
    }
}