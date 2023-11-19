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
    class CharacterEditorStatsPanel: HeadedPanel
    {
        protected override string Title => UIManager.GetLocale("CharacterEditor", "Stats", "Title");
        protected override float Width => 330f;
        protected override float Height => 410f;
        protected override float VerticalSpacing => 20f;
        protected override int HorizontalPadding => 25;
        protected override int VerticalPadding => 25;
        private Text _pointsLeftLabel;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(titleWidth: 130f, themePanel: ThemePanel);
            HumanCustomSettings settings = SettingsManager.HumanCustomSettings;
            string cat = "CharacterEditor";
            string sub = "Stats";
            HumanCustomSet set = (HumanCustomSet)settings.CustomSets.GetSelectedSet();
            _pointsLeftLabel = ElementFactory.CreateDefaultLabel(SinglePanel, style, "Points Left").GetComponent<Text>();
            ElementFactory.CreateIncrementSetting(SinglePanel, style, set.Speed, UIManager.GetLocale(cat, sub, "Speed"), 
                onValueChanged: () => OnStatChanged(set.Speed));
            ElementFactory.CreateIncrementSetting(SinglePanel, style, set.Gas, UIManager.GetLocale(cat, sub, "Gas"), 
                onValueChanged: () => OnStatChanged(set.Gas));
            ElementFactory.CreateIncrementSetting(SinglePanel, style, set.Blade, UIManager.GetLocale(cat, sub, "Blade"),
               onValueChanged: () => OnStatChanged(set.Blade));
            ElementFactory.CreateIncrementSetting(SinglePanel, style, set.Acceleration, UIManager.GetLocale(cat, sub, "Acceleration"),
               onValueChanged: () => OnStatChanged(set.Acceleration));
            OnStatChanged(set.Speed);
        }

        protected void OnStatChanged(IntSetting setting)
        {
            int maxPoints = 450;
            HumanCustomSettings settings = SettingsManager.HumanCustomSettings;
            HumanCustomSet set = (HumanCustomSet)settings.CustomSets.GetSelectedSet();
            int currentTotal = set.Speed.Value + set.Gas.Value + set.Blade.Value + set.Acceleration.Value;
            if (currentTotal > maxPoints)
            {
                int diff = currentTotal - maxPoints;
                setting.Value -= diff;
                if (setting.Value < 0)
                {
                    set.Speed.SetDefault();
                    set.Gas.SetDefault();
                    set.Blade.SetDefault();
                    set.Acceleration.SetDefault();
                }
                SyncSettingElements();
            }
            currentTotal = set.Speed.Value + set.Gas.Value + set.Blade.Value + set.Acceleration.Value;
            _pointsLeftLabel.text = "Points left: " + Math.Max(0, maxPoints - currentTotal).ToString();
        }
    }
}
