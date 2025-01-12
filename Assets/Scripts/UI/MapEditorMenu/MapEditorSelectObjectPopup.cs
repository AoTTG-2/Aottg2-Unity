using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Settings;
using Map;
using System.Linq;

namespace UI
{


    class MapEditorSelectObjectPopup : BasePopup
    {
        protected override string Title => "Select Object";
        protected override float Width => 500f;
        protected override float Height => 590f;
        protected override int VerticalPadding => 20;
        protected override int HorizontalPadding => 20;

        protected virtual int ItemFontSize => 18;
        protected virtual float ItemButtonWidth => 450f;
        protected virtual float DeleteButtonSize => 20f;

        protected override string ThemePanel => "SelectListPopup";

        private MapEditorObjects _mapEditorObjects = new MapEditorObjects();

        private List<GameObject> _gameObjects = new List<GameObject>();

        private List<int> _selectedObjectIDs = new List<int>();



        public override void Setup(BasePanel parent = null)
        {

            base.Setup(parent);



            ElementStyle style = new ElementStyle(titleWidth: 100f, fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Save"), onClick: () => OnButtonClick("Save"));
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Cancel"), onClick: () => OnButtonClick("Cancel"));
            // ElementFactory.InstantiateAndBind(SinglePanel, "Prefabs/Misc/SelectListButton");

            Sync();
        }

        public void Sync()
        {
            _selectedObjectIDs.Clear();
            foreach (GameObject go in _gameObjects)
                Destroy(go);
            _gameObjects.Clear();
            _mapEditorObjects.Sync();

            Dictionary<int, string> items = _mapEditorObjects.GetAll();
            Debug.Log(items.Count);
            CreateList(items);
        }

        protected void CreateList(Dictionary<int, string> items)
        {

            foreach (int item in items.Keys)
            {
                string itemValue = items[item];
                Debug.Log("item:");
                Debug.Log(item);
                GameObject button = ElementFactory.InstantiateAndBind(SinglePanel, "Prefabs/Misc/SelectListButton");

                _gameObjects.Add(button);

                // _itemButtons.Add(button);
                Button itemButton = button.transform.Find("ItemButton").GetComponent<Button>();
                Button deleteButton = button.transform.Find("DeleteButton").GetComponent<Button>();
                itemButton.onClick.AddListener(() => OnItemClick(item));
                itemButton.transform.Find("Text").GetComponent<Text>().text = itemValue;
                itemButton.transform.Find("Text").GetComponent<Text>().fontSize = ItemFontSize;
                itemButton.colors = UIManager.GetThemeColorBlock(ThemePanel, "ItemButton", "");
                itemButton.transform.Find("Text").GetComponent<Text>().color = UIManager.GetThemeColor(ThemePanel, "ItemButton", "TextColor");
                itemButton.GetComponent<LayoutElement>().preferredWidth = ItemButtonWidth + DeleteButtonSize + 10f;
                deleteButton.gameObject.SetActive(false);


            }

        }

        private void OnItemClick(int objectID)
        {
            if (_selectedObjectIDs.Contains(objectID))
            {
                _selectedObjectIDs.Remove(objectID);
            }
            else
            {
                _selectedObjectIDs.Add(objectID);
            }
        }

        public List<int> getSelectedObjectsIDs()
        {
            return _selectedObjectIDs;
        }

        private void OnButtonClick(string name)
        {
            if (name == "Cancel")
            {
                Hide();
            }
            else if (name == "Save")
            {

                Hide();
                Debug.Log(string.Join("/", _selectedObjectIDs));
            }
        }
    }
}
