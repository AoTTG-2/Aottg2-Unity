using ApplicationManagers;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class MapEditorCameraPopup: PromptPopup
    {
        protected override string Title => "Camera";
        protected override float Width => 645f;
        protected override float Height => 310f;
        protected override int VerticalPadding => 20;
        protected override float VerticalSpacing => 20f;
        protected override bool DoublePanel => true;
        private List<InputSettingElement> _inputs = new List<InputSettingElement>();
        private FloatSetting _positionX = new FloatSetting();
        private FloatSetting _positionY = new FloatSetting();
        private FloatSetting _positionZ = new FloatSetting();
        private FloatSetting _rotationX = new FloatSetting();
        private FloatSetting _rotationY = new FloatSetting();
        private FloatSetting _rotationZ = new FloatSetting();


        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(titleWidth: 100f, fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Save"), onClick: () => OnButtonClick("Save"));
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Cancel"), onClick: () => OnButtonClick("Cancel"));
            _inputs.Add(ElementFactory.CreateInputSetting(DoublePanelLeft, style, _positionX, "Position X", elementWidth: 120f).GetComponent<InputSettingElement>());
            _inputs.Add(ElementFactory.CreateInputSetting(DoublePanelLeft, style, _positionY, "Position Y", elementWidth: 120f).GetComponent<InputSettingElement>());
            _inputs.Add(ElementFactory.CreateInputSetting(DoublePanelLeft, style, _positionZ, "Position Z", elementWidth: 120f).GetComponent<InputSettingElement>());
            _inputs.Add(ElementFactory.CreateInputSetting(DoublePanelRight, style, _rotationX, "Rotation X", elementWidth: 120f).GetComponent<InputSettingElement>());
            _inputs.Add(ElementFactory.CreateInputSetting(DoublePanelRight, style, _rotationY, "Rotation Y", elementWidth: 120f).GetComponent<InputSettingElement>());
            _inputs.Add(ElementFactory.CreateInputSetting(DoublePanelRight, style, _rotationZ, "Rotation Z", elementWidth: 120f).GetComponent<InputSettingElement>());
        }

        public override void Show()
        {
            if (gameObject.activeSelf)
                return;
            base.Show();
            var transform = SceneLoader.CurrentCamera.Cache.Transform;
            _positionX.Value = transform.position.x;
            _positionY.Value = transform.position.y;
            _positionZ.Value = transform.position.z;
            _rotationX.Value = transform.rotation.eulerAngles.x;
            _rotationY.Value = transform.rotation.eulerAngles.y;
            _rotationZ.Value = transform.rotation.eulerAngles.z;
            foreach (var input in _inputs)
                input.SyncElement();
        }

        private void OnButtonClick(string name)
        {
            if (name == "Cancel")
            {
                Hide();
            }
            else if (name == "Save")
            {
                var transform = SceneLoader.CurrentCamera.Cache.Transform;
                transform.position = new Vector3(_positionX.Value, _positionY.Value, _positionZ.Value);
                transform.rotation = Quaternion.Euler(_rotationX.Value, _rotationY.Value, _rotationZ.Value);
                Hide();
            }
        }
    }
}
