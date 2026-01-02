using UnityEngine.UIElements;

namespace CustomLogic
{
    [CLType(Name = "Slider", Abstract = true, Description = "A UI element that represents a horizontal slider for selecting numeric values (both int and float).")]
    partial class CustomLogicSliderBuiltin : CustomLogicVisualElementBuiltin
    {
        private readonly Slider _floatSlider;
        private readonly SliderInt _intSlider;
        private readonly bool _isIntSlider;

        private UserMethod _valueChangedEvent;

        public CustomLogicSliderBuiltin(Slider slider) : base(slider)
        {
            _floatSlider = slider;
            _isIntSlider = false;
            _floatSlider.RegisterValueChangedCallback(evt => OnValueChanged(evt.newValue));
        }

        public CustomLogicSliderBuiltin(SliderInt sliderInt) : base(sliderInt)
        {
            _intSlider = sliderInt;
            _isIntSlider = true;
            _intSlider.RegisterValueChangedCallback(evt => OnValueChanged(evt.newValue));
        }

        private void OnValueChanged(float value)
        {
            if (_valueChangedEvent == null)
                return;

            CustomLogicManager.Evaluator.EvaluateMethod(_valueChangedEvent, new object[] { value });
        }

        private void OnValueChanged(int value)
        {
            if (_valueChangedEvent == null)
                return;

            CustomLogicManager.Evaluator.EvaluateMethod(_valueChangedEvent, new object[] { value });
        }

        [CLProperty("The label text displayed next to the Slider.")]
        public string Label
        {
            get => _isIntSlider ? _intSlider.label : _floatSlider.label;
            set
            {
                if (_isIntSlider)
                    _intSlider.label = value;
                else
                    _floatSlider.label = value;
            }
        }

        [CLProperty("The current value of the Slider (returns int for integer sliders, float for float sliders).")]
        public object Value
        {
            get
            {
                if (_isIntSlider)
                    return _intSlider.value;
                return _floatSlider.value;
            }
            set
            {
                if (_isIntSlider)
                {
                    if (value is int intValue)
                        _intSlider.value = intValue;
                    else if (value is float floatValue)
                        _intSlider.value = (int)floatValue;
                }
                else
                {
                    if (value is float floatValue)
                        _floatSlider.value = floatValue;
                    else if (value is int intValue)
                        _floatSlider.value = intValue;
                }
            }
        }

        [CLProperty("The minimum value of the Slider (returns int for integer sliders, float for float sliders).")]
        public object LowValue
        {
            get
            {
                if (_isIntSlider)
                    return _intSlider.lowValue;
                return _floatSlider.lowValue;
            }
            set
            {
                if (_isIntSlider)
                {
                    if (value is int intValue)
                        _intSlider.lowValue = intValue;
                    else if (value is float floatValue)
                        _intSlider.lowValue = (int)floatValue;
                }
                else
                {
                    if (value is float floatValue)
                        _floatSlider.lowValue = floatValue;
                    else if (value is int intValue)
                        _floatSlider.lowValue = intValue;
                }
            }
        }

        [CLProperty("The maximum value of the Slider (returns int for integer sliders, float for float sliders).")]
        public object HighValue
        {
            get
            {
                if (_isIntSlider)
                    return _intSlider.highValue;
                return _floatSlider.highValue;
            }
            set
            {
                if (_isIntSlider)
                {
                    if (value is int intValue)
                        _intSlider.highValue = intValue;
                    else if (value is float floatValue)
                        _intSlider.highValue = (int)floatValue;
                }
                else
                {
                    if (value is float floatValue)
                        _floatSlider.highValue = floatValue;
                    else if (value is int intValue)
                        _floatSlider.highValue = intValue;
                }
            }
        }

        [CLProperty("The page size for the slider. This is the amount by which the slider value changes when clicking in the slider track area. For integer sliders, this also controls the snapping/tick interval.")]
        public float PageSize
        {
            get => _isIntSlider ? _intSlider.pageSize : _floatSlider.pageSize;
            set
            {
                if (_isIntSlider)
                    _intSlider.pageSize = value;
                else
                    _floatSlider.pageSize = value;
            }
        }

        [CLProperty("The direction of the slider (Horizontal or Vertical).")]
        public string Direction
        {
            get => _isIntSlider ? _intSlider.direction.ToString() : _floatSlider.direction.ToString();
            set
            {
                if (value == "Horizontal")
                {
                    if (_isIntSlider)
                        _intSlider.direction = SliderDirection.Horizontal;
                    else
                        _floatSlider.direction = SliderDirection.Horizontal;
                }
                else if (value == "Vertical")
                {
                    if (_isIntSlider)
                        _intSlider.direction = SliderDirection.Vertical;
                    else
                        _floatSlider.direction = SliderDirection.Vertical;
                }
            }
        }

        [CLProperty("If true, the slider will show a text field for direct input.")]
        public bool ShowInputField
        {
            get => _isIntSlider ? _intSlider.showInputField : _floatSlider.showInputField;
            set
            {
                if (_isIntSlider)
                    _intSlider.showInputField = value;
                else
                    _floatSlider.showInputField = value;
            }
        }

        [CLProperty("Returns true if this is an integer slider, false if it's a float slider.")]
        public bool IsIntSlider => _isIntSlider;

        [CLMethod("Sets the method to be called when the Slider value changes.")]
        public CustomLogicSliderBuiltin OnValueChanged(
            [CLParam("Method that will be called with the new value as parameter (int for integer sliders, float for float sliders)")]
            UserMethod valueChangedEvent)
        {
            _valueChangedEvent = valueChangedEvent;
            return this;
        }

        [CLMethod("Sets the value of the Slider without triggering any change events.")]
        public void SetValueWithoutNotify(
            [CLParam("The value to set (int for integer sliders, float for float sliders).")]
            object value)
        {
            if (_isIntSlider)
            {
                if (value is int intValue)
                    _intSlider.SetValueWithoutNotify(intValue);
                else if (value is float floatValue)
                    _intSlider.SetValueWithoutNotify((int)floatValue);
            }
            else
            {
                if (value is float floatValue)
                    _floatSlider.SetValueWithoutNotify(floatValue);
                else if (value is int intValue)
                    _floatSlider.SetValueWithoutNotify(intValue);
            }
        }
    }
}
