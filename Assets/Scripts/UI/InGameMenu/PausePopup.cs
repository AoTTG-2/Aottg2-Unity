using GameManagers;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class PausePopup: BasePopup
    {
        protected override string Title => UIManager.GetLocale("SettingsPopup", "Keybinds.General", "Pause");
        protected override float Width => 220f;
        protected override float Height => 280f;
        protected override float VerticalSpacing => 20f;
        protected override int VerticalPadding => 20;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            string cat = "InGameMenu";
            string sub = "PausePopup";
            float width = 160f;
            ElementStyle style = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Quit"), onClick: () => OnButtonClick("Quit"));
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Back"), onClick: () => OnButtonClick("Back"));
            ElementFactory.CreateDefaultButton(SinglePanel, style, UIManager.GetLocaleCommon("Settings"), width, onClick: () => OnButtonClick("Settings"));
            ElementFactory.CreateDefaultButton(SinglePanel, style, UIManager.GetLocaleCommon("Game"), width, onClick: () => OnButtonClick("Game"));
        }

        protected void OnButtonClick(string name)
        {
            InGameMenu menu = (InGameMenu)UIManager.CurrentMenu;
            if (name == "Game")
            {
                menu._createGamePopup.Show();
                Hide();
            }
            else if (name == "Settings")
            {
                menu._settingsPopup.Show();
                Hide();
            }
            else if (name == "Back")
            {
                menu.SetPauseMenu(false);
            }    
            else if (name == "Quit")
            {
                InGameManager.LeaveRoom();
            }
        }
    }
}
