using Settings;
using UnityEngine.UI;

namespace UI
{
    class EditProfileProfilePanel: CategoryPanel
    {
        protected override bool ScrollBar => true;
        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ProfileSettings settings = SettingsManager.ProfileSettings;
            ElementStyle style = new ElementStyle(titleWidth: 100f, themePanel: ThemePanel);
            var group = ElementFactory.CreateHorizontalGroup(SinglePanel, 60f, UnityEngine.TextAnchor.MiddleLeft).transform;
            ElementFactory.CreateButtonSetting(group, style, settings.ProfileIcon, UIManager.GetLocaleCommon("Icon"),
                elementWidth: 180f, elementHeight: 40f, onClick: () => OnButtonClick("Icon"));
            ElementFactory.CreateRawImage(group, style, "Icons/Profile/" + UIManager.GetProfileIcon(settings.ProfileIcon.Value), 256, 256);
            CreateHorizontalDivider(SinglePanel);
            ElementFactory.CreateInputSetting(SinglePanel, style, settings.Name, UIManager.GetLocaleCommon("Name"), elementWidth: 260f);
            ElementFactory.CreateInputSetting(SinglePanel, style, settings.Guild, UIManager.GetLocaleCommon("Guild"), elementWidth: 260f);
            ElementFactory.CreateInputSetting(SinglePanel, style, settings.Social, UIManager.GetLocaleCommon("Social"), elementWidth: 260f);
            ElementFactory.CreateInputSetting(SinglePanel, style, settings.About, UIManager.GetLocaleCommon("About"), elementWidth: 260f, elementHeight: 120f, 
                multiLine: true);
        }

        private void OnButtonClick(string name)
        {
            if (name == "Icon")
            {
                ((MainMenu)UIManager.CurrentMenu)._profileIconPickPopup.Show();
            }
        }
    }
}
