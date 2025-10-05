using Haikara.Runtime.ViewModel;
using Unity.Properties;

namespace Haikara.Sample.Counter
{
    public class CounterTemplateViewModel : ViewModelBase
    {
        public CounterGrandChildTemplateViewModel GrandChildViewModel { get; } = new();

        private string _templateLabel = "0";

        [CreateProperty]
        public string TemplateLabel
        {
            get => _templateLabel;
            set
            {
                _templateLabel = value;
                OnPropertyChanged();
            }
        }

        public void OnAddCount(int newCount)
        {
            TemplateLabel = (newCount * 2).ToString();
            GrandChildViewModel.OnAddCount(newCount);
        }
    }

    public class CounterGrandChildTemplateViewModel : ViewModelBase
    {
        private string _grandChildLabel = "0";

        [CreateProperty]
        public string GrandChildLabel
        {
            get => _grandChildLabel;
            set
            {
                _grandChildLabel = value;
                OnPropertyChanged();
            }
        }

        public void OnAddCount(int newCount)
        {
            GrandChildLabel = (newCount * 10).ToString();
        }
    }

    public class CounterViewModel : ViewModelBase
    {
        public CounterTemplateViewModel TemplateViewModel { get; } = new();
        private string _label;
        [CreateProperty] public string Label => _countModel.Label;

        private readonly CountModel _countModel;

        public CounterViewModel(CountModel countModel)
        {
            _countModel = countModel;
        }

        public void AddCount()
        {
            _countModel.currentCount++;
            OnPropertyChanged(nameof(Label));
            _countModel.DebugLogCurrentCount();

            TemplateViewModel.OnAddCount(_countModel.currentCount);
        }
    }
}