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
using Characters;

namespace UI
{
    class ExpeditionPlayerPanel : SettingsCategoryPanel
    {
        protected override bool ScrollBar => true;
        private Player selectedPlayer;
        private string tpCoords;
        private List<SaveableSettingsContainer> _saveableSettings = new List<SaveableSettingsContainer>();

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ExpeditionPopup RolesPopup = (ExpeditionPopup)parent;
            string cat = RolesPopup.LocaleCategory;
            string sub = "Players";
            EMSettings settings = SettingsManager.EMSettings;

            ElementStyle style = new ElementStyle(titleWidth: 200f, themePanel: ThemePanel);
        }

        private void UpdateSettings()
        {
            _saveableSettings.Add(SettingsManager.EMSettings);

            foreach (SaveableSettingsContainer setting in _saveableSettings)
                setting.Load();
            RebuildCategoryPanel();

            _saveableSettings.Clear();
        }
    }
}
