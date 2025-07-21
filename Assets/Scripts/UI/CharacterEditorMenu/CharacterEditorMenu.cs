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
    class CharacterEditorMenu: BaseMenu
    {
        public CharacterEditorGameManager _gameManager;

        public override void Setup()
        {
            base.Setup();
            _gameManager = (CharacterEditorGameManager)SceneLoader.CurrentGameManager;
            RebuildPanels(true);
            ResetCharacter();
        }
        
        public virtual bool IsPopupActive()
        {
            return SelectListPopup.IsActive || IconPickPopup.IsActive;
        }

        public virtual void RebuildPanels(bool costumePopup)
        {
           
        }

        public virtual void ResetCharacter(bool fullReset = false)
        {
        }

        public virtual float GetMinMouseX()
        {
            return 0f;
        }

        public virtual float GetMaxMouseX()
        {
            return Screen.width;
        }
    }
}
