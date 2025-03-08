using Settings;
using System.Collections.Generic;
using UnityEngine.UI;
using Utility;
using UnityEngine;

namespace UI
{
    class EditProfileEmotePanel: CategoryPanel
    {
        protected override bool ScrollBar => true;
        protected override float VerticalSpacing => 20f;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            var settings = SettingsManager.EmoteSettings;
            ElementStyle style = new ElementStyle(titleWidth: 100f, themePanel: ThemePanel);
            string[] options = UIManager.AvailableProfileIcons.ToArray();
            string[] icons = GetProfileIconPaths(options);

            for (int i = 0; i < 8; i++)
                ElementFactory.CreateInputSetting(SinglePanel, style, settings.TextEmotes.GetItemAt(i), UIManager.GetLocaleCommon("Text") + " " + (i + 1),
                    elementWidth: 260f);
            CreateHorizontalDivider(SinglePanel);
            for (int i = 0; i < 8; i++)
            {
                ElementFactory.CreateIconPickSetting(SinglePanel, style, settings.EmojiEmotes.GetItemAt(i), UIManager.GetLocaleCommon("Emoji") + " " + (i + 1), options, icons,
                UIManager.CurrentMenu.IconPickPopup, elementWidth: 180f, elementHeight: 40f);
            }
        }

        private string[] GetProfileIconPaths(string[] options)
        {
            List<string> icons = new List<string>();
            foreach (string option in options)
                icons.Add(ResourcePaths.UI + "/Icons/Profile/" + option + "Icon");
            return icons.ToArray();
        }
    }
}
