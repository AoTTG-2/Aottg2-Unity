using UnityEngine.UIElements;

namespace CustomLogic
{
    [CLType(Name = "TextField", Abstract = true, Description = "A UI element that lets the user input and edit text.")]
    partial class CustomLogicTextFieldBuiltin : CustomLogicVisualElementBuiltin
    {
        private readonly TextField _textField;

        public CustomLogicTextFieldBuiltin(TextField textField) : base(textField)
        {
            _textField = textField;
        }

        [CLProperty("If true, the value property isn't updated until either the Enter key is pressed or the TextField loses focus.")]
        public bool IsDelayed
        {
            get => _textField.isDelayed;
            set => _textField.isDelayed = value;
        }

        [CLProperty("If true, the TextField supports multiple lines of text.")]
        public bool Multiline
        {
            get => _textField.multiline;
            set => _textField.multiline = value;
        }

        [CLProperty("The label text displayed next to the TextField.")]
        public string Label
        {
            get => _textField.label;
            set => _textField.label = value;
        }

        [CLProperty("The value of the TextField.")]
        public string Value
        {
            get => _textField.value;
            set => _textField.value = value;
        }

        [CLProperty("Color used to highlight selected text inside the field.")]
        public CustomLogicColorBuiltin SelectionColor
        {
            get => new CustomLogicColorBuiltin(_textField.selectionColor);
            set => _textField.textSelection.selectionColor = value.Value.ToColor();
        }

        [CLProperty("Color of the text cursor (caret).")]
        public CustomLogicColorBuiltin CursorColor
        {
            get => new CustomLogicColorBuiltin(_textField.cursorColor);
            set => _textField.textSelection.cursorColor = value.Value.ToColor();
        }

        [CLMethod("Registers a callback to be invoked when the value of the TextField changes.")]
        public CustomLogicTextFieldBuiltin RegisterValueChangedEventCallback(
            [CLParam("The method to call when the value changes. It will receive (newValue, previousValue) as parameters.")]
            UserMethod changeEvent)
        {
            _textField.RegisterValueChangedCallback(evt =>
            {
                CustomLogicManager.Evaluator.EvaluateMethod(changeEvent, new object[] { evt.newValue, evt.previousValue });
            });
            return this;
        }

        [CLMethod("Sets the value of the TextField without triggering any change events.")]
        public void SetValueWithoutNotify(
            [CLParam("The value to set.")]
            string value)
        {
            _textField.SetValueWithoutNotify(value);
        }
    }
}
