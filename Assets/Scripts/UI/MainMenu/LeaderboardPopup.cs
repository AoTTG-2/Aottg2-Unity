using ApplicationManagers;
using Settings;
using SimpleJSONFixed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UI
{
    class LeaderboardPopup: BasePopup
    {
        protected override string Title => UIManager.GetLocale("MainMenu", "LeaderboardPopup", "Title");
        protected override float Width => 800f;
        protected override float Height => 630f;
        protected override bool CategoryPanel => true;
        protected override string DefaultCategoryPanel => "Default";
        public StringSetting CurrentCategory = new StringSetting(string.Empty);
        public StringSetting CurrentSubcategory = new StringSetting(string.Empty);
        protected override bool UseSound => true;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            string cat = "MainMenu";
            string sub = "LeaderboardPopup";
            ElementStyle buttonStyle = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementStyle style = new ElementStyle(themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Back"),
                onClick: () => OnButtonClick("Back"));
        }

        protected override void RegisterCategoryPanels()
        {
            _categoryPanelTypes.Add("Default", typeof(LeaderboardDefaultPanel));
        }

        private void OnButtonClick(string name)
        {
            if (name == "Back")
                Hide();
        }
    }
}
