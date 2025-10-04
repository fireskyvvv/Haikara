using System;
using System.Collections.Generic;
using Haikara.Runtime.Util;
using UnityEngine;

namespace Haikara.Runtime.Catalog
{
    [Serializable]
    public class UIAssets<T> where T : UnityEngine.Object
    {
        [SerializeField] private List<AssetPathUIInfo<T>> assetPathUIInfoList = new();
        public List<AssetPathUIInfo<T>> AssetPathUIInfoList => assetPathUIInfoList;

        [SerializeField] private List<ResourceUIInfo<T>> resourceUIInfoList = new();
        public List<ResourceUIInfo<T>> ResourceUIInfoList => resourceUIInfoList;

        [SerializeField] private List<CustomUIInfo<T>> customUIInfoList = new();
        public List<CustomUIInfo<T>> CustomUIInfoList => customUIInfoList;

        public Dictionary<string, UIAssetInfo<T>> GetIdToUIAssetMap()
        {
            var result = new Dictionary<string, UIAssetInfo<T>>();

            AddToMap(result, AssetPathUIInfoList);
            AddToMap(result, ResourceUIInfoList);
            AddToMap(result, CustomUIInfoList);

            return result;
        }

        private void AddToMap<TUIAssetInfo>(
            Dictionary<string, UIAssetInfo<T>> map,
            List<TUIAssetInfo> uiAssetInfoList
        ) where TUIAssetInfo : UIAssetInfo<T>
        {
            foreach (var uiAssetInfo in uiAssetInfoList)
            {
                var id = uiAssetInfo.Id;
                if (map.TryGetValue(id, out var addedVisualTreeAsset))
                {
                    Debug.LogWarning(HaikaraLogUtil.GetMessage($"Detected same ui asset id. id: {id}"));
                }
                else
                {
                    map.Add(id, uiAssetInfo);
                }
            }
        }
    }
}