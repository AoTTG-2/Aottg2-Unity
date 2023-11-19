using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utility;

namespace UI
{
    class Vector3Popup: PromptPopup
    {
        protected override string Title => "Vector3";
        protected override float Width => 250f;
        protected override float Height => 330f;
        protected override float VerticalSpacing => 20f;
        protected override TextAnchor PanelAlignment => TextAnchor.UpperCenter;
        private Vector3Setting _setting;
        private FloatSetting _x = new FloatSetting(0f);
        private FloatSetting _y = new FloatSetting(0f);
        private FloatSetting _z = new FloatSetting(0f);
        private List<GameObject> _inputs = new List<GameObject>();
        private UnityAction _onChangeVector;
        private Text _text;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle buttonStyle = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Save"), onClick: () => OnButtonClick("Save"));
            ElementFactory.CreateTextButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Cancel"), onClick: () => OnButtonClick("Cancel"));
        }

        public void Show(Vector3Setting setting, Text text, UnityAction onChangeVector)
        {
            if (gameObject.activeSelf)
                return;
            base.Show();
            _setting = setting;
            _text = text;
            _x.Value = setting.Value.x;
            _y.Value = setting.Value.y;
            _z.Value = setting.Value.z;
            _onChangeVector = onChangeVector;
            CreateInputs();
        }

        private void CreateInputs()
        {
            foreach (GameObject obj in _inputs)
            {
                Destroy(obj);
            }
            ElementStyle style = new ElementStyle(titleWidth: 30f, themePanel: ThemePanel);
            _inputs.Add(ElementFactory.CreateInputSetting(SinglePanel, style, _x, "X"));
            _inputs.Add(ElementFactory.CreateInputSetting(SinglePanel, style, _y, "Y"));
            _inputs.Add(ElementFactory.CreateInputSetting(SinglePanel, style, _z, "Z"));
        }

        private void OnButtonClick(string name)
        {
            if (name == "Cancel")
            {
                Hide();
            }
            else if (name == "Save")
            {
                _setting.Value = new Vector3(_x.Value, _y.Value, _z.Value);
                _text.text = _setting.Value.ToDisplayString();
                if (_onChangeVector != null)
                    _onChangeVector.Invoke();
                Hide();
            }
        }
    }
}
