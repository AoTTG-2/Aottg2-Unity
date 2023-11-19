using Photon.Pun;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class MultiplayerPasswordPopup: PromptPopup
    {
        protected override string Title => UIManager.GetLocaleCommon("Password");
        protected override int VerticalPadding => 10;
        protected override int HorizontalPadding => 20;
        protected override float VerticalSpacing => 10f;
        protected override float Width => 300f;
        protected override float Height => 250f;
        protected override TextAnchor PanelAlignment => TextAnchor.MiddleCenter;
        protected StringSetting _enteredPassword = new StringSetting(string.Empty);
        protected string _actualPassword;
        protected string _roomName;
        protected GameObject _incorrectPasswordLabel;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            float elementWidth = 200f;
            ElementStyle buttonStyle = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementStyle labelStyle = new ElementStyle(fontSize: 20, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Confirm"), onClick: () => OnButtonClick("Confirm"));
            ElementFactory.CreateTextButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Back"), onClick: () => OnButtonClick("Back"));
            ElementFactory.CreateDefaultLabel(SinglePanel, labelStyle, string.Empty);
            ElementFactory.CreateInputSetting(SinglePanel, labelStyle, _enteredPassword, string.Empty,
                elementWidth: elementWidth);
            _incorrectPasswordLabel = ElementFactory.CreateDefaultLabel(SinglePanel, labelStyle,
                UIManager.GetLocale("MainMenu", "MultiplayerPasswordPopup", "IncorrectPassword"));
            _incorrectPasswordLabel.GetComponent<Text>().color = Color.red;
        }

        public void Show(string actualPassword, string roomName)
        {
            _actualPassword = actualPassword;
            _roomName = roomName;
            _incorrectPasswordLabel.SetActive(false);
            base.Show();
        }

        protected void OnButtonClick(string name)
        {
            if (name == "Confirm")
            {
                try
                {
                    if (_enteredPassword.Value == new SimpleAES().Decrypt(_actualPassword))
                    {
                        PhotonNetwork.JoinRoom(_roomName);
                        Hide();
                    }
                    else
                        _incorrectPasswordLabel.SetActive(true);
                }
                catch
                {
                    _incorrectPasswordLabel.SetActive(true);
                }
            }
            else if (name == "Back")
                Hide();
        }
    }
}
