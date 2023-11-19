using Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class MultiplayerMapPopup: BasePopup
    {
        protected override string ThemePanel => "MultiplayerMapPopup";
        protected override int HorizontalPadding => 0;
        protected override int VerticalPadding => 0;
        protected override float VerticalSpacing => 0f;
        protected override string Title => UIManager.GetLocale("MainMenu", "MultiplayerMapPopup", "Title");
        protected override bool HasPremadeContent => true;
        protected override float Width => 900f;
        protected override float Height => 560f;
        protected MultiplayerSettingsPopup _multiplayerSettingsPopup;
        protected MultiplayerLanPopup _lanPopup;
        protected override bool UseSound => true;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            GameObject map = SinglePanel.Find("MultiplayerMap").gameObject;
            foreach (Button button in map.GetComponentsInChildren<Button>())
            {
                button.onClick.AddListener(() => OnButtonClick(button.name));
                button.GetComponent<Button>().colors = UIManager.GetThemeColorBlock(style.ThemePanel, "DefaultButton", "");
                button.transform.Find("Text").GetComponent<Text>().color = UIManager.GetThemeColor(style.ThemePanel, "DefaultButton", "TextColor");
            }
            string cat = "MainMenu";
            string sub = "MultiplayerMapPopup";
            ElementFactory.CreateTextButton(BottomBar, style, "LAN",
                onClick: () => OnButtonClick("LAN"));
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocale(cat, sub, "ServerButton"),
                onClick: () => OnButtonClick("Server"));
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Back"),
                onClick: () => OnButtonClick("Back"));
            map.GetComponent<Image>().color = UIManager.GetThemeColor(style.ThemePanel, "MainBody", "MapColor");
        }

        protected override void SetupPopups()
        {
            base.SetupPopups();
            _multiplayerSettingsPopup = ElementFactory.CreateHeadedPanel<MultiplayerSettingsPopup>(transform).GetComponent<MultiplayerSettingsPopup>();
            _lanPopup = ElementFactory.CreateHeadedPanel<MultiplayerLanPopup>(transform).GetComponent<MultiplayerLanPopup>();
            _popups.Add(_multiplayerSettingsPopup);
            _popups.Add(_lanPopup);
        }

        private void OnButtonClick(string name)
        {
            HideAllPopups();
            MultiplayerSettings settings = SettingsManager.MultiplayerSettings;
            switch (name)
            {
                case "Back":
                    Hide();
                    break;
                case "Server":
                    _multiplayerSettingsPopup.Show();
                    break;
                case "Offline":
                    settings.ConnectOffline();
                    break;
                case "LAN":
                    _lanPopup.Show();
                    break;
                case "ButtonUS":
                    settings.ConnectServer(MultiplayerRegion.US);
                    break;
                case "ButtonSA":
                    settings.ConnectServer(MultiplayerRegion.SA);
                    break;
                case "ButtonEU":
                    settings.ConnectServer(MultiplayerRegion.EU);
                    break;
                case "ButtonASIA":
                    settings.ConnectServer(MultiplayerRegion.ASIA);
                    break;
                case "ButtonCN":
                    settings.ConnectServer(MultiplayerRegion.CN);
                    break;
            }
        }
    }
}
