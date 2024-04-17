using ApplicationManagers;
using GameManagers;
using Settings;
using Photon;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace UI
{
    class ExpeditionPlayerPanel : SettingsCategoryPanel
    {
        protected override bool ScrollBar => true;
        private Player selectedPlayer;

        public override void Setup(BasePanel parent = null) //Zippy: Finish the player List
        {
            base.Setup(parent);
            ExpeditionPopup RolesPopup = (ExpeditionPopup)parent;
            string cat = RolesPopup.LocaleCategory;
            string sub = "Players";
            EMSettings settings = SettingsManager.EMSettings;

            ElementStyle style = new ElementStyle(titleWidth: 200f, themePanel: ThemePanel);

            #region Left Side

            foreach (Player player in PhotonNetwork.PlayerList)
            {
                string name = ChatManager.GetIDString(player.ActorNumber, player.IsMasterClient) + player.GetStringProperty(PlayerProperty.Name);
                GameObject obj = ElementFactory.CreateDefaultButton(DoublePanelLeft, style, name, 400f, 35f,
                                                                    onClick: () => OnPlayerButtonClick(player));
            }

            #endregion

            #region Right Side



            #endregion
            #region //Code
            /*
            
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, style, settings.Language, "Language", UIManager.GetLanguages(),
                elementWidth: 160f, onDropdownOptionSelect: () => settingsPopup.RebuildCategoryPanel(), tooltip: UIManager.GetLocaleCommon("RequireRestart"));
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, style, settings.CameraMode, UIManager.GetLocale(cat, sub, "CameraMode"),
                 new string[] { "TPS", "Original" }, elementWidth: 200f);
            ElementFactory.CreateSliderSetting(DoublePanelLeft, style, settings.CameraDistance, UIManager.GetLocale(cat, sub, "CameraDistance"),
               elementWidth: 135f);
            ElementFactory.CreateSliderSetting(DoublePanelLeft, style, settings.CameraHeight, UIManager.GetLocale(cat, sub, "CameraHeight"),
               elementWidth: 135f);
            ElementFactory.CreateSliderSetting(DoublePanelLeft, style, settings.CameraSide, UIManager.GetLocale(cat, sub, "CameraSide"),
               elementWidth: 135f);
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.CameraTilt, UIManager.GetLocale(cat, sub, "CameraTilt"));
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, settings.CameraClipping, UIManager.GetLocale(cat, sub, "CameraClipping"),
                UIManager.GetLocale(cat, sub, "CameraClippingTooltip"));
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.FOVMin, UIManager.GetLocale(cat, sub, "FOVMin"), tooltip:
                UIManager.GetLocale(cat, sub, "FOVMinTooltip"), elementWidth: 100f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.FOVMax, UIManager.GetLocale(cat, sub, "FOVMax"), tooltip:
                UIManager.GetLocale(cat, sub, "FOVMaxTooltip"), elementWidth: 100f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.FPSFOVMin, UIManager.GetLocale(cat, sub, "FPSFOVMin"), tooltip:
                UIManager.GetLocale(cat, sub, "FPSFOVMinTooltip"), elementWidth: 100f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.FPSFOVMax, UIManager.GetLocale(cat, sub, "FPSFOVMax"), tooltip:
                UIManager.GetLocale(cat, sub, "FPSFOVMaxTooltip"), elementWidth: 100f);
            ElementFactory.CreateSliderInputSetting(DoublePanelRight, new ElementStyle(titleWidth: 165f, themePanel: ThemePanel), settings.MouseSpeed, UIManager.GetLocale(cat, sub, "MouseSpeed"),
              sliderWidth: 135f, decimalPlaces: 3);
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.InvertMouse, UIManager.GetLocale(cat, sub, "InvertMouse"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.MinimapEnabled, UIManager.GetLocale(cat, sub, "MinimapEnabled"));
            ElementFactory.CreateInputSetting(DoublePanelRight, style, settings.MinimapHeight, UIManager.GetLocale(cat, sub, "MinimapHeight"),
                elementWidth: 100f);
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.SnapshotsEnabled, UIManager.GetLocale(cat, sub, "SnapshotsEnabled"));
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.SnapshotsShowInGame, UIManager.GetLocale(cat, sub, "SnapshotsShowInGame"));
            ElementFactory.CreateInputSetting(DoublePanelRight, style, settings.SnapshotsMinimumDamage, UIManager.GetLocale(cat, sub, "SnapshotsMinimumDamage"),
                elementWidth: 100f);
            ElementFactory.CreateToggleSetting(DoublePanelRight, style, settings.SkipCutscenes, UIManager.GetLocale(cat, sub, "SkipCutscenes"));
            
             */
            #endregion
        }

        private void OnPlayerButtonClick(Player player)
        {
            selectedPlayer = player;
        }
    }
}
