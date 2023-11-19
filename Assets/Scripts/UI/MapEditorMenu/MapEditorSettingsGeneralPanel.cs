using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class MapEditorSettingsGeneralPanel: CategoryPanel
    {
        protected override bool DoublePanel => true;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            var settingsPopup = (MapEditorSettingsPopup)parent;
            string cat = settingsPopup.LocaleCategory;
            string sub = "General";
            var settings = SettingsManager.MapEditorSettings;
            ElementStyle style = new ElementStyle(titleWidth: 250f, themePanel: ThemePanel);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.CameraMoveSpeed, UIManager.GetLocale(cat, sub, "CameraMoveSpeed"), elementWidth: 120f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.CameraSlowMoveSpeed, UIManager.GetLocale(cat, sub, "CameraSlowMoveSpeed"), elementWidth: 120f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.CameraFastMoveSpeed, UIManager.GetLocale(cat, sub, "CameraFastMoveSpeed"), elementWidth: 120f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.CameraRotateSpeed, UIManager.GetLocale(cat, sub, "CameraRotateSpeed"), elementWidth: 120f);
            ElementFactory.CreateInputSetting(DoublePanelLeft, style, settings.RenderDistance, UIManager.GetLocale(cat, sub, "RenderDistance"), elementWidth: 120f);
            ElementFactory.CreateInputSetting(DoublePanelRight, style, settings.SnapMove, UIManager.GetLocale(cat, sub, "SnapMove"), elementWidth: 120f);
            // ElementFactory.CreateInputSetting(DoublePanelRight, style, settings.SnapRotate, UIManager.GetLocale(cat, sub, "SnapRotate"), elementWidth: 120f);
            // ElementFactory.CreateInputSetting(DoublePanelRight, style, settings.SnapScale, UIManager.GetLocale(cat, sub, "SnapScale"), elementWidth: 120f);

        }
    }
}
