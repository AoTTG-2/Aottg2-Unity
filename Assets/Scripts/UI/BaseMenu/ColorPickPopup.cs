using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utility;

namespace UI
{
    class ColorPickPopup : PromptPopup
    {
        protected override string Title => UIManager.GetLocale("SettingsPopup", "ColorPickPopup", "Title");
        protected override float Width => 440f;
        protected override float Height => 440f;
        protected override float VerticalSpacing => 20f;
        protected override TextAnchor PanelAlignment => TextAnchor.UpperCenter;
        private float PreviewWidth = 90f;
        private float PreviewHeight = 40f;
        private Image _image;
        private ColorSetting _setting;
        private Image _preview;
        private IntSetting _red = new IntSetting(0, minValue: 0, maxValue: 255);
        private IntSetting _green = new IntSetting(0, minValue: 0, maxValue: 255);
        private IntSetting _blue = new IntSetting(0, minValue: 0, maxValue: 255);
        private IntSetting _alpha = new IntSetting(0, minValue: 0, maxValue: 255);
        private List<GameObject> _sliders = new List<GameObject>();
        private UnityAction _onChangeColor;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle buttonStyle = new ElementStyle(fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Save"), onClick: () => OnButtonClick("Save"));
            ElementFactory.CreateTextButton(BottomBar, buttonStyle, UIManager.GetLocaleCommon("Cancel"), onClick: () => OnButtonClick("Cancel"));
            GameObject preview = ElementFactory.InstantiateAndBind(SinglePanel, "Prefabs/Elements/ColorPreview");
            preview.GetComponent<LayoutElement>().preferredWidth = PreviewWidth;
            preview.GetComponent<LayoutElement>().preferredHeight = PreviewHeight;
            _preview = preview.transform.Find("Image").GetComponent<Image>();
        }

        private new void Update()
        {
            base.Update();
            if (_preview != null)
            {
                _preview.color = GetColorFromSliders().ToColor();
            }
        }

        public void Show(ColorSetting setting, Image image, UnityAction onChangeColor)
        {
            if (gameObject.activeSelf)
                return;
            base.Show();
            _setting = setting;
            _image = image;
            _red.Value = setting.Value.R;
            _green.Value = setting.Value.G;
            _blue.Value = setting.Value.B;
            _alpha.MinValue = setting.MinAlpha;
            _alpha.Value = setting.Value.A;
            _preview.color = GetColorFromSliders().ToColor();
            _onChangeColor = onChangeColor;
            CreateSliders();
        }

        private void CreateSliders()
        {
            foreach (GameObject obj in _sliders)
            {
                Destroy(obj);
            }
            ElementStyle style = new ElementStyle(titleWidth: 85f, themePanel: ThemePanel);
            _sliders.Add(ElementFactory.CreateSliderInputSetting(SinglePanel, style, _red, "Red", decimalPlaces: 3));
            _sliders.Add(ElementFactory.CreateSliderInputSetting(SinglePanel, style, _green, "Green", decimalPlaces: 3));
            _sliders.Add(ElementFactory.CreateSliderInputSetting(SinglePanel, style, _blue, "Blue", decimalPlaces: 3));
            _sliders.Add(ElementFactory.CreateSliderInputSetting(SinglePanel, style, _alpha, "Alpha", decimalPlaces: 3));
        }

        private void OnButtonClick(string name)
        {
            if (name == "Cancel")
            {
                Hide();
            }
            else if (name == "Save")
            {
                _setting.Value = GetColorFromSliders();
                _image.color = _setting.Value.ToColor();
                if (_onChangeColor != null)
                    _onChangeColor.Invoke();
                Hide();
            }
        }

        private Color255 GetColorFromSliders()
        {
            return new Color255(_red.Value, _green.Value, _blue.Value, Mathf.Clamp(_alpha.Value, _setting.MinAlpha, 255));
        }
    }
}
