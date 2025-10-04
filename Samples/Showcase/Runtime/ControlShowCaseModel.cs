using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Haikara.Samples.Showcase.Runtime
{
    /// <summary>
    /// Model
    /// </summary>
    public class ControlShowCaseModel
    {
        public enum SampleEnum
        {
            Momoi,
            Midori,
            Yuzu,
            Alice,
            [Obsolete] sensei,
        }

        public string TabLabel => "Controls";
        public string Label => "Hello, Haikara";
        private bool _toggleValue = true;

        private ToggleButtonGroupState _toggleButtonGroupState = ToggleButtonGroupState.CreateFromOptions
        (
            new List<bool> { false, false, false, true, true }
        );

        private string _textFieldValue = "Hello";
        private float _sliderValue = 30;
        private int _sliderIntValue = 30;
        private Vector2 _minMaxSliderValue = new(-2, 3);

        private int _dropdownIndex = 1;

        public List<string> DropdownChoices => new()
        {
            "Hina",
            "Ako",
            "Iori",
            "Chinatsu"
        };

        private SampleEnum _sampleEnum = SampleEnum.Alice;

        private bool _radioButtonValue;

        public List<string> RadioButtonGroupChoices => new() { "Hoshino", "Nonomi", "Shiroko", "Ayane", "Serika" };
        private int _radioButtonGroupValue = 2;

        public bool GetToggleValue()
        {
            return _toggleValue;
        }

        public void SetToggleValue(bool newValue)
        {
            var previous = _toggleValue;
            _toggleValue = newValue;
            UnityEngine.Debug.Log($"{nameof(_toggleValue)} updated. {previous} => {newValue} ");
        }

        public ToggleButtonGroupState GetToggleButtonGroupState()
        {
            return _toggleButtonGroupState;
        }

        public void SetToggleButtonGroupState(ToggleButtonGroupState newValue)
        {
            var previous = _toggleButtonGroupState;
            _toggleButtonGroupState = newValue;
            UnityEngine.Debug.Log($"{nameof(_toggleButtonGroupState)} updated. {previous} => {newValue} ");
        }

        public string GetTextFieldValue()
        {
            return _textFieldValue;
        }

        public void SetTextFieldValue(string newValue)
        {
            var previous = _textFieldValue;
            _textFieldValue = newValue;
            UnityEngine.Debug.Log($"{nameof(_textFieldValue)} updated. {previous} => {newValue} ");
        }

        public float GetSliderValue()
        {
            return _sliderValue;
        }

        public void SetSliderValue(float newValue)
        {
            var previous = _sliderValue;
            _sliderValue = newValue;
            UnityEngine.Debug.Log($"{nameof(_sliderValue)} updated. {previous} => {newValue} ");
        }

        public int GetSliderIntValue()
        {
            return _sliderIntValue;
        }

        public void SetSliderIntValue(int newValue)
        {
            var previous = _sliderIntValue;
            _sliderIntValue = newValue;
            UnityEngine.Debug.Log($"{nameof(_sliderIntValue)} updated. {previous} => {newValue} ");
        }

        public Vector2 GetMinMaxSliderValue()
        {
            return _minMaxSliderValue;
        }

        public void SetMinMaxSliderValue(Vector2 newValue)
        {
            var previous = _minMaxSliderValue;
            _minMaxSliderValue = newValue;
            UnityEngine.Debug.Log($"{nameof(_minMaxSliderValue)} updated. {previous} => {newValue} ");
        }

        public async void RunProgress(Action<float> onProgress, Action onComplete)
        {
            UnityEngine.Debug.Log("ProgressStart");

            const int steps = 100;
            onProgress?.Invoke(0);

            // For 3 seconds, as a sample.
            for (int i = 1; i <= steps; i++)
            {
                onProgress?.Invoke(i);
                await Task.Delay((int)(0.03f * 1000));
            }

            onProgress?.Invoke(100);
            onComplete?.Invoke();

            UnityEngine.Debug.Log("ProgressComplete!");
        }

        public int GetDropdownIndex()
        {
            return _dropdownIndex;
        }

        public void SetDropdownIndex(int newValue)
        {
            var previous = _dropdownIndex;
            _dropdownIndex = newValue;
            UnityEngine.Debug.Log(
                $"{nameof(_dropdownIndex)} updated. " +
                $"{previous}({DropdownChoices[previous]}) => {newValue}({DropdownChoices[newValue]})"
            );
        }

        public SampleEnum GetSampleEnum()
        {
            return _sampleEnum;
        }

        public void SetSampleEnum(SampleEnum newValue)
        {
            var previous = _sampleEnum;
            _sampleEnum = newValue;
            UnityEngine.Debug.Log($"{nameof(_sampleEnum)} updated. {previous} => {newValue} ");
        }

        public bool GetRadioButtonValue()
        {
            return _radioButtonValue;
        }

        public void SetRadioButtonValue(bool newValue)
        {
            var previous = _radioButtonValue;
            _radioButtonValue = newValue;
            UnityEngine.Debug.Log($"{nameof(_radioButtonValue)} updated. {previous} => {newValue} ");
        }

        public int GetRadioButtonGroupValue()
        {
            return _radioButtonGroupValue;
        }

        public void SetRadioButtonGroupValue(int newValue)
        {
            var previous = _radioButtonGroupValue;
            _radioButtonGroupValue = newValue;
            UnityEngine.Debug.Log(
                $"{nameof(_radioButtonGroupValue)} updated. " +
                $"{previous}({RadioButtonGroupChoices[previous]}) => {newValue}({RadioButtonGroupChoices[newValue]}) "
            );
        }
    }
}