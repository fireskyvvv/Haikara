using System;
using UnityEngine;

namespace Haikara.Runtime.Catalog
{
    [Serializable]
    public class AssetPathUIInfo<T> : UIAssetInfo<T> where T : UnityEngine.Object
    {
        [SerializeField] private string assetPath = "";
        public string AssetPath => assetPath;

        protected AssetPathUIInfo()
        {
        }

        public AssetPathUIInfo(string id, string assetPath) : base(id)
        {
            this.assetPath = assetPath;
        }
    }
}