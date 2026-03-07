using UnityEngine.UIElements;

namespace CustomLogic
{
    /// <summary>
    /// A UI element that lets the user input and edit text.
    /// </summary>
    [CLType(Name = "TextField", Abstract = true)]
    partial class CustomLogicTextFieldBuiltin : CustomLogicVisualElementBuiltin
    {
        private readonly TextField _textField;

        public CustomLogicTextFieldBuiltin(TextField textField) : base(textField)
        {
            _textField = textField;
        }

        /// <summary>
        /// If true, the value property isn't updated until either the Enter key is pressed or the TextField loses focus.
        /// </summary>
        [CLProperty]
        public bool IsDelayed
        {
            get => _textField.isDelayed;
            set => _textField.isDelayed = value;
        }

        /// <summary>
        /// If true, the TextField supports multiple lines of text.
        /// </summary>
        [CLProperty]
        public bool Multiline
        {
            get => _textField.multiline;
            set => _textField.multiline = value;
        }

        /// <summary>
        /// The label text displayed next to the TextField.
        /// </summary>
        [CLProperty]
        public string Label
        {
            get => _textField.label;
            set => _textField.label = value;
        }

        /// <summary>
        /// The value of the TextField.
        /// </summary>
        [CLProperty]
        public string Value
        {
            get => _textField.value;
            set => _textField.value = value;
        }

        /// <summary>
        /// Color used to highlight selected text inside the field.
        /// </summary>
        [CLProperty]
        public CustomLogicColorBuiltin SelectionColor
        {
            get => new CustomLogicColorBuiltin(_textField.selectionColor);
            set => _textField.textSelection.selectionColor = value.Value.ToColor();
        }

        /// <summary>
        /// Color of the text cursor (caret).
        /// </summary>
        [CLProperty]
        public CustomLogicColorBuiltin CursorColor
        {
            get => new CustomLogicColorBuiltin(_textField.cursorColor);
            set => _textField.textSelection.cursorColor = value.Value.ToColor();
        }

        /// <summary>
        /// Registers a callback to be invoked when the value of the TextField changes.
        /// </summary>
        /// <param name="changeEvent">The method to call when the value changes. It will receive (newValue, previousValue) as parameters.</param>
        [CLMethod]
        public CustomLogicTextFieldBuiltin RegisterValueChangedEventCallback(UserMethod changeEvent)
        {
            _textField.RegisterValueChangedCallback(evt =>
            {
                CustomLogicManager.Evaluator.EvaluateMethod(changeEvent, new object[] { evt.newValue, evt.previousValue });
            });
            return this;
        }

        /// <summary>
        /// Sets the value of the TextField without triggering any change events.
        /// </summary>
        /// <param name="value">The value to set.</param>
        [CLMethod]
        public void SetValueWithoutNotify(string value)
        {
            _textField.SetValueWithoutNotify(value);
        }
    }
}
