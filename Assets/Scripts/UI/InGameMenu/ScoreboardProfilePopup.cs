using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using Settings;
using System.Collections;
using GameManagers;
using Photon.Realtime;

namespace UI
{
    class ScoreboardProfilePopup: PromptPopup
    {
        protected override string Title => UIManager.GetLocaleCommon("Profile");
        protected override float Width => 400f;
        protected override int VerticalPadding => 5;
        protected override float VerticalSpacing => 25f;
        protected override float Height => 640f;
        protected ProfileSettings _profile;
        protected List<GameObject> _items = new List<GameObject>();

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            SetupBottomButtons();
        }

        public void Show(Player player)
        {
            base.Show();
            foreach (var item in _items)
                Destroy(item);
            _items.Clear();
            var playerInfo = InGameManager.AllPlayerInfo[player.ActorNumber].Profile;
            _profile = playerInfo;
            ElementStyle style = new ElementStyle(titleWidth: 120f, themePanel: ThemePanel, fontSize: 22);
            var group = ElementFactory.CreateHorizontalGroup(SinglePanel, 25f, TextAnchor.MiddleCenter).transform;
            _items.Add(group.gameObject);
            ElementFactory.CreateRawImage(group, style, "Icons/Profile/" + UIManager.GetProfileIcon(_profile.ProfileIcon.Value), 256, 256);
            _items.Add(ElementFactory.CreateDefaultLabel(SinglePanel, style, "<b>" + UIManager.GetLocaleCommon("Name") + ": </b>" + _profile.Name.Value.HexColor(),
                alignment: TextAnchor.MiddleLeft));
            _items.Add(ElementFactory.CreateDefaultLabel(SinglePanel, style, "<b>" + UIManager.GetLocaleCommon("Guild") + ": </b>" + _profile.Guild.Value.HexColor(),
                alignment: TextAnchor.MiddleLeft));
            _items.Add(ElementFactory.CreateDefaultLabel(SinglePanel, style, "<b>" + UIManager.GetLocaleCommon("Social") + ": </b>" + _profile.Social.Value.HexColor(),
                alignment: TextAnchor.MiddleLeft));
            _items.Add(ElementFactory.CreateDefaultLabel(SinglePanel, style, "<b>" + UIManager.GetLocaleCommon("About") + ": </b>" + _profile.About.Value.HexColor(),
                alignment: TextAnchor.MiddleLeft));
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
