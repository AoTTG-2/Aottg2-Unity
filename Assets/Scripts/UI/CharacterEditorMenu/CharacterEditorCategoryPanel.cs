using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using Settings;
using System.Collections;
using ApplicationManagers;
using GameManagers;
using Characters;

namespace UI
{
    class CharacterEditorCategoryPanel: HeadedPanel
    {
        protected override string Title => UIManager.GetLocaleCommon("Editor");
        protected override float Width => 330f;
        protected override float Height => 240f;
        protected override float VerticalSpacing => 20f;
        protected override int HorizontalPadding => 25;
        protected override int VerticalPadding => 25;
        private StringSetting _category = new StringSetting();
        protected CharacterEditorGameManager _gameManager;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            _gameManager = (CharacterEditorGameManager)SceneLoader.CurrentGameManager;
            ElementStyle style = new ElementStyle(titleWidth: 95f, themePanel: ThemePanel);
            float dropdownWidth = 160f;
            string[] categories = new string[] { "Human", "Titan" };
            _category.Value = GetCurrentCategory();
            ElementFactory.CreateDropdownSetting(SinglePanel, style, _category, UIManager.GetLocaleCommon("Category"), categories,
                elementWidth: dropdownWidth, onDropdownOptionSelect: () => OnCategoryChange());
        }

        private string GetCurrentCategory()
        {
            string currentCategory = "Human";
            if (!CharacterEditorGameManager.HumanMode)
                currentCategory = "Titan";
            return currentCategory;
        }

        private void OnCategoryChange()
        {
            if (_category.Value != GetCurrentCategory())
            {
                if (_category.Value == "Human")
                    CharacterEditorGameManager.HumanMode = true;
                else
                    CharacterEditorGameManager.HumanMode = false;
                SceneLoader.LoadScene(SceneName.CharacterEditor);
            }
        }
    }
}
