using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GisketchUI
{
    public class SettingInputField : UIElement, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        public InputField inputField;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Text placeholderText;
        [SerializeField] private Text valueText;

        private bool isNumberOnly = false;
        private string defaultValue = "";

        public void Initialize(string placeholder, string defaultValue, bool numberOnly = false)
        {
            if (inputField == null) inputField = transform.Find("InputField").GetComponent<InputField>();
            if (backgroundImage == null) backgroundImage = transform.Find("InputField").GetComponent<Image>();
            if (placeholderText == null) placeholderText = transform.Find("InputField/Placeholder").GetComponent<Text>();
            if (valueText == null) valueText = transform.Find("InputField/Value").GetComponent<Text>();

            placeholderText.text = placeholder;
            this.defaultValue = defaultValue;
            isNumberOnly = numberOnly;

            if (isNumberOnly)
            {
                inputField.contentType = InputField.ContentType.IntegerNumber;
            }

            // Set initial alpha to 0.15
            Color bgColor = backgroundImage.color;
            bgColor.a = 0.15f;
            backgroundImage.color = bgColor;

            // Add value changed listener
            inputField.onValueChanged.AddListener(OnValueChanged);

            // Set the default value
            SetValue(defaultValue);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Color bgColor = ColorPalette.PrimaryLight;
            bgColor.a = 1f;
            backgroundImage.color = bgColor;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Color bgColor = backgroundImage.color;
            bgColor.a = 0.05f;
            backgroundImage.color = bgColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Color bgColor = ColorPalette.White;
            bgColor.a = 0.15f;
            backgroundImage.color = bgColor;
        }

        private void OnValueChanged(string value)
        {
            if (isNumberOnly)
            {
                int.TryParse(value, out int result);
                valueText.text = result.ToString();
            }
            else
            {
                valueText.text = value;
            }
        }

        public string GetValue()
        {
            return valueText.text;
        }

        public int GetIntValue()
        {
            return int.Parse(valueText.text);
        }

        public void SetValue(string value)
        {
            inputField.text = value;
            OnValueChanged(value);
        }

        public void ResetToDefault()
        {
            SetValue(defaultValue);
        }

        public bool IsDefaultValue()
        {
            return GetValue() == defaultValue;
        }
    }
}