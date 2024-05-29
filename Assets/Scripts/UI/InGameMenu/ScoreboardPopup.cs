using System;
using UnityEngine.UI;
using UnityEngine;
using Settings;

namespace UI
{
    class ScoreboardPopup : BasePopup
    {
        protected override string Title => string.Empty;
        protected override PopupAnimation PopupAnimationType => PopupAnimation.Fade;
        protected override float MinFadeAlpha => 0.5f;
        protected override float Width => 1010f;
        protected override float Height => 800f;
        protected override bool CategoryPanel => true;
        protected override bool CategoryButtons => true;
        protected override string DefaultCategoryPanel => "Scoreboard";
        public string LocaleCategory = "ScoreboardPopup";
        public ScoreboardProfilePopup _profilePopup;
        public ConfirmPopup _kickPopup;
        public ScoreboardMutePopup _mutePopup;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            SetupBottomButtons();
        }

        public override void Show()
        {
            base.Show();
            if (_currentCategoryPanelName.Value == "Scoreboard")
                _currentCategoryPanel.GetComponent<ScoreboardScorePanel>().Sync();
        }

        protected override void SetupTopButtons()
        {
            ElementStyle style = new ElementStyle(fontSize: 28, themePanel: ThemePanel);
            foreach (string buttonName in new string[] { "Scoreboard", "GameInfo" })
            {
                GameObject obj = ElementFactory.CreateCategoryButton(TopBar, style, UIManager.GetLocale(LocaleCategory, "Top", buttonName + "Button"),
                    onClick: () => SetCategoryPanel(buttonName));
                _topButtons.Add(buttonName, obj.GetComponent<Button>());
            }
            base.SetupTopButtons();
        }

        protected override void RegisterCategoryPanels()
        {
            _categoryPanelTypes.Add("Scoreboard", typeof(ScoreboardScorePanel));
            _categoryPanelTypes.Add("GameInfo", typeof(ScoreboardInfoPanel));
        }

        protected override void SetupPopups()
        {
            base.SetupPopups();
            _profilePopup = ElementFactory.CreateDefaultPopup<ScoreboardProfilePopup>(transform);
            _kickPopup = ElementFactory.CreateDefaultPopup<ConfirmPopup>(transform);
            _mutePopup = ElementFactory.CreateDefaultPopup<ScoreboardMutePopup>(transform);
            _popups.Add(_profilePopup);
            _popups.Add(_mutePopup);
            _popups.Add(_kickPopup);
        }

        private void SetupBottomButtons()
        {
            ElementStyle style = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Back"),
                    onClick: () => OnBottomBarButtonClick("Back"));
        }

        private void OnBottomBarButtonClick(string name)
        {
            switch (name)
            {
                case "Back":
                    ((InGameMenu)UIManager.CurrentMenu).SetScoreboardMenu(false, true);
                    break;
            }
        }
    }
}
