using Settings;
using System;
using UnityEngine.UI;
using GameProgress;
using UnityEngine;
using ApplicationManagers;
using SimpleJSONFixed;

namespace UI
{
    class AboutCreditsPanel: CategoryPanel
    {
        protected override float VerticalSpacing => 10f;
        protected override bool ScrollBar => true;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(titleWidth: 100f, themePanel: ThemePanel, fontSize: 20);
            if (MiscInfo.Credits != null)
            {
                foreach (JSONNode node in MiscInfo.Credits)
                {
                    ElementFactory.CreateDefaultLabel(SinglePanel, style, node["Category"].Value + ":", FontStyle.Bold, TextAnchor.MiddleLeft);
                    foreach (JSONNode change in node["Names"])
                    {
                        ElementFactory.CreateDefaultLabel(SinglePanel, style, "- " + change.Value, alignment: TextAnchor.MiddleLeft);
                    }
                    CreateHorizontalDivider(SinglePanel);
                }
            }
            else
                ElementFactory.CreateDefaultLabel(SinglePanel, style, "Error loading data.", alignment: TextAnchor.MiddleCenter);
        }
    }
}
