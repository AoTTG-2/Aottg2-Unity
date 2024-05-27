using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using Settings;
using System.Collections;
using ApplicationManagers;
using SimpleJSONFixed;

namespace UI
{
    class CreditsPopup: BasePopup
    {
        protected override string Title => UIManager.GetLocale("MainMenu", "Intro.CreditsButton");
        protected override float Width => 1400f;
        protected override float Height => 1060f;
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
            SetupBottomButtons();
        }

        private void SetupBottomButtons()
        {
            ElementStyle style = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            foreach (string buttonName in new string[] { "Back" })
            {
                GameObject obj = ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon(buttonName), 
                    onClick: () => OnBottomBarButtonClick(buttonName));
            }
        }

        private void OnBottomBarButtonClick(string name)
        {
            switch (name)
            {
                case "Back":
                    Hide();
                    break;
            }
        }
    }
}
