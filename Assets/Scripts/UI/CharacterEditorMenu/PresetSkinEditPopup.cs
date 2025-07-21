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
            
            _currentPreset.SkinHair.Value = string.Empty;
            _currentPreset.SkinEye.Value = string.Empty;
            _currentPreset.SkinGlass.Value = string.Empty;
            _currentPreset.SkinFace.Value = string.Empty;
            _currentPreset.SkinSkin.Value = string.Empty;
            _currentPreset.SkinCostume.Value = string.Empty;
            _currentPreset.SkinLogo.Value = string.Empty;
            _currentPreset.SkinGearL.Value = string.Empty;
            _currentPreset.SkinGearR.Value = string.Empty;
            _currentPreset.SkinGas.Value = string.Empty;
            _currentPreset.SkinHoodie.Value = string.Empty;
            _currentPreset.SkinWeaponTrail.Value = string.Empty;
            _currentPreset.SkinThunderspearL.Value = string.Empty;
            _currentPreset.SkinThunderspearR.Value = string.Empty;
            _currentPreset.SkinHookL.Value = string.Empty;
            _currentPreset.SkinHookLTiling.Value = 1f;
            _currentPreset.SkinHookR.Value = string.Empty;
            _currentPreset.SkinHookRTiling.Value = 1f;
            _currentPreset.SkinHat.Value = string.Empty;
            _currentPreset.SkinHead.Value = string.Empty;
            _currentPreset.SkinBack.Value = string.Empty;
            _currentPreset.SkinHorse.Value = string.Empty;
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
            _backupValues["SkinHair"] = _currentPreset.SkinHair.Value;
            _backupValues["SkinEye"] = _currentPreset.SkinEye.Value;
            _backupValues["SkinGlass"] = _currentPreset.SkinGlass.Value;
            _backupValues["SkinFace"] = _currentPreset.SkinFace.Value;
            _backupValues["SkinSkin"] = _currentPreset.SkinSkin.Value;
            _backupValues["SkinCostume"] = _currentPreset.SkinCostume.Value;
            _backupValues["SkinLogo"] = _currentPreset.SkinLogo.Value;
            _backupValues["SkinGearL"] = _currentPreset.SkinGearL.Value;
            _backupValues["SkinGearR"] = _currentPreset.SkinGearR.Value;
            _backupValues["SkinGas"] = _currentPreset.SkinGas.Value;
            _backupValues["SkinHoodie"] = _currentPreset.SkinHoodie.Value;
            _backupValues["SkinWeaponTrail"] = _currentPreset.SkinWeaponTrail.Value;
            _backupValues["SkinThunderspearL"] = _currentPreset.SkinThunderspearL.Value;
            _backupValues["SkinThunderspearR"] = _currentPreset.SkinThunderspearR.Value;
            _backupValues["SkinHookL"] = _currentPreset.SkinHookL.Value;
            _backupValues["SkinHookR"] = _currentPreset.SkinHookR.Value;
            _backupValues["SkinHat"] = _currentPreset.SkinHat.Value;
            _backupValues["SkinHead"] = _currentPreset.SkinHead.Value;
            _backupValues["SkinBack"] = _currentPreset.SkinBack.Value;
            _backupValues["SkinHorse"] = _currentPreset.SkinHorse.Value;
            _backupFloatValues["SkinHookLTiling"] = _currentPreset.SkinHookLTiling.Value;
            _backupFloatValues["SkinHookRTiling"] = _currentPreset.SkinHookRTiling.Value;
            _hasBackup = true;
        }
        
        private void RestoreBackup()
        {
            if (!_hasBackup || _currentPreset == null)
            {
                return;
            }
            if (_backupValues.ContainsKey("SkinHair")) _currentPreset.SkinHair.Value = _backupValues["SkinHair"];
            if (_backupValues.ContainsKey("SkinEye")) _currentPreset.SkinEye.Value = _backupValues["SkinEye"];
            if (_backupValues.ContainsKey("SkinGlass")) _currentPreset.SkinGlass.Value = _backupValues["SkinGlass"];
            if (_backupValues.ContainsKey("SkinFace")) _currentPreset.SkinFace.Value = _backupValues["SkinFace"];
            if (_backupValues.ContainsKey("SkinSkin")) _currentPreset.SkinSkin.Value = _backupValues["SkinSkin"];
            if (_backupValues.ContainsKey("SkinCostume")) _currentPreset.SkinCostume.Value = _backupValues["SkinCostume"];
            if (_backupValues.ContainsKey("SkinLogo")) _currentPreset.SkinLogo.Value = _backupValues["SkinLogo"];
            if (_backupValues.ContainsKey("SkinGearL")) _currentPreset.SkinGearL.Value = _backupValues["SkinGearL"];
            if (_backupValues.ContainsKey("SkinGearR")) _currentPreset.SkinGearR.Value = _backupValues["SkinGearR"];
            if (_backupValues.ContainsKey("SkinGas")) _currentPreset.SkinGas.Value = _backupValues["SkinGas"];
            if (_backupValues.ContainsKey("SkinHoodie")) _currentPreset.SkinHoodie.Value = _backupValues["SkinHoodie"];
            if (_backupValues.ContainsKey("SkinWeaponTrail")) _currentPreset.SkinWeaponTrail.Value = _backupValues["SkinWeaponTrail"];
            if (_backupValues.ContainsKey("SkinThunderspearL")) _currentPreset.SkinThunderspearL.Value = _backupValues["SkinThunderspearL"];
            if (_backupValues.ContainsKey("SkinThunderspearR")) _currentPreset.SkinThunderspearR.Value = _backupValues["SkinThunderspearR"];
            if (_backupValues.ContainsKey("SkinHookL")) _currentPreset.SkinHookL.Value = _backupValues["SkinHookL"];
            if (_backupValues.ContainsKey("SkinHookR")) _currentPreset.SkinHookR.Value = _backupValues["SkinHookR"];
            if (_backupValues.ContainsKey("SkinHat")) _currentPreset.SkinHat.Value = _backupValues["SkinHat"];
            if (_backupValues.ContainsKey("SkinHead")) _currentPreset.SkinHead.Value = _backupValues["SkinHead"];
            if (_backupValues.ContainsKey("SkinBack")) _currentPreset.SkinBack.Value = _backupValues["SkinBack"];
            if (_backupValues.ContainsKey("SkinHorse")) _currentPreset.SkinHorse.Value = _backupValues["SkinHorse"];
            if (_backupFloatValues.ContainsKey("SkinHookLTiling")) _currentPreset.SkinHookLTiling.Value = _backupFloatValues["SkinHookLTiling"];
            if (_backupFloatValues.ContainsKey("SkinHookRTiling")) _currentPreset.SkinHookRTiling.Value = _backupFloatValues["SkinHookRTiling"];
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