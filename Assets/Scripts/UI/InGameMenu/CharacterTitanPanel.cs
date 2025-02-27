using GameManagers;
using Map;
using Settings;
using System.Collections.Generic;

namespace UI
{
    class CharacterTitanPanel : CharacterCategoryPanel
    {
        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            string cat = "CharacterPopup";
            string sub = "General";
            InGameMiscSettings miscSettings = SettingsManager.InGameCurrent.Misc;
            InGameCharacterSettings charSettings = SettingsManager.InGameCharacterSettings;
            charSettings.CharacterType.Value = PlayerCharacter.Titan;
            ElementStyle dropdownStyle = new ElementStyle(titleWidth: 200f, themePanel: ThemePanel);
            List<string> loadouts = new List<string>() { "Small", "Medium", "Large" };
            if (!loadouts.Contains(charSettings.Loadout.Value))
                charSettings.Loadout.Value = loadouts[loadouts.Count - 1];
            string[] options = GetCharOptions();
            if (charSettings.CustomSet.Value >= options.Length)
                charSettings.CustomSet.Value = 0;
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, dropdownStyle, charSettings.CustomSet, UIManager.GetLocale(cat, sub, "Character"),
                options, elementWidth: 180f, optionsWidth: 180f);
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, dropdownStyle, charSettings.Loadout, UIManager.GetLocale(cat, sub, "Size"),
                loadouts.ToArray(), elementWidth: 180f, optionsWidth: 180f);
            if (miscSettings.PVP.Value == (int)PVPMode.Team)
            {
                ElementFactory.CreateDropdownSetting(DoublePanelRight, dropdownStyle, charSettings.Team, UIManager.GetLocaleCommon("Team"),
               new string[] { "Blue", "Red" }, elementWidth: 180f, optionsWidth: 180f);
            }
        }

        protected string[] GetCharOptions()
        {
            List<string> sets = new List<string>(SettingsManager.TitanCustomSettings.TitanCustomSets.GetSetNames());
            sets.Insert(0, "Random");
            return sets.ToArray();
        }
    }
}
