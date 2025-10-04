#if HAIKARA_IS_EXISTS_ADDRESSABLES
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Haikara.Runtime.Util;
using Haikara.Runtime.Catalog;
using Haikara.Runtime.Catalog.UILoader;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace Haikara.Runtime.AddressablesExtension
{
    public class AddressablesUILoader<TAsset> : CustomUILoader<TAsset> where TAsset : UnityEngine.Object
    {
        private readonly Dictionary<string, AsyncOperationHandle<TAsset>> _idToHandle = new();

        public override TAsset? Load(CustomUIInfo<TAsset> uiAssetInfo)
        {
            if (_idToHandle.TryGetValue(uiAssetInfo.Id, out var currentHandle))
            {
                return currentHandle.WaitForCompletion();
            }
            
            var handle = Addressables.LoadAssetAsync<TAsset>(uiAssetInfo.Id);
            var result = handle.WaitForCompletion();
            _idToHandle.Add(uiAssetInfo.Id, handle);
            return result;
        }

        public override async Task<TAsset?> LoadAsync(CustomUIInfo<TAsset> uiAssetInfo)
        {
            if (_idToHandle.TryGetValue(uiAssetInfo.Id, out var currentHandle))
            {
                return await currentHandle.Task;
            }
            
            var handle = Addressables.LoadAssetAsync<TAsset>(uiAssetInfo.Id);
            _idToHandle.Add(uiAssetInfo.Id, handle);
            return await handle.Task;
        }

        public override Task ReleaseAsync(CustomUIInfo<TAsset> uiAssetInfo, TAsset asset)
        {
            if (_idToHandle.TryGetValue(uiAssetInfo.Id, out var handle))
            {
                handle.Release();
                _idToHandle.Remove(uiAssetInfo.Id);
            }
            else
            {
                UnityEngine.Debug.LogError($"{uiAssetInfo.Id} has not been loaded");
            }

            return Task.CompletedTask;
        }
    }
}

#endif