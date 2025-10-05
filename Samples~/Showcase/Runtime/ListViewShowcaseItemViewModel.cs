using Haikara.Runtime.ViewModel;
using Unity.Properties;

namespace Haikara.Samples.Showcase.Runtime
{
    public class ListViewShowcaseItemViewModel : ViewModelBase
    {
        [CreateProperty] public string Label { get; }

        public ListViewShowcaseItemViewModel(string label)
        {
            Label = label;
        }
    }
}