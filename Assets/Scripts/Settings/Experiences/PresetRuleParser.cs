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