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
    class SettingsPopup: BasePopup
    {
        protected override string Title => string.Empty;
        protected override float Width => 1000f;
        protected override float Height => 630f;
        protected override bool CategoryPanel => true;
        protected override bool CategoryButtons => true;
        protected override string DefaultCategoryPanel => "General";
        public string LocaleCategory = "SettingsPopup";
        private List<BaseSettingsContainer> _ignoreDefaultButtonSettings = new List<BaseSettingsContainer>();
        private List<SaveableSettingsContainer> _saveableSettings = new List<SaveableSettingsContainer>();
        protected override bool UseSound => true;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            SetupBottomButtons();
            SetupSettingsList();
        }

        protected override void SetupTopButtons()
        {
            ElementStyle style = new ElementStyle(fontSize: 28, themePanel: ThemePanel);
            foreach (string buttonName in new string[] { "General", "Sound", "Graphics", "UI", "Keybinds", "Skins", "Ability" })
            {
                GameObject obj = ElementFactory.CreateCategoryButton(TopBar, style, UIManager.GetLocale(LocaleCategory, "Top", buttonName + "Button"),
                    onClick: () => SetCategoryPanel(buttonName));
                _topButtons.Add(buttonName, obj.GetComponent<Button>());
            }
            base.SetupTopButtons();
        }

        protected override void RegisterCategoryPanels()
        {
            _categoryPanelTypes.Add("General", typeof(SettingsGeneralPanel));
            _categoryPanelTypes.Add("Sound", typeof(SettingsSoundPanel));
            _categoryPanelTypes.Add("Graphics", typeof(SettingsGraphicsPanel));
            _categoryPanelTypes.Add("UI", typeof(SettingsUIPanel));
            _categoryPanelTypes.Add("Keybinds", typeof(SettingsKeybindsPanel));
            _categoryPanelTypes.Add("Skins", typeof(SettingsSkinsPanel));
            _categoryPanelTypes.Add("Ability", typeof(SettingsAbilityPanel));
        }

        private void SetupSettingsList()
        {
            _saveableSettings.Add(SettingsManager.GeneralSettings);
            _saveableSettings.Add(SettingsManager.SoundSettings);
            _saveableSettings.Add(SettingsManager.GraphicsSettings);
            _saveableSettings.Add(SettingsManager.UISettings);
            _saveableSettings.Add(SettingsManager.InputSettings);
            _saveableSettings.Add(SettingsManager.CustomSkinSettings);
            _saveableSettings.Add(SettingsManager.AbilitySettings);
            _ignoreDefaultButtonSettings.Add(SettingsManager.CustomSkinSettings);
        }

        private void SetupBottomButtons()
        {
            ElementStyle style = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            foreach (string buttonName in new string[] { "Default", "Load", "Save", "Back" })
            {
                GameObject obj = ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon(buttonName), 
                    onClick: () => OnBottomBarButtonClick(buttonName));
            }
        }

        private void OnConfirmSetDefault()
        {
            foreach (SaveableSettingsContainer setting in _saveableSettings)
            {
                if (!_ignoreDefaultButtonSettings.Contains(setting))
                {
                    setting.SetDefault();
                    setting.Save();
                }
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
                    if (SceneLoader.SceneName == SceneName.InGame)
                    {
                        ((InGameMenu)UIManager.CurrentMenu)._pausePopup.Show();
                        Hide();
                    }
                    else
                        Hide();
                    break;
                case "Load":
                    foreach (SaveableSettingsContainer setting in _saveableSettings)
                        setting.Load();
                    RebuildCategoryPanel();
                    UIManager.CurrentMenu.MessagePopup.Show("Settings loaded from file.");
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
