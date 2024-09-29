using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UI;
using Settings;

namespace GisketchUI
{
    public class IntroPanel : SidePanel
    {
        [SerializeField] private RectTransform logo;
        [SerializeField] private RectTransform backgroundPanel;
        private float firstButtonOffset = 360f;
        private float buttonSpacing = 0f;
        [SerializeField] private float buttonWidth = 400f;
        [SerializeField] private float buttonHeight = 72f;

        private Vector2 nextButtonPosition;
        private Vector3 logoOriginalPosition;
        private Vector2 logoOriginalSize;
        private Vector2 backgroundPanelOriginalPosition;

        [SerializeField] private GameObject iconsContainer;

        private List<UnityEngine.UI.Button> footerButtons = new List<UnityEngine.UI.Button>();
        private Dictionary<UnityEngine.UI.Button, Vector3> footerButtonOriginalPositions = new Dictionary<UnityEngine.UI.Button, Vector3>();
        private bool isAnimating = false;
        private List<LTDescr> activeAnimations = new List<LTDescr>();

        private MainMenu _mainMenu;

        public override void Initialize()
        {
            base.Initialize();
            _mainMenu = FindFirstObjectByType<MainMenu>();

            nextButtonPosition = new Vector2(0, -firstButtonOffset);
            SetupLogo();
            SetupBackground();
            SetupButtons();
            SetupFooterButtons();

            // Add click listener to the panel
            UnityEngine.UI.Button panelButton = gameObject.AddComponent<UnityEngine.UI.Button>();
            panelButton.onClick.AddListener(SkipAnimationIfPlaying);
        }

        private void SetupLogo()
        {
            logoOriginalPosition = logo.anchoredPosition;
            logoOriginalSize = logo.sizeDelta;
        }

        private void SetupBackground()
        {
            backgroundPanel = transform.Find("Background").GetComponent<RectTransform>();
            backgroundPanelOriginalPosition = backgroundPanel.anchoredPosition;
        }

        private bool isBackgroundAnimating = false;
        private bool isLogoAnimating = false;
        private bool isEntranceAnimationComplete = false;

        private void Update()
        {
            if (!isEntranceAnimationComplete)
                return;

            if (SettingsManager.UISettings.FadeMainMenu.Value)
            {
                if (HoverIntroPanel())
                {
                    if (!isBackgroundAnimating && !isLogoAnimating)
                    {
                        AnimateToOriginalState();
                    }
                }
                else
                {
                    if (!isBackgroundAnimating && !isLogoAnimating)
                    {
                        AnimateToFadedState();
                    }
                }
            }
            else
            {
                // Reset background and logo to original positions immediately
                backgroundPanel.anchoredPosition = backgroundPanelOriginalPosition;
                logo.anchoredPosition = logoOriginalPosition;
                logo.sizeDelta = logoOriginalSize;
            }
        }

        private void AnimateToOriginalState()
        {
            isBackgroundAnimating = true;
            isLogoAnimating = true;

            // Reset background position
            LeanTween.moveX(backgroundPanel, backgroundPanelOriginalPosition.x, 0.15f)
                .setEaseOutCubic()
                .setOnComplete(() => isBackgroundAnimating = false);

            // Reset logo position and size
            LeanTween.move(logo, logoOriginalPosition, 0.15f).setEaseOutCubic();
            LeanTween.size(logo, logoOriginalSize, 0.15f)
                .setEaseOutCubic()
                .setOnComplete(() => isLogoAnimating = false);
        }

        private void AnimateToFadedState()
        {
            isBackgroundAnimating = true;
            isLogoAnimating = true;

            // Move background out of view
            float targetX = -backgroundPanel.rect.width / 2;
            LeanTween.moveX(backgroundPanel, targetX, 0.15f)
                .setEaseOutCubic()
                .setOnComplete(() => isBackgroundAnimating = false);

            // Adjust logo size and position
            Vector3 targetLogoPosition = new Vector3(-96f, 60f, 0f);
            Vector2 targetLogoSize = logoOriginalSize * 0.7f;
            LeanTween.move(logo, targetLogoPosition, 0.15f).setEaseOutCubic();
            LeanTween.size(logo, targetLogoSize, 0.15f)
                .setEaseOutCubic()
                .setOnComplete(() => isLogoAnimating = false);
        }

        private bool HoverIntroPanel()
        {
            float x = backgroundPanel.rect.width * 0.25f;
            return Input.mousePosition.x < x;
        }

        private void SetupButtons()
        {
            string[] buttonKeys = new string[]
            {
                "Tutorial",
                "Singleplayer",
                "Multiplayer",
                "Profile",
                "Settings",
                "Tools",
                "Credits",
                "Quit"
            };

            foreach (string key in buttonKeys)
            {
                string localizedText = (key == "Profile" || key == "Settings" || key == "Quit")
                    ? UIManager.GetLocaleCommon(key)
                    : UIManager.GetLocale("MainMenu", "Intro", $"{key}Button");

                AddButton(localizedText.ToUpper(), () => OnButtonClicked($"{key}Button"));
            }
        }

        private void OnButtonClicked(string buttonName)
        {
            _mainMenu.HandleIntroButtonClick(buttonName);
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

        public override void Show(float duration)
        {
            base.Show(1f);
        }

        protected override void AnimateEntrance(float duration)
        {
            isAnimating = true;
            isEntranceAnimationComplete = false;
            activeAnimations.Clear();

            base.AnimateEntrance(duration);

            float timeToStart = duration * 0.7f;

            // Animate logo
            float logoDelay = timeToStart;
            activeAnimations.Add(LeanTween.moveX(logo, logoOriginalPosition.x, 0.6f).setDelay(logoDelay).setEaseInOutCubic());

            // Animate buttons
            float buttonDelay = timeToStart + 0.25f;
            float buttonDuration = 0.15f;
            for (int i = 0; i < buttons.Count; i++)
            {
                float delay = buttonDelay + (i * 0.025f);
                Vector3 startPosition = nextButtonPosition;
                nextButtonPosition.y -= buttonHeight + buttonSpacing;
                buttons[i].AnimateEntrance(buttonDuration, delay, startPosition);
            }

            // Animate footer buttons
            AnimateFooterButtons(buttonDelay + buttons.Count * 0.025f);

            // Set a callback to mark animation as complete
            LeanTween.delayedCall(timeToStart + 1f, () =>
            {
                isAnimating = false;
                isEntranceAnimationComplete = true;
            });
        }


        private void SetupFooterButtons()
        {
            if (iconsContainer == null)
            {
                Debug.LogError("Icons container is not assigned!");
                return;
            }

            string[] buttonNames = { "QuestButton", "LeaderboardButton", "SocialButton", "HelpButton", "PatreonButton" };

            foreach (string buttonName in buttonNames)
            {
                UnityEngine.UI.Button button = iconsContainer.transform.Find(buttonName)?.GetComponent<UnityEngine.UI.Button>();
                if (button != null)
                {
                    footerButtons.Add(button);
                    footerButtonOriginalPositions[button] = button.transform.localPosition;
                    SetupFooterButtonListeners(button, buttonName);
                }
                else
                {
                    Debug.LogWarning($"Button {buttonName} not found in Icons container!");
                }
            }
        }

        private void SetupFooterButtonListeners(UnityEngine.UI.Button button, string buttonName)
        {
            button.onClick.AddListener(() => OnFooterButtonClicked(buttonName));

            // Setup hover and press animations
            button.transition = Selectable.Transition.Animation;
            AnimationTriggers triggers = new AnimationTriggers();
            button.animationTriggers = triggers;

            // Add hover and press event listeners
            EventTrigger eventTrigger = button.gameObject.GetComponent<EventTrigger>() ?? button.gameObject.AddComponent<EventTrigger>();

            AddEventTriggerListener(eventTrigger, EventTriggerType.PointerEnter, (data) => { OnFooterButtonHover(button, true); });
            AddEventTriggerListener(eventTrigger, EventTriggerType.PointerExit, (data) => { OnFooterButtonHover(button, false); });
            AddEventTriggerListener(eventTrigger, EventTriggerType.PointerDown, (data) => { OnFooterButtonPress(button, true); });
            AddEventTriggerListener(eventTrigger, EventTriggerType.PointerUp, (data) => { OnFooterButtonPress(button, false); });
        }

        private void AddEventTriggerListener(EventTrigger trigger, EventTriggerType eventType, UnityEngine.Events.UnityAction<BaseEventData> action)
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = eventType;
            entry.callback.AddListener(action);
            trigger.triggers.Add(entry);
        }

        private void AnimateFooterButtons(float startDelay)
        {
            for (int i = 0; i < footerButtons.Count; i++)
            {
                UnityEngine.UI.Button button = footerButtons[i];
                float delay = startDelay + i * 0.05f;
                button.transform.localScale = Vector3.zero;
                activeAnimations.Add(LeanTween.scale(button.gameObject, Vector3.one, 0.2f).setDelay(delay).setEaseOutBack());
            }
        }

        private void SkipAnimationIfPlaying()
        {
            if (isAnimating)
            {
                SkipAnimation();
            }
        }

        private void SkipAnimation()
        {
            isAnimating = false;
            isEntranceAnimationComplete = true;

            // Cancel all active animations
            foreach (var anim in activeAnimations)
            {
                LeanTween.cancel(anim.uniqueId);
            }
            activeAnimations.Clear();

            // Set logo to final position
            logo.anchoredPosition = new Vector2(logoOriginalPosition.x, logo.anchoredPosition.y);

            // Set buttons to final positions
            Vector2 buttonPosition = new Vector2(0, -firstButtonOffset);
            foreach (var button in buttons)
            {
                button.GetComponent<RectTransform>().anchoredPosition = buttonPosition;
                buttonPosition.y -= buttonHeight + buttonSpacing;
            }

            // Set footer buttons to final scale and position
            foreach (var button in footerButtons)
            {
                button.transform.localScale = Vector3.one;
                button.transform.localPosition = footerButtonOriginalPositions[button];
            }
        }

        private void OnFooterButtonHover(UnityEngine.UI.Button button, bool isHovering)
        {
            Vector3 originalPosition = footerButtonOriginalPositions[button];
            if (isHovering)
            {
                LeanTween.moveLocalY(button.gameObject, originalPosition.y + 10f, 0.2f).setEaseOutCubic();
            }
            else
            {
                LeanTween.moveLocalY(button.gameObject, originalPosition.y, 0.2f).setEaseOutCubic();
            }
        }

        private void OnFooterButtonPress(UnityEngine.UI.Button button, bool isPressed)
        {
            Vector3 originalPosition = footerButtonOriginalPositions[button];
            if (isPressed)
            {
                LeanTween.moveLocalY(button.gameObject, originalPosition.y - 5f, 0.1f).setEaseOutCubic();
            }
            else
            {
                LeanTween.moveLocalY(button.gameObject, originalPosition.y, 0.1f).setEaseOutCubic();
            }
        }

        private void OnFooterButtonClicked(string buttonName)
        {
            _mainMenu.HandleIntroButtonClick(buttonName);
        }

    }
}