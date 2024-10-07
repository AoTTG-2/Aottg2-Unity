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
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, dropdownStyle, charSettings.Loadout, UIManager.GetLocale(cat, sub, "Character"),
                loadouts.ToArray(), elementWidth: 180f, optionsWidth: 180f);
            if (miscSettings.PVP.Value == (int)PVPMode.Team)
            {
                ElementFactory.CreateDropdownSetting(DoublePanelRight, dropdownStyle, charSettings.Team, UIManager.GetLocaleCommon("Team"),
               new string[] { "Blue", "Red" }, elementWidth: 180f, optionsWidth: 180f);
            }
        }
    }
}
