using Settings;
using System;
using UnityEngine.UI;
using GameProgress;
using UnityEngine;
using ApplicationManagers;
using SimpleJSONFixed;

namespace UI
{
    class AboutVersionPanel: CategoryPanel
    {
        protected override float VerticalSpacing => 10f;
        protected override bool ScrollBar => true;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(titleWidth: 100f, themePanel: ThemePanel, fontSize: 20);
            ElementFactory.CreateDefaultLabel(SinglePanel, style, "Current version: " + ApplicationConfig.GameVersion.ToString(), FontStyle.Bold, TextAnchor.MiddleLeft);
        }
    }
}
