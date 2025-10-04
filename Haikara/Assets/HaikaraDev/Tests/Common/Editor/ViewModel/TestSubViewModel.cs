using Haikara.Runtime.ViewModel;
using Unity.Properties;

namespace HaikaraDev.Tests.Common.Editor.ViewModel
{
    public class TestSubViewModel : ViewModelBase
    {
        [CreateProperty] public string Label { get; }

        public TestSubViewModel(string label)
        {
            Label = label;
        }
    }
}