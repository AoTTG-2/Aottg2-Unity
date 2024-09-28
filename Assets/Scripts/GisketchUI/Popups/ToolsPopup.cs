using ApplicationManagers;
using UnityEngine;
using UnityEngine.Events;

namespace GisketchUI
{
    public class ToolsPopup : BasePopup
    {
        protected override string Title => UI.UIManager.GetLocale("MainMenu", "ToolsPopup", "Title");
        protected override float Width => 480f;

        protected override void SetupContent()
        {
            string cat = "MainMenu";
            string sub = "ToolsPopup";

            CreateButton(UI.UIManager.GetLocale(cat, sub, "MapEditorButton"), () => OnButtonClick("MapEditor"));
            CreateButton(UI.UIManager.GetLocale(cat, sub, "CharacterEditorButton"), () => OnButtonClick("CharacterEditor"));
            CreateButton(UI.UIManager.GetLocale(cat, sub, "SnapshotViewerButton"), () => OnButtonClick("SnapshotViewer"));
            CreateButton(UI.UIManager.GetLocale(cat, sub, "GalleryButton"), () => OnButtonClick("Gallery"));
            CreateButton(UI.UIManager.GetLocaleCommon("Back"), () => OnButtonClick("Back"), ActionButton.ButtonVariant.Red);
        }

        private void CreateButton(string label, UnityAction onClick, ActionButton.ButtonVariant variant = ActionButton.ButtonVariant.Secondary)
        {
            ActionButton button = ElementFactory.CreateButton(contentView.rectTransform, label, onClick, variant);
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