using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Haikara.Runtime.Catalog
{
    [Serializable]
    public abstract class UIAssetInfo<T> where T : UnityEngine.Object
    {
        [SerializeField] private string id = "";
        public string Id => id;

        protected UIAssetInfo()
        {
        }

        protected UIAssetInfo(string id)
        {
            this.id = id;
        }
    }
}