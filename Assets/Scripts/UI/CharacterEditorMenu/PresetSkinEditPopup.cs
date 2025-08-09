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
using Characters;

namespace UI
{
    class PresetSkinEditPopup: BasePopup
    {
        protected override string Title => $"{UIManager.GetLocale("SettingsPopup", "Skins.Human", "Preset")}: {SettingsManager.HumanCustomSettings.CustomSets.GetSelectedSet()?.Name.Value ?? ""}";
        protected override float Width => 1100f;
        protected override float Height => 700f;
        protected override float VerticalSpacing => 15f;
        protected override int HorizontalPadding => 20;
        protected override int VerticalPadding => 20;
        protected override bool DoublePanel => true;
        protected override bool ScrollBar => true;
        private HumanCustomSet _currentPreset;
        private Dictionary<string, string> _backupValues = new Dictionary<string, string>();
        private Dictionary<string, float> _backupFloatValues = new Dictionary<string, float>();
        private bool _hasBackup = false;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(titleWidth: 200f, themePanel: ThemePanel);
            ElementStyle buttonStyle = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            HumanCustomSettings settings = SettingsManager.HumanCustomSettings;
            _currentPreset = (HumanCustomSet)settings.CustomSets.GetSelectedSet();
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, _currentPreset.SkinHair, UIManager.GetLocale("SettingsPopup", "Skins.Human", "Hair"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, _currentPreset.SkinEye, UIManager.GetLocale("SettingsPopup", "Skins.Human", "Eye"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, _currentPreset.SkinGlass, UIManager.GetLocale("SettingsPopup", "Skins.Human", "Glass"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, _currentPreset.SkinFace, UIManager.GetLocale("SettingsPopup", "Skins.Human", "Face"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, _currentPreset.SkinSkin, UIManager.GetLocale("SettingsPopup", "Skins.Human", "Skin"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, _currentPreset.SkinCostume, UIManager.GetLocale("SettingsPopup", "Skins.Human", "Costume"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, _currentPreset.SkinLogo, UIManager.GetLocale("SettingsPopup", "Skins.Human", "Logo"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, _currentPreset.SkinGearL, UIManager.GetLocale("SettingsPopup", "Skins.Human", "GearL"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, _currentPreset.SkinGearR, UIManager.GetLocale("SettingsPopup", "Skins.Human", "GearR"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, _currentPreset.SkinGas, UIManager.GetLocale("SettingsPopup", "Skins.Human", "Gas"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, _currentPreset.SkinHoodie, UIManager.GetLocale("SettingsPopup", "Skins.Human", "Hoodie"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelRight, style, _currentPreset.SkinWeaponTrail, UIManager.GetLocale("SettingsPopup", "Skins.Human", "WeaponTrail"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelRight, style, _currentPreset.SkinHorse, UIManager.GetLocale("SettingsPopup", "Skins.Human", "Horse"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelRight, style, _currentPreset.SkinThunderspearL, UIManager.GetLocale("SettingsPopup", "Skins.Human", "ThunderspearL"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelRight, style, _currentPreset.SkinThunderspearR, UIManager.GetLocale("SettingsPopup", "Skins.Human", "ThunderspearR"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelRight, style, _currentPreset.SkinHookL, UIManager.GetLocale("SettingsPopup", "Skins.Human", "HookL"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelRight, style, _currentPreset.SkinHookLTiling, UIManager.GetLocale("SettingsPopup", "Skins.Human", "HookLTiling"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelRight, style, _currentPreset.SkinHookR, UIManager.GetLocale("SettingsPopup", "Skins.Human", "HookR"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelRight, style, _currentPreset.SkinHookRTiling, UIManager.GetLocale("SettingsPopup", "Skins.Human", "HookRTiling"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelRight, style, _currentPreset.SkinHat, UIManager.GetLocale("SettingsPopup", "Skins.Human", "Hat"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelRight, style, _currentPreset.SkinHead, UIManager.GetLocale("SettingsPopup", "Skins.Human", "Head"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelRight, style, _currentPreset.SkinBack, UIManager.GetLocale("SettingsPopup", "Skins.Human", "Back"), elementWidth: 300f);
            ElementFactory.CreateTextButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Clear"), onClick: () => OnButtonClick("Clear"));
            ElementFactory.CreateTextButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Apply"), onClick: () => OnButtonClick("Save"));
            ElementFactory.CreateTextButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Cancel"), onClick: () => OnButtonClick("Cancel"));
        }

        private void OnButtonClick(string name)
        {
            if (name == "Cancel")
            {
                OnCancelClick();
            }
            else if (name == "Clear")
            {
                OnClearClick();
            }
            else if (name == "Save")
            {
                OnSaveClick();
            }
        }

        private void OnClearClick()
        {
            if (!_hasBackup)
            {
                CreateBackup();
            }
            foreach (var typedSetting in _currentPreset.TypedSettings)
            {
                typedSetting.Value.SetDefault();
            }
            var inputElements = GetComponentsInChildren<InputSettingElement>();
            foreach (var element in inputElements)
            {
                element.SyncElement();
            }
        }

        private void CreateBackup()
        {
            _backupValues.Clear();
            _backupFloatValues.Clear();
            foreach (var kvp in _currentPreset.TypedSettings)
            {
                string key = kvp.Key;
                var setting = kvp.Value;
                var type = SettingsUtil.GetSettingType(setting);
                switch (type)
                {
                    case SettingType.String:
                        _backupValues[key] = ((StringSetting)setting).Value;
                        break;
                    case SettingType.Float:
                        _backupFloatValues[key] = ((FloatSetting)setting).Value;
                        break;
                    default:
                        break;
                }
            }
            _hasBackup = true;
        }
        
        private void RestoreBackup()
        {
            if (!_hasBackup || _currentPreset == null)
            {
                return;
            }
            foreach (var kvp in _currentPreset.TypedSettings)
            {
                string key = kvp.Key;
                var setting = kvp.Value;
                var type = SettingsUtil.GetSettingType(setting);
                switch (type)
                {
                    case SettingType.String:
                        if (_backupValues.ContainsKey(key))
                            ((StringSetting)setting).Value = _backupValues[key];
                        break;
                    case SettingType.Float:
                        if (_backupFloatValues.ContainsKey(key))
                            ((FloatSetting)setting).Value = _backupFloatValues[key];
                        break;
                    default:
                        break;
                }
            }
            var inputElements = GetComponentsInChildren<InputSettingElement>();
            foreach (var element in inputElements)
            {
                element.SyncElement();
            }
        }
        
        public void OnCancelClick()
        {
            if (_hasBackup)
            {
                RestoreBackup();
            }
            Hide();
        }
        
        private void OnSaveClick()
        {
            if (_currentPreset != null)
            {
                _backupValues.Clear();
                _backupFloatValues.Clear();
                _hasBackup = false;
                
                bool shouldApply = CharacterEditorCostumePanel.GetPersistentCustomPreview();
                
                if (shouldApply)
                {
                    var humanMenu = UIManager.CurrentMenu as CharacterEditorHumanMenu;
                    if (humanMenu != null)
                    {
                        var character = ((CharacterEditorGameManager)ApplicationManagers.SceneLoader.CurrentGameManager).Character as DummyHuman;
                        if (character != null)
                        {
                            bool globalEnabled = CharacterEditorCostumePanel.GetPersistentGlobalPreview();
                            bool customEnabled = CharacterEditorCostumePanel.GetPersistentCustomPreview();
                            bool originalGlobal = SettingsManager.CustomSkinSettings.Human.GlobalSkinOverridesEnabled.Value;
                            bool originalCustom = SettingsManager.CustomSkinSettings.Human.SetSpecificSkinsEnabled.Value;
                            SettingsManager.CustomSkinSettings.Human.GlobalSkinOverridesEnabled.Value = globalEnabled;
                            SettingsManager.CustomSkinSettings.Human.SetSpecificSkinsEnabled.Value = customEnabled;
                            character.Setup.Load(character.Setup.CustomSet, character.Setup.Weapon, false);
                            humanMenu.StartCoroutine(LoadSkinAndRestoreSettingsCoroutine(character, originalGlobal, originalCustom));
                        }
                    }
                }
                Hide();
            }
        }
        
        private void LoadSkinAndRestoreSettings(DummyHuman character, bool originalGlobal, bool originalCustom)
        {
            StartCoroutine(LoadSkinAndRestoreSettingsCoroutine(character, originalGlobal, originalCustom));
        }
        
        private System.Collections.IEnumerator LoadSkinAndRestoreSettingsCoroutine(DummyHuman character, bool originalGlobal, bool originalCustom)
        {
            yield return new WaitForSeconds(0.1f);
            character.LoadSkin();
            yield return new WaitForSeconds(0.1f);
            SettingsManager.CustomSkinSettings.Human.GlobalSkinOverridesEnabled.Value = originalGlobal;
            SettingsManager.CustomSkinSettings.Human.SetSpecificSkinsEnabled.Value = originalCustom;
        }
        
        public override void Show()
        {
            HumanCustomSettings settings = SettingsManager.HumanCustomSettings;
            if (settings?.CustomSets != null)
            {
                _currentPreset = (HumanCustomSet)settings.CustomSets.GetSelectedSet();
                if (_currentPreset != null && !_hasBackup)
                {
                    CreateBackup();
                }
            }
            base.Show();
        }
        
        protected override void OnDisable()
        {
            if (_hasBackup)
            {
                RestoreBackup();
            }
            base.OnDisable();
        }
        
        public override void HideImmediate()
        {
            if (_hasBackup)
            {
                RestoreBackup();
            }
            base.HideImmediate();
        }
    }
}