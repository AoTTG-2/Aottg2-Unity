using GameManagers;
using Map;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Utility;
using ApplicationManagers;
using System.IO;

namespace UI
{
    class CharacterTitanPanel : CharacterCategoryPanel
    {
        private readonly string LocaleCategory = "CharacterPopup";

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            string sub = "General";
            InGameMiscSettings miscSettings = SettingsManager.InGameCurrent.Misc;
            InGameCharacterSettings charSettings = SettingsManager.InGameCharacterSettings;
            InGameCharacterSettings lastCharSettings = SettingsManager.InGameSettings.LastCharacter;
            charSettings.CharacterType.Value = PlayerCharacter.Titan;
            ElementStyle dropdownStyle = new ElementStyle(titleWidth: 200f, themePanel: ThemePanel);
            List<string> loadouts = new List<string>() { "Small", "Medium", "Large" };
            if (lastCharSettings.CharacterType.Value == PlayerCharacter.Titan)
            {
                charSettings.CustomSet.Value = lastCharSettings.CustomSet.Value;
                charSettings.Loadout.Value = lastCharSettings.Loadout.Value;
            }
            if (!loadouts.Contains(charSettings.Loadout.Value))
                charSettings.Loadout.Value = loadouts[loadouts.Count - 1];
            string[] options = GetCharOptions();
            if (charSettings.CustomSet.Value >= options.Length)
                charSettings.CustomSet.Value = 0;
            ElementFactory.CreateIconPickSetting(DoublePanelLeft, dropdownStyle, charSettings.CustomSet, UIManager.GetLocale(LocaleCategory, sub, "Character"),
                options, GetCharIcons(options), UIManager.CurrentMenu.IconPickPopup, elementWidth: 180f, elementHeight: 40f, onSelect: () => { });
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, dropdownStyle, charSettings.Loadout, UIManager.GetLocale(LocaleCategory, sub, "Size"),
                loadouts.ToArray(), elementWidth: 180f, optionsWidth: 180f, onDropdownOptionSelect: () => OnLoadoutClick());
            if (miscSettings.PVP.Value == (int)PVPMode.Team)
            {
                ElementFactory.CreateDropdownSetting(DoublePanelRight, new ElementStyle(titleWidth: 100f, themePanel: ThemePanel), charSettings.Team, UIManager.GetLocaleCommon("Team"),
               new string[] { "Blue", "Red" }, elementWidth: 180f, optionsWidth: 180f);
            }
        }

        protected void OnLoadoutClick()
        {
            InGameCharacterSettings charSettings = SettingsManager.InGameCharacterSettings;
            InGameCharacterSettings lastCharSettings = SettingsManager.InGameSettings.LastCharacter;
            lastCharSettings.CustomSet.Value = charSettings.CustomSet.Value;
            lastCharSettings.Loadout.Value = charSettings.Loadout.Value;
        }


        protected string[] GetCharOptions()
        {
            List<string> sets = new List<string>(SettingsManager.TitanCustomSettings.TitanCustomSets.GetSetNames());
            sets.Insert(0, "Random");
            return sets.ToArray();
        }

        protected string[] GetCharIcons(string[] options)
        {
            List<string> icons = new List<string>();
            var titanSets = SettingsManager.TitanCustomSettings.TitanCustomSets.GetSets().GetItems();
            foreach (string option in options)
            {
                if (option == "Random")
                {
                    icons.Add(ResourcePaths.UI + "/Icons/Navigation/TooltipIcon");
                }
                else
                {
                    string uniqueId = null;
                    foreach (var baseSetting in titanSets)
                    {
                        var set = (Settings.TitanCustomSet)baseSetting;
                        if (set.Name.Value == option)
                        {
                            uniqueId = set.UniqueId.Value;
                            break;
                        }
                    }
                    
                    if (uniqueId != null)
                    {
                        string cacheKey = "CharacterPreview_Titans_" + uniqueId;
                        Texture2D texture = ResourceManager.GetExternalTexture(cacheKey);
                        if (texture == null)
                        {
                            string customPreviewPath = Path.Combine(FolderPaths.CharacterPreviews, "Titans", "Preset" + uniqueId + ".png");
                            texture = ResourceManager.LoadExternalTexture(customPreviewPath, cacheKey, persistent: true);
                        }
                        if (texture != null)
                        {
                            icons.Add(cacheKey);
                        }
                        else
                        {
                            icons.Add(ResourcePaths.Characters + "/Human/Previews/PresetNone");
                        }
                    }
                    else
                    {
                        icons.Add(ResourcePaths.Characters + "/Human/Previews/PresetNone");
                    }
                }
            }
            return icons.ToArray();
        }
    }
}
