using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GisketchUI
{
    public class OptionsPopup : BasePopup
    {
        protected override string Title => "Select Option";
        protected override float Width => 480f;

        private List<string> options;
        private UnityAction<string> onOptionSelected;

        public void Initialize(List<string> optionsList, UnityAction<string> callback)
        {
            options = optionsList;
            onOptionSelected = callback;
        }

        protected override void SetupContent()
        {
            if (options == null || options.Count == 0)
            {
                CreateButton("No options available", null);
                return;
            }

            foreach (string option in options)
            {
                CreateButton(option, () => OnOptionSelected(option));
            }

            CreateButton(UI.UIManager.GetLocaleCommon("Cancel"), () => Hide(), ActionButton.ButtonVariant.Red);
        }

        private void CreateButton(string label, UnityAction onClick, ActionButton.ButtonVariant variant = ActionButton.ButtonVariant.Secondary)
        {
            ActionButton button = ElementFactory.CreateButton(contentView.rectTransform, label, onClick, variant);
            contentView.AddElement(button);
        }

        private void OnOptionSelected(string selectedOption)
        {
            onOptionSelected?.Invoke(selectedOption);
            Hide();
        }
    }
}