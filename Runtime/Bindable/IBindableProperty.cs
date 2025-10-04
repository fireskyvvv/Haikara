namespace Haikara.Runtime.Bindable
{
    public interface IBindableProperty : IElementProperty
    {
        public object? DataSource { get; set; }
    }
}