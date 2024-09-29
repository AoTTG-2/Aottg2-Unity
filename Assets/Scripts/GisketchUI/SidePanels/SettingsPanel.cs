using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

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

        public override void Initialize()
        {
            base.Initialize();
            // headerBG = transform.Find("HeaderBg").GetComponent<RectTransform>();
            // headerLabel = transform.Find("HeaderLabel").GetComponent<Text>();
            nextButtonPosition = new Vector2(0, -headerBG.rect.height - firstButtonOffset);
            nextFooterButtonPosition = new Vector2(0, +firstButtonOffset);
            SetupButtons();
        }

        private void SetupButtons()
        {
            AddButton("General", OnGeneralClicked);
            AddButton("Sound", OnSoundClicked);
            AddButton("UI", OnUIClicked);

            AddFooterButton("Load", OnBackClicked);
            AddFooterButton("Save", OnBackClicked);
            AddFooterButton("Back", OnBackClicked);
        }

        private void AddButton(string label, Action onClick)
        {
            SidePanelButton button = ElementFactory.CreateSidePanelButton(contentContainer, label, onClick);
            RectTransform buttonRect = button.GetComponent<RectTransform>();

            buttonRect.anchorMin = new Vector2(0f, 1);
            buttonRect.anchorMax = new Vector2(0f, 1);
            buttonRect.pivot = new Vector2(0f, 1);
            buttonRect.sizeDelta = new Vector2(buttonWidth, buttonHeight);

            AddButton(button);
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
            // SidePanelManager.Instance.HideCurrentPanel();
        }
    }
}