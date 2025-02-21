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
    class CharacterEditorTitanCategoryPanel: CharacterEditorCategoryPanel
    {
        protected override float Height => 280f;
        private StringSetting _emote = new StringSetting("Laugh");

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(titleWidth: 95f, themePanel: ThemePanel);
            float dropdownWidth = 160f;
            string[] emotes = new string[] { "Laugh", "Nod", "Shake", "Roar" };
            ElementFactory.CreateDropdownSetting(SinglePanel, style, _emote, UIManager.GetLocale("CharacterEditor", "Preview", "Emote"), emotes,
                elementWidth: dropdownWidth, onDropdownOptionSelect: () => OnEmote());
        }

        private void OnEmote()
        {
            _gameManager.Character.EmoteAction(_emote.Value);
        }
    }
}
