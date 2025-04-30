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

            string[] options = GetAvailableEmotes();
            string[] icons = GetEmotePaths(options);
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

        private string[] GetAvailableEmotes()
        {
            List<string> options = new List<string>();
            foreach (string option in UIManager.AvailableEmojis)
                options.Add(option);
            foreach (string option in UIManager.AvailableProfileIcons)
            {
                if (!option.StartsWith("Emoji"))
                    options.Add(option);
            }
            return options.ToArray();
        }

        private string[] GetEmotePaths(string[] options)
        {
            List<string> icons = new List<string>();
            foreach (string option in options)
            {
                if (option.StartsWith("Emoji"))
                {
                    if (UIManager.AnimatedEmojis.Contains(option))
                        icons.Add(ResourcePaths.UI + "/Icons/Emotes/" + option + "_0");
                    else
                        icons.Add(ResourcePaths.UI + "/Icons/Emotes/" + option);
                }
                else
                    icons.Add(ResourcePaths.UI + "/Icons/Profile/" + option + "Icon");
            }
            return icons.ToArray();
        }
    }
}
