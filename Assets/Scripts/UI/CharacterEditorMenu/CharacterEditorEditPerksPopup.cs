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
using Unity.VisualScripting;
using GameProgress;
using Utility;
using static UnityEngine.Rendering.DebugUI.MessageBox;

namespace UI
{
    class CharacterEditorEditPerksPopup: BasePopup
    {
        protected override string Title => UIManager.GetLocale("CharacterEditor", "Perks", "Title");
        protected override float Width => 580f;
        protected override float Height => 590f;
        protected override float VerticalSpacing => 25f;
        protected override int HorizontalPadding => 60;
        protected override int VerticalPadding => 30;
        private Text _pointsLeftLabel;
        private Dictionary<string, GameObject> _perkButtons = new Dictionary<string, GameObject>();
        private Dictionary<string, string> _perkDescriptions = new Dictionary<string, string>();

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(titleWidth: 130f, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Save"), onClick: () => OnButtonClick("Save"));
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Back"), onClick: () => OnButtonClick("Back"));
            HumanCustomSettings settings = SettingsManager.HumanCustomSettings;
            HumanCustomSet set = (HumanCustomSet)settings.CustomSets.GetSelectedSet();
            var stats = HumanStats.Deserialize(new HumanStats(null), set.Stats.Value);
            _pointsLeftLabel = ElementFactory.CreateDefaultLabel(SinglePanel, style, "Points Left").GetComponent<Text>();
            var group = ElementFactory.CreateHorizontalGroup(SinglePanel, 20f, TextAnchor.MiddleLeft).transform;
            CreatePerkButton(group, style, "RefillTime", "Reduces gas refill time.");
            group = ElementFactory.CreateHorizontalGroup(SinglePanel, 20f, TextAnchor.MiddleLeft).transform;
            CreatePerkButton(group, style, "DurableBlades", "Have fewer but more durable blades.");
            CreateArrow(group, style);
            CreatePerkButton(group, style, "AdvancedAlloy", "Blades consume no durability, but break immediately on low speed hits.");
            group = ElementFactory.CreateHorizontalGroup(SinglePanel, 20f, TextAnchor.MiddleLeft).transform;
            CreatePerkButton(group, style, "VerticalDash", "Allow air-dash up or down depending on camera.");
            CreateArrow(group, style);
            CreatePerkButton(group, style, "OmniDash", "Allow air-dash in any direction depending on camera.");
            OnPerkChanged("");
        }

        protected void CreatePerkButton(Transform group, ElementStyle style, string name, string desc)
        {
            float buttonWidth = 195f;
            float buttonHeight = 95;
            float offset = 120f;
            _perkDescriptions.Add(name, desc);
            _perkButtons.Add(name, ElementFactory.CreatePerkButton(group, style, name, desc,
                elementWidth: buttonWidth, elementHeight: buttonHeight, offset: offset, onClick: () => OnPerkChanged(name)));
        }

        protected void CreateArrow(Transform group, ElementStyle style)
        {
            ElementFactory.CreateRawImage(group, style, "Icons/Navigation/ArrowRightIcon", elementWidth: 32, elementHeight: 32);
        }

        protected void OnButtonClick(string button)
        {
            if (button == "Back")
                Hide();
            else if (button == "Save")
            {
                HumanCustomSettings settings = SettingsManager.HumanCustomSettings;
                HumanCustomSet set = (HumanCustomSet)settings.CustomSets.GetSelectedSet();
                var stats = HumanStats.Deserialize(new HumanStats(null), set.Stats.Value);
                set.Stats.Value = stats.Serialize();
                ((CharacterEditorMenu)UIManager.CurrentMenu).RebuildPanels(false);
                Hide();
            }
        }

        protected void OnPerkChanged(string perkName)
        {
            HumanCustomSettings settings = SettingsManager.HumanCustomSettings;
            HumanCustomSet set = (HumanCustomSet)settings.CustomSets.GetSelectedSet();
            var stats = HumanStats.Deserialize(new HumanStats(null), set.Stats.Value);
            int maxPoints = HumanStats.MaxPerkPoints;
            int currentTotal = stats.GetPerkPoints();
            if (stats.Perks.ContainsKey(perkName))
            {
                var perk = stats.Perks[perkName];
                if (perk.CurrPoints >= perk.MaxPoints || currentTotal >= maxPoints)
                    perk.CurrPoints = 0;
                else if (perk.HasRequirements(stats.Perks) && currentTotal < maxPoints)
                    perk.CurrPoints += 1;
            }
            foreach (string key in _perkButtons.Keys)
            {
                var perk = stats.Perks[key];
                if (!perk.HasRequirements(stats.Perks))
                    perk.CurrPoints = 0;
                string text = key;
                text = Util.PascalToSentence(key);
                text = "<b>" + text + "</b>\n" + "(" + perk.CurrPoints.ToString() + "/" + perk.MaxPoints.ToString() + ")";
                _perkButtons[key].transform.Find("Text").GetComponent<Text>().text = text;
            }
            currentTotal = stats.GetPerkPoints();
            _pointsLeftLabel.text = "<b>Points left: " + Math.Max(0, maxPoints - currentTotal).ToString() + "</b>";
            set.Stats.Value = stats.Serialize();
        }
    }
}
