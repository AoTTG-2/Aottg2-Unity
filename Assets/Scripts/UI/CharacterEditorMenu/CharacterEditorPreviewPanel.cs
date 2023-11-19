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
    class CharacterEditorPreviewPanel: HeadedPanel
    {
        protected override string Title => UIManager.GetLocale("CharacterEditor", "Preview", "Title");
        protected override float Width => 330f;
        protected override float Height => 280f;
        protected override float VerticalSpacing => 20f;
        protected override int HorizontalPadding => 25;
        protected override int VerticalPadding => 25;
        private CharacterEditorMenu _menu;
        private StringSetting _emote = new StringSetting("Salute");

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            _menu = (CharacterEditorMenu)UIManager.CurrentMenu;
            ElementStyle style = new ElementStyle(titleWidth: 95f, themePanel: ThemePanel);
            HumanCustomSettings settings = SettingsManager.HumanCustomSettings;
            float dropdownWidth = 160f;
            string cat = "CharacterEditor";
            string sub = "Preview";
            HumanCustomSet set = (HumanCustomSet)settings.CustomSets.GetSelectedSet();
            ElementFactory.CreateDropdownSetting(SinglePanel, style, _menu.Weapon, UIManager.GetLocale(cat, sub, "Weapon"), 
                new string[] { "Blade", "AHSS", "ThunderSpear", "APG" }, elementWidth: dropdownWidth, onDropdownOptionSelect: () => _menu.ResetCharacter(true));
            string[] emotes = new string[] { "Salute", "Wave", "Nod", "Shake", "Dance", "Eat", "Flip" };
            ElementFactory.CreateDropdownSetting(SinglePanel, style, _emote, UIManager.GetLocale(cat, sub, "Emote"), emotes,
                elementWidth: dropdownWidth, onDropdownOptionSelect: () => OnEmote());
        }

        private void OnEmote()
        {
            _menu.Character.EmoteAction(_emote.Value);
        }
    }
}
