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

namespace UI
{
    class CharacterEditorEditStatsPopup: BasePopup
    {
        protected override string Title => UIManager.GetLocale("CharacterEditor", "Stats", "Title");
        protected override float Width => 325f;
        protected override float Height => 425f;
        protected override float VerticalSpacing => 20f;
        protected override int HorizontalPadding => 25;
        protected override int VerticalPadding => 25;
        protected IntSetting Speed = new IntSetting(80, 50, 100);
        protected IntSetting Acceleration = new IntSetting(80, 50, 100);
        protected IntSetting Ammunition = new IntSetting(80, 50, 100);
        protected IntSetting Gas = new IntSetting(80, 50, 100);

        private Text _pointsLeftLabel;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(titleWidth: 130f, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Save"), onClick: () => OnButtonClick("Save"));
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Back"), onClick: () => OnButtonClick("Back"));
            HumanCustomSettings settings = SettingsManager.HumanCustomSettings;
            string cat = "CharacterEditor";
            string sub = "Stats";
            HumanCustomSet set = (HumanCustomSet)settings.CustomSets.GetSelectedSet();
            var stats = HumanStats.Deserialize(new HumanStats(null), set.Stats.Value);
            _pointsLeftLabel = ElementFactory.CreateDefaultLabel(SinglePanel, style, "Points Left").GetComponent<Text>();
            Speed.Value = stats.Speed;
            Acceleration.Value = stats.Acceleration;
            Gas.Value = stats.Gas;
            Ammunition.Value = stats.Ammunition;
            ElementFactory.CreateIncrementSetting(SinglePanel, style, Acceleration, UIManager.GetLocale(cat, sub, "Acceleration"),
               onValueChanged: () => OnStatChanged(Acceleration));
            ElementFactory.CreateIncrementSetting(SinglePanel, style, Speed, UIManager.GetLocale(cat, sub, "Speed"), 
                onValueChanged: () => OnStatChanged(Speed));
            ElementFactory.CreateIncrementSetting(SinglePanel, style, Gas, UIManager.GetLocale(cat, sub, "Gas"), 
                onValueChanged: () => OnStatChanged(Gas));
            ElementFactory.CreateIncrementSetting(SinglePanel, style, Ammunition, UIManager.GetLocale(cat, sub, "Ammunition"),
               onValueChanged: () => OnStatChanged(Ammunition));
            OnStatChanged(Speed);
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
                stats.Speed = Speed.Value;
                stats.Acceleration = Acceleration.Value;
                stats.Gas = Gas.Value;
                stats.Ammunition = Ammunition.Value;
                set.Stats.Value = stats.Serialize();
                SettingsManager.HumanCustomSettings.Save();
                ((CharacterEditorMenu)UIManager.CurrentMenu).RebuildPanels(false);
            }
        }

        protected void OnStatChanged(IntSetting setting)
        {
            int maxPoints = 320;
            int currentTotal = Speed.Value + Gas.Value + Ammunition.Value + Acceleration.Value;
            if (currentTotal > maxPoints)
            {
                int diff = currentTotal - maxPoints;
                setting.Value -= diff;
                if (setting.Value < 0)
                {
                    Speed.SetDefault();
                    Gas.SetDefault();
                    Ammunition.SetDefault();
                    Acceleration.SetDefault();
                }
                SyncSettingElements();
            }
            currentTotal = Speed.Value + Gas.Value + Ammunition.Value + Acceleration.Value;
            _pointsLeftLabel.text = "Points left: " + Math.Max(0, maxPoints - currentTotal).ToString();
        }
    }
}
