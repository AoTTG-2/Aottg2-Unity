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
using Map;

namespace UI
{
    class MapEditorAddVariantPopup : BasePopup
    {
        protected override string Title => "Choose Variant";
        protected override float Width => 1155f;
        protected override float Height => 865f;
        protected override float TopBarHeight => 65f;
        protected override bool ScrollBar => true;
        protected const int Columns = 8;
        protected override float VerticalSpacing => 12f;
        protected override int HorizontalPadding => 15;
        protected override int VerticalPadding => 10;
        protected List<GameObject> _rows = new List<GameObject>();

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(fontSize: 24, themePanel: ThemePanel, titleWidth: 70f);
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Back"),
                    onClick: () => OnBottomBarButtonClick("Back"));
        }

        private void OnBottomBarButtonClick(string name)
        {
            Hide();
        }

        public void Show(string prefab)
        {
            foreach (var row in _rows)
                Destroy(row);
            var variants = new List<string>();
            variants.Add(prefab);
            if (BuiltinMapPrefabs.PrefabVariants.ContainsKey(prefab))
                variants.AddRange(BuiltinMapPrefabs.PrefabVariants[prefab]);
            var rows = Util.GroupItems(variants, Columns);
            foreach (var row in rows)
                CreateRow(row);
            base.Show();
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
            _rows.Add(group);
        }

        protected virtual string GetPreviewName(string item)
        {
            if (BuiltinMapPrefabs.PrefabPreviews.ContainsKey(item))
                return BuiltinMapPrefabs.PrefabPreviews[item] + "Preview";
            return item + "Preview";
        }

        protected virtual void OnSelectObject(string name)
        {
            ((MapEditorGameManager)SceneLoader.CurrentGameManager).AddObject(name);
            Hide();
        }
    }
}
