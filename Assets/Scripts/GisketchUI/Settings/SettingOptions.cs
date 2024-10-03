using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace GisketchUI
{
    public class SettingOptions : UIElement
    {
        [SerializeField] public Button leftArrow;
        [SerializeField] public Button rightArrow;
        [SerializeField] public Button itemButton;
        [SerializeField] private Text optionLabel;

        [SerializeField] private List<string> options = new List<string> { "Option 1", "Option 2", "Option 3" };
        private int currentIndex = 0;

        protected virtual void Awake()
        {
            if (leftArrow == null) leftArrow = transform.Find("LeftArrow").GetComponent<Button>();
            if (rightArrow == null) rightArrow = transform.Find("RightArrow").GetComponent<Button>();
            if (itemButton == null) itemButton = transform.Find("ItemButton").GetComponent<Button>();
            if (optionLabel == null) optionLabel = transform.Find("ItemButton/Label").GetComponent<Text>();

            leftArrow.onClick.AddListener(OnLeftArrowClick);
            rightArrow.onClick.AddListener(OnRightArrowClick);
            itemButton.onClick.AddListener(OnItemButtonClick);

            UpdateOptionText();
        }

        private void OnLeftArrowClick()
        {
            currentIndex = (currentIndex - 1 + options.Count) % options.Count;
            UpdateOptionText();
        }

        private void OnRightArrowClick()
        {
            currentIndex = (currentIndex + 1) % options.Count;
            UpdateOptionText();
        }

        private void OnItemButtonClick()
        {
            OptionsPopup popup = PopupManager.Instance.GetOrCreatePopup<OptionsPopup>();
            popup.Initialize(options, OnOptionSelected);
            PopupManager.Instance.ShowPopup<OptionsPopup>();
        }

        private void OnOptionSelected(string selectedOption)
        {
            SetOption(selectedOption);
        }

        private void UpdateOptionText()
        {
            if (options.Count > 0)
            {
                optionLabel.text = options[currentIndex];
            }
            else
            {
                optionLabel.text = "No options";
            }
        }

        public override void Show(float duration = 0.3f)
        {
            base.Show(duration);
        }

        public override void Hide(float duration = 0.3f)
        {
            base.Hide(duration);
        }

        public string GetCurrentOption()
        {
            return options.Count > 0 ? options[currentIndex] : null;
        }

        public void SetOption(string option)
        {
            int index = options.IndexOf(option);
            if (index != -1)
            {
                currentIndex = index;
                UpdateOptionText();
            }
        }

        public void SetOptions(List<string> newOptions)
        {
            options = newOptions;
            currentIndex = 0;
            UpdateOptionText();
        }
    }
}