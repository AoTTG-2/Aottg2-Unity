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
using UnityEngine.SceneManagement;

namespace UI
{
    class CreateGameSelectMapPanel : CategoryPanel
    {
        protected override bool ScrollBar => true;
        protected const int Columns = 4;
        protected override float VerticalSpacing => 10f;
        protected override int HorizontalPadding => 15;
        protected override int VerticalPadding => 10;
        private bool IsCustom = false;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            var popup = (CreateGameSelectMapPopup)parent;
            string category = popup.GetCurrentCategoryName();
            IsCustom = category == "Custom";
            List<string> maps = GetItems(category);
            var rows = Util.GroupItems(maps, Columns);
            foreach (var row in rows)
                CreateRow(row);
        }

        protected virtual List<string> GetItems(string category)
        {
            return BuiltinLevels.GetMapNames(category).ToList();
        }

        protected void CreateRow(List<string> items)
        {
            var group = ElementFactory.CreateHorizontalGroup(SinglePanel, VerticalSpacing, TextAnchor.MiddleLeft);
            foreach (var item in items)
            {
                var obj = ElementFactory.InstantiateAndBind(group.transform, "Prefabs/Misc/MapSelectObjectButton");
                obj.GetComponent<Button>().onClick.AddListener(() => OnSelectObject(item));
                object texture = null;
                if (!IsCustom)
                {
                    string preview = GetPreviewName(item);
                    texture = ResourceManager.LoadAsset(ResourcePaths.BuiltinMaps, "Previews/" + preview);
                }
                if (texture != null)
                    obj.transform.Find("Icon").GetComponent<RawImage>().texture = (Texture2D)texture;
                else
                    obj.transform.Find("Icon").GetComponent<RawImage>().color = new Color(0.32f, 0.32f, 0.32f);
                obj.transform.Find("Text").GetComponent<Text>().text = item;
                obj.transform.Find("Text").GetComponent<Text>().color = UIManager.GetThemeColor("DefaultPanel", "DefaultLabel", "TextColor");
            }
        }

        protected virtual string GetPreviewName(string item)
        {
            return item + "Preview";
        }

        protected virtual void OnSelectObject(string name)
        {
            var popup = (CreateGameSelectMapPopup)Parent;
            string category = popup.GetCurrentCategoryName();
            SettingsManager.InGameUI.General.MapCategory.Value = category;
            SettingsManager.InGameUI.General.MapName.Value = name;
            if (SceneLoader.SceneName == SceneName.InGame)
                ((InGameMenu)UIManager.CurrentMenu)._createGamePopup.RebuildCategoryPanel();
            else
                ((MainMenu)UIManager.CurrentMenu)._createGamePopup.RebuildCategoryPanel();
            Parent.Hide();
        }
    }
}
