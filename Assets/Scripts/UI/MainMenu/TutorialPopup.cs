using ApplicationManagers;
using Settings;

namespace UI
{
    class TutorialPopup : BasePopup
    {
        protected override string Title => UIManager.GetLocale("MainMenu", "TutorialPopup", "Title");
        protected override float Width => 500;
        protected override float Height => 380;
        protected override float VerticalSpacing => 20f;
        protected override int VerticalPadding => 20;
        protected override bool UseSound => true;

        private string _basicTutorials = "https://youtube.com/playlist?list=PL7WLQbO_kV5D9LaXN50370-OzXfv33oM7&si=1V3Uza7ppikW0TTt";
        private string _advancedTutorials = "https://youtube.com/playlist?list=PL7WLQbO_kV5CPCIXIQaAz3fJaYiugh1-F&si=Nf1tZhX1xgxYIh9I";
        private string _miscTutorials = "https://youtube.com/playlist?list=PL7WLQbO_kV5Bt1srvwS1KJhTLi8usCuNe&si=i_YhJPqYgop_lWaH";

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            string cat = "MainMenu";
            string sub = "TutorialPopup";
            float width = 450;
            ElementStyle style = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Back"), onClick: () => OnButtonClick("Back"));
            ElementFactory.CreateTextButton(SinglePanel, style, UIManager.GetLocale(cat, sub, "BasicButton"), width, onClick: () => OnButtonClick("Basic"));
            ElementFactory.CreateTextButton(SinglePanel, style, UIManager.GetLocale(cat, sub, "VideoBasicTutorialsButton"), width, onClick: () => OnButtonClick("BasicVideoTutorials"));
            ElementFactory.CreateTextButton(SinglePanel, style, UIManager.GetLocale(cat, sub, "VideoAdvancedTutorialsButton"), width, onClick: () => OnButtonClick("AdvancedVideoTutorials"));
            ElementFactory.CreateTextButton(SinglePanel, style, UIManager.GetLocale(cat, sub, "VideoMiscTutorialsButton"), width, onClick: () => OnButtonClick("MiscVideoTutorials"));
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
            else if (name == "BasicVideoTutorials")
            {
                UIManager.CurrentMenu.ExternalLinkPopup.Show(_basicTutorials);
            }
            else if (name == "AdvancedVideoTutorials")
            {
                UIManager.CurrentMenu.ExternalLinkPopup.Show(_advancedTutorials);
            }
            else if (name == "MiscVideoTutorials")
            {
                UIManager.CurrentMenu.ExternalLinkPopup.Show(_miscTutorials);
            }
            else if (name == "Back")
                Hide();
        }
    }
}
