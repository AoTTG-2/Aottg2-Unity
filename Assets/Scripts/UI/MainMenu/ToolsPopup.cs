using ApplicationManagers;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class ToolsPopup: BasePopup
    {
        protected override string Title => UIManager.GetLocale("MainMenu", "ToolsPopup", "Title");
        protected override float Width => 280f;
        protected override float Height => 375f;
        protected override float VerticalSpacing => 20f;
        protected override int VerticalPadding => 20;
        protected override bool UseSound => true;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            string cat = "MainMenu";
            string sub = "ToolsPopup";
            float width = 220f;
            ElementStyle style = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Back"), onClick: () => OnButtonClick("Back"));
            ElementFactory.CreateTextButton(SinglePanel, style, UIManager.GetLocale(cat, sub, "MapEditorButton"), width, onClick: () => OnButtonClick("MapEditor"));
            ElementFactory.CreateTextButton(SinglePanel, style, UIManager.GetLocale(cat, sub, "CharacterEditorButton"), width, onClick: () => OnButtonClick("CharacterEditor"));
            ElementFactory.CreateTextButton(SinglePanel, style, UIManager.GetLocale(cat, sub, "SnapshotViewerButton"), width, onClick: () => OnButtonClick("SnapshotViewer"));
            ElementFactory.CreateTextButton(SinglePanel, style, UIManager.GetLocale(cat, sub, "GalleryButton"), width, onClick: () => OnButtonClick("Gallery"));
        }

        protected void OnButtonClick(string name)
        {
            if (name == "MapEditor")
                SceneLoader.LoadScene(SceneName.MapEditor);
            else if (name == "CharacterEditor")
                SceneLoader.LoadScene(SceneName.CharacterEditor);
            else if (name == "SnapshotViewer")
                SceneLoader.LoadScene(SceneName.SnapshotViewer);
            else if (name == "Gallery")
                SceneLoader.LoadScene(SceneName.Gallery);
            else if (name == "Back")
                Hide();
        }
    }
}
