using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;
using Settings;
using ApplicationManagers;
using Utility;
using UnityEngine.Events;
using Unity.VisualScripting;

namespace UI
{
    class IconPickPopup : PromptPopup
    {
        protected override string Title => string.Empty;
        protected override float Width => 1000f;
        protected override float Height => 800f;
        protected override bool ScrollBar => true;
        protected const int Columns = 7;
        protected override float VerticalSpacing => 10f;
        protected override int HorizontalPadding => 10;
        protected override int VerticalPadding => 10;
        protected string[] _options;
        protected BaseSetting _setting;
        protected UnityAction _onSelect;
        protected Text _label;
        protected List<GameObject> _groups = new List<GameObject>();
        protected int _currentIndex;


        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(fontSize: 24, themePanel: ThemePanel, titleWidth: 70f);
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Back"),
                    onClick: () => OnBottomBarButtonClick("Back"));
        }

        public void Show(BaseSetting setting, Text label, string[] options, string[] icons, string[] tooltips, UnityAction onSelect, TooltipPopup tooltipPopup)
        {
            _setting = setting;
            _onSelect = onSelect;
            _options = options;
            _label = label;
            foreach (var go in _groups)
                Destroy(go);
            _groups.Clear();
            if (options.Length != icons.Length)
                throw new Exception("Options and icons not equal length");
            if (tooltips != null && options.Length != tooltips.Length)
                throw new Exception("Options and tooltips not equal length");
            var itemNames = Util.GroupItems(options.ToList(), Columns);
            var iconNames = Util.GroupItems(icons.ToList(), Columns);
            var tooltipNames = tooltips != null
                ? Util.GroupItems(tooltips.ToList(), Columns)
                : null;
            _currentIndex = 0;
            for (int i = 0; i < itemNames.Count; i++)
            {
                if (tooltipNames == null || i >= tooltipNames.Count)
                    CreateRow(itemNames[i], iconNames[i], new List<string>(), tooltipPopup);
                else
                    CreateRow(itemNames[i], iconNames[i], tooltipNames[i], tooltipPopup);
            }
            base.Show();
        }

        private void OnBottomBarButtonClick(string name)
        {
            Hide();
        }

        protected void CreateRow(List<string> items, List<string> icons, List<string> tooltips, TooltipPopup tooltipPopup)
        {
            var group = ElementFactory.CreateHorizontalGroup(SinglePanel, VerticalSpacing, TextAnchor.MiddleLeft);
            _groups.Add(group);
            for (int i = 0; i < items.Count; i++)
            {
                var obj = ElementFactory.InstantiateAndBind(group.transform, "Prefabs/Misc/MapEditorObjectButton");
                string itemName = items[i];
                int index = _currentIndex;
                obj.GetComponent<Button>().onClick.AddListener(() => OnSelectObject(itemName, index));
                _currentIndex++;
                try
                {
                    if (icons[i] != string.Empty)
                    {
                        var texture = (Texture2D)ResourceManager.LoadAsset(string.Empty, icons[i], true);
                        obj.transform.Find("Icon").GetComponent<RawImage>().texture = texture;
                        if (tooltips.Count > i && tooltips[i] != string.Empty)
                        {
                            var tooltip = obj.GetOrAddComponent<HoverTooltip>();
                            tooltip.Message = tooltips[i];
                            tooltip.PopupOverride = tooltipPopup;
                        }
                    }
                }
                catch
                {
                    Debug.Log("Failed to load icon: " + itemName);
                }
                obj.transform.Find("Text").GetComponent<Text>().text = itemName;
                obj.transform.Find("Text").GetComponent<Text>().color = UIManager.GetThemeColor("DefaultPanel", "DefaultLabel", "TextColor");
            }
        }

        protected virtual void OnSelectObject(string name, int index)
        {
            _label.text = name;
            if (_setting is StringSetting)
                ((StringSetting)_setting).Value = name;
            else if (_setting is IntSetting)
                ((IntSetting)_setting).Value = index;
            _onSelect?.Invoke();
            Hide();
        }
    }
}
