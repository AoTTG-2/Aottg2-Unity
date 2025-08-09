using System.Collections.Generic;
using System.Linq;

namespace Settings
{
    public static class PresetMerger
    {
        public static PresetDefinition Merge(PresetDefinition basePreset, PresetDefinition child)
        {
            var merged = new PresetDefinition
            {
                Category = child.Category ?? basePreset.Category,
                Subcategory = child.Subcategory ?? basePreset.Subcategory,
                Name = child.Name ?? basePreset.Name,
                Description = child.Description ?? basePreset.Description,

                Modes = MergeUIRuleLists(basePreset.Modes, child.Modes),
                Maps = MergeUIRuleLists(basePreset.Maps, child.Maps),
                UIRules = MergeUIRuleLists(basePreset.UIRules, child.UIRules)
            };
            return merged;
        }

        private static List<UIRule> MergeUIRuleLists(List<UIRule> baseRules, List<UIRule> childRules)
        {
            if (baseRules == null) return childRules ?? new List<UIRule>();
            if (childRules == null) return baseRules;

            var merged = new List<UIRule>();

            // Build a lookup for child rules by SettingId for quick override detection
            var childRuleMap = childRules.ToDictionary(r => r.SettingId, r => r);

            foreach (var baseRule in baseRules)
            {
                if (!childRuleMap.ContainsKey(baseRule.SettingId))
                {
                    // Base rule not overridden by child, keep it
                    merged.Add(baseRule);
                }
                // else child overrides this setting → skip base rule
            }

            // Add all child rules (overrides or new)
            merged.AddRange(childRules);

            return merged;
        }
    }

}