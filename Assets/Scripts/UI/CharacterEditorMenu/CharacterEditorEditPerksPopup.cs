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
        protected override float Width => 550f;
        protected override float Height => 745f;
        protected override float VerticalSpacing => 25f;
        protected override int HorizontalPadding => 60;
        protected override int VerticalPadding => 30;
        private Text _pointsLeftLabel;
        private Dictionary<string, GameObject> _perkButtons = new Dictionary<string, GameObject>();

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
            CreatePerkButton(group, style, "RefillTime");
            group = ElementFactory.CreateHorizontalGroup(SinglePanel, 20f, TextAnchor.MiddleLeft).transform;
            CreatePerkButton(group, style, "FlareCD");
            CreateArrow(group, style);
            CreatePerkButton(group, style, "FlareSize");
            group = ElementFactory.CreateHorizontalGroup(SinglePanel, 20f, TextAnchor.MiddleLeft).transform;
            CreatePerkButton(group, style, "DurableBlades");
            CreateArrow(group, style);
            CreatePerkButton(group, style, "AdvancedAlloy");
            group = ElementFactory.CreateHorizontalGroup(SinglePanel, 20f, TextAnchor.MiddleLeft).transform;
            CreatePerkButton(group, style, "VerticalDash");
            CreateArrow(group, style);
            CreatePerkButton(group, style, "OmniDash");
            group = ElementFactory.CreateHorizontalGroup(SinglePanel, 20f, TextAnchor.MiddleLeft).transform;
            CreatePerkButton(group, style, "HookSpeed");
            CreateArrow(group, style);
            CreatePerkButton(group, style, "HookLength");
            OnPerkChanged("");
        }

        protected void CreatePerkButton(Transform group, ElementStyle style, string name)
        {
            float buttonWidth = 180f;
            float buttonHeight = 80f;
            _perkButtons.Add(name, ElementFactory.CreateDefaultButton(group, style, name, 
                elementWidth: buttonWidth, elementHeight: buttonHeight, onClick: () => OnPerkChanged(name)));
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
            if (stats.Perks.ContainsKey(perkName))
            {
                var perk = stats.Perks[perkName];
                if (perk.CurrPoints >= perk.MaxPoints)
                    perk.CurrPoints = 0;
                else if (perk.HasRequirements(stats.Perks))
                    perk.CurrPoints += 1;
            }
            foreach (string key in _perkButtons.Keys)
            {
                var perk = stats.Perks[key];
                if (!perk.HasRequirements(stats.Perks))
                    perk.CurrPoints = 0;
                string text = key;
                if (text == "FlareCD")
                    text = "Flare CD";
                else
                    text = Util.PascalToSentence(key);
                _perkButtons[key].transform.Find("Text").GetComponent<Text>().text = text +
                    " (" + perk.CurrPoints.ToString() + "/" + perk.MaxPoints.ToString() + ")";
            }
            int maxPoints = GameProgressManager.GameProgress.GameStat.Level.Value;
            int currentTotal = stats.GetPerkPoints();
            _pointsLeftLabel.text = "Points left: " + Math.Max(0, maxPoints - currentTotal).ToString();
            set.Stats.Value = stats.Serialize();
        }
    }
}
