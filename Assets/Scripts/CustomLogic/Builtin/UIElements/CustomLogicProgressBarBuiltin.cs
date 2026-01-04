using UnityEngine.UIElements;

namespace CustomLogic
{
    /// <summary>
    /// A UI element that represents a progress bar for displaying progress from 0% to 100%.
    /// </summary>
    [CLType(Name = "ProgressBar", Abstract = true)]
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

        /// <summary>
        /// The title text displayed on the progress bar.
        /// </summary>
        [CLProperty]
        public string Title
        {
            get => _progressBar.title;
            set => _progressBar.title = value;
        }

        /// <summary>
        /// The current value of the progress bar (0-100).
        /// </summary>
        [CLProperty]
        public float Value
        {
            get => _progressBar.value;
            set => _progressBar.value = value;
        }

        /// <summary>
        /// The minimum value of the progress bar (default: 0).
        /// </summary>
        [CLProperty]
        public float LowValue
        {
            get => _progressBar.lowValue;
            set => _progressBar.lowValue = value;
        }

        /// <summary>
        /// The maximum value of the progress bar (default: 100).
        /// </summary>
        [CLProperty]
        public float HighValue
        {
            get => _progressBar.highValue;
            set => _progressBar.highValue = value;
        }

        /// <summary>
        /// Sets the method to be called when the progress bar value changes.
        /// </summary>
        /// <param name="valueChangedEvent">Method that will be called with the new value as parameter.</param>
        [CLMethod]
        public CustomLogicProgressBarBuiltin OnValueChanged(UserMethod valueChangedEvent)
        {
            _valueChangedEvent = valueChangedEvent;
            return this;
        }

        /// <summary>
        /// Sets the value of the progress bar without triggering any change events.
        /// </summary>
        /// <param name="value">The value to set.</param>
        [CLMethod]
        public void SetValueWithoutNotify(float value)
        {
            _progressBar.SetValueWithoutNotify(value);
        }

        /// <summary>
        /// Gets the current progress as a percentage (0-100).
        /// </summary>
        [CLMethod]
        public float GetPercentage()
        {
            if (_progressBar.highValue == _progressBar.lowValue)
                return 0f;
            return ((_progressBar.value - _progressBar.lowValue) / (_progressBar.highValue - _progressBar.lowValue)) * 100f;
        }

        /// <summary>
        /// Sets the progress by percentage (0-100).
        /// </summary>
        /// <param name="percentage">The percentage value (0-100).</param>
        [CLMethod]
        public CustomLogicProgressBarBuiltin SetPercentage(float percentage)
        {
            percentage = UnityEngine.Mathf.Clamp(percentage, 0f, 100f);
            _progressBar.value = _progressBar.lowValue + ((_progressBar.highValue - _progressBar.lowValue) * percentage / 100f);
            return this;
        }
    }
}
