using Settings;
using System.Collections.Generic;
using UnityEngine.UI;
using Utility;
using UnityEngine;

namespace UI
{
    class EditProfileProfilePanel: CategoryPanel
    {
        protected override bool ScrollBar => true;
        GameObject _profileImage;
        Transform _group;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ProfileSettings settings = SettingsManager.ProfileSettings;
            ElementStyle style = new ElementStyle(titleWidth: 100f, themePanel: ThemePanel);
            _group = ElementFactory.CreateHorizontalGroup(SinglePanel, 60f, UnityEngine.TextAnchor.MiddleLeft).transform;
            string[] options = UIManager.AvailableProfileIcons.ToArray();
            string[] icons = GetProfileIconPaths(options);
            ElementFactory.CreateIconPickSetting(_group, style, settings.ProfileIcon, UIManager.GetLocaleCommon("Icon"), options, icons,
                UIManager.CurrentMenu.IconPickPopup, elementWidth: 180f, elementHeight: 40f, onSelect: () => CreateProfileImage());
            CreateProfileImage();
            CreateHorizontalDivider(SinglePanel);
            ElementFactory.CreateInputSetting(SinglePanel, style, settings.Name, UIManager.GetLocaleCommon("Name"), elementWidth: 260f);
            ElementFactory.CreateInputSetting(SinglePanel, style, settings.Guild, UIManager.GetLocaleCommon("Guild"), elementWidth: 260f);
            ElementFactory.CreateInputSetting(SinglePanel, style, settings.Social, UIManager.GetLocaleCommon("Social"), elementWidth: 260f);
            ElementFactory.CreateInputSetting(SinglePanel, style, settings.About, UIManager.GetLocaleCommon("About"), elementWidth: 260f, elementHeight: 120f, 
                multiLine: true);
        }

        private void CreateProfileImage()
        {
            ElementStyle style = new ElementStyle(titleWidth: 100f, themePanel: ThemePanel);
            ProfileSettings settings = SettingsManager.ProfileSettings;
            if (_profileImage != null)
                Destroy(_profileImage);
            _profileImage = ElementFactory.CreateRawImage(_group, style, "Icons/Profile/" + UIManager.GetProfileIcon(settings.ProfileIcon.Value), 256, 256);

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
