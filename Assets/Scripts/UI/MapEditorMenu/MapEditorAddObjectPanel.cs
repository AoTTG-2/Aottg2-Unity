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
    class MapEditorAddObjectPanel : CategoryPanel
    {
        protected override bool ScrollBar => true;
        protected const int Columns = 7;
        protected override float VerticalSpacing => 10f;
        protected override int HorizontalPadding => 15;
        protected override int VerticalPadding => 10;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            var popup = (MapEditorAddObjectPopup)parent;
            string category = popup.GetCurrentCategoryName();
            string search = popup.Search.Value;
            List<string> prefabs;
            if (search == string.Empty)
                prefabs = GetItems(category);
            else
            {
                prefabs = GetItems("All");
                prefabs = Filter(prefabs, search);
            }
            var rows = Util.GroupItems(prefabs, Columns);
            foreach (var row in rows)
                CreateRow(row);
        }

        protected virtual List<string> GetItems(string category)
        {
            if (category == "All")
                return BuiltinMapPrefabs.AllPrefabs.Keys.ToList();
            else
                return BuiltinMapPrefabs.PrefabCategories[category].Select(x => x.Name).ToList();
        }

        protected List<string> Filter(List<string> original, string search)
        {
            var result = new List<string>();
            if (search == string.Empty)
                return original;
            search = search.ToLower();
            foreach (var item in original)
            {
                if (item.ToLower().Contains(search))
                    result.Add(item);
            }
            return result;
        }

        protected void CreateRow(List<string> items)
        {
            var group = ElementFactory.CreateHorizontalGroup(SinglePanel, VerticalSpacing, TextAnchor.MiddleLeft);
            foreach (var item in items)
            {
                var obj = ElementFactory.InstantiateAndBind(group.transform, "Prefabs/Misc/MapEditorObjectButton");
                obj.GetComponent<Button>().onClick.AddListener(() => OnSelectObject(item));
                string preview = GetPreviewName(item);
                var texture = ResourceManager.LoadAsset(ResourcePaths.Map, "Previews/" + preview);
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
            ((MapEditorGameManager)SceneLoader.CurrentGameManager).AddObject(name);
            Parent.Hide();
        }
    }
}
