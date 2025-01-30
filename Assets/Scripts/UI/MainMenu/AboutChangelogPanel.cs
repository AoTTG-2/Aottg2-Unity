using Settings;
using System;
using UnityEngine.UI;
using GameProgress;
using UnityEngine;
using ApplicationManagers;
using SimpleJSONFixed;

namespace UI
{
    class AboutChangelogPanel: CategoryPanel
    {
        protected override float VerticalSpacing => 10f;
        protected override bool ScrollBar => true;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(titleWidth: 100f, themePanel: ThemePanel, fontSize: 20);
            if (PastebinLoader.Status == PastebinStatus.Loaded)
            {
                ElementFactory.CreateDefaultLabel(SinglePanel, style, PastebinLoader.Changelog, FontStyle.Normal, TextAnchor.MiddleLeft);
            }
            else
                ElementFactory.CreateDefaultLabel(SinglePanel, style, "Loading changelog...", alignment: TextAnchor.MiddleCenter);
        }
    }
}
