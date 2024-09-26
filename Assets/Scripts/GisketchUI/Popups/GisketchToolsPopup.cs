using UnityEngine;
using UnityEngine.Events;

namespace GisketchUI
{
    public class GisketchToolsPopup : GisketchBasePopup
    {
        protected override string Title => "Tools";

        protected override void SetupContent()
        {
            CreateButton("Map Editor", () => OnButtonClick("MapEditor"));
            CreateButton("Character Editor", () => OnButtonClick("CharacterEditor"));
            CreateButton("Snapshot Viewer", () => OnButtonClick("SnapshotViewer"));
            CreateButton("Gallery", () => OnButtonClick("Gallery"));
            CreateButton("Back", () => OnButtonClick("Back"));
        }

        private void CreateButton(string label, UnityAction onClick)
        {
            GisketchButton button = GisketchElementFactory.CreateButton(contentView.rectTransform, label, onClick);
            contentView.AddElement(button);
        }

        protected void OnButtonClick(string name)
        {
            Debug.Log($"Button clicked: {name}");
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