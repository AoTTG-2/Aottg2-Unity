using UnityEngine.UIElements;

namespace CustomLogic
{
    [CLType(Name = "Toggle", Abstract = true, Description = "A UI element that represents a toggle button with on/off states.")]
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

        [CLProperty("The label text displayed next to the Toggle.")]
        public string Label
        {
            get => _toggle.label;
            set => _toggle.label = value;
        }

        [CLProperty("The text displayed by the Toggle.")]
        public string Text
        {
            get => _toggle.text;
            set => _toggle.text = value;
        }

        [CLProperty("The current value of the Toggle (true = checked, false = unchecked).")]
        public bool Value
        {
            get => _toggle.value;
            set => _toggle.value = value;
        }

        [CLMethod("Sets the method to be called when the Toggle value changes.")]
        public CustomLogicToggleBuiltin OnValueChanged(
            [CLParam("Method that will be called with the new boolean value as parameter")]
            UserMethod valueChangedEvent)
        {
            _valueChangedEvent = valueChangedEvent;
            return this;
        }

        [CLMethod("Sets the value of the Toggle without triggering any change events.")]
        public void SetValueWithoutNotify(
            [CLParam("The value to set.")]
            bool value)
        {
            _toggle.SetValueWithoutNotify(value);
        }
    }
}
