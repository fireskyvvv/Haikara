namespace Haikara.Runtime.Style
{
    public abstract class HaikaraStyleBase : IHaikaraStyle
    {
        public virtual string GetGuid() => "";
        public virtual AssetReferenceMode AssetReferenceMode => AssetReferenceMode.Resource;
    }
}