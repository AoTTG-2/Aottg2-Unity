using ApplicationManagers;
using GameManagers;
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
        protected override float Height => 420f;
        protected override int VerticalPadding => 20;
        private StringSetting _description = new StringSetting(string.Empty);
        private InputSettingElement _inputDescription;
        private MapEditorGameManager _gameManager;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(titleWidth: 130f, fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Save"), onClick: () => OnButtonClick("Save"));
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Cancel"), onClick: () => OnButtonClick("Cancel"));
            _inputDescription = ElementFactory.CreateInputSetting(SinglePanel, style, _description, "Description", elementWidth: 300f, elementHeight: 270f, multiLine: true)
                .GetComponent<InputSettingElement>();
        }

        public override void Show()
        {
            base.Show();
            _gameManager = (MapEditorGameManager)SceneLoader.CurrentGameManager;
            _description.Value = _gameManager.MapScript.Options.Description;
            _inputDescription.SyncElement();
        }

        private void OnButtonClick(string name)
        {
            if (name == "Cancel")
            {
                Hide();
            }
            else if (name == "Save")
            {
                _gameManager.MapScript.Options.Description = _description.Value;
                ((MapEditorMenu)UIManager.CurrentMenu)._topPanel.Save();
                Hide();
            }
        }
    }
}
