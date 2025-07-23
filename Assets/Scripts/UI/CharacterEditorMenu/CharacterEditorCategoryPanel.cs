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
    class CharacterEditorCategoryPanel: HeadedPanel
    {
        protected override string Title => UIManager.GetLocaleCommon("Editor");
        protected override float Width => 330f;
        protected override float Height => 240f;
        protected override float VerticalSpacing => 20f;
        protected override int HorizontalPadding => 25;
        protected override int VerticalPadding => 25;
        private StringSetting _category = new StringSetting();
        protected CharacterEditorGameManager _gameManager;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            _gameManager = (CharacterEditorGameManager)SceneLoader.CurrentGameManager;
            ElementStyle style = new ElementStyle(titleWidth: 95f, themePanel: ThemePanel);
            float dropdownWidth = 160f;
            string[] categories = new string[] { "Human", "Titan" };
            _category.Value = GetCurrentCategory();
            ElementFactory.CreateDropdownSetting(SinglePanel, style, _category, UIManager.GetLocaleCommon("Category"), categories,
                elementWidth: dropdownWidth, onDropdownOptionSelect: () => OnCategoryChange());
        }

        private string GetCurrentCategory()
        {
            string currentCategory = "Human";
            if (!CharacterEditorGameManager.HumanMode)
                currentCategory = "Titan";
            return currentCategory;
        }

        private void OnCategoryChange()
        {
            if (_category.Value != GetCurrentCategory())
            {
                bool currentlyHuman = CharacterEditorGameManager.HumanMode;
                bool shouldPreserveSkinPreview = CharacterEditorSkinsPanel.GetPersistentGlobalPreview() || CharacterEditorSkinsPanel.GetPersistentCustomPreview();
                if (currentlyHuman && _gameManager?.Character is DummyHuman dummyHuman)
                {
                    var currentSet = (HumanCustomSet)SettingsManager.HumanCustomSettings.CustomSets.GetSelectedSet();
                    var currentWeapon = dummyHuman.Setup.Weapon;
                    dummyHuman.Setup.Load(currentSet, currentWeapon, false);
                }
                else if (!currentlyHuman && _gameManager?.Character is DummyTitan dummyTitan)
                {
                    var currentSet = (TitanCustomSet)SettingsManager.TitanCustomSettings.TitanCustomSets.GetSelectedSet();
                    dummyTitan.Setup.Load(currentSet);
                }
                if (_category.Value == "Titan")
                {
                    CharacterEditorSkinsPanel.ResetSkinPreviewToggles();
                }
                _gameManager.StartCoroutine(CategoryChangeCaptureCoroutine(currentlyHuman, shouldPreserveSkinPreview));
            }
        }
        
        private void ResetHumanSkinPreviewToggles()
        {
            CharacterEditorCostumePanel.ResetSkinPreviewToggles();
        }
        
        private System.Collections.IEnumerator CategoryChangeCaptureCoroutine(bool isHuman, bool shouldPreserveSkinPreview)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            Utility.CharacterPreviewGenerator.CaptureCurrentCharacterPreview(isHuman);
            if (_category.Value == "Human")
                CharacterEditorGameManager.HumanMode = true;
            else
                CharacterEditorGameManager.HumanMode = false;
            SceneLoader.LoadScene(SceneName.CharacterEditor);
            if (shouldPreserveSkinPreview)
            {
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                yield return new WaitForEndOfFrame();
                var gameManager = (CharacterEditorGameManager)ApplicationManagers.SceneLoader.CurrentGameManager;
                if (gameManager?.Character is DummyHuman dummyHuman)
                {
                    bool originalGlobalEnabled = SettingsManager.CustomSkinSettings.Human.GlobalSkinOverridesEnabled.Value;
                    bool originalSetEnabled = SettingsManager.CustomSkinSettings.Human.SetSpecificSkinsEnabled.Value;
                    SettingsManager.CustomSkinSettings.Human.GlobalSkinOverridesEnabled.Value = CharacterEditorSkinsPanel.GetPersistentGlobalPreview();
                    SettingsManager.CustomSkinSettings.Human.SetSpecificSkinsEnabled.Value = CharacterEditorSkinsPanel.GetPersistentCustomPreview();
                    dummyHuman.LoadSkin();
                    yield return new WaitForEndOfFrame();
                    yield return new WaitForEndOfFrame();
                    SettingsManager.CustomSkinSettings.Human.GlobalSkinOverridesEnabled.Value = originalGlobalEnabled;
                    SettingsManager.CustomSkinSettings.Human.SetSpecificSkinsEnabled.Value = originalSetEnabled;
                }
            }
        }
    }
}
