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
using Map;
using Utility;

namespace UI
{
    class ProfileIconPickPopup : PromptPopup
    {
        protected override string Title => string.Empty;
        protected override float Width => 1000f;
        protected override float Height => 800f;
        protected override bool ScrollBar => true;
        protected const int Columns = 7;
        protected override float VerticalSpacing => 10f;
        protected override int HorizontalPadding => 10;
        protected override int VerticalPadding => 10;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(fontSize: 24, themePanel: ThemePanel, titleWidth: 70f);
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Back"),
                    onClick: () => OnBottomBarButtonClick("Back"));
            var rows = Util.GroupItems(UIManager.AvailableProfileIcons, Columns);
            foreach (var row in rows)
                CreateRow(row);
        }

        private void OnBottomBarButtonClick(string name)
        {
            Hide();
        }

        protected void CreateRow(List<string> items)
        {
            var group = ElementFactory.CreateHorizontalGroup(SinglePanel, VerticalSpacing, TextAnchor.MiddleLeft);
            foreach (var item in items)
            {
                string icon = item + "Icon";
                var obj = ElementFactory.InstantiateAndBind(group.transform, "Prefabs/Misc/MapEditorObjectButton");
                obj.GetComponent<Button>().onClick.AddListener(() => OnSelectObject(item));
                try
                {
                    var texture = (Texture2D)ResourceManager.LoadAsset(ResourcePaths.UI, "Icons/Profile/" + icon);
                    obj.transform.Find("Icon").GetComponent<RawImage>().texture = texture;
                }
                catch
                {
                    Debug.Log("Failed to load icon: " + icon);
                }
                obj.transform.Find("Text").GetComponent<Text>().text = item;
                obj.transform.Find("Text").GetComponent<Text>().color = UIManager.GetThemeColor("DefaultPanel", "DefaultLabel", "TextColor");
            }
        }

        protected virtual void OnSelectObject(string name)
        {
            SettingsManager.ProfileSettings.ProfileIcon.Value = name;
            ((MainMenu)UIManager.CurrentMenu)._editProfilePopup.RebuildCategoryPanel();
            Hide();
        }
    }
}
