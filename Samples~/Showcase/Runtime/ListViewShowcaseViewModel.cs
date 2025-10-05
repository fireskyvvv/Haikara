using System.Collections.Generic;
using Haikara.Runtime.ViewModel;
using Unity.Properties;

namespace Haikara.Samples.Showcase.Runtime
{
    public class ListViewShowcaseViewModel : ViewModelBase
    {
        [CreateProperty] public string TabLabel => "ListView";
        [CreateProperty] public List<ListViewShowcaseItemViewModel> Current { get; } = new();
    }
}