using UnityEngine;
using UnityEngine.UI;
using System;

namespace GisketchUI
{
    public class SettingsPanel : SidePanel
    {
        [SerializeField] private RectTransform headerBG;
        [SerializeField] private Text headerLabel;
        [SerializeField] private float firstButtonOffset = 48f;
        [SerializeField] private float buttonSpacing = 8f;
        [SerializeField] private float buttonWidth = 400f;
        [SerializeField] private float buttonHeight = 72f;

        private Vector2 nextButtonPosition;

        public override void Initialize()
        {
            base.Initialize();
            nextButtonPosition = new Vector2(0, -headerBG.rect.height - firstButtonOffset);
            SetupButtons();
        }

        private void SetupButtons()
        {
            AddButton("General", OnGeneralClicked);
            AddButton("Sound", OnSoundClicked);
            AddButton("UI", OnUIClicked);
            AddButton("Back", OnBackClicked);
        }

        private void AddButton(string label, Action onClick)
        {
            SidePanelButton button = ElementFactory.CreateSidePanelButton(contentContainer, label, onClick);
            RectTransform buttonRect = button.GetComponent<RectTransform>();

            buttonRect.anchorMin = new Vector2(0.5f, 1);
            buttonRect.anchorMax = new Vector2(0.5f, 1);
            buttonRect.sizeDelta = new Vector2(buttonWidth, buttonHeight);
            buttonRect.anchoredPosition = nextButtonPosition;

            nextButtonPosition.y -= (buttonHeight + buttonSpacing);

            AddButton(button);
        }

        protected override void AnimateEntrance(float duration)
        {
            base.AnimateEntrance(duration);

            // Animate header
            UIAnimator.SlideIn(headerBG, UIAnimator.SlideDirection.Left, duration * 0.5f, 0f);

            headerLabel.text = "Settings";
            headerLabel.transform.localScale = Vector3.zero;
            LeanTween.scale(headerLabel.rectTransform, Vector3.one, duration * 0.5f)
                .setEaseOutBack();

            // Animate buttons
            float buttonDelay = duration * 0.25f;
            float buttonDuration = duration * 0.6f;

            for (int i = 0; i < buttons.Count; i++)
            {
                float delay = buttonDelay + (i * 0.05f); // Small stagger between buttons
                buttons[i].AnimateEntrance(buttonDuration, delay);
            }
        }

        private void OnGeneralClicked()
        {
            Debug.Log("General settings clicked");
        }

        private void OnSoundClicked()
        {
            Debug.Log("Sound settings clicked");
        }

        private void OnUIClicked()
        {
            Debug.Log("UI settings clicked");
        }

        private void OnBackClicked()
        {
            SidePanelManager.Instance.HideCurrentPanel();
        }
    }
}