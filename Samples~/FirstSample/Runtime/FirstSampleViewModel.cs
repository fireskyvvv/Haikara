using Haikara.Runtime.ViewModel;
using Unity.Properties;

namespace Haikara.Samples.FirstSample.Runtime
{
    public class FirstSampleViewModel : ViewModelBase
    {
        [CreateProperty] public string Label { get; } = "Hello, Haikara!";
    }
}