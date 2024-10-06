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
        private ElementStyle _style;
        private Telemetry _telemetry;
        private KDRPanel _kdr;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            _style = new ElementStyle(themePanel: ThemePanel);
            // Get vertical layout group component on this gameobject
            var verticalLayoutGroup = gameObject.GetComponent<VerticalLayoutGroup>();

            var telemetrySection = ElementFactory.CreateVerticalGroup(verticalLayoutGroup.transform, 0f);
            ElementFactory.SetAnchor(telemetrySection, TextAnchor.UpperLeft, TextAnchor.UpperLeft, new Vector2(0f, 0f));
            _telemetry = telemetrySection.AddComponent<Telemetry>();
            _telemetry.Setup(_style);

            var kdrSection = ElementFactory.CreateVerticalGroup(verticalLayoutGroup.transform, 0f);
            ElementFactory.SetAnchor(kdrSection, TextAnchor.UpperLeft, TextAnchor.UpperLeft, new Vector2(0f, 0f));
            _kdr = telemetrySection.AddComponent<KDRPanel>();
            _kdr.Setup(_style);
        }
    }
}
