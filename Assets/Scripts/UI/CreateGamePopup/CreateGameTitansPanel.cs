using Map;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;

namespace UI
{
    class CreateGameTitansPanel : CreateGameCategoryPanel
    {
        protected override float VerticalSpacing => 10f;
        protected override bool ScrollBar => true;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            InGameTitanSettings settings = SettingsManager.InGameUI.Titan;
            string cat = "CreateGamePopup";
            string sub = "Titans";
            ElementStyle style = new ElementStyle(titleWidth: 240f, themePanel: ThemePanel);
            float inputWidth = 120f;
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.TitanSpawnEnabled, UIManager.GetLocale(cat, sub, "SpawnEnabled"),
                tooltip: UIManager.GetLocale(cat, sub, "SpawnEnabledTooltip"));
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.TitanSpawnNormal, UIManager.GetLocale(cat, sub, "Normal"), elementWidth: inputWidth);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.TitanSpawnAbnormal, UIManager.GetLocale(cat, sub, "Abnormal"), elementWidth: inputWidth);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.TitanSpawnJumper, UIManager.GetLocale(cat, sub, "Jumper"), elementWidth: inputWidth);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.TitanSpawnCrawler, UIManager.GetLocale(cat, sub, "Crawler"), elementWidth: inputWidth);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.TitanSpawnPunk, UIManager.GetLocale(cat, sub, "Punk"), elementWidth: inputWidth);
            CreateHorizontalDivider(DoublePanelLeft);
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.TitanSizeEnabled, UIManager.GetLocale(cat, sub, "SizeEnabled"));
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.TitanSizeMin, UIManager.GetLocale(cat, sub, "MinSize"), elementWidth: inputWidth);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.TitanSizeMax, UIManager.GetLocale(cat, sub, "MaxSize"), elementWidth: inputWidth);
            ElementFactory.CreateToggleGroupSetting(DoublePanelRight, style, settings.TitanHealthMode, UIManager.GetLocale(cat, sub, "HealthMode"),
                UIManager.GetLocaleArray(cat, sub, "HealthOptions"));
            ElementFactory.CreateInputSetting(DoublePanelRight, style, settings.TitanHealthMin, UIManager.GetLocale(cat, sub, "MinHealth"), elementWidth: inputWidth);
            ElementFactory.CreateInputSetting(DoublePanelRight, style, settings.TitanHealthMax, UIManager.GetLocale(cat, sub, "MaxHealth"), elementWidth: inputWidth);
            CreateHorizontalDivider(DoublePanelRight);
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.TitanArmorEnabled, UIManager.GetLocale(cat, sub, "ArmorEnabled"));
            ElementFactory.CreateInputSetting(DoublePanelRight, style, settings.TitanArmor, UIManager.GetLocale(cat, sub, "Armor"), elementWidth: inputWidth);
            CreateHorizontalDivider(DoublePanelRight);
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.TitanStandardModels, UIManager.GetLocale(cat, sub, "StandardModels"),
                tooltip: UIManager.GetLocale(cat, sub, "StandardModelsTooltip"));
        }
    }
}
