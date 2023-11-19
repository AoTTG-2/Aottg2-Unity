using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using Settings;
using System.Collections;
using ApplicationManagers;
using GameManagers;

namespace UI
{
    class MapEditorSettingsPopup: BasePopup
    {
        protected override string Title => string.Empty;
        protected override float Width => 1010f;
        protected override float Height => 630f;
        protected override bool CategoryPanel => true;
        protected override bool CategoryButtons => true;
        protected override string DefaultCategoryPanel => "General";
        public string LocaleCategory = "MapEditorSettings";
        protected List<SaveableSettingsContainer> _saveableSettings = new List<SaveableSettingsContainer>();

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            SetupBottomButtons();
            _saveableSettings.Add(SettingsManager.MapEditorSettings);
            _saveableSettings.Add(SettingsManager.InputSettings.MapEditor);
        }

        protected override void SetupTopButtons()
        {
            ElementStyle style = new ElementStyle(fontSize: 28, themePanel: ThemePanel);
            foreach (string buttonName in new string[] { "General", "Keybinds" })
            {
                GameObject obj = ElementFactory.CreateCategoryButton(TopBar, style, UIManager.GetLocale(LocaleCategory, "Top", buttonName + "Button"),
                    onClick: () => SetCategoryPanel(buttonName));
                _topButtons.Add(buttonName, obj.GetComponent<Button>());
            }
            base.SetupTopButtons();
        }

        protected override void RegisterCategoryPanels()
        {
            _categoryPanelTypes.Add("General", typeof(MapEditorSettingsGeneralPanel));
            _categoryPanelTypes.Add("Keybinds", typeof(MapEditorSettingsKeybindsPanel));
        }

        private void SetupBottomButtons()
        {
            ElementStyle style = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            foreach (string buttonName in new string[] { "Default", "Save", "Back" })
            {
                GameObject obj = ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon(buttonName), 
                    onClick: () => OnBottomBarButtonClick(buttonName));
            }
        }

        private void OnConfirmSetDefault()
        {
            foreach (SaveableSettingsContainer setting in _saveableSettings)
            {
                setting.SetDefault();
                setting.Save();
            }
            RebuildCategoryPanel();
            UIManager.CurrentMenu.MessagePopup.Show("Settings reset to default.");
        }

        private void OnBottomBarButtonClick(string name)
        {
            switch (name)
            {
                case "Save":
                    foreach (SaveableSettingsContainer setting in _saveableSettings)
                        setting.Save();
                    Hide();
                    break;
                case "Back":
                    Hide();
                    break;
                case "Default":
                    UIManager.CurrentMenu.ConfirmPopup.Show("Are you sure you want to reset to default?", () => OnConfirmSetDefault(),
                        "Reset default");
                    break;
            }
        }
        
        public override void Hide()
        {
            if (gameObject.activeSelf)
            {
                foreach (SaveableSettingsContainer setting in _saveableSettings)
                    setting.Apply();
                
            }
            base.Hide();
        }
    }
}
