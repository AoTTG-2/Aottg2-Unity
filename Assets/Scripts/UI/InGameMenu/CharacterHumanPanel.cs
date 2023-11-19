using Map;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using GameManagers;
using Characters;

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
            List<string> sets = new List<string>(SettingsManager.HumanCustomSettings.Costume1Sets.GetSetNames());
            sets.AddRange(SettingsManager.HumanCustomSettings.CustomSets.GetSetNames());
            List<string> specials = HumanSpecials.GetSpecialNames(charSettings.Loadout.Value, miscSettings.AllowShifterSpecials.Value);
            if (!specials.Contains(charSettings.Special.Value))
                charSettings.Special.Value = specials[0];
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, dropdownStyle, charSettings.CustomSet, UIManager.GetLocale(cat, sub, "Character"),
                sets.ToArray(), elementWidth: 180f, optionsWidth: 180f);
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, dropdownStyle, charSettings.Costume, UIManager.GetLocale(cat, sub, "Costume"),
                new string[] {"Costume1", "Costume2", "Costume3"}, elementWidth: 180f, optionsWidth: 180f);
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, dropdownStyle, charSettings.Loadout, UIManager.GetLocale(cat, sub, "Loadout"),
                loadouts.ToArray(), elementWidth: 180f, optionsWidth: 180f, onDropdownOptionSelect: () => parent.RebuildCategoryPanel());
            ElementFactory.CreateDropdownSetting(DoublePanelRight, dropdownStyle, charSettings.Special, UIManager.GetLocale(cat, sub, "Special"),
                specials.ToArray(), elementWidth: 180f, optionsWidth: 180f);
            if (miscSettings.PVP.Value == (int)PVPMode.Team)
            {
                ElementFactory.CreateDropdownSetting(DoublePanelRight, dropdownStyle, charSettings.Team, UIManager.GetLocaleCommon("Team"),
               new string[] { "Blue", "Red" }, elementWidth: 180f, optionsWidth: 180f);
            }
        }
    }
}
