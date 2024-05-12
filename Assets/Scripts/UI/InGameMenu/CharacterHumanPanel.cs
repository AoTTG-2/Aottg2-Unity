using Map;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using GameManagers;
using Characters;
using Utility;

namespace UI
{
    class CharacterHumanPanel : CharacterCategoryPanel
    {
        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            string cat = "CharacterPopup";
            string sub = "General";
            InGameMiscSettings miscSettings = SettingsManager.InGameCurrent.Misc;
            InGameCharacterSettings charSettings = SettingsManager.InGameCharacterSettings;
            charSettings.CharacterType.Value = PlayerCharacter.Human;
            ElementStyle dropdownStyle = new ElementStyle(titleWidth: 200f, themePanel: ThemePanel);
            List<string> loadouts = new List<string>();
            if (miscSettings.AllowBlades.Value)
                loadouts.Add(HumanLoadout.Blades);
            if (miscSettings.AllowAHSS.Value)
                loadouts.Add(HumanLoadout.AHSS);
            if (miscSettings.AllowAPG.Value)
                loadouts.Add(HumanLoadout.APG);
            if (miscSettings.AllowThunderspears.Value)
                loadouts.Add(HumanLoadout.Thunderspears);
            if (loadouts.Count == 0)
                loadouts.Add(HumanLoadout.Blades);
            if (!loadouts.Contains(charSettings.Loadout.Value))
                charSettings.Loadout.Value = loadouts[0];
            List<string> specials = HumanSpecials.GetSpecialNames(charSettings.Loadout.Value, miscSettings.AllowShifterSpecials.Value);
            if (!specials.Contains(charSettings.Special.Value))
                charSettings.Special.Value = specials[0];
            string[] options = GetCharOptions();
            ElementFactory.CreateIconPickSetting(DoublePanelLeft, dropdownStyle, charSettings.CustomSet, UIManager.GetLocale(cat, sub, "Character"),
                options, GetCharIcons(options), UIManager.CurrentMenu.IconPickPopup, elementWidth: 180f, elementHeight: 40f);
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, dropdownStyle, charSettings.Costume, UIManager.GetLocale(cat, sub, "Costume"),
                new string[] {"Costume1", "Costume2", "Costume3"}, elementWidth: 180f, optionsWidth: 180f);
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, dropdownStyle, charSettings.Loadout, UIManager.GetLocale(cat, sub, "Loadout"),
                loadouts.ToArray(), elementWidth: 180f, optionsWidth: 180f, onDropdownOptionSelect: () => parent.RebuildCategoryPanel());
            options = specials.ToArray();
            ElementFactory.CreateIconPickSetting(DoublePanelRight, dropdownStyle, charSettings.Special, UIManager.GetLocale(cat, sub, "Special"),
                options, GetSpecialIcons(options), UIManager.CurrentMenu.IconPickPopup, elementWidth: 180f, elementHeight: 40f);
            ElementFactory.CreateIconPickSetting(DoublePanelRight, dropdownStyle, charSettings.Special_2, UIManager.GetLocale(cat, sub, "Special2"), // added by Ata 12 May 2024 for Ability Wheel //
                options, GetSpecialIcons(options), UIManager.CurrentMenu.IconPickPopup, elementWidth: 180f, elementHeight: 40f);
            ElementFactory.CreateIconPickSetting(DoublePanelRight, dropdownStyle, charSettings.Special_3, UIManager.GetLocale(cat, sub, "Special3"), // added by Ata 12 May 2024 for Ability Wheel //
                options, GetSpecialIcons(options), UIManager.CurrentMenu.IconPickPopup, elementWidth: 180f, elementHeight: 40f);
            if (miscSettings.PVP.Value == (int)PVPMode.Team)
            {
                ElementFactory.CreateDropdownSetting(DoublePanelRight, dropdownStyle, charSettings.Team, UIManager.GetLocaleCommon("Team"),
               new string[] { "Blue", "Red" }, elementWidth: 180f, optionsWidth: 180f);
            }
        }

        protected string[] GetCharOptions()
        {
            List<string> sets = new List<string>(SettingsManager.HumanCustomSettings.Costume1Sets.GetSetNames());
            sets.AddRange(SettingsManager.HumanCustomSettings.CustomSets.GetSetNames());
            return sets.ToArray();
        }

        protected string[] GetCharIcons(string[] options)
        {
            List<string> icons = new List<string>();
            List<string> sets = new List<string>(SettingsManager.HumanCustomSettings.Costume1Sets.GetSetNames());
            foreach (string option in options)
            {
                if (sets.Contains(option))
                    icons.Add(ResourcePaths.Characters + "/Human/Previews/Preset" + option);
                else
                    icons.Add(ResourcePaths.Characters + "/Human/Previews/PresetNone");
            }
            return icons.ToArray();
        }

        protected string[] GetSpecialIcons(string[] options)
        {
            List<string> icons = new List<string>();
            foreach (string option in options)
                icons.Add(ResourcePaths.UI + "/Icons/Specials/" + HumanSpecials.GetSpecialIcon(option));
            return icons.ToArray();
        }
    }
}
