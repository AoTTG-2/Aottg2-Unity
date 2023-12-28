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
using Utility;

namespace UI
{
    class MapEditorAddObjectPopup : BasePopup
    {
        protected override string Title => string.Empty;
        protected override float Width => 1000f;
        protected override float Height => 865f;
        protected override bool CategoryPanel => true;
        protected override bool CategoryButtons => true;
        protected override float TopBarHeight => 130f;
        protected override string DefaultCategoryPanel => "General";
        public StringSetting Search = new StringSetting(string.Empty);
        private InputSettingElement _searchInput;
        protected virtual bool TwoRows => true;

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
            return new string[] { "All", "General", "Interact", "Geometry", "Buildings", "Nature", "Decor", 
                "Arenas", "Custom" };
        }

        public override float GetPanelVerticalOffset()
        {
            if (TwoRows)
                return -32.5f;
            return 0f;
        }

        protected override void SetupTopButtons()
        {
            ElementStyle style = new ElementStyle(fontSize: 24, themePanel: ThemePanel);
            if (TwoRows)
            {
                DestroyImmediate(TopBar.GetComponent<HorizontalLayoutGroup>());
                var layout = TopBar.gameObject.AddComponent<VerticalLayoutGroup>();
                layout.childAlignment = TextAnchor.MiddleCenter;
                layout.childControlHeight = true;
                layout.childControlWidth = true;
                layout.childForceExpandHeight = true;
                layout.childForceExpandWidth = true;
                layout.spacing = -30f;
                var row1 = CreateRow();
                var row2 = CreateRow();
                var groups = Util.GroupBuckets(GetCategories().ToList(), 2);
                foreach (string buttonName in groups[0])
                {
                    GameObject obj = ElementFactory.CreateCategoryButton(row1.transform, style, buttonName,
                        onClick: () => OnTopBarButtonClick(buttonName));
                    _topButtons.Add(buttonName, obj.GetComponent<Button>());
                }
                foreach (string buttonName in groups[1])
                {
                    GameObject obj = ElementFactory.CreateCategoryButton(row2.transform, style, buttonName,
                        onClick: () => OnTopBarButtonClick(buttonName));
                    _topButtons.Add(buttonName, obj.GetComponent<Button>());
                }
                Canvas.ForceUpdateCanvases();
                row1.GetComponent<HorizontalLayoutGroup>().spacing = 80f;
                row2.GetComponent<HorizontalLayoutGroup>().spacing = 80f;
            }
            else
            {
                foreach (string buttonName in GetCategories())
                {
                    GameObject obj = ElementFactory.CreateCategoryButton(TopBar, style, buttonName,
                        onClick: () => OnTopBarButtonClick(buttonName));
                    _topButtons.Add(buttonName, obj.GetComponent<Button>());
                }
                base.SetupTopButtons();
            }
        }

        protected GameObject CreateRow()
        {
            var row = new GameObject();
            row.transform.SetParent(TopBar);
            var layout = row.AddComponent<HorizontalLayoutGroup>();
            layout.childAlignment = TextAnchor.MiddleCenter;
            layout.childControlHeight = true;
            layout.childControlWidth = true;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = false;
            return row;
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
