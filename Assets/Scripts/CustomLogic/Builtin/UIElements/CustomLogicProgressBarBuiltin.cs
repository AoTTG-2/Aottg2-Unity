using UnityEngine.UIElements;

namespace CustomLogic
{
    [CLType(Name = "ProgressBar", Abstract = true, Description = "A UI element that represents a progress bar for displaying progress from 0% to 100%.")]
    partial class CustomLogicProgressBarBuiltin : CustomLogicVisualElementBuiltin
    {
        private readonly ProgressBar _progressBar;

        private UserMethod _valueChangedEvent;

        public CustomLogicProgressBarBuiltin(ProgressBar progressBar) : base(progressBar)
        {
            _progressBar = progressBar;
            _progressBar.RegisterValueChangedCallback(evt => OnValueChanged(evt.newValue));
        }

        private void OnValueChanged(float value)
        {
            if (_valueChangedEvent == null)
                return;

            CustomLogicManager.Evaluator.EvaluateMethod(_valueChangedEvent, new object[] { value });
        }

        [CLProperty("The title text displayed on the progress bar.")]
        public string Title
        {
            get => _progressBar.title;
            set => _progressBar.title = value;
        }

        [CLProperty("The current value of the progress bar (0-100).")]
        public float Value
        {
            get => _progressBar.value;
            set => _progressBar.value = value;
        }

        [CLProperty("The minimum value of the progress bar (default: 0).")]
        public float LowValue
        {
            get => _progressBar.lowValue;
            set => _progressBar.lowValue = value;
        }

        [CLProperty("The maximum value of the progress bar (default: 100).")]
        public float HighValue
        {
            get => _progressBar.highValue;
            set => _progressBar.highValue = value;
        }

        [CLMethod("Sets the method to be called when the progress bar value changes.")]
        public CustomLogicProgressBarBuiltin OnValueChanged(
            [CLParam("Method that will be called with the new value as parameter")]
            UserMethod valueChangedEvent)
        {
            _valueChangedEvent = valueChangedEvent;
            return this;
        }

        [CLMethod("Sets the value of the progress bar without triggering any change events.")]
        public void SetValueWithoutNotify(
            [CLParam("The value to set.")]
            float value)
        {
            _progressBar.SetValueWithoutNotify(value);
        }

        [CLMethod("Gets the current progress as a percentage (0-100).")]
        public float GetPercentage()
        {
            if (_progressBar.highValue == _progressBar.lowValue)
                return 0f;
            return ((_progressBar.value - _progressBar.lowValue) / (_progressBar.highValue - _progressBar.lowValue)) * 100f;
        }

        [CLMethod("Sets the progress by percentage (0-100).")]
        public CustomLogicProgressBarBuiltin SetPercentage(
            [CLParam("The percentage value (0-100).")]
            float percentage)
        {
            percentage = UnityEngine.Mathf.Clamp(percentage, 0f, 100f);
            _progressBar.value = _progressBar.lowValue + ((_progressBar.highValue - _progressBar.lowValue) * percentage / 100f);
            return this;
        }
    }
}
