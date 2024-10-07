using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using Settings;
using System.Collections;
using ApplicationManagers;
using GameManagers;
using Characters;

namespace UI
{
    class SnapshotViewerMainPanel: HeadedPanel
    {
        protected override string Title => UIManager.GetLocale("MainMenu", "ToolsPopup", "SnapshotViewerButton");
        protected override float Width => 280f;
        protected override float Height => 270f;
        protected override float VerticalSpacing => 20f;
        protected override int HorizontalPadding => 25;
        protected override int VerticalPadding => 25;
        private SnapshotViewerMenu _menu;
        private Text _indexLabel;
        private int _index;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            _menu = (SnapshotViewerMenu)UIManager.CurrentMenu;
            ElementStyle style = new ElementStyle(titleWidth: 95f, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Back"), onClick: () => OnButtonClick("Back"));
            _indexLabel = ElementFactory.CreateDefaultLabel(SinglePanel, style, "").GetComponent<Text>();
            UpdateIndexLabel();
            Transform group = ElementFactory.CreateHorizontalGroup(SinglePanel, 10f, TextAnchor.MiddleCenter).transform;
            ElementFactory.CreateDefaultButton(group, style, UIManager.GetLocaleCommon("Previous"), onClick: () => OnButtonClick("Previous"));
            ElementFactory.CreateDefaultButton(group, style, UIManager.GetLocaleCommon("Next"), onClick: () => OnButtonClick("Next"));
            ElementFactory.CreateDefaultButton(group, style, UIManager.GetLocaleCommon("Save"), onClick: () => OnButtonClick("Save"));
        }

        private void OnButtonClick(string name)
        {
            if (name == "Back")
                SceneLoader.LoadScene(SceneName.MainMenu);
            else if (name == "Save")
                _menu.Save();
            else if (name == "Previous")
            {
                if (_index > 0)
                {
                    _index--;
                    UpdateIndexLabel();
                    _menu.LoadSnapshot(_index);
                }
            }
            else if (name == "Next")
            {
                if (_index < SnapshotManager.GetLength() - 1)
                {
                    _index++;
                    UpdateIndexLabel();
                    _menu.LoadSnapshot(_index);
                }
            }
        }

        private void UpdateIndexLabel()
        {
            if (SnapshotManager.GetLength() == 0)
                _indexLabel.text = "0/0";
            else
                _indexLabel.text = (_index + 1).ToString() + "/" + SnapshotManager.GetLength().ToString();
        }
    }
}
