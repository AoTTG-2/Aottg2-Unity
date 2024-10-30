using Discord;
using Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class TopLeftHUD : BasePanel
    {
        protected override string ThemePanel => "TopLeftHUD";
        public GameObject panel;
        public GameObject telemetryCanvas;
        public GameObject kdrCanvas;
        public GameObject kdrAndLabel;
        private ElementStyle _style;
        private Telemetry _telemetry;
        private KDRPanel _kdr;



        public override void Setup(BasePanel parent = null)
        {
            panel = transform.Find("Content/Panel").gameObject;

            // Create a nested canvas for the telemetry and KDR panels
            telemetryCanvas = new GameObject();
            telemetryCanvas.name = "Telemetry";
            var tcanvas = telemetryCanvas.AddComponent<Canvas>();

            // set canvas pivot
            var rect = telemetryCanvas.GetComponent<RectTransform>();
            rect.pivot = ElementFactory.GetAnchorVector(TextAnchor.UpperLeft);

            kdrCanvas = new GameObject();
            kdrCanvas.name = "KDR";
            kdrCanvas.AddComponent<Canvas>();

            // set canvas pivot
            rect = kdrCanvas.GetComponent<RectTransform>();
            rect.pivot = ElementFactory.GetAnchorVector(TextAnchor.UpperLeft);

            telemetryCanvas.transform.SetParent(panel.transform);
            kdrCanvas.transform.SetParent(panel.transform);

            _style = new ElementStyle(themePanel: ThemePanel);
            var telemetrySection = ElementFactory.CreateVerticalGroup(telemetryCanvas.transform, 0f);
            ElementFactory.SetAnchor(telemetrySection, TextAnchor.UpperLeft, TextAnchor.UpperLeft, new Vector2(0f, 0f));
            var trect = telemetrySection.GetComponent<RectTransform>();
            trect.sizeDelta = new Vector2(400, 60f);
            _telemetry = telemetrySection.AddComponent<Telemetry>();
            _telemetry.Setup(_style);

            kdrAndLabel = ElementFactory.CreateVerticalGroup(kdrCanvas.transform, 0f);
            ElementFactory.SetAnchor(kdrAndLabel, TextAnchor.UpperLeft, TextAnchor.UpperLeft, new Vector2(0f, 0f));

            var krect = kdrAndLabel.GetComponent<RectTransform>();
            krect.sizeDelta = new Vector2(500, 700f);

            var kdrScroll = ElementFactory.CreateVerticalGroup(kdrAndLabel.transform, 0f);
            ElementFactory.SetAnchor(kdrScroll, TextAnchor.UpperLeft, TextAnchor.UpperLeft, new Vector2(0f, 0f));

            _kdr = kdrScroll.AddComponent<KDRPanel>();
            _kdr.Setup(_style);
            ApplySettings();
        }

        public void ApplySettings()
        {
            var rect = kdrAndLabel.GetComponent<RectTransform>();

            float verticaloffset = 0f;
            if (SettingsManager.GraphicsSettings.ShowFPS.Value || SettingsManager.UISettings.ShowPing.Value)
                verticaloffset += 30;
            if (SettingsManager.UISettings.ShowGameTime.Value)
                verticaloffset += 30;

            // Set the offset for the KDR panel
            rect.anchoredPosition = new Vector2(0, -verticaloffset);
        }
    }
}
