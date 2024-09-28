using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace GisketchUI
{
    public class IntroPanel : SidePanel
    {
        [SerializeField] private RectTransform logo;
        private float firstButtonOffset = 360f;
        private float buttonSpacing = 0f;
        [SerializeField] private float buttonWidth = 400f;
        [SerializeField] private float buttonHeight = 72f;

        private UnityEngine.UI.Button questButton;
        private UnityEngine.UI.Button leaderboardButton;
        private UnityEngine.UI.Button socialButton;
        private UnityEngine.UI.Button helpButton;
        private UnityEngine.UI.Button patreonButton;

        private Vector2 nextButtonPosition;
        private Vector3 logoOriginalPosition;

        [SerializeField] private GameObject iconsContainer;

        private List<UnityEngine.UI.Button> footerButtons = new List<UnityEngine.UI.Button>();
        private Dictionary<UnityEngine.UI.Button, Vector3> footerButtonOriginalPositions = new Dictionary<UnityEngine.UI.Button, Vector3>();

        public override void Initialize()
        {
            base.Initialize();
            nextButtonPosition = new Vector2(0, -firstButtonOffset);
            SetupLogo();
            SetupButtons();
            SetupFooterButtons();
        }

        private void SetupLogo()
        {
            logo.anchoredPosition = new Vector3(-700f, 0f, 0f);
            logo.sizeDelta = new Vector2(640f, 520f);
        }

        private void SetupButtons()
        {
            AddButton("TUTORIAL", OnTutorialClicked);
            AddButton("SINGLEPLAYER", OnSingleplayerClicked);
            AddButton("MULTIPLAYER", OnMultiplayerClicked);
            AddButton("PROFILE", OnProfileClicked);
            AddButton("SETTINGS", OnSettingsClicked);
            AddButton("TOOLS", OnToolsClicked);
            AddButton("CREDITS", OnCreditsClicked);
            AddButton("QUIT", OnQuitClicked);
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
            base.Show(1.5f);
        }

        protected override void AnimateEntrance(float duration)
        {
            base.AnimateEntrance(duration);

            float timeToStart = duration * 0.5f;

            // Animate logo
            float logoDelay = timeToStart;
            LeanTween.moveX(logo, logoOriginalPosition.x, 1f).setDelay(logoDelay).setEaseInOutCubic();

            // Animate buttons
            float buttonDelay = timeToStart + 0.7f;
            float buttonDuration = 0.3f;
            for (int i = 0; i < buttons.Count; i++)
            {
                float delay = buttonDelay + (i * 0.05f);
                Vector3 startPosition = nextButtonPosition;
                nextButtonPosition.y -= buttonHeight + buttonSpacing;
                buttons[i].AnimateEntrance(buttonDuration, delay, startPosition);
            }

            // Animate footer buttons
            AnimateFooterButtons(buttonDelay + buttons.Count * 0.05f);
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
                float delay = startDelay + i * 0.1f;
                button.transform.localScale = Vector3.zero;
                LeanTween.scale(button.gameObject, Vector3.one, 0.3f).setDelay(delay).setEaseOutBack();
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
            Debug.Log($"{buttonName} clicked");
        }


        // Button click handlers...
        private void OnTutorialClicked()
        {
            Debug.Log("Tutorial clicked");
        }

        private void OnSingleplayerClicked()
        {
            Debug.Log("Singleplayer clicked");
        }

        private void OnMultiplayerClicked()
        {
            Debug.Log("Multiplayer clicked");
        }

        private void OnProfileClicked()
        {
            Debug.Log("Profile clicked");
        }

        private void OnSettingsClicked()
        {
            Debug.Log("Settings clicked");
        }

        private void OnToolsClicked()
        {
            Debug.Log("Tools clicked");
        }

        private void OnCreditsClicked()
        {
            Debug.Log("Credits clicked");
        }

        private void OnQuitClicked()
        {
            Debug.Log("Quit clicked");
        }

        private void OnQuestClicked()
        {
            Debug.Log("Quest clicked");
        }

        private void OnLeaderboardClicked()
        {
            Debug.Log("Leaderboard clicked");
        }

        private void OnSocialClicked()
        {
            Debug.Log("Social clicked");
        }

        private void OnHelpClicked()
        {
            Debug.Log("Help clicked");
        }

        private void OnPatreonClicked()
        {
            Debug.Log("Patreon clicked");
        }
    }
}