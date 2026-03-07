using UnityEngine.UIElements;

namespace CustomLogic
{
    /// <summary>
    /// A UI element that represents a clickable button.
    /// </summary>
    [CLType(Name = "Button", Abstract = true)]
    partial class CustomLogicButtonBuiltin : CustomLogicVisualElementBuiltin
    {
        private readonly Button _button;

        private UserMethod _clickEvent;

        public CustomLogicButtonBuiltin(Button button) : base(button)
        {
            _button = button;
            _button.clickable.clicked += OnClick;
        }

        private void OnClick()
        {
            if (_clickEvent == null)
                return;

            CustomLogicManager.Evaluator.EvaluateMethod(_clickEvent);
        }

        /// <summary>
        /// The text displayed by the Button.
        /// </summary>
        [CLProperty]
        public string Text
        {
            get => _button.text;
            set => _button.text = value;
        }

        /// <summary>
        /// When false, rich text tags will not be parsed.
        /// </summary>
        [CLProperty]
        public bool EnableRichText
        {
            get => _button.enableRichText;
            set => _button.enableRichText = value;
        }

        /// <summary>
        /// Sets the method to be called when the Button is clicked.
        /// </summary>
        /// <param name="clickEvent">The method to call when the button is clicked.</param>
        [CLMethod]
        public CustomLogicButtonBuiltin OnClick(UserMethod clickEvent)
        {
            _clickEvent = clickEvent;
            return this;
        }
    }
}
