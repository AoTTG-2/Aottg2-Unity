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
        private ElementStyle _style;
        private Telemetry _telemetry;
        private KDRPanel _kdr;

        public override void Setup(BasePanel parent = null)
        {
            panel = transform.Find("Content/Panel").gameObject;
            _style = new ElementStyle(themePanel: ThemePanel);
            var telemetrySection = ElementFactory.CreateVerticalGroup(panel.transform, 0f);
            ElementFactory.SetAnchor(telemetrySection, TextAnchor.UpperLeft, TextAnchor.UpperLeft, new Vector2(0f, 0f));
            _telemetry = telemetrySection.AddComponent<Telemetry>();
            _telemetry.Setup(_style);

            var kdrSection = ElementFactory.CreateVerticalGroup(panel.transform, 10f);
            ElementFactory.SetAnchor(kdrSection, TextAnchor.UpperLeft, TextAnchor.UpperLeft, new Vector2(0f, 0f));
            _kdr = kdrSection.AddComponent<KDRPanel>();
            _kdr.Setup(_style);
        }
    }
}
