using ApplicationManagers;
using Settings;
using SimpleJSONFixed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace UI
{
    class SocialPopup: BasePopup
    {
        protected override string Title => UIManager.GetLocaleCommon("Social");
        protected override float Width => 630f;
        protected override float Height => 400f;
        protected override bool DoublePanel => false;

        protected override int HorizontalPadding => 35;

        protected override TextAnchor PanelAlignment => TextAnchor.MiddleLeft;
        protected override bool UseSound => true;


        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle buttonStyle = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Back"), onClick: () => OnButtonClick("Back"));
            ElementStyle mainStyle = new ElementStyle(themePanel: ThemePanel);
            if (MiscInfo.Social != null)
            {
                foreach (JSONNode node in MiscInfo.Social)
                    CreateLink(mainStyle, node["Title"].Value, node["Link"].Value, node["About"].Value);
            }
            else
                ElementFactory.CreateDefaultLabel(SinglePanel, mainStyle, "Error loading data.", alignment: TextAnchor.MiddleCenter);
        }

        private void CreateLink(ElementStyle style, string title, string link, string about)
        {
            Transform group = ElementFactory.CreateHorizontalGroup(SinglePanel, 5f).transform;
            ElementFactory.CreateTooltipIcon(group, style, about, 30f, 30f);
            ElementFactory.CreateDefaultLabel(group, style, " " + title + ":");
            ElementFactory.CreateLinkButton(group, style, link,
                onClick: () => UIManager.CurrentMenu.ExternalLinkPopup.Show(link));
        }

        private void OnButtonClick(string name)
        {
            if (name == "Back")
                Hide();
        }
    }
}
