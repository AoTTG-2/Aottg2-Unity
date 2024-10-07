using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using Settings;
using System.Collections;

namespace UI
{
    class EditProfilePopup: BasePopup
    {
        protected override string Title => string.Empty;
        protected override float Width => 730f;
        protected override float Height => 690f;
        protected override bool CategoryPanel => true;
        protected override bool CategoryButtons => true;
        protected override string DefaultCategoryPanel => "Profile";
        protected override bool UseSound => true;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            SetupBottomButtons();
        }

        protected override void SetupTopButtons()
        {
            ElementStyle style = new ElementStyle(fontSize: 28, themePanel: ThemePanel);
            foreach (string buttonName in new string[] { "Profile", "Stats" })
            {
                GameObject obj = ElementFactory.CreateCategoryButton(TopBar, style, UIManager.GetLocaleCommon(buttonName),
                    onClick: () => SetCategoryPanel(buttonName));
                _topButtons.Add(buttonName, obj.GetComponent<Button>());
            }
            base.SetupTopButtons();
        }

        protected override void RegisterCategoryPanels()
        {
            _categoryPanelTypes.Add("Profile", typeof(EditProfileProfilePanel));
            _categoryPanelTypes.Add("Stats", typeof(EditProfileStatsPanel));
        }

        protected override void SetupPopups()
        {
            base.SetupPopups();
        }

        private void SetupBottomButtons()
        {
            ElementStyle style = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            foreach (string buttonName in new string[] { "Save" })
            {
                GameObject obj = ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon(buttonName), 
                    onClick: () => OnBottomBarButtonClick(buttonName));
            }
        }

        private void OnBottomBarButtonClick(string name)
        {
            switch (name)
            {
                case "Save":
                    SettingsManager.ProfileSettings.Save();
                    Hide();
                    break;
            }
        }
    }
}
