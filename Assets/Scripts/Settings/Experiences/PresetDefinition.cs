using System;
using System.Collections.Generic;

namespace Settings
{
    [System.Serializable]
    public class PresetDefinition
    {
        public string Category;
        public string Subcategory;
        public string Name;
        public string Description;
        public string Extends; // Name of base preset

        public List<string> ModesRaw;  // raw rule strings from JSON
        public List<string> MapsRaw;   // raw rule strings from JSON
        public List<string> UIRulesRaw;

        // These are runtime-only, non-serialized
        public List<UIRule> Modes
        {
            get
            {
                if (_modes == null)
                {
                    _modes = RuleParser.Parse(ModesRaw);
                }
                return _modes;
            }
            set
            {
                _modes = value;
            }
        }

        public List<UIRule> Maps
        {
            get
            {
                if (_maps == null)
                {
                    _maps = RuleParser.Parse(MapsRaw);
                }
                return _maps;
            }
            set
            {
                _maps = value;
            }
        }

        public List<UIRule> UIRules
        {
            get
            {
                if (_uiRules == null)
                {
                    _uiRules = RuleParser.Parse(UIRulesRaw);
                }
                return _uiRules;
            }
            set
            {
                _uiRules = value;
            }
        }

        [NonSerialized]
        private List<UIRule> _modes;

        [NonSerialized]
        private List<UIRule> _maps;

        [NonSerialized]
        private List<UIRule> _uiRules;
    }

}