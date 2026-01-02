using UnityEngine.UIElements;

namespace CustomLogic
{
    [CLType(Name = "Label", Abstract = true, Description = "UI element for displaying text.")]
    partial class CustomLogicLabelBuiltin : CustomLogicVisualElementBuiltin
    {
        private readonly Label _label;

        public CustomLogicLabelBuiltin(Label label) : base(label)
        {
            _label = label;
        }

        [CLProperty("The text displayed by the Label.")]
        public string Text
        {
            get => _label.text;
            set => _label.text = value;
        }

        [CLProperty("When false, rich text tags will not be parsed.")]
        public bool EnableRichText
        {
            get => _label.enableRichText;
            set => _label.enableRichText = value;
        }
    }
}
