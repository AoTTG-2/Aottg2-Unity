using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ApplicationManagers;
using Settings;
using Characters;
using GameManagers;

namespace UI
{
    class CharacterEditorHumanMenu: CharacterEditorMenu
    {
        private CharacterEditorCostumePanel _costumePanel;
        private CharacterEditorStatsPanel _statsPanel;
        private CharacterEditorSkinsPanel _skinsPanel;
        private CharacterEditorHumanCategoryPanel _categoryPanel;
        public CharacterEditorEditStatsPopup _editStatsPopup;
        public CharacterEditorEditPerksPopup _editPerksPopup;
        public PresetSkinEditPopup _editPresetSkinPopup;
        public GlobalSkinEditPopup _editGlobalSkinPopup;
        public IntSetting Weapon = new IntSetting((int)HumanWeapon.Blade);

        public override void Setup()
        {
            base.Setup();
        }

        protected override void SetupPopups()
        {
            base.SetupPopups();
            _editStatsPopup = ElementFactory.CreateDefaultPopup<CharacterEditorEditStatsPopup>(transform, false);
            _editPerksPopup = ElementFactory.CreateDefaultPopup<CharacterEditorEditPerksPopup>(transform, false);
            _editPresetSkinPopup = ElementFactory.CreateDefaultPopup<PresetSkinEditPopup>(transform, false);
            _editGlobalSkinPopup = ElementFactory.CreateDefaultPopup<GlobalSkinEditPopup>(transform, false);
        }

        public override bool IsPopupActive()
        {
            return base.IsPopupActive() || _editStatsPopup.IsActive || _editPerksPopup.IsActive || _editPresetSkinPopup.IsActive || _editGlobalSkinPopup.IsActive;
        }

        public override void RebuildPanels(bool costumePopup)
        {
            if (_costumePanel != null)
            {
                Destroy(_costumePanel.gameObject);
                Destroy(_statsPanel.gameObject);
                Destroy(_skinsPanel.gameObject);
                Destroy(_categoryPanel.gameObject);
                Destroy(_editStatsPopup.gameObject);
                Destroy(_editPerksPopup.gameObject);
                Destroy(_editPresetSkinPopup.gameObject);
            }
            _costumePanel = ElementFactory.CreateHeadedPanel<CharacterEditorCostumePanel>(transform, enabled: true).GetComponent<CharacterEditorCostumePanel>();
            _statsPanel = ElementFactory.CreateHeadedPanel<CharacterEditorStatsPanel>(transform, enabled: true).GetComponent<CharacterEditorStatsPanel>();
            _skinsPanel = ElementFactory.CreateHeadedPanel<CharacterEditorSkinsPanel>(transform, enabled: true).GetComponent<CharacterEditorSkinsPanel>();
            _categoryPanel = ElementFactory.CreateHeadedPanel<CharacterEditorHumanCategoryPanel>(transform, enabled: true).GetComponent<CharacterEditorHumanCategoryPanel>();
            ElementFactory.SetAnchor(_costumePanel.gameObject, TextAnchor.UpperLeft, TextAnchor.UpperLeft, new Vector2(20f, -20f));
            ElementFactory.SetAnchor(_statsPanel.gameObject, TextAnchor.UpperRight, TextAnchor.UpperRight, new Vector2(-20f, -380f));
            ElementFactory.SetAnchor(_skinsPanel.gameObject, TextAnchor.UpperRight, TextAnchor.UpperRight, new Vector2(-20f, -760f));
            ElementFactory.SetAnchor(_categoryPanel.gameObject, TextAnchor.UpperRight, TextAnchor.UpperRight, new Vector2(-20f, -20f));
            _editStatsPopup = ElementFactory.CreateDefaultPopup<CharacterEditorEditStatsPopup>(transform, false);
            _editPerksPopup = ElementFactory.CreateDefaultPopup<CharacterEditorEditPerksPopup>(transform, false);
            _editPresetSkinPopup = ElementFactory.CreateDefaultPopup<PresetSkinEditPopup>(transform, false);
        }

        public override void ResetCharacter(bool fullReset = false)
        {
            var newSet = (HumanCustomSet)SettingsManager.HumanCustomSettings.CustomSets.GetSelectedSet();
            var character = (DummyHuman)_gameManager.Character;
            character.Setup.Load(newSet, (HumanWeapon)Weapon.Value, false);
            if (fullReset)
                character.Idle();
        }
        
        public void ResetCharacterWithSkins(bool fullReset)
        {
            if (fullReset)
            {
                ((CharacterEditorGameManager)ApplicationManagers.SceneLoader.CurrentGameManager).ReinstantiateCharacter();
            }
            else
            {
                var character = (DummyHuman)((CharacterEditorGameManager)ApplicationManagers.SceneLoader.CurrentGameManager).Character;
                if (character != null)
                {
                    character.LoadSkin();
                }
            }
        }

        public override float GetMinMouseX()
        {
            return _costumePanel.GetPhysicalWidth() + 20f;
        }

        public override float GetMaxMouseX()
        {
            return Screen.width - _categoryPanel.GetPhysicalWidth() - 20f;
        }
    }
}
