using Settings;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class SelectListPopup : PromptPopup
    {
        protected override string ThemePanel => "SelectListPopup";
        protected override int HorizontalPadding => 0;
        protected override int VerticalPadding => 5;
        protected override float VerticalSpacing => 10f;
        protected override bool DoublePanel => false;
        protected override bool ScrollBar => true;
        protected override float Width => 450f;
        protected override float Height => 500f;
        protected virtual float ItemButtonWidth => 380f;
        protected virtual int ItemFontSize => 28;
        protected virtual float DeleteButtonSize => 32f;
        protected GameObject _noItemsLabel;
        protected List<GameObject> _itemButtons = new List<GameObject>();
        protected List<GameObject> _saveElements = new List<GameObject>();
        protected InputSettingElement _inputElement;
        protected List<string> _items = new List<string>();
        protected UnityAction _onLoad;
        protected UnityAction _onSave;
        protected UnityAction _onDelete;
        protected bool _isSave;
        protected bool _delete;
        protected List<string> _disallowedDelete;
        protected List<string> _disallowedSave;
        protected string _title;
        public StringSetting FinishSetting = new StringSetting(string.Empty);

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle buttonStyle = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel, titleWidth: 60f);
            _inputElement = ElementFactory.CreateInputSetting(BottomBar, buttonStyle, FinishSetting, UIManager.GetLocaleCommon("Name"), elementWidth: 185f,
                onValueChanged: () => OnSearchChanged(), onEndEdit: () => OnSearchChanged())
                .GetComponent<InputSettingElement>();
            // _saveElements.Add(_inputElement.gameObject);
            _saveElements.Add(ElementFactory.CreateTextButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Save"),
                onClick: () => OnButtonClick("Save")));
            ElementFactory.CreateTextButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Back"),
                onClick: () => OnButtonClick("Back"));
            _noItemsLabel = ElementFactory.CreateDefaultLabel(SinglePanel, new ElementStyle(themePanel: ThemePanel), "No items found.");
        }

        private void OnSearchChanged()
        {
            string query = FinishSetting.Value.ToLowerInvariant();
            RefreshList();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                var match = _items.FirstOrDefault(item => item.ToLowerInvariant().Contains(query));
                if (!string.IsNullOrEmpty(query) && match != null)
                {
                    OnItemClick(match);
                }
            }
        }

        public void ShowLoad(List<string> items, string title = "", UnityAction onLoad = null, UnityAction onDelete = null, List<string> disallowedDelete = null)
        {
            base.Show();
            _items = items;
            _isSave = false;
            _onLoad = onLoad;
            _delete = onDelete != null;
            _disallowedDelete = disallowedDelete;
            _onDelete = onDelete;
            FinishSetting.Value = "";
            ToggleSaveElements();
            if (title != string.Empty)
                SetTitle(title);
            RefreshList();

            // Focus search input
            _inputElement._inputField.Select();
            _inputElement._inputField.ActivateInputField();
        }

        public void ShowSave(List<string> items, string title = "", string initial = "", UnityAction onSave = null, List<string> disallowedSave = null,
            UnityAction onDelete = null, List<string> disallowedDelete = null)
        {
            base.Show();
            _items = items;
            _isSave = true;
            _onSave = onSave;
            _disallowedSave = disallowedSave;
            _delete = _onDelete != null;
            _disallowedDelete = disallowedDelete;
            _onDelete = onDelete;
            FinishSetting.Value = initial;
            _inputElement.SyncElement();
            ToggleSaveElements();
            if (title != string.Empty)
                SetTitle(title);
            RefreshList();

            // Focus search input
            _inputElement._inputField.Select();
            _inputElement._inputField.ActivateInputField();
        }

        private void ToggleSaveElements()
        {
            if (_isSave)
            {
                foreach (GameObject obj in _saveElements)
                    obj.SetActive(true);
                SetTitle(UIManager.GetLocaleCommon("Save"));
            }
            else
            {
                foreach (GameObject obj in _saveElements)
                    obj.SetActive(false);
                SetTitle(UIManager.GetLocaleCommon("Load"));
            }
        }

        public void RefreshList()
        {
            ElementStyle style = new ElementStyle(themePanel: ThemePanel);
            ClearListButtons();

            string query = FinishSetting.Value.ToLowerInvariant();
            List<string> filteredItems = string.IsNullOrEmpty(query)
                ? _items
                : _items.Where(item => item.ToLowerInvariant().Contains(query)).ToList();

            foreach (string item in filteredItems)
            {
                GameObject button = ElementFactory.InstantiateAndBind(SinglePanel, "Prefabs/Misc/SelectListButton");
                _itemButtons.Add(button);
                Button itemButton = button.transform.Find("ItemButton").GetComponent<Button>();
                Button deleteButton = button.transform.Find("DeleteButton").GetComponent<Button>();
                itemButton.onClick.AddListener(() => OnItemClick(item));
                itemButton.transform.Find("Text").GetComponent<Text>().text = item;
                itemButton.transform.Find("Text").GetComponent<Text>().fontSize = ItemFontSize;
                itemButton.colors = UIManager.GetThemeColorBlock(ThemePanel, "ItemButton", "");
                itemButton.transform.Find("Text").GetComponent<Text>().color = UIManager.GetThemeColor(ThemePanel, "ItemButton", "TextColor");
                if (_delete)
                    itemButton.GetComponent<LayoutElement>().preferredWidth = ItemButtonWidth;
                else
                    itemButton.GetComponent<LayoutElement>().preferredWidth = ItemButtonWidth + DeleteButtonSize + 10f;
                if (_delete && (_disallowedDelete == null || !_disallowedDelete.Contains(item)))
                {
                    deleteButton.onClick.AddListener(() => OnDeleteClick(item));
                    deleteButton.GetComponent<LayoutElement>().preferredWidth = DeleteButtonSize;
                    deleteButton.GetComponent<LayoutElement>().preferredHeight = DeleteButtonSize;
                    deleteButton.colors = UIManager.GetThemeColorBlock(ThemePanel, "DeleteButton", "");
                }
                else
                    deleteButton.gameObject.SetActive(false);
            }
            if (_items.Count > 0)
                _noItemsLabel.SetActive(false);
            else
                _noItemsLabel.SetActive(true);
        }

        protected void ClearListButtons()
        {
            foreach (GameObject obj in _itemButtons)
                Destroy(obj);
            _itemButtons.Clear();
        }

        private void OnItemClick(string name)
        {
            if (_isSave)
            {
                if (_disallowedSave == null || !_disallowedSave.Contains(name))
                {
                    UIManager.CurrentMenu.ConfirmPopup.Show("Overwrite this item?", () => OnConfirmOverwrite(name));
                }
                else
                    UIManager.CurrentMenu.MessagePopup.Show("Cannot overwrite this item.");
            }
            else
            {
                FinishSetting.Value = name;
                _onLoad.Invoke();
                Hide();
            }
        }

        private void OnConfirmOverwrite(string name)
        {
            FinishSetting.Value = name;
            _onSave.Invoke();
            Hide();
        }

        private void OnConfirmDelete(string name)
        {
            FinishSetting.Value = name;
            _onDelete.Invoke();
            FinishSetting.Value = string.Empty;
            _items.Remove(name);
            RefreshList();
        }

        private void OnDeleteClick(string name)
        {
            UIManager.CurrentMenu.ConfirmPopup.Show("Delete this item?", () => OnConfirmDelete(name));
        }

        private void OnButtonClick(string name)
        {
            switch (name)
            {
                case "Back":
                    Hide();
                    break;
                case "Save":
                    if (FinishSetting.Value == string.Empty)
                        UIManager.CurrentMenu.MessagePopup.Show("Field cannot be empty.");
                    else
                    {
                        if (_disallowedSave != null && _disallowedSave.Contains(FinishSetting.Value))
                            UIManager.CurrentMenu.MessagePopup.Show("Cannot overwrite this item.");
                        else if (_items.Contains(FinishSetting.Value))
                            UIManager.CurrentMenu.ConfirmPopup.Show("Overwrite this item?", () => OnConfirmOverwrite(FinishSetting.Value));
                        else
                        {
                            _onSave.Invoke();
                            Hide();
                        }
                    }
                    break;
            }
        }
    }
}