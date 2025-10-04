using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Haikara.Runtime.Catalog.UILoader;
using Haikara.Runtime.Util;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Haikara.Runtime.Catalog
{
    public class UICollection<T> where T : Object
    {
        private Dictionary<string, UIAssetInfo<T>> _idToUIAssetInfo = new();
        private readonly AssetPathUILoader<T> _assetPathUILoader = new();
        private readonly ResourceUILoader<T> _resourceUILoader = new();
        private CustomUILoader<T>? _customUILoader;

        private readonly Dictionary<string, int> _counter = new();
        private readonly Dictionary<string, T> _idToAsset = new();

        public void Initialize(UIAssets<T> uiAssets)
        {
            _idToUIAssetInfo = uiAssets.GetIdToUIAssetMap();
        }

        public void RegisterCustomUILoader(CustomUILoader<T> customUILoader)
        {
            _customUILoader = customUILoader;
        }

        public void UnregisterCustomUILoader()
        {
            _customUILoader = null;
        }

        public T? LoadOrIncrementUIAsset(string id)
        {
            if (TryGetLoadedUI(id, out var uiAssetInfo, out var loadedUI))
            {
                return loadedUI;
            }
            
            switch (uiAssetInfo)
            {
                case AssetPathUIInfo<T> assetPathUIInfo:
                    loadedUI = _assetPathUILoader.Load(assetPathUIInfo);
                    break;
                case ResourceUIInfo<T> resourceUIInfo:
                    loadedUI = _resourceUILoader.Load(resourceUIInfo);
                    break;
                case CustomUIInfo<T> customUIInfo:
                    if (_customUILoader != null)
                    {
                        loadedUI = _customUILoader.Load(customUIInfo);
                        break;
                    }

                    UnityEngine.Debug.LogError(
                        HaikaraLogUtil.GetMessage($"CustomUILoader<{typeof(T)}> is not registered"));
                    break;
                default:
                    if (uiAssetInfo == null)
                    {
                        throw new Exception(
                            HaikaraLogUtil.GetMessage($"Invalid UIAssetInfo. UIAssetInfo is null. Id: {id}")
                        );
                    }

                    throw new Exception(
                        HaikaraLogUtil.GetMessage($"Invalid UIAssetInfo. Type: {uiAssetInfo.GetType()}")
                    );
            }

            if (loadedUI != null)
            {
                _idToAsset.TryAdd(id, loadedUI);
            }

            return loadedUI;
        }

        public async Task<T?> LoadOrIncrementUIAssetAsync(string id)
        {
            if (TryGetLoadedUI(id, out var uiAssetInfo, out var loadedUI))
            {
                return loadedUI;
            }
            
            switch (uiAssetInfo)
            {
                case AssetPathUIInfo<T> assetPathUIInfo:
                    loadedUI = await _assetPathUILoader.LoadAsync(assetPathUIInfo);
                    break;
                case ResourceUIInfo<T> resourceUIInfo:
                    loadedUI = await _resourceUILoader.LoadAsync(resourceUIInfo);
                    break;
                case CustomUIInfo<T> customUIInfo:
                    if (_customUILoader != null)
                    {
                        loadedUI = await _customUILoader.LoadAsync(customUIInfo);
                        break;
                    }

                    UnityEngine.Debug.LogError(
                        HaikaraLogUtil.GetMessage($"CustomUILoader<{typeof(T)}> is not registered"));
                    break;
                default:
                    if (uiAssetInfo == null)
                    {
                        throw new Exception(
                            HaikaraLogUtil.GetMessage($"Invalid UIAssetInfo. UIAssetInfo is null. Id: {id}")
                        );
                    }

                    throw new Exception(
                        HaikaraLogUtil.GetMessage($"Invalid UIAssetInfo. Type: {uiAssetInfo.GetType()}")
                    );
            }

            if (loadedUI != null)
            {
                _idToAsset.TryAdd(id, loadedUI);
            }

            return loadedUI;
        }

        private bool TryGetLoadedUI(string id, out UIAssetInfo<T> uiAssetInfo, [NotNullWhen(true)] out T? loadedUI)
        {
            if (!_idToUIAssetInfo.TryGetValue(id, out uiAssetInfo))
            {
                throw new Exception($"Invalid ui asset id: {id}");
            }

            loadedUI = null;
            if (!_counter.TryGetValue(id, out var count))
            {
                count = 1;
                _counter.Add(id, count);
            }
            else
            {
                _counter[id] += 1;
            }

            if (_counter[id] > 0 && _idToAsset.TryGetValue(id, out loadedUI))
            {
                return true;
            }

            return false;
        }

        public async Task ReleaseOrDecrementUIAssetAsync(string id)
        {
            if (!_counter.TryGetValue(id, out _) || !_idToUIAssetInfo.TryGetValue(id, out var uiAssetInfo))
            {
                UnityEngine.Debug.LogError(HaikaraLogUtil.GetMessage($"UIAsset has not been loaded. Id: {id}"));
                return;
            }

            _counter[id] -= 1;

            if (_counter[id] > 0)
            {
                return;
            }

            if (!_idToAsset.TryGetValue(id, out var asset))
            {
                UnityEngine.Debug.LogError(
                    HaikaraLogUtil.GetMessage($"There is no UIAsset instance. Could not release UIAsset. Id: {id}")
                );
                return;
            }

            switch (uiAssetInfo)
            {
                case AssetPathUIInfo<T> assetPathUIInfo:
                    await _assetPathUILoader.ReleaseAsync(assetPathUIInfo, asset);
                    break;
                case ResourceUIInfo<T> resourceUIInfo:
                    await _resourceUILoader.ReleaseAsync(resourceUIInfo, asset);
                    break;
                case CustomUIInfo<T> customUIInfo:
                    if (_customUILoader != null)
                    {
                        await _customUILoader.ReleaseAsync(customUIInfo, asset);
                        break;
                    }

                    UnityEngine.Debug.LogError(
                        HaikaraLogUtil.GetMessage($"CustomUILoader<{typeof(T)}> is not registered")
                    );
                    break;
                default:
                    throw new Exception($"Invalid UIAssetInfo. Type: {uiAssetInfo.GetType()}");
            }

            _idToAsset.Remove(id);
        }
    }
}