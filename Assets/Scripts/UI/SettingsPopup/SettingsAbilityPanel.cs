using Settings;
using System;
using UnityEngine.UI;

namespace UI
{
    class SettingsAbilityPanel: SettingsCategoryPanel
    {
        protected Text _pointsLeftLabel;
        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            SettingsPopup settingsPopup = (SettingsPopup)parent;
            string cat = settingsPopup.LocaleCategory;
            string sub = "Ability";
            AbilitySettings settings = SettingsManager.AbilitySettings;
            ElementStyle style = new ElementStyle(titleWidth: 200f, themePanel: ThemePanel);
            ElementFactory.CreateColorSetting(DoublePanelRight, style, settings.BombColor, UIManager.GetLocale(cat, sub, "BombColor"), UIManager.CurrentMenu.ColorPickPopup);
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.ShowBombColors, UIManager.GetLocale(cat, sub, "ShowBombColors"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.UseOldEffect, UIManager.GetLocale(cat, sub, "UseOldEffect"));
            _pointsLeftLabel = ElementFactory.CreateDefaultLabel(DoublePanelLeft, style, UIManager.GetLocale(cat, sub, "PointsLeft")).GetComponent<Text>();
            ElementFactory.CreateIncrementSetting(DoublePanelLeft, style, settings.BombRadius, UIManager.GetLocale(cat, sub, "BombRadius") + " (0-10)", onValueChanged: () => OnStatChanged(settings.BombRadius));
            ElementFactory.CreateIncrementSetting(DoublePanelLeft, style, settings.BombRange, UIManager.GetLocale(cat, sub, "BombRange") + " (0-3)", onValueChanged: () => OnStatChanged(settings.BombRange));
            ElementFactory.CreateIncrementSetting(DoublePanelLeft, style, settings.BombSpeed, UIManager.GetLocale(cat, sub, "BombSpeed") + " (0-10)", onValueChanged: () => OnStatChanged(settings.BombSpeed));
            ElementFactory.CreateIncrementSetting(DoublePanelLeft, style, settings.BombCooldown, UIManager.GetLocale(cat, sub, "BombCooldown") + " (0-6)", onValueChanged: () => OnStatChanged(settings.BombCooldown));
            OnStatChanged(settings.BombRadius);
        }

        protected void OnStatChanged(IntSetting setting)
        {
            int maxPoints = 16;
            AbilitySettings settings = SettingsManager.AbilitySettings;
            int currentTotal = settings.BombRadius.Value + settings.BombRange.Value + settings.BombSpeed.Value + settings.BombCooldown.Value;
            if (currentTotal > maxPoints)
            {
                int diff = currentTotal - maxPoints;
                setting.Value -= diff;
                if (setting.Value < 0)
                {
                    settings.BombRadius.SetDefault();
                    settings.BombRange.SetDefault();
                    settings.BombSpeed.SetDefault();
                    settings.BombCooldown.SetDefault();
                }
                SyncSettingElements();
            }
            currentTotal = settings.BombRadius.Value + settings.BombRange.Value + settings.BombSpeed.Value + settings.BombCooldown.Value;
            _pointsLeftLabel.text = UIManager.GetLocale("SettingsPopup", "Ability", "PointsLeft") + ": " + Math.Max(0, maxPoints - currentTotal).ToString();
        }
    }
}
