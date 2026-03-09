using System;
using UnityEngine.UIElements;

namespace CustomLogic
{
    /// <summary>
    /// A UI element that represents a horizontal slider for selecting numeric values (both int and float).
    /// </summary>
    [CLType(Name = "Slider", Abstract = true)]
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

        /// <summary>
        /// The label text displayed next to the Slider.
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// The current value of the Slider (returns int for integer sliders, float for float sliders).
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// The minimum value of the Slider (returns int for integer sliders, float for float sliders).
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// The maximum value of the Slider (returns int for integer sliders, float for float sliders).
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// The page size for the slider. This is the amount by which the slider value changes when clicking in the slider track area. For integer sliders, this also controls the snapping/tick interval.
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// The direction of the slider.
        /// </summary>
        [CLProperty(Enum = new Type[] { typeof(CustomLogicSliderDirectionEnum) })]
        public int Direction
        {
            get => _isIntSlider ? (int)_intSlider.direction : (int)_floatSlider.direction;
            set
            {
                if (!System.Enum.IsDefined(typeof(SliderDirection), value))
                    throw new System.ArgumentException($"Invalid slider direction: {value}");

                if (_isIntSlider)
                    _intSlider.direction = (SliderDirection)value;
                else
                    _floatSlider.direction = (SliderDirection)value;
            }
        }

        /// <summary>
        /// If true, the slider will show a text field for direct input.
        /// </summary>
        [CLProperty]
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

        /// <summary>
        /// Returns true if this is an integer slider, false if it's a float slider.
        /// </summary>
        [CLProperty]
        public bool IsIntSlider => _isIntSlider;

        /// <summary>
        /// Sets the method to be called when the Slider value changes.
        /// </summary>
        /// <param name="valueChangedEvent">Method that will be called with the new value as parameter (int for integer sliders, float for float sliders).</param>
        [CLMethod]
        public CustomLogicSliderBuiltin OnValueChanged(UserMethod valueChangedEvent)
        {
            _valueChangedEvent = valueChangedEvent;
            return this;
        }

        /// <summary>
        /// Sets the value of the Slider without triggering any change events.
        /// </summary>
        /// <param name="value">The value to set (int for integer sliders, float for float sliders).</param>
        [CLMethod]
        public void SetValueWithoutNotify(object value)
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
