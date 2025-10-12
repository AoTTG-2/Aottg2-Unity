using ApplicationManagers;
using Settings;

namespace UI
{
    class TutorialPopup : BasePopup
    {
        protected override string Title => UIManager.GetLocale("MainMenu", "TutorialPopup", "Title");
        protected override float Width => 280f;
        protected override float Height => 270f;
        protected override float VerticalSpacing => 20f;
        protected override int VerticalPadding => 20;
        protected override bool UseSound => true;

        private string _videoTutorials = "https://www.youtube.com/watch?v=ceehjYiOTYs";

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            string cat = "MainMenu";
            string sub = "TutorialPopup";
            float width = 220f;
            ElementStyle style = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Back"), onClick: () => OnButtonClick("Back"));
            ElementFactory.CreateTextButton(SinglePanel, style, UIManager.GetLocale(cat, sub, "BasicButton"), width, onClick: () => OnButtonClick("Basic"));
            ElementFactory.CreateTextButton(SinglePanel, style, UIManager.GetLocale(cat, sub, "VideoButton"), width, onClick: () => OnButtonClick("VideoTutorials"));
        }

        protected void OnButtonClick(string name)
        {
            if (name == "Basic")
            {
                MusicManager.PlayEffect();
                MusicManager.PlayTransition();
                SettingsManager.InGameUI.SetDefault();
                SettingsManager.InGameUI.General.MapCategory.Value = "Tutorial";
                SettingsManager.InGameUI.General.MapName.Value = "Basic Tutorial";
                SettingsManager.InGameUI.General.GameMode.Value = "Map Logic";
                SettingsManager.InGameUI.Misc.AllowPlayerTitans.Value = false;
                SettingsManager.InGameUI.Misc.EndlessRespawnEnabled.Value = true;
                SettingsManager.InGameUI.Misc.EndlessRespawnTime.Value = 1f;
                SettingsManager.InGameCurrent.Copy(SettingsManager.InGameUI);
                SettingsManager.MultiplayerSettings.ConnectOffline();
                SettingsManager.MultiplayerSettings.StartRoom();
            }
            else if (name == "VideoTutorials")
            {
                UIManager.CurrentMenu.ExternalLinkPopup.Show(_videoTutorials);
            }
            else if (name == "Back")
                Hide();
        }
    }
}
