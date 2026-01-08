using UnityEngine.UIElements;

namespace CustomLogic
{
    /// <summary>
    /// A UI element that represents a toggle button with on/off states.
    /// </summary>
    [CLType(Name = "Toggle", Abstract = true)]
    partial class CustomLogicToggleBuiltin : CustomLogicVisualElementBuiltin
    {
        private readonly Toggle _toggle;

        private UserMethod _valueChangedEvent;

        public CustomLogicToggleBuiltin(Toggle toggle) : base(toggle)
        {
            _toggle = toggle;
            _toggle.RegisterValueChangedCallback(evt => OnValueChanged(evt.newValue));
        }

        private void OnValueChanged(bool value)
        {
            if (_valueChangedEvent == null)
                return;

            CustomLogicManager.Evaluator.EvaluateMethod(_valueChangedEvent, new object[] { value });
        }

        /// <summary>
        /// The label text displayed next to the Toggle.
        /// </summary>
        [CLProperty]
        public string Label
        {
            get => _toggle.label;
            set => _toggle.label = value;
        }

        /// <summary>
        /// The text displayed by the Toggle.
        /// </summary>
        [CLProperty]
        public string Text
        {
            get => _toggle.text;
            set => _toggle.text = value;
        }

        /// <summary>
        /// The current value of the Toggle (true = checked, false = unchecked).
        /// </summary>
        [CLProperty]
        public bool Value
        {
            get => _toggle.value;
            set => _toggle.value = value;
        }

        /// <summary>
        /// Sets the method to be called when the Toggle value changes.
        /// </summary>
        /// <param name="valueChangedEvent">Method that will be called with the new boolean value as parameter.</param>
        [CLMethod]
        public CustomLogicToggleBuiltin OnValueChanged(UserMethod valueChangedEvent)
        {
            _valueChangedEvent = valueChangedEvent;
            return this;
        }

        /// <summary>
        /// Sets the value of the Toggle without triggering any change events.
        /// </summary>
        /// <param name="value">The value to set.</param>
        [CLMethod]
        public void SetValueWithoutNotify(bool value)
        {
            _toggle.SetValueWithoutNotify(value);
        }
    }
}
