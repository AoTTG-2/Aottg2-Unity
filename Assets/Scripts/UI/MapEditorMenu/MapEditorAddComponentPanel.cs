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
using Photon.Pun.Demo.Asteroids;
using UnityEditor.Rendering;
using UnityEngine.Events;
using Photon.Pun.Demo.PunBasics;
using CustomLogic;
using Unity.VisualScripting;

namespace UI
{
    class MapEditorAddComponentPanel : CategoryPanel
    {
        protected override bool ScrollBar => true;
        protected override int HorizontalPadding => 0;
        protected override int VerticalPadding => 5;
        protected virtual float ItemButtonWidth => 450f;
        protected override float VerticalSpacing => 5f;
        protected virtual int ItemFontSize => 18;
        protected virtual float DeleteButtonSize => 20f;
        protected GameObject _noItemsLabel;
        protected List<GameObject> _itemButtons = new List<GameObject>();
        protected override string ThemePanel => "SelectListPopup";

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            var popup = (MapEditorAddComponentPopup)parent;
            string category = popup.GetCurrentCategoryName();
            string search = popup.Search.Value;
            List<string> components;
            _noItemsLabel = ElementFactory.CreateDefaultLabel(SinglePanel, new ElementStyle(themePanel: ThemePanel), "No items found.");
            var gameManager = (MapEditorGameManager)SceneLoader.CurrentGameManager;
            if (gameManager.LogicEvaluator != null)
            {
                components = GetItems(category);
                if (search != string.Empty)
                    components = Filter(components, search);
                CreateList(components);
            }
        }

        protected virtual List<string> GetItems(string category)
        {
            var gameManager = (MapEditorGameManager)SceneLoader.CurrentGameManager;
            List<string> components = new List<string>();
            var allComponents = gameManager.LogicEvaluator.GetComponentNames();
            allComponents.Sort();
            if (category == "All")
                return allComponents;
            else if (category == "General")
            {
                foreach (var component in allComponents)
                {
                    if (CustomLogicManager.GeneralComponents.Contains(component))
                        components.Add(component);
                }
            }
            else if (category == "Internal")
            {
                foreach (var component in allComponents)
                {
                    if (CustomLogicManager.InternalComponents.Contains(component))
                        components.Add(component);
                }
            }
            else if (category == "Custom")
            {
                foreach (var component in allComponents)
                {
                    if (!CustomLogicManager.InternalComponents.Contains(component) && !CustomLogicManager.GeneralComponents.Contains(component))
                        components.Add(component);
                }
            }
            return components;
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

        protected void CreateList(List<string> items)
        {
            foreach (string item in items)
            {
                GameObject button = ElementFactory.InstantiateAndBind(SinglePanel, "Prefabs/Misc/SelectListButton");
                // _itemButtons.Add(button);
                Button itemButton = button.transform.Find("ItemButton").GetComponent<Button>();
                Button deleteButton = button.transform.Find("DeleteButton").GetComponent<Button>();
                itemButton.onClick.AddListener(() => OnItemClick(item));
                itemButton.transform.Find("Text").GetComponent<Text>().text = item;
                itemButton.transform.Find("Text").GetComponent<Text>().fontSize = ItemFontSize;
                itemButton.colors = UIManager.GetThemeColorBlock(ThemePanel, "ItemButton", "");
                itemButton.transform.Find("Text").GetComponent<Text>().color = UIManager.GetThemeColor(ThemePanel, "ItemButton", "TextColor");
                itemButton.GetComponent<LayoutElement>().preferredWidth = ItemButtonWidth + DeleteButtonSize + 10f;
                deleteButton.gameObject.SetActive(false);
            }
            if (items.Count > 0)
                _noItemsLabel.SetActive(false);
            else
                _noItemsLabel.SetActive(true);
        }

        private void OnItemClick(string name)
        {
            var parent = (MapEditorAddComponentPopup)Parent;
            parent.OnSelectItem(name);
        }
    }
}
