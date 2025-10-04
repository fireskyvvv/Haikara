using Haikara.Runtime.ViewModel;
using Unity.Properties;

namespace Haikara.Samples.Showcase.Runtime
{
    public class ShowcaseViewModel : ViewModelBase
    {
        [CreateProperty] public ControlsShowcaseViewModel Controls { get; } = new();
        [CreateProperty] public ListViewShowcaseViewModel ListView { get; } = new();
    }
}