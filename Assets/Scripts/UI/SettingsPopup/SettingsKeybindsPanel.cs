using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class SettingsKeybindsPanel: SettingsCategoryPanel
    {
        protected override bool CategoryPanel => true;
        protected override string DefaultCategoryPanel => "General";
        protected string[] _categories = new string[] { "General", "Human", "Titan", "Shifter", "Interaction" };

        public void CreateGategoryDropdown(Transform panel)
        {
            ElementStyle style = new ElementStyle(titleWidth: 140f, themePanel: ThemePanel);
            ElementFactory.CreateDropdownSetting(panel, style, _currentCategoryPanelName, "Category", _categories, elementWidth: 260f,
                                                 onDropdownOptionSelect: () => RebuildCategoryPanel());
        }

        protected override void RegisterCategoryPanels()
        {
            foreach (string category in _categories)
                _categoryPanelTypes.Add(category, typeof(SettingsKeybindsDefaultPanel));
        }
    }
}
