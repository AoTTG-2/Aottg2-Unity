using Settings;
using System;
using UnityEngine.UI;
using GameProgress;
using UnityEngine;
using ApplicationManagers;
using SimpleJSONFixed;
using System.Linq;
using System.Collections.Generic;

namespace UI
{
    class AboutCreditsPanel: CategoryPanel
    {
        protected override float VerticalSpacing => 15f;
        protected override bool ScrollBar => true;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(titleWidth: 100f, themePanel: ThemePanel, fontSize: 24);
            if (MiscInfo.Credits != null)
            {
                foreach (JSONNode node in MiscInfo.Credits)
                {
                    // ElementFactory.CreateDefaultLabel(SinglePanel, style, node["Category"].Value + ":", FontStyle.Bold, TextAnchor.MiddleLeft);
                    List<string> names = new List<string>();
                    foreach (JSONNode name in node["Names"])
                    {
                        if (!names.Contains(name.Value))
                            names.Add(name.Value);
                    }
                    names.Sort();
                    string nameStr = string.Join(", ", names);
                    nameStr = "<b>" + node["Category"].Value + "</b>: " + nameStr;
                    ElementFactory.CreateDefaultLabel(SinglePanel, style, nameStr, FontStyle.Normal, TextAnchor.MiddleLeft);
                }
                ElementFactory.CreateDefaultLabel(SinglePanel, style, "Based on the original game created by Feng Li and Jiang Li.", FontStyle.Normal, TextAnchor.MiddleLeft);
            }
            else
                ElementFactory.CreateDefaultLabel(SinglePanel, style, "Error loading data.", alignment: TextAnchor.MiddleCenter);
        }
    }
}
