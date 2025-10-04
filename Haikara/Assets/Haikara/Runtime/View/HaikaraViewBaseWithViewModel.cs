using Haikara.Runtime.ViewModel;
using Unity.Properties;

namespace Haikara.Runtime.View
{
    public abstract class HaikaraViewBaseWithViewModel<T> : HaikaraViewBase where T : ViewModelBase
    {
        [CreateProperty]
        protected T? ViewModel
        {
            get
            {
                if (DataSource is T viewModel)
                {
                    return viewModel;
                }

                return null;
            }
        }

        public override bool ValidateDataSource(object? dataSource)
        {
            return dataSource is T;
        }
    }
}