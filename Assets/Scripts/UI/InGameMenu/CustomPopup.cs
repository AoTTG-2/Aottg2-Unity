using CustomLogic;
using GameManagers;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class CustomPopup: BasePopup
    {
        protected override string Title => string.Empty;
        protected override float VerticalSpacing => 20f;
        protected override int VerticalPadding => 20;
        protected List<GameObject> _items = new List<GameObject>();
        protected ElementStyle _style;
        protected float _width;
        protected float _height;

        public void Setup(BasePanel parent, string title, float width, float height)
        {
            _width = width;
            _height = height;
            base.Setup(parent);
            SetTitle(title);
            _style = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, _style, UIManager.GetLocaleCommon("Back"), onClick: () => OnButtonClick("Back"));
        }

        protected override float GetHeight()
        {
            return _height;
        }

        protected override float GetWidth()
        {
            return _width;
        }

        public void Clear()
        {
            foreach (var obj in _items)
                Destroy(obj);
            _items.Clear();
        }

        public void AddLabel(string label)
        {
            var obj = ElementFactory.CreateDefaultLabel(SinglePanel, _style, label);
            _items.Add(obj);
        }

        public void AddButton(string name, string title)
        {
            var obj = ElementFactory.CreateTextButton(SinglePanel, _style, title, 
                onClick: () => OnButtonClick(name));
        }

        public void AddButtons(List<object> names, List<object> titles)
        {
            for (int i = 0; i < names.Count; i++)
                AddButton((string)names[i], (string)titles[i]);
        }

        protected void OnButtonClick(string name)
        {
            if (name == "Back")
            {
                Hide();
            }
            CustomLogicManager.Evaluator.OnButtonClick(name);
        }
    }
}
