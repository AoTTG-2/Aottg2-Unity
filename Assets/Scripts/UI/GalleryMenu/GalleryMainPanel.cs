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
    class GalleryMainPanel : HeadedPanel
    {
        protected override string Title => UIManager.GetLocale("MainMenu", "ToolsPopup", "GalleryButton");
        protected override float Width => 1960f;
        protected override float Height => 60f;

        protected override float TopBarHeight => 0f;
        protected override float BottomBarHeight => 0f;
        protected override float VerticalSpacing => 0f;
        protected override int HorizontalPadding => 40;
        protected override int VerticalPadding => 10;
        private GalleryMenu _menu;
        private Text _indexLabel;
        private int _index;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            _menu = (GalleryMenu)UIManager.CurrentMenu;
            ElementStyle style = new ElementStyle(titleWidth: 95f, themePanel: ThemePanel);
            Transform group = ElementFactory.CreateHorizontalGroup(SinglePanel, 10f, TextAnchor.MiddleLeft).transform;
            _indexLabel = ElementFactory.CreateDefaultLabel(group, style, "").GetComponent<Text>();
            UpdateIndexLabel();
            ElementFactory.CreateDefaultButton(group, style, UIManager.GetLocaleCommon("Previous"), onClick: () => OnButtonClick("Previous"));
            ElementFactory.CreateDefaultButton(group, style, UIManager.GetLocaleCommon("Next"), onClick: () => OnButtonClick("Next"));
            ElementFactory.CreateDefaultButton(group, style, UIManager.GetLocaleCommon("Back"), onClick: () => OnButtonClick("Back"));
        }

        private void OnButtonClick(string name)
        {
            if (name == "Back")
                SceneLoader.LoadScene(SceneName.MainMenu);
            else if (name == "Previous")
            {
                if (_index > 0)
                {
                    _index--;
                    UpdateIndexLabel();
                    _menu.LoadGallery(_index);
                }
            }
            else if (name == "Next")
            {
                if (_index < _menu.TotalBackgroundCount - 1)
                {
                    _index++;
                    UpdateIndexLabel();
                    _menu.LoadGallery(_index);
                }
            }
        }

        private void UpdateIndexLabel()
        {
            _indexLabel.text = (_index + 1).ToString() + "/" + _menu.TotalBackgroundCount.ToString();
        }
    }
}
