using ApplicationManagers;
using GameManagers;
using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    class MapEditorCustomAssetPopup: PromptPopup
    {
        protected override string Title => "Custom Assets";
        protected override float Width => 500f;
        protected override float Height => 420f;
        protected override int VerticalPadding => 20;
        private MapEditorGameManager _gameManager;
        private List<GameObject> _elements = new List<GameObject>();
        private List<StringSetting> _settings = new List<StringSetting>();

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(titleWidth: 130f, fontSize: ButtonFontSize, themePanel: ThemePanel);
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Add"), onClick: () => OnButtonClick("Add"));
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Clear"), onClick: () => OnButtonClick("Clear"));
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Back"), onClick: () => OnButtonClick("Back"));
            ElementFactory.CreateDefaultLabel(SinglePanel, style, "Add any asset bundle located in Documents/Aottg2/CustomAssets");
        }

        public override void Show()
        {
            base.Show();
            _gameManager = (MapEditorGameManager)SceneLoader.CurrentGameManager;
            foreach (var element in _elements)
                Destroy(element);
            _elements.Clear();
            _settings.Clear();
            foreach (string str in _gameManager.MapScript.CustomAssets.CustomAssets)
                AddAssetRow(str);
        }

        private void AddAssetRow(string str)
        {
            ElementStyle style = new ElementStyle(titleWidth: 120f, fontSize: ButtonFontSize, themePanel: ThemePanel);
            string[] strArr = str.Split(',');
            string bundle = strArr[0];
            string url = strArr[1];
            var setting = new StringSetting();
            setting.Value = url;
            var element = ElementFactory.CreateInputSetting(SinglePanel, style, setting, bundle, 
                "Optional: add a URL for players to download the asset bundle, otherwise they must also have the file in CustomAssets folder", 
                elementWidth: 220f);
            _elements.Add(element);
            _settings.Add(setting);
        }

        private void OnButtonClick(string name)
        {
            var menu = ((MapEditorMenu)UIManager.CurrentMenu);
            if (name == "Back")
            {
                List<string> newBundles = new List<string>();
                for (int i = 0; i < _elements.Count; i++)
                {
                    string bundle = _gameManager.MapScript.CustomAssets.CustomAssets[i].Split(',')[0].Trim();
                    string url = _settings[i].Value;
                    newBundles.Add(bundle + "," + url.Trim());
                }
                _gameManager.MapScript.CustomAssets.CustomAssets = newBundles;
                menu._topPanel.Save();
                Hide();
            }
            else if (name == "Clear")
            {
                menu.ConfirmPopup.Show("Do you want to clear all custom asset bundles?", () => OnButtonClick("ConfirmClear"));
            }
            else if (name == "ConfirmClear")
            {
                _gameManager.MapScript.CustomAssets.CustomAssets = new List<string>();
                AssetBundleManager.Clear();
                foreach (var element in _elements)
                    Destroy(element);
                _settings.Clear();
                _elements.Clear();
            }
            else if (name == "Add")
            {
                menu.SetNamePopup.Show("", () => OnButtonClick("ConfirmAdd"), "Add asset bundle");
            }
            else if (name == "ConfirmAdd")
            {
                string bundle = menu.SetNamePopup.NameSetting.Value.Trim();
                if (bundle == string.Empty)
                    menu.MessagePopup.Show("Error: bundle name cannot be empty.");
                else
                {
                    menu.MessagePopup.Show("Loading asset bundle...", false);
                    StartCoroutine(LoadAssetBundle(bundle));
                }
            }

        }

        private IEnumerator LoadAssetBundle(string bundle)
        {
            var menu = ((MapEditorMenu)UIManager.CurrentMenu);
            if (AssetBundleManager.LoadedBundle(bundle))
                menu.MessagePopup.Hide();
            else
            {
                yield return StartCoroutine(AssetBundleManager.LoadBundle(bundle, "", true));
                if (AssetBundleManager.LoadedBundle(bundle))
                {
                    _gameManager.MapScript.CustomAssets.CustomAssets.Add(bundle + ",");
                    AddAssetRow(bundle + ",");
                    menu.MessagePopup.Hide();
                }
                else
                    menu.MessagePopup.Show("Error loading asset bundle");
            }
        }
    }
}
