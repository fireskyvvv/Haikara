namespace Haikara.Runtime.View
{
    public interface IViewModelProvidable : IHaikaraView
    {
        public object? ProvideSubViewModel(object? parentViewModel);
    }
}