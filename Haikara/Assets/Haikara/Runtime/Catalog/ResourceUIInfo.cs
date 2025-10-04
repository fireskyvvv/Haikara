using System;
using UnityEngine;

namespace Haikara.Runtime.Catalog
{
    [Serializable]
    public class ResourceUIInfo<T> : UIAssetInfo<T> where T : UnityEngine.Object
    {
        [SerializeField] private T? asset;
        public T? Asset => asset;

        protected ResourceUIInfo()
        {
        }

        public ResourceUIInfo(string id, T? asset) : base(id)
        {
            this.asset = asset;
        }
    }
}