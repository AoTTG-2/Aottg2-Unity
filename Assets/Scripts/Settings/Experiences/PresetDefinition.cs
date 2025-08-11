using System;
using System.Collections.Generic;

namespace Settings
{
    [System.Serializable]
    public class PresetDefinition
    {
        public string Category;
        public string SubCategory;
        public string Name;
        public string Description;
        public string Extends; // Name of base preset

        public List<string> UIRules;

        public List<UIRule> UIRulesObject
        {
            get
            {
                if (_uiRules == null)
                {
                    _uiRules = RuleParser.Parse(UIRules);
                }
                return _uiRules;
            }
            set
            {
                _uiRules = value;
            }
        }

        [NonSerialized]
        private List<UIRule> _uiRules;

        public UIRule MapRule
        {
            get
            {
                if (_mapRule == null)
                {
                    foreach (var rule in UIRulesObject)
                    {
                        if (rule.SettingId == "general.maps")
                            _mapRule = rule;
                    }
                }
                return _mapRule;
            }
        }

        public UIRule ModeRule
        {
            get
            {
                if (_modeRule == null)
                {
                    foreach (var rule in UIRulesObject)
                    {
                        if (rule.SettingId == "general.modes")
                            _modeRule = rule;
                    }
                }
                return _modeRule;
            }
        }

        [NonSerialized]
        private UIRule _mapRule;

        [NonSerialized]
        private UIRule _modeRule;

        private UIRule GetRule(string id)
        {
            // find the rule with general.maps
            foreach (UIRule rule in _uiRules)
                if (rule.SettingId == id)
                    return rule;
            return null;
        }
    }

}