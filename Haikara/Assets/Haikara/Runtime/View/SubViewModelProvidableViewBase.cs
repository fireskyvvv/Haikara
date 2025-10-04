using Haikara.Runtime.ViewModel;

namespace Haikara.Runtime.View
{
    /// <summary>
    /// Represents a view that is designed to receive a TViewModel as its DataSource from a parent ViewModel.
    /// The logic for providing the TViewModel is defined in the <see cref="ProvideSubViewModel"/> method.
    /// </summary>
    /// <typeparam name="TParentViewModel"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    public abstract class
        SubViewModelProvidableViewBase<TParentViewModel, TViewModel>
        : HaikaraViewBaseWithViewModel<TViewModel>, IViewModelProvidable
        where TParentViewModel : ViewModelBase
        where TViewModel : ViewModelBase
    {
        public object? ProvideSubViewModel(object? parentViewModel)
        {
            if (parentViewModel is TParentViewModel parent)
            {
                return ProvideSubViewModel(parent);
            }

            return null;
        }

        protected abstract TViewModel? ProvideSubViewModel(TParentViewModel parentViewModel);
    }
}