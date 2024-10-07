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
    class CreateGameSelectMapPopup : BasePopup
    {
        protected override string Title => string.Empty;
        protected override float Width => 1095f;
        protected override float Height => 1000f;
        protected override bool CategoryPanel => true;
        protected override bool CategoryButtons => true;
        protected override string DefaultCategoryPanel => "General";

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(fontSize: 24, themePanel: ThemePanel, titleWidth: 70f);
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Back"),
                    onClick: () => OnBottomBarButtonClick("Back"));
        }

        protected virtual string[] GetCategories()
        {
            return new string[] { "General", "Mission", "PVP", "Cage Fight", "Racing Basic", "Racing Hard", "Custom" };
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
            SetCategoryPanel(name);
        }

        protected override void RegisterCategoryPanels()
        {
            foreach (string buttonName in _topButtons.Keys)
                _categoryPanelTypes.Add(buttonName, typeof(CreateGameSelectMapPanel));
        }

        private void OnBottomBarButtonClick(string name)
        {
            Hide();
        }
    }
}
