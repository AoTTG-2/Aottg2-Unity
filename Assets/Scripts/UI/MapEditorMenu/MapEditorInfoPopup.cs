using ApplicationManagers;
using GameManagers;
using Map;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class MapEditorInfoPopup: PromptPopup
    {
        protected override string Title => "Map Info";
        protected override float Width => 500f;
        protected override float Height => 400f;
        protected override int VerticalPadding => 20;
        private StringSetting _description = new StringSetting(string.Empty);
        private MapEditorGameManager _gameManager;
        private List<BaseSettingElement> _inputs = new List<BaseSettingElement>();
        /*
        private StringSetting _background = new StringSetting("None");
        private FloatSetting _backgroundPositionX = new FloatSetting();
        private FloatSetting _backgroundPositionY = new FloatSetting();
        private FloatSetting _backgroundPositionZ = new FloatSetting();
        private FloatSetting _backgroundRotationX = new FloatSetting();
        private FloatSetting _backgroundRotationY = new FloatSetting();
        private FloatSetting _backgroundRotationZ = new FloatSetting();
        */

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(titleWidth: 130f, fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Save"), onClick: () => OnButtonClick("Save"));
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Cancel"), onClick: () => OnButtonClick("Cancel"));
            _inputs.Add(ElementFactory.CreateInputSetting(SinglePanel, style, _description, "Description", elementWidth: 300f, elementHeight: 250f, multiLine: true)
                .GetComponent<InputSettingElement>());
            /*
            string[] backgrounds = new string[] { "None", "Forest1", "City1" };
            _inputs.Add(ElementFactory.CreateDropdownSetting(SinglePanel, style, _background, "Background", backgrounds).GetComponent<DropdownSettingElement>());
            _inputs.Add(ElementFactory.CreateInputSetting(SinglePanel, style, _backgroundPositionX, "Background Position X", elementWidth: 120f).GetComponent<InputSettingElement>());
            _inputs.Add(ElementFactory.CreateInputSetting(SinglePanel, style, _backgroundPositionY, "Backgruund Position Y", elementWidth: 120f).GetComponent<InputSettingElement>());
            _inputs.Add(ElementFactory.CreateInputSetting(SinglePanel, style, _backgroundPositionZ, "Background Position Z", elementWidth: 120f).GetComponent<InputSettingElement>());
            _inputs.Add(ElementFactory.CreateInputSetting(SinglePanel, style, _backgroundRotationX, "Background Rotation X", elementWidth: 120f).GetComponent<InputSettingElement>());
            _inputs.Add(ElementFactory.CreateInputSetting(SinglePanel, style, _backgroundRotationY, "Background Rotation Y", elementWidth: 120f).GetComponent<InputSettingElement>());
            _inputs.Add(ElementFactory.CreateInputSetting(SinglePanel, style, _backgroundRotationZ, "Background Rotation Z", elementWidth: 120f).GetComponent<InputSettingElement>());
            */
        }

        public override void Show()
        {
            base.Show();
            _gameManager = (MapEditorGameManager)SceneLoader.CurrentGameManager;
            var options = _gameManager.MapScript.Options;
            _description.Value = options.Description;
            /*
            _background.Value = options.Background;
            Vector3 position = options.BackgroundPosition;
            Vector3 rotation = options.BackgroundRotation;
            _backgroundPositionX.Value = position.x;
            _backgroundPositionY.Value = position.y;
            _backgroundPositionZ.Value = position.z;
            _backgroundRotationX.Value = rotation.x;
            _backgroundRotationY.Value = rotation.y;
            _backgroundRotationZ.Value = rotation.z;
            */
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
                var options = _gameManager.MapScript.Options;
                options.Description = _description.Value;
                /*
                options.Background = _background.Value;
                options.BackgroundPosition = new Vector3(_backgroundPositionX.Value, _backgroundPositionY.Value, _backgroundPositionZ.Value);
                options.BackgroundRotation = new Vector3(_backgroundRotationX.Value, _backgroundRotationY.Value, _backgroundRotationZ.Value);
                MapLoader.LoadBackground(options.Background, options.BackgroundPosition, options.BackgroundRotation);
                */
                ((MapEditorMenu)UIManager.CurrentMenu)._topPanel.Save();
                Hide();
            }
        }
    }
}
