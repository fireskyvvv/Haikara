using System;

namespace Haikara.Runtime.Catalog
{
    [Serializable]
    public class CustomUIInfo<T> : UIAssetInfo<T> where T : UnityEngine.Object
    {
        protected CustomUIInfo()
        {
        }

        public CustomUIInfo(string id) : base(id)
        {
        }
    }
}