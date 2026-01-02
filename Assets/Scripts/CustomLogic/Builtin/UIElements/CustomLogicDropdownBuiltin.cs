using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace CustomLogic
{
    [CLType(Name = "Dropdown", Abstract = true, Description = "A UI element that represents a dropdown selection field.")]
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

        [CLProperty("The label text displayed next to the Dropdown.")]
        public string Label
        {
            get => _dropdown.label;
            set => _dropdown.label = value;
        }

        [CLProperty("The currently selected value (option text).")]
        public string Value
        {
            get => _dropdown.value;
            set => _dropdown.value = value;
        }

        [CLProperty("The index of the currently selected option (0-based).")]
        public int Index
        {
            get => _dropdown.index;
            set => _dropdown.index = value;
        }

        [CLProperty(TypeArguments = new[] { "string" }, Description = "Gets the list of available choices.")]
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

        [CLMethod("Sets the method to be called when the Dropdown value changes.")]
        public CustomLogicDropdownBuiltin OnValueChanged(
            [CLParam("Method that will be called with the new selected value as parameter")]
            UserMethod valueChangedEvent)
        {
            _valueChangedEvent = valueChangedEvent;
            return this;
        }

        [CLMethod("Sets the value of the Dropdown without triggering any change events.")]
        public void SetValueWithoutNotify(
            [CLParam("The value to set.")]
            string value)
        {
            _dropdown.SetValueWithoutNotify(value);
        }

        [CLMethod("Sets the index of the selected option without triggering any change events.")]
        public void SetIndexWithoutNotify(
            [CLParam("The index of the option to select (0-based).")]
            int index)
        {
            if (index >= 0 && index < _dropdown.choices.Count)
                _dropdown.SetValueWithoutNotify(_dropdown.choices[index]);
        }

        [CLMethod("Adds a choice to the dropdown options.")]
        public CustomLogicDropdownBuiltin AddChoice(
            [CLParam("The choice text to add.")]
            string choice)
        {
            var choices = new List<string>(_dropdown.choices);
            choices.Add(choice);
            _dropdown.choices = choices;
            return this;
        }

        [CLMethod("Removes a choice from the dropdown options.")]
        public CustomLogicDropdownBuiltin RemoveChoice(
            [CLParam("The choice text to remove.")]
            string choice)
        {
            var choices = new List<string>(_dropdown.choices);
            choices.Remove(choice);
            _dropdown.choices = choices;
            return this;
        }

        [CLMethod("Clears all choices from the dropdown.")]
        public CustomLogicDropdownBuiltin ClearChoices()
        {
            _dropdown.choices = new List<string>();
            return this;
        }
    }
}
