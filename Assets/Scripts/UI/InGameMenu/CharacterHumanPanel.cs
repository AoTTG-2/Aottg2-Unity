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
        protected List<GameObject> _statBars = new List<GameObject>();

        private readonly string LocaleCategory = "CharacterPopup";

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            string sub = "General";
            InGameMiscSettings miscSettings = SettingsManager.InGameCurrent.Misc;
            InGameCharacterSettings charSettings = SettingsManager.InGameCharacterSettings;
            InGameCharacterSettings lastCharSettings = SettingsManager.InGameSettings.LastCharacter;
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
            if (lastCharSettings.CharacterType.Value == PlayerCharacter.Human)
            {
                charSettings.Special.Value = lastCharSettings.Special.Value;
                charSettings.Costume.Value = lastCharSettings.Costume.Value;
                charSettings.CustomSet.Value = lastCharSettings.CustomSet.Value;
                charSettings.Loadout.Value = lastCharSettings.Loadout.Value;
            }
            if (!loadouts.Contains(charSettings.Loadout.Value))
                charSettings.Loadout.Value = loadouts[0];
            var specials = HumanSpecials.GetSpecialNames(charSettings.Loadout.Value, miscSettings.AllowShifterSpecials.Value);
            if (!specials.Contains(charSettings.Special.Value))
                charSettings.Special.Value = HumanSpecials.DefaultSpecial;
            string[] options = GetCharOptions();
            if (charSettings.CustomSet.Value >= options.Length)
            {
                charSettings.CustomSet.Value = 0;
            }
            ElementFactory.CreateIconPickSetting(DoublePanelLeft, dropdownStyle, charSettings.CustomSet, UIManager.GetLocale(LocaleCategory, sub, "Character"),
                options, GetCharIcons(options), UIManager.CurrentMenu.IconPickPopup, elementWidth: 180f, elementHeight: 40f, onSelect: () => SyncStatBars());
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, dropdownStyle, charSettings.Costume, UIManager.GetLocale(LocaleCategory, sub, "Costume"),
                new string[] {"Costume1", "Costume2", "Costume3"}, elementWidth: 180f, optionsWidth: 180f);
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, dropdownStyle, charSettings.Loadout, UIManager.GetLocale(LocaleCategory, sub, "Loadout"),
                loadouts.ToArray(), elementWidth: 180f, optionsWidth: 180f, onDropdownOptionSelect: () => OnLoadoutClick());
            options = specials.ToArray();
            var skillPopup = ElementFactory.CreateTooltipPopup<SkillTooltipPopup>(UIManager.CurrentMenu.IconPickPopup.transform).GetComponent<TooltipPopup>();
            ElementFactory.CreateIconPickSetting(DoublePanelLeft, dropdownStyle, charSettings.Special, UIManager.GetLocale(LocaleCategory, sub, "Special"),
                options, GetSpecialIcons(options), UIManager.CurrentMenu.IconPickPopup, tooltips: GetSpecialTooltips(options), elementWidth: 180f, elementHeight: 40f, tooltipPopup: skillPopup);
            if (miscSettings.PVP.Value == (int)PVPMode.Team)
            {
                ElementFactory.CreateDropdownSetting(DoublePanelRight, new ElementStyle(titleWidth: 100f, themePanel: ThemePanel), charSettings.Team, UIManager.GetLocaleCommon("Team"),
               new string[] { "Blue", "Red" }, elementWidth: 180f, optionsWidth: 180f);
            }
            SyncStatBars();
        }

        protected void OnLoadoutClick()
        {
            InGameCharacterSettings charSettings = SettingsManager.InGameCharacterSettings;
            InGameCharacterSettings lastCharSettings = SettingsManager.InGameSettings.LastCharacter;
            lastCharSettings.Special.Value = charSettings.Special.Value;
            lastCharSettings.Costume.Value = charSettings.Costume.Value;
            lastCharSettings.CustomSet.Value = charSettings.CustomSet.Value;
            lastCharSettings.Loadout.Value = charSettings.Loadout.Value;
            Parent.RebuildCategoryPanel();
        }

        protected void SyncStatBars()
        {
            foreach (var go in _statBars)
                Destroy(go);
            _statBars.Clear();
            InGameCharacterSettings charSettings = SettingsManager.InGameCharacterSettings;
            var customSets = SettingsManager.HumanCustomSettings.CustomSets;
            int setIndex = charSettings.CustomSet.Value;
            int costumeIndex = charSettings.Costume.Value;
            int preCount = SettingsManager.HumanCustomSettings.Costume1Sets.Sets.GetCount();
            HumanCustomSet customSet;
            if (setIndex < preCount)
            {
                if (costumeIndex == 1)
                    customSet = (HumanCustomSet)SettingsManager.HumanCustomSettings.Costume2Sets.Sets.GetItemAt(setIndex);
                else if (costumeIndex == 2)
                    customSet = (HumanCustomSet)SettingsManager.HumanCustomSettings.Costume3Sets.Sets.GetItemAt(setIndex);
                else
                    customSet = (HumanCustomSet)SettingsManager.HumanCustomSettings.Costume1Sets.Sets.GetItemAt(setIndex);
            }
            else
                customSet = (HumanCustomSet)customSets.Sets.GetItemAt(setIndex - preCount);
            var stats = HumanStats.Deserialize(new HumanStats(null), customSet.Stats.Value);
            string cat = "CharacterEditor";
            string sub = "Stats";
            CreateStatBar(UIManager.GetLocale(cat, sub, "Acceleration"), stats.Acceleration);
            CreateStatBar(UIManager.GetLocale(cat, sub, "Speed"), stats.Speed);
            CreateStatBar(UIManager.GetLocale(cat, sub, "Gas"), stats.Gas);
            CreateStatBar(UIManager.GetLocale(cat, sub, "Ammunition"), stats.Ammunition);
        }

        protected void CreateStatBar(string title, int value)
        {
            var statbar = ElementFactory.InstantiateAndBind(DoublePanelRight, "Prefabs/Misc/StatBar").transform;
            float percentage = Mathf.Clamp((value - 50f) / 50f, 0f, 1f);
            statbar.Find("Label").GetComponent<Text>().text = title;
            statbar.Find("Label").GetComponent<Text>().color = UIManager.GetThemeColor("DefaultPanel", "DefaultLabel", "TextColor");
            statbar.Find("ProgressBar").GetComponent<Slider>().value = percentage;
            statbar.Find("ProgressBar/Background").GetComponent<Image>().color = UIManager.GetThemeColor("QuestPopup", "QuestItem", "ProgressBarBackgroundColor");
            statbar.Find("ProgressBar/Fill Area/Fill").GetComponent<Image>().color = UIManager.GetThemeColor("QuestPopup", "QuestItem", "ProgressBarFillColor");
            _statBars.Add(statbar.gameObject);
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

        protected string[] GetSpecialTooltips(string[] options) =>
            options.Select(option => UIManager.GetLocale(LocaleCategory, "SpecialTooltip", option)).ToArray();
    }
}
