using Haikara.Runtime.ViewModel;
using Unity.Properties;

namespace HaikaraDev.Tests.Common.Editor.ViewModel
{
    public class TestViewModel : ViewModelBase
    {
        [CreateProperty] public string Label { get; }

        private bool _toggle;

        [CreateProperty]
        public bool Toggle
        {
            get => _toggle;
            set
            {
                _toggle = value;
                OnPropertyChanged();
            }
        }

        private string _textField = "";

        [CreateProperty]
        public string TextField
        {
            get => _textField;
            set
            {
                _textField = value;
                OnPropertyChanged();
            }
        }

        public TestSubViewModel SubViewModel { get; }
        public int ClickCount { get; private set; }

        public TestViewModel(string label, TestSubViewModel subViewModel)
        {
            Label = label;
            SubViewModel = subViewModel;
        }

        public void AddClickCount()
        {
            ClickCount++;
        }
    }
}