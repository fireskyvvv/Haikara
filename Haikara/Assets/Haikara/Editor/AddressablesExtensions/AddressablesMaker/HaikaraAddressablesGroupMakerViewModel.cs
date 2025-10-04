#if HAIKARA_IS_EXISTS_ADDRESSABLES
using System.Text;
using Haikara.Runtime.ViewModel;
using Unity.Properties;

namespace Haikara.Editor.AddressablesExtensions.AddressablesMaker
{
    internal class HaikaraAddressablesGroupMakerViewModel : ViewModelBase
    {
        private static HaikaraAddressablesGroupMakerSettings Settings => HaikaraAddressablesGroupMakerSettings.instance;

        [CreateProperty] public string LabelAddressableGroupNames => HaikaraAddressablesMakerLocalizableTexts.LabelAddressableGroupNames;
        [CreateProperty] public string LabelRunBuildButton => HaikaraAddressablesMakerLocalizableTexts.LabelRunBuildButton;
        [CreateProperty] public string LabelWindowInfo => HaikaraAddressablesMakerLocalizableTexts.LabelWindowInfo;

        [CreateProperty]
        public string UxmlGroupName
        {
            get => Settings.uxmlGroupName;
            set
            {
                Settings.uxmlGroupName = value;
                Settings.SaveAsText();
                OnPropertyChanged();
                UpdateValidationInfo();
            }
        }

        [CreateProperty]
        public string UssGroupName
        {
            get => Settings.ussGroupName;
            set
            {
                Settings.ussGroupName = value;
                Settings.SaveAsText();
                OnPropertyChanged();
                UpdateValidationInfo();
            }
        }

        private bool _canBuild;

        [CreateProperty]
        public bool CanRunBuild
        {
            get => _canBuild;
            private set
            {
                _canBuild = value;
                OnPropertyChanged();
            }
        }

        private string _validationInfo = string.Empty;

        [CreateProperty]
        public string ValidationInfo
        {
            get => _validationInfo;
            private set
            {
                _validationInfo = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ShowValidationInfo));
            }
        }

        [CreateProperty] public bool ShowValidationInfo => !string.IsNullOrWhiteSpace(ValidationInfo);

        public HaikaraAddressablesGroupMakerViewModel()
        {
            UpdateValidationInfo();
        }

        private void UpdateValidationInfo()
        {
            var sb = new StringBuilder();
            var canRunBuild = true;
            if (string.IsNullOrWhiteSpace(UxmlGroupName))
            {
                canRunBuild = false;
                sb.AppendLine($"{nameof(UxmlGroupName)} must not be null, empty, or whitespace");
            }

            if (string.IsNullOrEmpty(UssGroupName))
            {
                canRunBuild = false;
                sb.AppendLine($"{nameof(UssGroupName)} must not be null, empty, or whitespace");
            }

            CanRunBuild = canRunBuild;
            ValidationInfo = sb.ToString();
        }
    }
}
#endif