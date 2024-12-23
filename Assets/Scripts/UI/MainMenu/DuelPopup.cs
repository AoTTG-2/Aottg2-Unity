using System;
using UnityEngine.UI;
using UnityEngine;
using Settings;

namespace UI
{
    class DuelPopup : BasePopup
    {
        protected override string Title => string.Empty;
        protected override float Width => 1000f;
        protected override float Height => 800f;
        protected override bool CategoryPanel => true;
        protected override bool CategoryButtons => true;
        protected override string DefaultCategoryPanel => "Play";
        public string LocaleCategory = "DuelPopup";

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            SetupBottomButtons();
        }

        public override void Show()
        {
            base.Show();
        }

        protected override void SetupTopButtons()
        {
            ElementStyle style = new ElementStyle(fontSize: 28, themePanel: ThemePanel);
            foreach (string buttonName in new string[] { "Play", "Spectate" })
            {
                GameObject obj = ElementFactory.CreateCategoryButton(TopBar, style, UIManager.GetLocaleCommon(buttonName),
                    onClick: () => SetCategoryPanel(buttonName));
                _topButtons.Add(buttonName, obj.GetComponent<Button>());
            }
            base.SetupTopButtons();
        }

        protected override void RegisterCategoryPanels()
        {
            _categoryPanelTypes.Add("Play", typeof(DuelPlayPanel));
            _categoryPanelTypes.Add("Spectate", typeof(DuelSpectatePanel));
        }

        protected override void SetupPopups()
        {
            base.SetupPopups();
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
                    Hide();
                    ((MainMenu)UIManager.CurrentMenu).ShowMultiplayerMapPopup();
                    break;
            }
        }
    }
}
