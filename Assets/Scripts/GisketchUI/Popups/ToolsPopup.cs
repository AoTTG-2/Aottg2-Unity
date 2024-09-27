using UnityEngine;
using UnityEngine.Events;

namespace GisketchUI
{
    public class ToolsPopup : BasePopup
    {
        protected override string Title => "Tools";
        protected override float Width => 480f;

        protected override void SetupContent()
        {
            CreateButton("Map Editor", () => OnButtonClick("MapEditor"));
            CreateButton("Character Editor", () => OnButtonClick("CharacterEditor"));
            CreateButton("Snapshot Viewer", () => OnButtonClick("SnapshotViewer"));
            CreateButton("Gallery", () => OnButtonClick("Gallery"));
            CreateButton("Back", () => OnButtonClick("Back"), Button.ButtonVariant.Red);
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
                    Debug.Log("Load Map Editor Scene");
                    break;
                case "CharacterEditor":
                    Debug.Log("Load Character Editor Scene");
                    break;
                case "SnapshotViewer":
                    Debug.Log("Load Snapshot Viewer Scene");
                    break;
                case "Gallery":
                    Debug.Log("Load Gallery Scene");
                    break;
                case "Back":
                    Hide();
                    break;
            }
        }
    }
}