using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using Settings;
using DentedPixel;

namespace GisketchUI
{
    public class SettingsPanel : SidePanel
    {
        [SerializeField] private RectTransform headerBG;
        [SerializeField] private Text headerLabel;
        private float firstButtonOffset = 48f;
        private float buttonSpacing = 0f;
        private float buttonWidth = 400f;
        private float buttonHeight = 72f;


        private Vector2 nextButtonPosition;
        private Vector2 nextFooterButtonPosition;

        private Image bgShadow;

        private SettingsListView _settingsListView;

        private GeneralSettingsView generalSettingsView;

        private SidePanelButton activeButton;

        public override void Initialize()
        {
            base.Initialize();

            nextButtonPosition = new Vector2(0, -headerBG.rect.height - firstButtonOffset);
            nextFooterButtonPosition = new Vector2(0, +firstButtonOffset);

            bgShadow = transform.Find("BgShadow").GetComponent<Image>();
            Color startColor = bgShadow.color;
            startColor.a = 0f;
            bgShadow.color = startColor;
            bgShadow.rectTransform.localScale = new Vector2(bgShadow.rectTransform.localScale.x * 2, bgShadow.rectTransform.localScale.y * 2);


            SetupButtons();
        }

        private void SetupSettingsListView()
        {
            if (_settingsListView == null)
            {
                _settingsListView = Instantiate(Resources.Load<SettingsListView>("GisketchUI/Prefabs/Settings/SettingsListView"), GisketchUIManager.Instance.MainCanvas.transform);
                _settingsListView.Initialize();
            }
            _settingsListView.Show();
            OnCategoryClicked("General");
        }

        private void SetupButtons()
        {
            SidePanelButton generalButton = AddButton("General", () => OnCategoryClicked("General"));
            AddButton("Sound", () => OnCategoryClicked("Sound"));
            AddButton("Graphics", () => OnCategoryClicked("Graphics"));
            AddButton("UI", () => OnCategoryClicked("UI"));
            AddButton("Keybinds", () => OnCategoryClicked("Keybinds"));
            AddButton("Skins", () => OnCategoryClicked("Skins"));
            AddButton("Ability", () => OnCategoryClicked("Ability"));

            AddFooterButton("Load", OnLoadClicked);
            AddFooterButton("Save", OnSaveClicked);
            AddFooterButton("Back", OnBackClicked);

            // Set General as the default active button
            SetActiveButton(generalButton);
        }

        private SidePanelButton AddButton(string label, Action onClick)
        {
            SidePanelButton button = ElementFactory.CreateSidePanelButton(contentContainer, label, onClick);
            RectTransform buttonRect = button.GetComponent<RectTransform>();

            buttonRect.anchorMin = new Vector2(0f, 1);
            buttonRect.anchorMax = new Vector2(0f, 1);
            buttonRect.pivot = new Vector2(0f, 1);
            buttonRect.sizeDelta = new Vector2(buttonWidth, buttonHeight);

            AddButton(button);
            return button;
        }

        private void SetActiveButton(SidePanelButton button)
        {
            if (activeButton != null)
            {
                activeButton.SetActive(false);
            }
            activeButton = button;
            activeButton.SetActive(true);
        }

        private void AddFooterButton(string label, Action onClick)
        {
            SidePanelButton footerButton = ElementFactory.CreateSidePanelButton(contentContainer, label, onClick);
            RectTransform footerButtonRect = footerButton.GetComponent<RectTransform>();

            footerButtonRect.anchorMin = new Vector2(0f, 0f);
            footerButtonRect.anchorMax = new Vector2(0f, 0f);
            footerButtonRect.pivot = new Vector2(0f, 0f);
            footerButtonRect.sizeDelta = new Vector2(buttonWidth, buttonHeight);

            AddFooterButton(footerButton);
        }

        protected override void AnimateEntrance(float duration)
        {
            base.AnimateEntrance(duration);

            float timeToStart = duration * 0.5f;

            // Call SetupSettingsListView
            LeanTween.delayedCall(timeToStart, SetupSettingsListView);

            // Animate header
            UIAnimator.SlideIn(headerBG, UIAnimator.SlideDirection.Left, 0.3f, timeToStart);

            headerLabel.text = "Settings";
            headerLabel.transform.localScale = Vector3.zero;
            LeanTween.scale(headerLabel.rectTransform, Vector3.one, 0.3f)
                .setDelay(timeToStart + 0.15f)
                .setEaseOutBack();

            // Animate buttons
            float buttonDelay = timeToStart + 0.1f;
            float buttonDuration = 0.2f;

            for (int i = 0; i < buttons.Count; i++)
            {
                float delay = buttonDelay + (i * 0.025f);
                Vector3 startPosition = nextButtonPosition;
                nextButtonPosition.y -= buttonHeight + buttonSpacing;
                buttons[i].AnimateEntrance(buttonDuration, delay, startPosition);
            }

            // Reverse list before animating and setting positions
            footerButtons.Reverse();

            for (int i = 0; i < footerButtons.Count; i++)
            {
                float delay = buttonDelay + (i * 0.025f);
                Vector3 startPosition = nextFooterButtonPosition;
                nextFooterButtonPosition.y += buttonHeight + buttonSpacing;
                footerButtons[i].AnimateEntrance(buttonDuration, delay, startPosition);
            }

            // Animate BG to darken
            LeanTween.alpha(bgShadow.rectTransform, 0.95f, 0.2f).setDelay(timeToStart * 0.5f).setEaseInOutCubic();


        }

        public override void Hide(float duration = 0.3f)
        {
            base.Hide(duration);
            // Animate BG to transparent
            LeanTween.alpha(bgShadow.rectTransform, 0f, 0.2f).setEaseInOutCubic();
            if (_settingsListView != null)
            {
                _settingsListView.Hide();
            }
        }

        private void CleanupSettingsListView()
        {
            if (_settingsListView != null)
            {
                Destroy(_settingsListView.gameObject);
                _settingsListView = null;
            }
        }

        private void ShowGeneralSettings()
        {
            if (generalSettingsView == null)
            {
                generalSettingsView = _settingsListView.gameObject.AddComponent<GeneralSettingsView>();
            }
            generalSettingsView.Initialize(_settingsListView.Content);
        }

        private void ClearSettingsView()
        {
            foreach (Transform child in _settingsListView.Content)
            {
                Destroy(child.gameObject);
            }
        }

        private void OnCategoryClicked(string category)
        {
            ClearSettingsView();
            SidePanelButton clickedButton = buttons.Find(b => b.GetComponentInChildren<Text>().text == category);
            if (clickedButton != null)
            {
                SetActiveButton(clickedButton);
            }

            switch (category)
            {
                case "General":
                    ShowGeneralSettings();
                    break;
                // Add cases for other categories when implemented
                default:
                    Debug.Log($"{category} settings not implemented yet.");
                    break;
            }
        }


        private void OnLoadClicked()
        {
            foreach (var setting in SettingsManager.GetSaveableSettings())
            {
                setting.Load();
            }
            ClearSettingsView();
            ShowGeneralSettings(); // Refresh the current view
            Debug.Log("Settings loaded from file.");
        }

        private void OnSaveClicked()
        {
            foreach (var setting in SettingsManager.GetSaveableSettings())
            {
                setting.Save();
            }
            Debug.Log("Settings saved to file.");
        }

        private void OnBackClicked()
        {
            foreach (var setting in SettingsManager.GetSaveableSettings())
            {
                setting.Apply();
            }
            Hide();
        }
    }
}