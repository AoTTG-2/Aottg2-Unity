using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace CustomLogic
{
    /// <summary>
    /// A UI element that represents a dropdown selection field.
    /// </summary>
    [CLType(Name = "Dropdown", Abstract = true)]
    partial class CustomLogicDropdownBuiltin : CustomLogicVisualElementBuiltin
    {
        private readonly DropdownField _dropdown;

        private UserMethod _valueChangedEvent;

        public CustomLogicDropdownBuiltin(DropdownField dropdown) : base(dropdown)
        {
            _dropdown = dropdown;
            _dropdown.RegisterValueChangedCallback(evt => OnValueChanged(evt.newValue));
        }

        private void OnValueChanged(string value)
        {
            if (_valueChangedEvent == null)
                return;

            CustomLogicManager.Evaluator.EvaluateMethod(_valueChangedEvent, new object[] { value });
        }

        /// <summary>
        /// The label text displayed next to the Dropdown.
        /// </summary>
        [CLProperty]
        public string Label
        {
            get => _dropdown.label;
            set => _dropdown.label = value;
        }

        /// <summary>
        /// The currently selected value (option text).
        /// </summary>
        [CLProperty]
        public string Value
        {
            get => _dropdown.value;
            set => _dropdown.value = value;
        }

        /// <summary>
        /// The index of the currently selected option (0-based).
        /// </summary>
        [CLProperty]
        public int Index
        {
            get => _dropdown.index;
            set => _dropdown.index = value;
        }

        /// <summary>
        /// Gets the list of available choices.
        /// </summary>
        [CLProperty(TypeArguments = new[] { "string" })]
        public CustomLogicListBuiltin Choices
        {
            get
            {
                var list = new CustomLogicListBuiltin();
                foreach (var choice in _dropdown.choices)
                    list.List.Add(choice);
                return list;
            }
            set
            {
                var choices = new List<string>();
                foreach (var item in value.List)
                {
                    if (item != null)
                        choices.Add(item.ToString());
                }
                _dropdown.choices = choices;
            }
        }

        /// <summary>
        /// Sets the method to be called when the Dropdown value changes.
        /// </summary>
        /// <param name="valueChangedEvent">Method that will be called with the new selected value as parameter.</param>
        [CLMethod]
        public CustomLogicDropdownBuiltin OnValueChanged(UserMethod valueChangedEvent)
        {
            _valueChangedEvent = valueChangedEvent;
            return this;
        }

        /// <summary>
        /// Sets the value of the Dropdown without triggering any change events.
        /// </summary>
        /// <param name="value">The value to set.</param>
        [CLMethod]
        public void SetValueWithoutNotify(string value)
        {
            _dropdown.SetValueWithoutNotify(value);
        }

        /// <summary>
        /// Sets the index of the selected option without triggering any change events.
        /// </summary>
        /// <param name="index">The index of the option to select (0-based).</param>
        [CLMethod]
        public void SetIndexWithoutNotify(int index)
        {
            if (index >= 0 && index < _dropdown.choices.Count)
                _dropdown.SetValueWithoutNotify(_dropdown.choices[index]);
        }

        /// <summary>
        /// Adds a choice to the dropdown options.
        /// </summary>
        /// <param name="choice">The choice text to add.</param>
        [CLMethod]
        public CustomLogicDropdownBuiltin AddChoice(string choice)
        {
            var choices = new List<string>(_dropdown.choices);
            choices.Add(choice);
            _dropdown.choices = choices;
            return this;
        }

        /// <summary>
        /// Removes a choice from the dropdown options.
        /// </summary>
        /// <param name="choice">The choice text to remove.</param>
        [CLMethod]
        public CustomLogicDropdownBuiltin RemoveChoice(string choice)
        {
            var choices = new List<string>(_dropdown.choices);
            choices.Remove(choice);
            _dropdown.choices = choices;
            return this;
        }

        /// <summary>
        /// Clears all choices from the dropdown.
        /// </summary>
        [CLMethod]
        public CustomLogicDropdownBuiltin ClearChoices()
        {
            _dropdown.choices = new List<string>();
            return this;
        }
    }
}
