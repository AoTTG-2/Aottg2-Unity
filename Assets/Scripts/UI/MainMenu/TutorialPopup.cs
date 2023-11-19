using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class TutorialPopup: BasePopup
    {
        protected override string Title => UIManager.GetLocale("MainMenu", "TutorialPopup", "Title");
        protected override float Width => 280f;
        protected override float Height => 270f;
        protected override float VerticalSpacing => 20f;
        protected override int VerticalPadding => 20;
        protected override bool UseSound => true;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            string cat = "MainMenu";
            string sub = "TutorialPopup";
            float width = 220f;
            ElementStyle style = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Back"), onClick: () => OnButtonClick("Back"));
            ElementFactory.CreateTextButton(SinglePanel, style, UIManager.GetLocale(cat, sub, "BasicButton"), width, onClick: () => OnButtonClick("Basic"));
            ElementFactory.CreateTextButton(SinglePanel, style, UIManager.GetLocale(cat, sub, "AdvancedButton"), width, onClick: () => OnButtonClick("Advanced"));
        }

        protected void OnButtonClick(string name)
        {
            if (name == "MapEditor")
            {
                Application.LoadLevel(2);
            }
            else if (name == "CharacterEditor")
            {
                Application.LoadLevel("characterCreation");
            }
            else if (name == "SnapshotViewer")
            {
                Application.LoadLevel("SnapShot");
            }
            else if (name == "Back")
                Hide();
        }
    }
}
