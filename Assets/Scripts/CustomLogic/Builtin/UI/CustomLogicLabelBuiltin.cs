using UnityEngine.UIElements;

namespace CustomLogic
{
    /// <summary>
    /// UI element for displaying text
    /// </summary>
    [CLType(Name = "Label", Abstract = true)]
    partial class CustomLogicLabelBuiltin : CustomLogicVisualElementBuiltin
    {
        private readonly Label _label;

        public CustomLogicLabelBuiltin(Label label) : base(label)
        {
            _label = label;
        }

        /// <summary>
        /// The text displayed by the Label
        /// </summary>
        [CLProperty]
        public string Text
        {
            get => _label.text;
            set => _label.text = value;
        }

        /// <summary>
        /// When false, rich text tags will not be parsed
        /// </summary>
        [CLProperty]
        public bool EnableRichText
        {
            get => _label.enableRichText;
            set => _label.enableRichText = value;
        }
    }
}
