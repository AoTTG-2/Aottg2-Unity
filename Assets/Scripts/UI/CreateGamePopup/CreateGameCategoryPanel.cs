using Map;
using Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class CreateGameCategoryPanel : CategoryPanel
    {
        protected override bool DoublePanel => true;
        protected override bool DoublePanelDivider => true;

        private static readonly string[] DifficultyNames = { "training", "easy", "normal", "hard", "abnormal" };

        protected PresetDefinition GetActivePreset()
        {
            var popup = Parent as CreateGamePopup;
            return popup?.ActivePreset;
        }

        protected UIRule GetRule(string ruleId)
        {
            var preset = GetActivePreset();
            if (preset?.UIRulesObject == null) return null;
            foreach (var rule in preset.UIRulesObject)
            {
                if (rule.SettingId == ruleId)
                    return rule;
            }
            return null;
        }

        protected bool IsSettingAllowed(string ruleId)
        {
            var preset = GetActivePreset();
            if (preset?.UIRulesObject == null) return true;
            foreach (var rule in preset.UIRulesObject)
            {
                if (rule.SettingId == ruleId)
                {
                    if (rule.Type == UIRuleType.Hide) return false;
                    if (rule.DisableEditing) return false;
                }
            }
            return true;
        }

        public static void ApplyPresetDefaults(PresetDefinition preset, InGameSet settings)
        {
            if (preset?.UIRulesObject == null) return;
            foreach (var rule in preset.UIRulesObject)
            {
                if (rule.DefaultValue == null) continue;
                string val = rule.DefaultValue.ToString();
                ApplySettingValue(rule.SettingId, val, settings);
            }
        }

        private static BaseSettingsContainer GetContainer(string category, InGameSet settings)
        {
            switch (category)
            {
                case "general": return settings.General;
                case "misc": return settings.Misc;
                case "titan": return settings.Titan;
                default: return null;
            }
        }

        private static void SetSettingFromString(BaseSetting setting, string val)
        {
            if (setting is BoolSetting boolSetting)
            {
                if (bool.TryParse(val, out bool b)) boolSetting.Value = b;
            }
            else if (setting is IntSetting intSetting)
            {
                if (int.TryParse(val, out int i)) intSetting.Value = i;
            }
            else if (setting is FloatSetting floatSetting)
            {
                if (float.TryParse(val, out float f)) floatSetting.Value = f;
            }
            else if (setting is StringSetting strSetting)
            {
                strSetting.Value = val;
            }
        }

        private static void ApplySettingValue(string settingId, string val, InGameSet settings)
        {
            int dotIdx = settingId.IndexOf('.');
            if (dotIdx < 0) return;

            string category = settingId.Substring(0, dotIdx);
            string fieldName = settingId.Substring(dotIdx + 1);

            // Special cases that need extra logic beyond simple value assignment
            if (settingId == "general.Difficulty")
            {
                int idx = Array.FindIndex(DifficultyNames, d => d.Equals(val, StringComparison.OrdinalIgnoreCase));
                if (idx >= 0) settings.General.Difficulty.Value = idx;
                return;
            }
            if (settingId == "general.MapName")
            {
                settings.General.MapName.Value = val;
                settings.General.MapCategory.Value = BuiltinLevels.FindMapCategory(val);
                return;
            }
            if (settingId == "general.WeatherIndex")
            {
                if (int.TryParse(val, out int wi)) settings.WeatherIndex.Value = wi;
                return;
            }

            // Auto-resolve: category.FieldName → container.Settings[FieldName]
            BaseSettingsContainer container = GetContainer(category, settings);
            if (container == null || !container.Settings.Contains(fieldName)) return;

            SetSettingFromString((BaseSetting)container.Settings[fieldName], val);
        }

        protected void ApplySettingVisibility()
        {
            foreach (BaseSettingElement element in GetComponentsInChildren<BaseSettingElement>(true))
            {
                if (element.Setting != null && element.Setting.IsHidden)
                    element.gameObject.SetActive(false);
            }
        }
    }
}
