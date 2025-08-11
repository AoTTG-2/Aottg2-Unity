using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Settings
{
    public enum UIRuleType { Show, Hide, SetDefault, ForceValue, LimitRange, AllowedValues }
    public enum UIRuleValueType { Number, Bool, String }

    [System.Serializable]
    public class UIRule
    {
        public UIRuleType Type;
        // public UIRuleValueType ValueType;
        public string SettingId;
        public object DefaultValue;
        public float? Min;
        public float? Max;
        public List<string> AllowedValues; // for strings and numbers with discrete sets
        public bool DisableEditing;

        public bool MatchesAllowedValues(string value)
        {
            if (string.IsNullOrEmpty(value) || AllowedValues == null || AllowedValues.Count == 0)
                return false;

            foreach (var allowed in AllowedValues)
            {
                // Escape regex characters except '*'
                string pattern = "^" + Regex.Escape(allowed).Replace("\\*", ".*") + "$";

                if (Regex.IsMatch(value, pattern, RegexOptions.IgnoreCase))
                    return true;
            }

            return false;
        }

        public static bool MatchesRule(object value, UIRule rule)
        {
            if (value == null || rule == null) return false;

            switch (rule.Type)
            {
                case UIRuleType.SetDefault:
                    return MatchesDefault(value, rule.DefaultValue);

                case UIRuleType.LimitRange:
                    return MatchesRange(value, rule.Min, rule.Max);

                case UIRuleType.AllowedValues:
                    return MatchesAllowedValues(value, rule.AllowedValues);

                case UIRuleType.Show:
                case UIRuleType.Hide:
                case UIRuleType.ForceValue:
                    // These types are UI behavior-related, not value validation
                    return true;

                default:
                    return false;
            }
        }

        private static bool MatchesDefault(object value, object defaultValue)
        {
            return value?.ToString() == defaultValue?.ToString();
        }

        private static bool MatchesRange(object value, float? min, float? max)
        {
            if (value is IConvertible)
            {
                try
                {
                    float val = Convert.ToSingle(value);
                    if (min.HasValue && val < min.Value) return false;
                    if (max.HasValue && val > max.Value) return false;
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }

        private static bool MatchesAllowedValues(object value, List<string> allowedValues)
        {
            if (allowedValues == null || value == null) return false;

            string valStr = value.ToString();
            return allowedValues.Contains(valStr);
        }
    }

    public static class RuleParser
    {
        public static List<UIRule> Parse(IEnumerable<string> rawRules, Func<string, UIRuleValueType> typeResolver = null)
        {
            var rules = new List<UIRule>();

            foreach (var rule in rawRules)
            {
                var text = rule.Trim();
                if (string.IsNullOrWhiteSpace(text)) continue;

                bool disableEditing = false;
                if (text.StartsWith("!"))
                {
                    disableEditing = true;
                    text = text.Substring(1);
                }

                if (text.StartsWith("-"))
                {
                    rules.Add(new UIRule { Type = UIRuleType.Hide, SettingId = text.Substring(1) });
                    continue;
                }
                if (text.StartsWith("+"))
                {
                    rules.Add(new UIRule { Type = UIRuleType.Show, SettingId = text.Substring(1) });
                    continue;
                }

                // Match pattern: id=value[min-max]{allowed1,allowed2}
                var match = Regex.Match(text, @"([^=]+)=([^\[\{]+)(?:\[([^\]]+)\])?(?:\{([^\}]+)\})?");
                if (!match.Success)
                    continue;

                var id = match.Groups[1].Value.Trim();
                var valueStr = match.Groups[2].Value.Trim();
                var rangeStr = match.Groups[3].Value.Trim();
                var allowedStr = match.Groups[4].Value.Trim();

                // var valueType = typeResolver(id);

                var ruleObj = new UIRule
                {
                    SettingId = id,
                    DefaultValue = valueStr,
                    DisableEditing = disableEditing,
                    // ValueType = valueType
                };

                if (!string.IsNullOrEmpty(rangeStr))
                {
                    var parts = rangeStr.Split('-');
                    if (float.TryParse(parts[0], out var minVal)) ruleObj.Min = minVal;
                    if (float.TryParse(parts[1], out var maxVal)) ruleObj.Max = maxVal;
                    ruleObj.Type = UIRuleType.LimitRange;
                }

                if (!string.IsNullOrEmpty(allowedStr))
                {
                    ruleObj.AllowedValues = allowedStr.Split(',').Select(s => s.Trim()).ToList();
                    ruleObj.Type = UIRuleType.AllowedValues;
                }

                if (ruleObj.Min == null && ruleObj.Max == null && ruleObj.AllowedValues == null)
                    ruleObj.Type = UIRuleType.SetDefault;

                rules.Add(ruleObj);
            }

            return rules;
        }
    }

}