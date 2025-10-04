namespace Haikara.Runtime
{
    public interface IHaikaraUI
    {
        public string GetGuid() => "";
        public AssetReferenceMode AssetReferenceMode { get; }
    }
}