using System.Collections.Generic;
using System.Text;
using Haikara.Runtime.ViewModel;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

namespace Haikara.Samples.Showcase.Runtime
{
    public class ControlsShowcaseViewModel : ViewModelBase
    {
        private readonly ControlShowCaseModel _model = new();
        [CreateProperty] public string TabLabel => _model.TabLabel;

        [CreateProperty] public string Label => _model.Label;

        [CreateProperty]
        public bool ToggleValue
        {
            get => _model.GetToggleValue();
            set => _model.SetToggleValue(value);
        }

        [CreateProperty]
        public ToggleButtonGroupState ToggleButtonGroupState
        {
            get => _model.GetToggleButtonGroupState();
            set => _model.SetToggleButtonGroupState(value);
        }

        [CreateProperty]
        public string TextFieldValue
        {
            get => _model.GetTextFieldValue();
            set => _model.SetTextFieldValue(value);
        }

        [CreateProperty]
        public float SliderValue
        {
            get => _model.GetSliderValue();
            set => _model.SetSliderValue(value);
        }

        [CreateProperty]
        public int SliderIntValue
        {
            get => _model.GetSliderIntValue();
            set => _model.SetSliderIntValue(value);
        }

        [CreateProperty]
        public Vector2 MinMaxSliderValue
        {
            get => _model.GetMinMaxSliderValue();
            set
            {
                _model.SetMinMaxSliderValue(value);
                // Notify of a change to the property used for displaying the MinMaxSlider's value in a Label.
                OnPropertyChanged(nameof(MinMaxSliderInfo));
            }
        }

        [CreateProperty] public string MinMaxSliderInfo => MinMaxSliderValue.ToString();

        private bool _progressBarStartButtonIsEnabled = true;

        [CreateProperty]
        public bool ProgressStartButtonIsEnabled
        {
            get => _progressBarStartButtonIsEnabled;
            private set
            {
                _progressBarStartButtonIsEnabled = value;
                OnPropertyChanged();
            }
        }

        private string _progressBarTitle = "Progress Bar";

        [CreateProperty]
        public string ProgressBarTitle
        {
            get => _progressBarTitle;
            private set
            {
                _progressBarTitle = value;
                OnPropertyChanged();
            }
        }

        private float _progressBarValue;

        [CreateProperty]
        public float ProgressBarValue
        {
            get => _progressBarValue;
            private set
            {
                _progressBarValue = value;
                OnPropertyChanged();
            }
        }

        [CreateProperty]
        public int DropdownIndex
        {
            get => _model.GetDropdownIndex();
            set => _model.SetDropdownIndex(value);
        }

        [CreateProperty] public List<string> DropdownChoices => _model.DropdownChoices;

        [CreateProperty]
        public ControlShowCaseModel.SampleEnum EnumFieldValue
        {
            get => _model.GetSampleEnum();
            set => _model.SetSampleEnum(value);
        }

        [CreateProperty]
        public bool RadioButtonValue
        {
            get => _model.GetRadioButtonValue();
            set => _model.SetRadioButtonValue(value);
        }

        [CreateProperty] public List<string> RadioButtonGroupChoices => _model.RadioButtonGroupChoices;

        [CreateProperty]
        public int RadioButtonGroupValue
        {
            get => _model.GetRadioButtonGroupValue();
            set => _model.SetRadioButtonGroupValue(value);
        }

        public void OutputLog()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"///{nameof(ControlsShowcaseViewModel)}///");

            sb.AppendLine($"{nameof(ToggleValue)}:{ToggleValue}");
            sb.AppendLine($"{nameof(ToggleButtonGroupState)}:{ToggleButtonGroupState}");
            sb.AppendLine($"{nameof(TextFieldValue)}:{TextFieldValue}");
            sb.AppendLine($"{nameof(SliderValue)}:{SliderValue}");
            sb.AppendLine($"{nameof(SliderIntValue)}:{SliderIntValue}");
            sb.AppendLine($"{nameof(MinMaxSliderValue)}:{MinMaxSliderValue}");
            sb.AppendLine($"{nameof(DropdownIndex)}:{DropdownIndex}({DropdownChoices[DropdownIndex]})");
            sb.AppendLine($"{nameof(EnumFieldValue)}:{EnumFieldValue}");
            sb.AppendLine($"{nameof(RadioButtonValue)}:{RadioButtonValue}");
            sb.AppendLine(
                $"{nameof(RadioButtonGroupValue)}:{RadioButtonGroupValue}({RadioButtonGroupChoices[RadioButtonGroupValue]})"
            );

            UnityEngine.Debug.Log(sb.ToString());
        }


        public void StartProgress()
        {
            ProgressStartButtonIsEnabled = false;
            _model.RunProgress(
                onProgress: progress =>
                {
                    ProgressBarTitle = $"Progress... {progress}%";
                    ProgressBarValue = progress;
                },
                onComplete: () =>
                {
                    ProgressBarTitle = $"Complete!";
                    ProgressStartButtonIsEnabled = true;
                }
            );
        }
    }
}