using Settings;
using System;
using UnityEngine.UI;
using GameProgress;
using UnityEngine;
using ApplicationManagers;
using SimpleJSONFixed;

namespace UI
{
    class DiscordLoginPanel : CategoryPanel
    {
        protected override float VerticalSpacing => 10f;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(titleWidth: 100f, themePanel: ThemePanel);
            Transform group = ElementFactory.CreateHorizontalGroup(SinglePanel, 5f).transform;
            ElementFactory.CreateLinkButton(group, style, "Login with Discord",
                onClick: () => APIConnectionManager.Instance.StartDiscordAuth());
        }
    }
}
