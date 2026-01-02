using UnityEngine.UIElements;

namespace CustomLogic
{
    [CLType(Name = "Button", Abstract = true, Description = "A UI element that represents a clickable button.")]
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

        [CLProperty("The text displayed by the Button.")]
        public string Text
        {
            get => _button.text;
            set => _button.text = value;
        }

        [CLProperty("When false, rich text tags will not be parsed.")]
        public bool EnableRichText
        {
            get => _button.enableRichText;
            set => _button.enableRichText = value;
        }

        [CLMethod("Sets the method to be called when the Button is clicked.")]
        public CustomLogicButtonBuiltin OnClick(
            [CLParam("The method to call when the button is clicked.")]
            UserMethod clickEvent)
        {
            _clickEvent = clickEvent;
            return this;
        }
    }
}
