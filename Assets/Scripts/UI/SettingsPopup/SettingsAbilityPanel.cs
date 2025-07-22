using Settings;
using System;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
    class SettingsAbilityPanel: SettingsCategoryPanel
    {
        protected override TextAnchor PanelAlignment => TextAnchor.UpperCenter;
        protected Text _pointsLeftLabel;

        protected GameObject _radiusElement;
        protected GameObject _rangeElement;
        protected GameObject _speedElement;
        protected GameObject _cooldownElement;
        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            

            
            SettingsPopup settingsPopup = (SettingsPopup)parent;
            string cat = settingsPopup.LocaleCategory;
            string sub = "Ability";
            AbilitySettings settings = SettingsManager.AbilitySettings;
            ElementStyle style = new ElementStyle(titleWidth: 200f, themePanel: ThemePanel);
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.CursorCooldown, UIManager.GetLocale(cat, sub, "CursorCooldown"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.ShowBombColors, UIManager.GetLocale(cat, sub, "ShowBombColors"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.UseOldEffect, UIManager.GetLocale(cat, sub, "UseOldEffect"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.BombCollision, UIManager.GetLocale(cat, sub, "BombCollision")); 


            _pointsLeftLabel = ElementFactory.CreateDefaultLabel(DoublePanelLeft, style, UIManager.GetLocale(cat, sub, "UnusedPoints")).GetComponent<Text>();
            // Validation function for bomb stats - ensures total points don't exceed 20
            Func<bool> bombValidation = () => {
                var a = SettingsManager.AbilitySettings;
                return a.BombRadius.Value + a.BombRange.Value + a.BombSpeed.Value + a.BombCooldown.Value <= 20;
            };
            
            _radiusElement = ElementFactory.CreateIncrementSetting(DoublePanelLeft, style, settings.BombRadius, GetBombStatLabel(cat, sub, "BombRadius", settings.BombRadius.Value, 5.40f, 7.4f, 7f, "m"), onValueChanged: () => OnStatChanged(settings.BombRadius), validation: bombValidation);
            _rangeElement = ElementFactory.CreateIncrementSetting(DoublePanelLeft, style, settings.BombRange, GetBombStatLabel(cat, sub, "BombRange", settings.BombRange.Value, 0f, 4f, 7f, "m"), onValueChanged: () => OnStatChanged(settings.BombRange), validation: bombValidation);
            _speedElement = ElementFactory.CreateIncrementSetting(DoublePanelLeft, style, settings.BombSpeed, GetBombStatLabel(cat, sub, "BombSpeed", settings.BombSpeed.Value, 3f, 10.5f, 10.5f, "k", 100f), onValueChanged: () => OnStatChanged(settings.BombSpeed), validation: bombValidation);
            _cooldownElement = ElementFactory.CreateIncrementSetting(DoublePanelLeft, style, settings.BombCooldown, GetBombStatLabel(cat, sub, "BombCooldown", settings.BombCooldown.Value, 4f, 7f, 7f, "s"), onValueChanged: () => OnStatChanged(settings.BombCooldown), validation: bombValidation);
            ElementFactory.CreateColorSetting(DoublePanelLeft, style, settings.BombColor, UIManager.GetLocale(cat, sub, "BombColor"), UIManager.CurrentMenu.ColorPickPopup);
            

            
            OnStatChanged(settings.BombRadius);
        }

        protected string GetBombStatLabel(string cat, string sub, string statName, int pointsSpent, float oldMinCost, float oldMaxCost, float cutoff, string unit, float divisor = 1f)
        {
            float actualValue;
            float oldCost;
            
            switch (statName)
            {
                case "BombRadius":
                    actualValue = BombUtil.GetBombRadius(pointsSpent, oldMinCost, oldMaxCost, cutoff);
                    oldCost = BombUtil.GetOldRadiusCost(actualValue);
                    break;
                case "BombRange":
                    actualValue = BombUtil.GetBombRange(pointsSpent, oldMinCost, oldMaxCost, cutoff);
                    oldCost = BombUtil.GetOldRangeCost(actualValue);
                    break;
                case "BombSpeed":
                    actualValue = BombUtil.GetBombSpeed(pointsSpent, oldMinCost, oldMaxCost, cutoff);
                    oldCost = BombUtil.GetOldSpeedCost(actualValue);
                    break;
                case "BombCooldown":
                    actualValue = BombUtil.GetBombCooldown(pointsSpent, oldMinCost, oldMaxCost, cutoff);
                    oldCost = BombUtil.GetOldCooldownCost(actualValue);
                    break;
                default:
                    actualValue = 0f;
                    oldCost = 0f;
                    break;
            }
            
            string actualValueStr = (actualValue / divisor).ToString("0.##");
            string oldCostStr = oldCost.ToString("0.##");

            // Format: statName\n(centered combined parentheses)
            string statNameStr = UIManager.GetLocale(cat, sub, statName);
            string combinedValues = $"({oldCostStr}) ({actualValueStr}{unit})";
            
            // Center the combined parentheses line under the stat name
            string centeredValues = CenterString(combinedValues, Math.Max(statNameStr.Length, combinedValues.Length));
            
            return statNameStr + "\n" + centeredValues;
        }

        private string CenterString(string text, int totalWidth)
        {
            if (text.Length >= totalWidth) return text;
            int padding = totalWidth - text.Length;
            int leftPad = padding / 2;
            int rightPad = padding - leftPad;
            return new string(' ', leftPad) + text + new string(' ', rightPad);
        }

        protected void OnStatChanged(IntSetting setting)
        {
            RefreshBombStatLabels();
        }
        
        protected void RefreshBombStatLabels()
        {
            AbilitySettings settings = SettingsManager.AbilitySettings;
            string cat = "SettingsPopup";
            string sub = "Ability";
            int maxPoints = 20;
            int used = settings.BombRadius.Value + settings.BombRange.Value + settings.BombSpeed.Value + settings.BombCooldown.Value;
            int unused = Math.Max(0, maxPoints - used);
            _pointsLeftLabel.text = UIManager.GetLocale(cat, sub, "UnusedPoints") + ": " + unused;
            // Update each bomb stat label with new calculated values
            UpdateElementLabel(_radiusElement, GetBombStatLabel(cat, sub, "BombRadius", settings.BombRadius.Value, 5.40f, 7.4f, 7f, "m"));
            UpdateElementLabel(_rangeElement, GetBombStatLabel(cat, sub, "BombRange", settings.BombRange.Value, 0f, 4f, 7f, "m"));
            UpdateElementLabel(_speedElement, GetBombStatLabel(cat, sub, "BombSpeed", settings.BombSpeed.Value, 3f, 10.5f, 10.5f, "k", 100f));
            UpdateElementLabel(_cooldownElement, GetBombStatLabel(cat, sub, "BombCooldown", settings.BombCooldown.Value, 4f, 7f, 7f, "s"));
        }
        
        protected void UpdateElementLabel(GameObject element, string newText)
        {
            if (element != null)
            {
                Text labelText = element.GetComponentInChildren<Text>();
                if (labelText != null)
                {
                    labelText.text = newText;
                }
            }
        }
    }
}
