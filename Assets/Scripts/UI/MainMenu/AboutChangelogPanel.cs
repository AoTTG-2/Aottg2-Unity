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
                foreach (JSONNode node in PastebinLoader.Changelog)
                {
                    ElementFactory.CreateDefaultLabel(SinglePanel, style, "Version " + node["Version"].Value + ":", FontStyle.Bold, TextAnchor.MiddleLeft);
                    foreach (JSONNode change in node["Changes"])
                    {
                        ElementFactory.CreateDefaultLabel(SinglePanel, style, "- " + change.Value, alignment: TextAnchor.MiddleLeft);
                    }
                    CreateHorizontalDivider(SinglePanel);
                }
            }
            else
                ElementFactory.CreateDefaultLabel(SinglePanel, style, "Loading changelog...", alignment: TextAnchor.MiddleCenter);
        }
    }
}
