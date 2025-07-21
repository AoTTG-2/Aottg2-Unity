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
using Utility;

namespace UI
{
    class CharacterEditorSkinsPanel: HeadedPanel
    {
        protected override string Title => UIManager.GetLocale("SettingsPopup", "Top", "SkinsButton");
        protected override float Width => 330f;
        protected override float Height => 310f;
        protected override float VerticalSpacing => 20f;
        protected override int HorizontalPadding => 20;
        protected override int VerticalPadding => 20;
        private CharacterEditorMenu _menu;
        private BoolSetting _globalSkinPreview;
        private BoolSetting _customSkinPreview;
        private static bool _persistentGlobalSkinPreview = false;
        private static bool _persistentCustomSkinPreview = false;
        private static bool _hasInitializedPersistentStates = false;
        private bool _isRebuildingPanel = false;
        private IntSetting _globalSetDropdownSetting;
        
        public static bool GetPersistentGlobalPreview()
        {
            return _persistentGlobalSkinPreview;
        }
        
        public static bool GetPersistentCustomPreview()
        {
            return _persistentCustomSkinPreview;
        }
        
        public static void ResetSkinPreviewToggles()
        {
            _persistentGlobalSkinPreview = false;
            _persistentCustomSkinPreview = false;
        }

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            _menu = (CharacterEditorMenu)UIManager.CurrentMenu;
            _isRebuildingPanel = true;
            if (!_hasInitializedPersistentStates)
            {
                _persistentGlobalSkinPreview = false;
                _persistentCustomSkinPreview = false;
                _hasInitializedPersistentStates = true;
            }
            _globalSkinPreview = new BoolSetting(_persistentGlobalSkinPreview);
            _customSkinPreview = new BoolSetting(_persistentCustomSkinPreview);
            ElementStyle style = new ElementStyle(titleWidth: 95f, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocale("SettingsPopup", "Skins.Human", "EditPreset"), onClick: () => OnButtonClick("EditCustomSkins"));
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocale("SettingsPopup", "Skins.Human", "EditGlobal"), onClick: () => OnButtonClick("EditGlobalSkin"));
            float dropdownWidth = 160f;
            string[] globalSetNames = GetFilteredGlobalSetNames();
            int currentGlobalIndex = GetFilteredGlobalSetIndex();
            _globalSetDropdownSetting = new IntSetting(currentGlobalIndex);
            GameObject globalSetsDropdown = ElementFactory.CreateDropdownSetting(SinglePanel, style, _globalSetDropdownSetting, UIManager.GetLocale("SettingsPopup", "Skins.Human", "Global"), globalSetNames,
                elementWidth: dropdownWidth, onDropdownOptionSelect: () => OnGlobalSkinSetSelected(_globalSetDropdownSetting.Value));
            Text globalSetsLabel = globalSetsDropdown.transform.Find("Label").GetComponent<Text>();
            GameObject globalToggle = ElementFactory.CreateToggleSetting(SinglePanel, style, _globalSkinPreview, UIManager.GetLocale("SettingsPopup", "Skins.Human", "GlobalSkinOverridesEnabled"), onValueChanged: () => OnSkinPreviewToggle());
            GameObject customToggle = ElementFactory.CreateToggleSetting(SinglePanel, style, _customSkinPreview, UIManager.GetLocale("SettingsPopup", "Skins.Human", "SetSpecificSkinsEnabled"), onValueChanged: () => OnSkinPreviewToggle());
            Text globalText = globalToggle.transform.Find("Label").GetComponent<Text>();
            Text customText = customToggle.transform.Find("Label").GetComponent<Text>();
            globalText.horizontalOverflow = HorizontalWrapMode.Overflow;
            customText.horizontalOverflow = HorizontalWrapMode.Overflow;
            globalText.verticalOverflow = VerticalWrapMode.Overflow;
            customText.verticalOverflow = VerticalWrapMode.Overflow;
            LayoutElement globalLayout = globalToggle.transform.Find("Label").GetComponent<LayoutElement>();
            LayoutElement customLayout = customToggle.transform.Find("Label").GetComponent<LayoutElement>();
            if (globalLayout != null) globalLayout.preferredWidth = 210f;
            if (customLayout != null) customLayout.preferredWidth = 210f;
            if (_globalSkinPreview.Value || _customSkinPreview.Value)
            {
                StartCoroutine(ApplySkinPreviewAfterInitialSetup());
            }
            _isRebuildingPanel = false;
        }
        
        private string[] GetFilteredGlobalSetNames()
        {
            string[] originalSetNames = SettingsManager.CustomSkinSettings.Human.GetSetNames();
            List<string> filteredSetNames = new List<string>();
            for (int i = 0; i < originalSetNames.Length; i++)
            {
                if (!originalSetNames[i].StartsWith("Custom Set:"))
                {
                    filteredSetNames.Add(originalSetNames[i]);
                }
            }
            return filteredSetNames.ToArray();
        }
        
        private int GetFilteredGlobalSetIndex()
        {
            int rememberedIndex = SettingsManager.CustomSkinSettings.Human.LastGlobalPresetIndex.Value;
            string[] originalSetNames = SettingsManager.CustomSkinSettings.Human.GetSetNames();
            List<int> originalIndices = new List<int>();
            for (int i = 0; i < originalSetNames.Length; i++)
            {
                if (!originalSetNames[i].StartsWith("Custom Set:"))
                {
                    originalIndices.Add(i);
                }
            }
            int filteredIndex = originalIndices.IndexOf(rememberedIndex);
            return filteredIndex >= 0 ? filteredIndex : 0;
        }
        
        private void OnGlobalSkinSetSelected(int filteredIndex)
        {
            string[] originalSetNames = SettingsManager.CustomSkinSettings.Human.GetSetNames();
            List<int> originalIndices = new List<int>();            
            for (int i = 0; i < originalSetNames.Length; i++)
            {
                if (!originalSetNames[i].StartsWith("Custom Set:"))
                {
                    originalIndices.Add(i);
                }
            }
            if (filteredIndex >= 0 && filteredIndex < originalIndices.Count)
            {
                int originalIndex = originalIndices[filteredIndex];
                SettingsManager.CustomSkinSettings.Human.GetSelectedSetIndex().Value = originalIndex;
                SettingsManager.CustomSkinSettings.Human.LastGlobalPresetIndex.Value = originalIndex;
                string selectedSetName = originalSetNames[originalIndex];
                var humanMenu = _menu as CharacterEditorHumanMenu;
                if (humanMenu != null && humanMenu._editGlobalSkinPopup != null && humanMenu._editGlobalSkinPopup.IsActive)
                {
                    humanMenu._editGlobalSkinPopup.OnCancelClick();
                }
                bool shouldApplySkinPreview = _persistentGlobalSkinPreview || _persistentCustomSkinPreview;
                if (shouldApplySkinPreview)
                {
                    StartCoroutine(ApplySkinPreviewAfterGlobalSetChange());
                }
            }
        }
        
        private System.Collections.IEnumerator ApplySkinPreviewAfterGlobalSetChange()
        {
            yield return null;
            var gameManager = (CharacterEditorGameManager)ApplicationManagers.SceneLoader.CurrentGameManager;
            if (gameManager?.Character is DummyHuman dummyHuman)
            {
                bool originalGlobalEnabled = SettingsManager.CustomSkinSettings.Human.GlobalSkinOverridesEnabled.Value;
                bool originalSetEnabled = SettingsManager.CustomSkinSettings.Human.SetSpecificSkinsEnabled.Value;
                _menu.ResetCharacter(fullReset: false);
                yield return null;
                yield return null;
                SettingsManager.CustomSkinSettings.Human.GlobalSkinOverridesEnabled.Value = _persistentGlobalSkinPreview;
                SettingsManager.CustomSkinSettings.Human.SetSpecificSkinsEnabled.Value = _persistentCustomSkinPreview;
                dummyHuman.LoadSkin();
                yield return null;
                yield return null;
                yield return null;
                SettingsManager.CustomSkinSettings.Human.GlobalSkinOverridesEnabled.Value = originalGlobalEnabled;
                SettingsManager.CustomSkinSettings.Human.SetSpecificSkinsEnabled.Value = originalSetEnabled;
            }
        }
        
        private void OnSkinPreviewToggle()
        {
            if (_isRebuildingPanel)
            {
                return;
            }
            try
            {
                _persistentGlobalSkinPreview = _globalSkinPreview.Value;
                _persistentCustomSkinPreview = _customSkinPreview.Value;
                var gameManager = (CharacterEditorGameManager)ApplicationManagers.SceneLoader.CurrentGameManager;
                if (gameManager?.Character is DummyHuman dummyHuman)
                {
                    bool originalGlobalEnabled = SettingsManager.CustomSkinSettings.Human.GlobalSkinOverridesEnabled.Value;
                    bool originalSetEnabled = SettingsManager.CustomSkinSettings.Human.SetSpecificSkinsEnabled.Value;
                    SettingsManager.CustomSkinSettings.Human.GlobalSkinOverridesEnabled.Value = _globalSkinPreview.Value;
                    SettingsManager.CustomSkinSettings.Human.SetSpecificSkinsEnabled.Value = _customSkinPreview.Value;
                    var humanMenu = _menu as CharacterEditorHumanMenu;
                    if (humanMenu != null)
                    {
                        var currentSet = (HumanCustomSet)SettingsManager.HumanCustomSettings.CustomSets.GetSelectedSet();
                        dummyHuman.Setup.Load(currentSet, (HumanWeapon)humanMenu.Weapon.Value, false);
                        StartCoroutine(LoadSkinAfterReset(dummyHuman, originalGlobalEnabled, originalSetEnabled));
                    }
                    else
                    {
                        dummyHuman.LoadSkin();
                        StartCoroutine(RestoreOriginalSkinSettings(originalGlobalEnabled, originalSetEnabled));
                    }
                }
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError($"Error updating skin preview: {ex.Message}");
            }
        }
        
        private System.Collections.IEnumerator LoadSkinAfterReset(DummyHuman dummyHuman, bool originalGlobalEnabled, bool originalSetEnabled)
        {
            yield return null;
            yield return null;
            if (dummyHuman.Setup == null)
            {
                StartCoroutine(RestoreOriginalSkinSettings(originalGlobalEnabled, originalSetEnabled));
                yield break;
            }
            dummyHuman.LoadSkin();
            yield return null;
            yield return null;
            SettingsManager.CustomSkinSettings.Human.GlobalSkinOverridesEnabled.Value = originalGlobalEnabled;
            SettingsManager.CustomSkinSettings.Human.SetSpecificSkinsEnabled.Value = originalSetEnabled;
        }
        
        private System.Collections.IEnumerator ApplySkinPreviewAfterInitialSetup()
        {
            yield return null;
            yield return null;
            yield return null;
            var gameManager = (CharacterEditorGameManager)ApplicationManagers.SceneLoader.CurrentGameManager;
            if (gameManager?.Character is DummyHuman dummyHuman)
            {
                bool originalGlobalEnabled = SettingsManager.CustomSkinSettings.Human.GlobalSkinOverridesEnabled.Value;
                bool originalSetEnabled = SettingsManager.CustomSkinSettings.Human.SetSpecificSkinsEnabled.Value;
                SettingsManager.CustomSkinSettings.Human.GlobalSkinOverridesEnabled.Value = _globalSkinPreview.Value;
                SettingsManager.CustomSkinSettings.Human.SetSpecificSkinsEnabled.Value = _customSkinPreview.Value;
                dummyHuman.LoadSkin();
                yield return null;
                yield return null;
                SettingsManager.CustomSkinSettings.Human.GlobalSkinOverridesEnabled.Value = originalGlobalEnabled;
                SettingsManager.CustomSkinSettings.Human.SetSpecificSkinsEnabled.Value = originalSetEnabled;
            }
        }
        
        private System.Collections.IEnumerator RestoreOriginalSkinSettings(bool originalGlobalEnabled, bool originalSetEnabled)
        {
            yield return null;
            SettingsManager.CustomSkinSettings.Human.GlobalSkinOverridesEnabled.Value = originalGlobalEnabled;
            SettingsManager.CustomSkinSettings.Human.SetSpecificSkinsEnabled.Value = originalSetEnabled;
        }

        private void OnButtonClick(string name)
        {
            switch (name)
            {
                case "EditCustomSkins":
                    var humanMenu = _menu as CharacterEditorHumanMenu;
                    if (humanMenu != null && humanMenu._editPresetSkinPopup != null)
                    {
                        humanMenu._editGlobalSkinPopup.Hide();
                        humanMenu._editStatsPopup.Hide();
                        humanMenu._editPerksPopup.Hide();
                        humanMenu._editPresetSkinPopup.Show();
                    }
                    break;
                case "EditGlobalSkin":
                    var humanMenu2 = _menu as CharacterEditorHumanMenu;
                    if (humanMenu2 != null && humanMenu2._editGlobalSkinPopup != null)
                    {
                        humanMenu2._editPresetSkinPopup.Hide();
                        humanMenu2._editStatsPopup.Hide();
                        humanMenu2._editPerksPopup.Hide();
                        humanMenu2._editGlobalSkinPopup.Show();
                    }
                    break;
            }
        }
    }
} 