using UnityEngine;
using UnityEngine.UI;

namespace GisketchUI
{
    public class SettingSlider : UIElement
    {
        public Slider slider;
        [SerializeField] private Text valueText;

        private bool isInteger = false;
        private int decimalPlaces = 2;

        public void Initialize(float minValue, float maxValue, float defaultValue, bool isInteger = false, int decimalPlaces = 2)
        {
            if (slider == null) slider = transform.Find("Slider").GetComponent<Slider>();
            if (valueText == null) valueText = transform.Find("Slider/Value").GetComponent<Text>();

            this.isInteger = isInteger;
            this.decimalPlaces = decimalPlaces;

            slider.minValue = minValue;
            slider.maxValue = maxValue;
            slider.value = defaultValue;

            slider.wholeNumbers = isInteger;

            UpdateValueText(defaultValue);

            slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float value)
        {
            UpdateValueText(value);
        }

        private void UpdateValueText(float value)
        {
            if (isInteger)
            {
                valueText.text = Mathf.RoundToInt(value).ToString();
            }
            else
            {
                valueText.text = value.ToString($"F{decimalPlaces}");
            }
        }

        public float GetValue()
        {
            return slider.value;
        }

        public int GetIntValue()
        {
            return Mathf.RoundToInt(slider.value);
        }

        public void SetValue(float value)
        {
            slider.value = value;
            UpdateValueText(value);
        }

        public void SetIntValue(int value)
        {
            slider.value = value;
            UpdateValueText(value);
        }
    }
}