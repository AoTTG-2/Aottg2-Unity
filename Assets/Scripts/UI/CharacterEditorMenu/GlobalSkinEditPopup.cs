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
    class GlobalSkinEditPopup: BasePopup
    {
        protected override string Title => $"{UIManager.GetLocale("SettingsPopup", "Skins.Human", "EditGlobal")}: {SettingsManager.CustomSkinSettings.Human.GetSelectedSet()?.Name.Value ?? ""}";
        protected override float Width => 1100f;
        protected override float Height => 700f;
        protected override float VerticalSpacing => 15f;
        protected override int HorizontalPadding => 20;
        protected override int VerticalPadding => 20;
        protected override bool DoublePanel => true;
        protected override bool ScrollBar => true;

        private HumanCustomSkinSet _currentGlobalSet;
        private Dictionary<string, string> _backupValues = new Dictionary<string, string>();
        private Dictionary<string, float> _backupFloatValues = new Dictionary<string, float>();
        private bool _hasBackup = false;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(titleWidth: 200f, themePanel: ThemePanel);
            ElementStyle buttonStyle = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            HumanCustomSkinSet currentSet = (HumanCustomSkinSet)SettingsManager.CustomSkinSettings.Human.GetSelectedSet();
            _currentGlobalSet = currentSet;
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, _currentGlobalSet.Hair, UIManager.GetLocale("SettingsPopup", "Skins.Human", "Hair"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, _currentGlobalSet.Eye, UIManager.GetLocale("SettingsPopup", "Skins.Human", "Eye"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, _currentGlobalSet.Glass, UIManager.GetLocale("SettingsPopup", "Skins.Human", "Glass"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, _currentGlobalSet.Face, UIManager.GetLocale("SettingsPopup", "Skins.Human", "Face"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, _currentGlobalSet.Skin, UIManager.GetLocale("SettingsPopup", "Skins.Human", "Skin"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, _currentGlobalSet.Costume, UIManager.GetLocale("SettingsPopup", "Skins.Human", "Costume"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, _currentGlobalSet.Logo, UIManager.GetLocale("SettingsPopup", "Skins.Human", "Logo"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, _currentGlobalSet.GearL, UIManager.GetLocale("SettingsPopup", "Skins.Human", "GearL"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, _currentGlobalSet.GearR, UIManager.GetLocale("SettingsPopup", "Skins.Human", "GearR"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, _currentGlobalSet.Gas, UIManager.GetLocale("SettingsPopup", "Skins.Human", "Gas"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, _currentGlobalSet.Hoodie, UIManager.GetLocale("SettingsPopup", "Skins.Human", "Hoodie"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelRight, style, _currentGlobalSet.WeaponTrail, UIManager.GetLocale("SettingsPopup", "Skins.Human", "WeaponTrail"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelRight, style, _currentGlobalSet.Horse, UIManager.GetLocale("SettingsPopup", "Skins.Human", "Horse"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelRight, style, _currentGlobalSet.ThunderspearL, UIManager.GetLocale("SettingsPopup", "Skins.Human", "ThunderspearL"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelRight, style, _currentGlobalSet.ThunderspearR, UIManager.GetLocale("SettingsPopup", "Skins.Human", "ThunderspearR"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelRight, style, _currentGlobalSet.HookL, UIManager.GetLocale("SettingsPopup", "Skins.Human", "HookL"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelRight, style, _currentGlobalSet.HookLTiling, UIManager.GetLocale("SettingsPopup", "Skins.Human", "HookLTiling"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelRight, style, _currentGlobalSet.HookR, UIManager.GetLocale("SettingsPopup", "Skins.Human", "HookR"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelRight, style, _currentGlobalSet.HookRTiling, UIManager.GetLocale("SettingsPopup", "Skins.Human", "HookRTiling"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelRight, style, _currentGlobalSet.Hat, UIManager.GetLocale("SettingsPopup", "Skins.Human", "Hat"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelRight, style, _currentGlobalSet.Head, UIManager.GetLocale("SettingsPopup", "Skins.Human", "Head"), elementWidth: 300f);
            ElementFactory.CreateInputSetting(DoublePanelRight, style, _currentGlobalSet.Back, UIManager.GetLocale("SettingsPopup", "Skins.Human", "Back"), elementWidth: 300f);
            ElementFactory.CreateTextButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Clear"), onClick: () => OnButtonClick("Clear"));
            ElementFactory.CreateTextButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Apply"), onClick: () => OnButtonClick("Save"));
            ElementFactory.CreateTextButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Cancel"), onClick: () => OnButtonClick("Cancel"));
        }
        
        private void OnButtonClick(string name)
        {
            switch (name)
            {
                case "Save":
                    OnSaveClick();
                    break;
                case "Cancel":
                    OnCancelClick();
                    break;
                case "Clear":
                    OnClearClick();
                    break;
            }
        }

        private void CreateBackup()
        {
            if (_currentGlobalSet == null || _hasBackup) return;
            _backupValues.Clear();
            _backupFloatValues.Clear();
            _backupValues["Hair"] = _currentGlobalSet.Hair.Value;
            _backupValues["Eye"] = _currentGlobalSet.Eye.Value;
            _backupValues["Glass"] = _currentGlobalSet.Glass.Value;
            _backupValues["Face"] = _currentGlobalSet.Face.Value;
            _backupValues["Skin"] = _currentGlobalSet.Skin.Value;
            _backupValues["Costume"] = _currentGlobalSet.Costume.Value;
            _backupValues["Logo"] = _currentGlobalSet.Logo.Value;
            _backupValues["Hat"] = _currentGlobalSet.Hat.Value;
            _backupValues["Head"] = _currentGlobalSet.Head.Value;
            _backupValues["Back"] = _currentGlobalSet.Back.Value;
            _backupValues["GearL"] = _currentGlobalSet.GearL.Value;
            _backupValues["GearR"] = _currentGlobalSet.GearR.Value;
            _backupValues["Gas"] = _currentGlobalSet.Gas.Value;
            _backupValues["Hoodie"] = _currentGlobalSet.Hoodie.Value;
            _backupValues["WeaponTrail"] = _currentGlobalSet.WeaponTrail.Value;
            _backupValues["ThunderspearL"] = _currentGlobalSet.ThunderspearL.Value;
            _backupValues["ThunderspearR"] = _currentGlobalSet.ThunderspearR.Value;
            _backupValues["HookL"] = _currentGlobalSet.HookL.Value;
            _backupValues["HookR"] = _currentGlobalSet.HookR.Value;
            _backupValues["Horse"] = _currentGlobalSet.Horse.Value;
            _backupFloatValues["HookLTiling"] = _currentGlobalSet.HookLTiling.Value;
            _backupFloatValues["HookRTiling"] = _currentGlobalSet.HookRTiling.Value;
            _hasBackup = true;
        }

        private void RestoreBackup()
        {
            if (_currentGlobalSet == null || !_hasBackup) return;
            _currentGlobalSet.Hair.Value = _backupValues["Hair"];
            _currentGlobalSet.Eye.Value = _backupValues["Eye"];
            _currentGlobalSet.Glass.Value = _backupValues["Glass"];
            _currentGlobalSet.Face.Value = _backupValues["Face"];
            _currentGlobalSet.Skin.Value = _backupValues["Skin"];
            _currentGlobalSet.Costume.Value = _backupValues["Costume"];
            _currentGlobalSet.Logo.Value = _backupValues["Logo"];
            _currentGlobalSet.Hat.Value = _backupValues["Hat"];
            _currentGlobalSet.Head.Value = _backupValues["Head"];
            _currentGlobalSet.Back.Value = _backupValues["Back"];
            _currentGlobalSet.GearL.Value = _backupValues["GearL"];
            _currentGlobalSet.GearR.Value = _backupValues["GearR"];
            _currentGlobalSet.Gas.Value = _backupValues["Gas"];
            _currentGlobalSet.Hoodie.Value = _backupValues["Hoodie"];
            _currentGlobalSet.WeaponTrail.Value = _backupValues["WeaponTrail"];
            _currentGlobalSet.ThunderspearL.Value = _backupValues["ThunderspearL"];
            _currentGlobalSet.ThunderspearR.Value = _backupValues["ThunderspearR"];
            _currentGlobalSet.HookL.Value = _backupValues["HookL"];
            _currentGlobalSet.HookR.Value = _backupValues["HookR"];
            _currentGlobalSet.Horse.Value = _backupValues["Horse"];
            _currentGlobalSet.HookLTiling.Value = _backupFloatValues["HookLTiling"];
            _currentGlobalSet.HookRTiling.Value = _backupFloatValues["HookRTiling"];
        }

        public void OnCancelClick()
        {
            if (_hasBackup)
            {
                RestoreBackup();
            }
            Hide();
        }
        
        private void OnClearClick()
        {
            if (!_hasBackup)
            {
                CreateBackup();
            }
            
            _currentGlobalSet.Hair.Value = string.Empty;
            _currentGlobalSet.Eye.Value = string.Empty;
            _currentGlobalSet.Glass.Value = string.Empty;
            _currentGlobalSet.Face.Value = string.Empty;
            _currentGlobalSet.Skin.Value = string.Empty;
            _currentGlobalSet.Costume.Value = string.Empty;
            _currentGlobalSet.Logo.Value = string.Empty;
            _currentGlobalSet.Hat.Value = string.Empty;
            _currentGlobalSet.Head.Value = string.Empty;
            _currentGlobalSet.Back.Value = string.Empty;
            _currentGlobalSet.GearL.Value = string.Empty;
            _currentGlobalSet.GearR.Value = string.Empty;
            _currentGlobalSet.Gas.Value = string.Empty;
            _currentGlobalSet.Hoodie.Value = string.Empty;
            _currentGlobalSet.WeaponTrail.Value = string.Empty;
            _currentGlobalSet.ThunderspearL.Value = string.Empty;
            _currentGlobalSet.ThunderspearR.Value = string.Empty;
            _currentGlobalSet.HookL.Value = string.Empty;
            _currentGlobalSet.HookR.Value = string.Empty;
            _currentGlobalSet.Horse.Value = string.Empty;
            _currentGlobalSet.HookLTiling.Value = 1f;
            _currentGlobalSet.HookRTiling.Value = 1f;
            var inputElements = GetComponentsInChildren<InputSettingElement>();
            foreach (var element in inputElements)
            {
                element.SyncElement();
            }
        }
        
        private void OnSaveClick()
        {
            if (_currentGlobalSet != null)
            {
                _backupValues.Clear();
                _backupFloatValues.Clear();
                _hasBackup = false;
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
                        humanMenu.StartCoroutine(LoadSkinAndRestoreSettings(character, originalGlobal, originalCustom));
                    }
                }                    
                Hide();
            }
        }
        
        private System.Collections.IEnumerator LoadSkinAndRestoreSettings(DummyHuman character, bool originalGlobal, bool originalCustom)
        {
            yield return new WaitForSeconds(0.1f);
            character.LoadSkin();
            yield return new WaitForSeconds(0.1f);
            SettingsManager.CustomSkinSettings.Human.GlobalSkinOverridesEnabled.Value = originalGlobal;
            SettingsManager.CustomSkinSettings.Human.SetSpecificSkinsEnabled.Value = originalCustom;
        }

        public override void Show()
        {
            _currentGlobalSet = (HumanCustomSkinSet)SettingsManager.CustomSkinSettings.Human.GetSelectedSet();
            _backupValues.Clear();
            _backupFloatValues.Clear();
            _hasBackup = false;
            foreach (Transform child in DoublePanelLeft.transform) Destroy(child.gameObject);
            foreach (Transform child in DoublePanelRight.transform) Destroy(child.gameObject);
            foreach (Transform child in BottomBar.transform) Destroy(child.gameObject);
            Setup();
            if (_currentGlobalSet != null && !_hasBackup)
            {
                CreateBackup();
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