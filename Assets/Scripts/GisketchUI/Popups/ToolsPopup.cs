using ApplicationManagers;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace GisketchUI
{
    public class ToolsPopup : BasePopup
    {
        protected override string Title => UIManager.GetLocale("MainMenu", "ToolsPopup", "Title");
        protected override float Width => 480f;

        protected override void SetupContent()
        {
            string cat = "MainMenu";
            string sub = "ToolsPopup";

            CreateButton(UIManager.GetLocale(cat, sub, "MapEditorButton"), () => OnButtonClick("MapEditor"));
            CreateButton(UIManager.GetLocale(cat, sub, "CharacterEditorButton"), () => OnButtonClick("CharacterEditor"));
            CreateButton(UIManager.GetLocale(cat, sub, "SnapshotViewerButton"), () => OnButtonClick("SnapshotViewer"));
            CreateButton(UIManager.GetLocale(cat, sub, "GalleryButton"), () => OnButtonClick("Gallery"));
            CreateButton(UIManager.GetLocaleCommon("Back"), () => OnButtonClick("Back"), Button.ButtonVariant.Red);
        }

        private void CreateButton(string label, UnityAction onClick, Button.ButtonVariant variant = Button.ButtonVariant.Secondary)
        {
            Button button = ElementFactory.CreateButton(contentView.rectTransform, label, onClick, variant);
            contentView.AddElement(button);
        }

        protected void OnButtonClick(string name)
        {
            switch (name)
            {
                case "MapEditor":
                    SceneLoader.LoadScene(SceneName.MapEditor);
                    break;
                case "CharacterEditor":
                    SceneLoader.LoadScene(SceneName.CharacterEditor);
                    break;
                case "SnapshotViewer":
                    SceneLoader.LoadScene(SceneName.SnapshotViewer);
                    break;
                case "Gallery":
                    SceneLoader.LoadScene(SceneName.Gallery);
                    break;
                case "Back":
                    Hide();
                    break;
            }
            Hide();
        }
    }
}