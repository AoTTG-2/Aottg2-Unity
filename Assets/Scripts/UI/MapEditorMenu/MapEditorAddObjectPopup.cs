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

namespace UI
{
    class MapEditorAddObjectPopup : BasePopup
    {
        protected override string Title => string.Empty;
        protected override float Width => 1000f;
        protected override float Height => 800f;
        protected override bool CategoryPanel => true;
        protected override bool CategoryButtons => true;
        protected override string DefaultCategoryPanel => "General";
        public StringSetting Search = new StringSetting(string.Empty);
        private InputSettingElement _searchInput;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(fontSize: 24, themePanel: ThemePanel, titleWidth: 70f);
            _searchInput = ElementFactory.CreateInputSetting(BottomBar, style, Search, "Search", elementWidth: 200f, onEndEdit: () => RebuildCategoryPanel())
                .GetComponent<InputSettingElement>();
            _searchInput.GetComponent<HorizontalLayoutGroup>().spacing = 5f;
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Back"),
                    onClick: () => OnBottomBarButtonClick("Back"));
            BottomBar.GetComponent<HorizontalLayoutGroup>().spacing = Width - 365f;
        }

        protected virtual string[] GetCategories()
        {
            return new string[] { "All", "General", "Interact", "Geometry", "Buildings", "Nature", "Decor", "Arenas" };
        }

        protected override void SetupTopButtons()
        {
            ElementStyle style = new ElementStyle(fontSize: 24, themePanel: ThemePanel);
            foreach (string buttonName in GetCategories())
            {
                GameObject obj = ElementFactory.CreateCategoryButton(TopBar, style, buttonName,
                    onClick: () => OnTopBarButtonClick(buttonName));
                _topButtons.Add(buttonName, obj.GetComponent<Button>());
            }
            base.SetupTopButtons();
        }

        protected void OnTopBarButtonClick(string name)
        {
            Search.Value = string.Empty;
            if (_searchInput != null)
                _searchInput.SyncElement();
            SetCategoryPanel(name);
        }

        protected override void RegisterCategoryPanels()
        {
            foreach (string buttonName in _topButtons.Keys)
                _categoryPanelTypes.Add(buttonName, typeof(MapEditorAddObjectPanel));
        }

        private void OnBottomBarButtonClick(string name)
        {
            Hide();
        }
    }
}
