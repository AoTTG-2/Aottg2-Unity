using System;
using UnityEngine.UI;
using UnityEngine;
using Settings;

namespace UI
{
    class QuestPopup: BasePopup
    {
        protected override string Title => string.Empty;
        protected override float Width => 990f;
        protected override float Height => 740f;
        protected override bool CategoryPanel => true;
        protected override bool CategoryButtons => true;
        protected override string DefaultCategoryPanel => "Daily";
        public StringSetting TierSelection = new StringSetting("Bronze");
        public StringSetting CompletedSelection = new StringSetting("In Progress");
        protected override bool UseSound => true;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            SetupBottomButtons();
        }

        public void CreateAchievementDropdowns(Transform panel)
        {
            ElementStyle style = new ElementStyle(titleWidth: 0f, themePanel: ThemePanel);
            ElementFactory.CreateDropdownSetting(panel, style, TierSelection, "",
                new string[] { "Bronze", "Silver", "Gold" }, elementWidth: 180f, onDropdownOptionSelect: () => RebuildCategoryPanel());
            ElementFactory.CreateDropdownSetting(panel, style, CompletedSelection, "",
                new string[] { "In Progress", "Completed" }, elementWidth: 180f, onDropdownOptionSelect: () => RebuildCategoryPanel());
        }

        protected override void SetupTopButtons()
        {
            ElementStyle style = new ElementStyle(fontSize: 28, themePanel: ThemePanel);
            foreach (string buttonName in new string[] { "Daily", "Weekly", "Achievements" })
            {
                string locale;
                if (buttonName == "Daily" || buttonName == "Weekly")
                    locale = UIManager.GetLocale("MainMenu", "QuestsPopup", buttonName);
                else
                    locale = UIManager.GetLocaleCommon(buttonName);
                GameObject obj = ElementFactory.CreateCategoryButton(TopBar, style, locale, onClick: () => SetCategoryPanel(buttonName));
                _topButtons.Add(buttonName, obj.GetComponent<Button>());
            }
            base.SetupTopButtons();
        }

        protected override void RegisterCategoryPanels()
        {
            _categoryPanelTypes.Add("Daily", typeof(QuestDailyPanel));
            _categoryPanelTypes.Add("Weekly", typeof(QuestWeeklyPanel));
            _categoryPanelTypes.Add("Achievements", typeof(QuestAchievementsPanel));
        }

        protected override void SetupPopups()
        {
            base.SetupPopups();
        }

        private void SetupBottomButtons()
        {
            ElementStyle style = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            foreach (string buttonName in new string[] { "Back" })
            {
                GameObject obj = ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon(buttonName), 
                    onClick: () => OnBottomBarButtonClick(buttonName));
            }
        }

        private void OnBottomBarButtonClick(string name)
        {
            switch (name)
            {
                case "Back":
                    Hide();
                    break;
            }
        }
    }
}
