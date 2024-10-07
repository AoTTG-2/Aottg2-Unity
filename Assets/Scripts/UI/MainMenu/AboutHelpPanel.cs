using Settings;
using System;
using UnityEngine.UI;
using GameProgress;
using UnityEngine;
using ApplicationManagers;
using SimpleJSONFixed;

namespace UI
{
    class AboutHelpPanel: CategoryPanel
    {
        protected override float VerticalSpacing => 10f;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(titleWidth: 100f, themePanel: ThemePanel);
            if (MiscInfo.Help != null)
            {
                foreach (JSONNode node in MiscInfo.Help)
                {
                    CreateLink(style, node["Title"].Value, node["Link"].Value);
                }
            }
            else
                ElementFactory.CreateDefaultLabel(SinglePanel, style, "Error loading data.", alignment: TextAnchor.MiddleCenter);
        }

        private void CreateLink(ElementStyle style, string title, string link)
        {
            Transform group = ElementFactory.CreateHorizontalGroup(SinglePanel, 5f).transform;
            ElementFactory.CreateDefaultLabel(group, style, " " + title + ":");
            ElementFactory.CreateLinkButton(group, style, link,
                onClick: () => UIManager.CurrentMenu.ExternalLinkPopup.Show(link));
        }
    }
}
