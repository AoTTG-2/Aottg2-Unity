﻿using ApplicationManagers;
using CustomLogic;
using GameManagers;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class MapEditorCustomLogicPopup: PromptPopup
    {
        protected override string Title => "Custom Logic";
        protected override float Width => 500f;
        protected override float Height => 585f;
        protected override int VerticalPadding => 20;
        protected override float VerticalSpacing => 10f;
        private StringSetting _logic = new StringSetting(string.Empty);
        private GameObject _logicInput;
        private MapEditorGameManager _gameManager;
        private Text _error;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(titleWidth: 130f, fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Save"), onClick: () => OnButtonClick("Save"));
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Cancel"), onClick: () => OnButtonClick("Cancel"));
            _error = ElementFactory.CreateDefaultLabel(SinglePanel, style, "").GetComponent<Text>();
            _error.color = Color.red;
        }

        public override void Show()
        {
            _gameManager = (MapEditorGameManager)SceneLoader.CurrentGameManager;
            if (_logicInput != null)
                Destroy(_logicInput);
            ElementStyle style = new ElementStyle(titleWidth: 130f, fontSize: ButtonFontSize, themePanel: ThemePanel);
            base.Show();
            _logic.Value = _gameManager.MapScript.Logic;
            _logicInput = ElementFactory.CreateInputSetting(SinglePanel, style, _logic, "", elementWidth: 450f, elementHeight: 400f, multiLine: true);
            _error.text = "";
        }

        private void OnButtonClick(string name)
        {
            if (name == "Cancel")
            {
                Hide();
            }
            else if (name == "Save")
            {
                string error = "";
                if (_logic.Value != string.Empty)
                {
                    error = CustomLogicManager.TryParseLogic(_logic.Value);
                }
                if (error != string.Empty)
                {
                    _error.text = error;
                    if (_logicInput != null)
                        Destroy(_logicInput);
                    ElementStyle style = new ElementStyle(titleWidth: 130f, fontSize: ButtonFontSize, themePanel: ThemePanel);
                    _logicInput = ElementFactory.CreateInputSetting(SinglePanel, style, _logic, "", elementWidth: 450f, elementHeight: 400f, multiLine: true);
                }
                else
                {
                    _gameManager.MapScript.Logic = _logic.Value;
                    _gameManager.LogicEvaluator = CustomLogicManager.GetEditorEvaluator(_logic.Value);
                    ((MapEditorMenu)UIManager.CurrentMenu)._topPanel.Save();
                    Hide();
                }
            }
        }
    }
}
