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
    class CharacterEditorTitanMenu: CharacterEditorMenu
    {
        private CharacterEditorTitanCostumePanel _costumePanel;
        private CharacterEditorTitanCategoryPanel _categoryPanel;

        public override void Setup()
        {
            base.Setup();
        }

        protected override void SetupPopups()
        {
            base.SetupPopups();
        }

        public override bool IsPopupActive()
        {
            return base.IsPopupActive();
        }

        public override void RebuildPanels(bool costumePopup)
        {
            if (_costumePanel != null)
            {
                Destroy(_costumePanel.gameObject);
                Destroy(_categoryPanel.gameObject);
            }
            _costumePanel = ElementFactory.CreateHeadedPanel<CharacterEditorTitanCostumePanel>(transform, enabled: true).GetComponent<CharacterEditorTitanCostumePanel>();
            _categoryPanel = ElementFactory.CreateHeadedPanel<CharacterEditorTitanCategoryPanel>(transform, enabled: true).GetComponent<CharacterEditorTitanCategoryPanel>();
            ElementFactory.SetAnchor(_costumePanel.gameObject, TextAnchor.UpperLeft, TextAnchor.UpperLeft, new Vector2(20f, -20f));
            ElementFactory.SetAnchor(_categoryPanel.gameObject, TextAnchor.UpperRight, TextAnchor.UpperRight, new Vector2(-20f, -20f));
        }

        public override void ResetCharacter(bool fullReset = false)
        {
            _gameManager.ReinstantiateCharacter();
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
