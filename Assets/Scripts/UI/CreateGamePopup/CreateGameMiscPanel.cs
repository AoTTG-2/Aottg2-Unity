using Map;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using GameManagers;

namespace UI
{
    class CreateGameMiscPanel : CreateGameCategoryPanel
    {
        protected override bool ScrollBar => true;
        protected override float VerticalSpacing => 15f;
        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            InGameMiscSettings settings = SettingsManager.InGameUI.Misc;
            string cat = "CreateGamePopup";
            string sub = "Misc";
            ElementStyle style = new ElementStyle(titleWidth: 240f, themePanel: ThemePanel);
            float inputWidth = 120f;
            ElementFactory.CreateToggleGroupSetting(DoublePanelLeft, style, settings.PVP, UIManager.GetLocale(cat, sub, "PVP"), UIManager.GetLocaleArray(cat, sub, "PVPOptions"));
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.CustomStats, UIManager.GetLocale(cat, sub, "CustomStats"), UIManager.GetLocale(cat, sub, "CustomStatsTooltip"));
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.CustomPerks, UIManager.GetLocale(cat, sub, "CustomPerks"), UIManager.GetLocale(cat, sub, "CustomPerksTooltip"));
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.RealismMode, UIManager.GetLocale(cat, sub, "RealismMode"), UIManager.GetLocale(cat, sub, "RealismModeTooltip"));
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.Horses, UIManager.GetLocale(cat, sub, "Horses"));
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.HorsebackCombat, UIManager.GetLocale(cat, sub, "HorsebackCombat"));
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.EndlessRespawnEnabled, UIManager.GetLocale(cat, sub, "EndlessRespawnEnabled"));
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.EndlessRespawnTime, UIManager.GetLocale(cat, sub, "EndlessRespawnTime"), elementWidth: inputWidth);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.AllowSpawnTime, UIManager.GetLocale(cat, sub, "AllowSpawnTime"), UIManager.GetLocale(cat, sub, "AllowSpawnTimeTooltip"), elementWidth: inputWidth);
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.ThunderspearPVP, UIManager.GetLocale(cat, sub, "ThunderspearPVP"), UIManager.GetLocale(cat, sub, "ThunderspearPVPTooltip"));
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.APGPVP, UIManager.GetLocale(cat, sub, "APGPVP"), UIManager.GetLocale(cat, sub, "APGPVPTooltip"));
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.HumanHealth, UIManager.GetLocale(cat, sub, "HumanHealth"), elementWidth: inputWidth);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.ShifterHealth, UIManager.GetLocale(cat, sub, "ShifterHealth"), elementWidth: inputWidth);
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.AllowBlades, UIManager.GetLocale(cat, sub, "AllowBlades"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.AllowAHSS, UIManager.GetLocale(cat, sub, "AllowAHSS"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.AllowThunderspears, UIManager.GetLocale(cat, sub, "AllowThunderspears"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.AllowAPG, UIManager.GetLocale(cat, sub, "AllowAPG"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.AllowPlayerTitans, UIManager.GetLocale(cat, sub, "AllowPlayerTitans"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.AllowShifters, UIManager.GetLocale(cat, sub, "AllowShifters"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.AllowShifterSpecials, UIManager.GetLocale(cat, sub, "AllowShifterSpecials"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.AllowVoteKicking, UIManager.GetLocale(cat, sub, "AllowVoteKicking"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.ClearKDROnRestart, UIManager.GetLocale(cat, sub, "ClearKDROnRestart"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.GlobalMinimapDisable, UIManager.GetLocale(cat, sub, "GlobalMinimapDisable"));
            ElementFactory.CreateDropdownSetting(DoublePanelRight, style, settings.VoiceChat, UIManager.GetLocale(cat, sub, "VoiceChat"), 
                               UIManager.GetLocaleArray(cat, sub, "VoiceChatOptions"), elementWidth: inputWidth);
            ElementFactory.CreateInputSetting(DoublePanelRight, style, settings.ProximityMinDistance, UIManager.GetLocale(cat, sub, "ProximityMinDistance"), elementWidth: inputWidth);
            ElementFactory.CreateInputSetting(DoublePanelRight, style, settings.ProximityMaxDistance, UIManager.GetLocale(cat, sub, "ProximityMaxDistance"), elementWidth: inputWidth);
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.GunsAirReload, UIManager.GetLocale(cat, sub, "GunsAirReload"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.AllowStock, UIManager.GetLocale(cat, sub, "AllowStock"));
            ElementFactory.CreateInputSetting(DoublePanelRight, style, settings.InvincibilityTime, UIManager.GetLocale(cat, sub, "InvincibilityTime"), elementWidth: inputWidth);
            ElementFactory.CreateInputSetting(DoublePanelRight, style, settings.Motd, UIManager.GetLocale(cat, sub, "MOTD"), elementWidth: inputWidth);
        }
    }
}
