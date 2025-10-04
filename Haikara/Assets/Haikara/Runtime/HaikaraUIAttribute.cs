using System;

namespace Haikara.Runtime
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class HaikaraUIAttribute : Attribute
    {
        public AssetReferenceMode ReferenceMode { get; set; } = AssetReferenceMode.Resource;
    }
}