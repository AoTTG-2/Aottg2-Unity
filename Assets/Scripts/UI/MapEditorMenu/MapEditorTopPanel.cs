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
using Map;
using System.Threading;
using System.IO;
using Utility;

namespace UI
{
    class MapEditorTopPanel: HeadedPanel
    {
        public enum LayerOption
        {
            All,
            Active,
            Visible,
            Static,
            NonStatic,
            Colliders,
            Triggers,
            NoColliders,
            Networked
        }

        protected override float Width => 1960f;
        protected override float Height => 60f;

        protected override float TopBarHeight => 0f;
        protected override float BottomBarHeight => 0f;
        protected override float VerticalSpacing => 0f;
        protected override int HorizontalPadding => 30;
        protected override int VerticalPadding => 10;

        private IntSetting _dropdownSelection = new IntSetting(0);
        private MapEditorMenu _menu;
        private MapEditorGameManager _gameManager;
        private StringSetting _currentMap;
        private List<DropdownSelectElement> _dropdowns = new List<DropdownSelectElement>();
        private GameObject _gizmoButton;
        private GameObject _gizmoOrientationButton;
        private GameObject _snapButton;
        protected override string ThemePanel => "MapEditor";

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            _menu = ((MapEditorMenu)UIManager.CurrentMenu);
            _currentMap = SettingsManager.MapEditorSettings.CurrentMap;
            _gameManager = (MapEditorGameManager)SceneLoader.CurrentGameManager;
            string cat = "MapEditor";
            ElementStyle style = new ElementStyle(titleWidth: 0f, themePanel: ThemePanel);
            float dropdownWidth = 100f;
            float dropdownHeight = 40f;
            Transform group = ElementFactory.CreateHorizontalGroup(SinglePanel, 10f, TextAnchor.MiddleLeft).transform;

            // file dropdown
            List<string> options = new List<string>();
            foreach (string option in new string[] { "New", "Open", "Rename", "Save", "Import", "Export", "LoadPreset", "LoadAutosave", "Quit", "SaveQuit" })
                options.Add(UIManager.GetLocaleCommon(option));
            var fileDropdown = ElementFactory.CreateDropdownSelect(group, style, _dropdownSelection, UIManager.GetLocale(cat, "Top", "File"),
               options.ToArray(), elementWidth: dropdownWidth, optionsWidth: 180f, maxScrollHeight: 500f, onDropdownOptionSelect: () => OnFileClick());
            _dropdowns.Add(fileDropdown.GetComponent<DropdownSelectElement>());

            // edit dropdown
            options = new List<string>();
            foreach (string option in new string[] {"Undo", "Redo", "Copy", "Paste", "Cut", "Delete"})
            {
                if (option == "Copy" || option == "Delete")
                    options.Add(UIManager.GetLocaleCommon(option));
                else
                    options.Add(UIManager.GetLocale("MapEditorSettings", "Keybinds", option));
            }
            var editDropdown = ElementFactory.CreateDropdownSelect(group, style, _dropdownSelection, UIManager.GetLocaleCommon("Edit"),
               options.ToArray(), elementWidth: dropdownWidth, optionsWidth: 180f, maxScrollHeight: 500f, onDropdownOptionSelect: () => OnEditClick());
            _dropdowns.Add(editDropdown.GetComponent<DropdownSelectElement>());

            // options dropdown
            dropdownWidth = 130f;
            options = new List<string>();
            foreach (string option in new string[] { "Editor", "MapInfo", "Weather", "CustomLogic", "CustomAssets" })
            {
                options.Add(UIManager.GetLocale(cat, "Top", option));
            }
            var optionsDropdown = ElementFactory.CreateDropdownSelect(group, style, _dropdownSelection, UIManager.GetLocaleCommon("Options"),
               options.ToArray(), elementWidth: dropdownWidth, optionsWidth: 180f, maxScrollHeight: 500f, onDropdownOptionSelect: () => OnOptionsClick());
            _dropdowns.Add(optionsDropdown.GetComponent<DropdownSelectElement>());

            // gizmos
            ElementFactory.CreateDefaultButton(group, style, UIManager.GetLocale("MapEditorSettings", "Keybinds", "AddObject"), elementHeight: dropdownHeight, onClick: () => OnButtonClick("AddObject"));
            _gizmoButton = ElementFactory.CreateDefaultButton(group, style, "Gizmo: Position", elementHeight: dropdownHeight, onClick: () => OnButtonClick("Gizmo"));
            _gizmoOrientationButton = ElementFactory.CreateDefaultButton(group, style, "Orientation: Center", elementHeight: dropdownHeight, onClick: () => OnButtonClick("GizmoOrientation"));
            _snapButton = ElementFactory.CreateDefaultButton(group, style, "Snap: Off", elementHeight: dropdownHeight,onClick: () => OnButtonClick("Snap"));
            ElementFactory.CreateDefaultButton(group, style, "Camera", elementHeight: dropdownHeight,onClick: () => OnButtonClick("Camera"));

            // lights
            ElementFactory.CreateDefaultButton(group, style, "Light", elementHeight: dropdownHeight, onClick: () => OnButtonClick("Light"));

            // layers
            var layerDropdown = ElementFactory.CreateDropdownSelect(group, style, _dropdownSelection, UIManager.GetLocaleCommon("Layers"),
               Enum.GetNames(typeof(LayerOption)), elementWidth: dropdownWidth, optionsWidth: 180f, maxScrollHeight: 500f, onDropdownOptionSelect: () => OnLayersClick());
            _dropdowns.Add(layerDropdown.GetComponent<DropdownSelectElement>());

            // tutorial
            ElementFactory.CreateDefaultButton(group, style, UIManager.GetLocale("MainMenu", "Intro", "TutorialButton"), elementHeight: dropdownHeight, onClick: () => OnButtonClick("Tutorial"));
        }

        public bool IsDropdownOpen()
        {
            foreach (DropdownSelectElement element in _dropdowns)
            {
                if (element.IsOpen())
                    return true;
            }
            return false;
        }

        protected void OnFileClick()
        {
            var files = BuiltinLevels.GetMapNames("Custom").ToList();
            int index = _dropdownSelection.Value;
            var disallowedDelete = new List<string>();
            disallowedDelete.Add(_currentMap.Value);
            if (index == 0) // new
            {
                string newName = "Untitled";
                int i = 1;
                while (files.Contains(newName + i.ToString()))
                    i++;
                _menu.SelectListPopup.ShowSave(files, UIManager.GetLocaleCommon("New"), newName, onSave: () => OnNewFinish(), onDelete: () => OnDeleteMap(),
                    disallowedDelete: disallowedDelete);
            }
            else if (index == 1) // open
            {
                _menu.SelectListPopup.ShowLoad(files, UIManager.GetLocaleCommon("Open"), onLoad: () => OnOpenFinish(), onDelete: () => OnDeleteMap(),
                    disallowedDelete: disallowedDelete);
            }
            else if (index == 2) // rename
            {
                _menu.SelectListPopup.ShowSave(files, UIManager.GetLocaleCommon("Rename"), onSave: () => OnRenameFinish(), onDelete: () => OnDeleteMap(), 
                    disallowedDelete: disallowedDelete);
            }
            else if (index == 3) // save
            {
                Save();
            }
            else if (index == 4) // import
                _menu.ImportPopup.Show(() => OnImportFinish(), topText: "Warning: importing will override current save.");
            else if (index == 5) // export
            {
                var mapScript = new MapScript();
                mapScript.Deserialize(_gameManager.MapScript.Serialize());
                var objs = mapScript.Objects.Objects;
                objs.Clear();
                foreach (var obj in MapLoader.IdToMapObject.Values)
                    objs.Add(obj.ScriptObject);
                _menu.ExportPopup.Show(mapScript.Serialize());
            }
            else if (index == 6) // load preset
            {
                var presets = new List<string>();
                foreach (string category in BuiltinLevels.GetMapCategories())
                {
                    if (category == "Custom")
                        continue;
                    foreach (string map in BuiltinLevels.GetMapNames(category))
                        presets.Add(category + "/" + map);
                }
                _menu.SelectListPopup.ShowLoad(presets, UIManager.GetLocaleCommon("LoadPreset"), onLoad: () => OnImportPresetFinish());
            }
            else if (index == 7) // load autosave
            {
                var autosaves = BuiltinLevels.GetAutosaveNames().OrderByDescending(c => c).ToList();
                _menu.SelectListPopup.ShowLoad(autosaves, UIManager.GetLocaleCommon("LoadAutosave"), onLoad: () => OnImportAutosaveFinish());
            }
            else if (index == 8) // quit
                SceneLoader.LoadScene(SceneName.MainMenu);
            else if (index == 9) // save + quit
            {
                Save();
                SceneLoader.LoadScene(SceneName.MainMenu);
            }
        }

        public void Save()
        {
            var objs = _gameManager.MapScript.Objects.Objects;
            objs.Clear();
            foreach (var obj in MapLoader.IdToMapObject.Values)
                objs.Add(obj.ScriptObject);
            BuiltinLevels.SaveCustomMap(_currentMap.Value, _gameManager.MapScript);
        }

        public void Autosave()
        {
            var objs = _gameManager.MapScript.Objects.Objects;
            objs.Clear();
            foreach (var obj in MapLoader.IdToMapObject.Values)
                objs.Add(obj.ScriptObject);
            foreach (var fi in new DirectoryInfo(FolderPaths.CustomMapAutosave).GetFiles().OrderByDescending(x => x.LastWriteTime).Skip(100))
                fi.Delete();
            BuiltinLevels.AutosaveCustomMap(_currentMap.Value + DateTime.Now.ToString("MM-dd-yyyy-HH-mm"), _gameManager.MapScript);
        }

        protected void OnEditClick()
        {
            if (_menu.IsPopupActive())
                return;
            int index = _dropdownSelection.Value;
            if (index == 0) // undo
                _gameManager.Undo();
            else if (index == 1) // redo
                _gameManager.Redo();
            else if (index == 2) // copy
                _gameManager.Copy();
            else if (index == 3) // paste
                _gameManager.Paste();
            else if (index == 4) // cut
                _gameManager.Cut();
            else if (index == 5) // delete
                _gameManager.Delete();
        }

        protected void OnOptionsClick()
        {
            if (_menu.IsPopupActive())
                return;
            int index = _dropdownSelection.Value;
            if (index == 0) // editor options
                _menu.SettingsPopup.Show();
            else if (index == 1) // map info
                _menu.InfoPopup.Show();
            else if (index == 2) // weather
                _menu.WeatherPopup.Show();
            else if (index == 3) // custom logic
                _menu.CustomLogicPopup.Show();
            else if (index == 4) // custom assets
                _menu.CustomAssetsPopup.Show();
        }

        protected void OnButtonClick(string name)
        {
            if (_menu.IsPopupActive())
                return;
            if (name == "AddObject")
                _menu.AddObjectPopup.Show();
            else if (name == "Camera")
                _menu.CameraPopup.Show();
            else if (name == "Gizmo")
                NextGizmo();
            else if (name == "GizmoOrientation")
                NextGizmoOrientation();
            else if (name == "Snap")
                ToggleSnap();
            else if (name == "Light")
                ToggleLights();
            else if (name == "Tutorial")
                _menu.ExternalLinkPopup.Show("https://aottg2.gitbook.io/custom-maps");
        }

        public void ToggleSnap()
        {
            var text = _snapButton.transform.Find("Text").GetComponent<Text>();
            if (_gameManager.Snap)
            {
                _gameManager.Snap = false;
                text.text = "Snap: Off";
            }
            else
            {
                _gameManager.Snap = true;
                text.text = "Snap: On";
            }
        }

        public void ToggleLights()
        {
            _gameManager.ToggleLights();
        }

        public void OnLayersClick()
        {
            if (_menu.IsPopupActive())
                return;
            LayerOption index = (LayerOption)_dropdownSelection.Value;
            _gameManager.SetLayerVisibility(index);
        }

        public void NextGizmoOrientation()
        {
            var text = _gizmoOrientationButton.transform.Find("Text").GetComponent<Text>();
            if (_gameManager.CurrentGizmoMode == GizmoMode.Center)
            {
                _gameManager.SetGizmoMode(GizmoMode.Local);
                text.text = "Orientation: Local";
            }
            else
            {
                _gameManager.SetGizmoMode(GizmoMode.Center);
                text.text = "Orientation: Center";
            }
        }

        public void NextGizmo()
        {
            var text = _gizmoButton.transform.Find("Text").GetComponent<Text>();
            if (_gameManager.CurrentGizmo == _gameManager._positionGizmo)
            {
                _gameManager.SetGizmo("Rotation");
                text.text = "Gizmo: Rotation";
            }
            else if (_gameManager.CurrentGizmo == _gameManager._rotationGizmo)
            {
                _gameManager.SetGizmo("Scale");
                text.text = "Gizmo: Scale";
            }
            else
            {
                _gameManager.SetGizmo("Position");
                text.text = "Gizmo: Position";
            }
        }

        protected void OnDeleteMap()
        {
            BuiltinLevels.DeleteCustomMap(_menu.SelectListPopup.FinishSetting.Value);
        }

        protected void OnNewFinish()
        {
            _currentMap.Value = _menu.SelectListPopup.FinishSetting.Value;
            BuiltinLevels.SaveCustomMap(_currentMap.Value, MapScript.CreateDefault());
            SettingsManager.MapEditorSettings.Save();
            SceneLoader.LoadScene(SceneName.MapEditor);
        }

        protected void OnRenameFinish()
        {
            string oldMap = _currentMap.Value;
            _currentMap.Value = _menu.SelectListPopup.FinishSetting.Value;
            BuiltinLevels.SaveCustomMap(_currentMap.Value, _gameManager.MapScript);
            SettingsManager.MapEditorSettings.Save();
            if (oldMap != _currentMap.Value)
                BuiltinLevels.DeleteCustomMap(oldMap);
        }

        protected void OnOpenFinish()
        {
            _currentMap.Value = _menu.SelectListPopup.FinishSetting.Value;
            SettingsManager.MapEditorSettings.Save();
            SceneLoader.LoadScene(SceneName.MapEditor);
        }

        protected void OnImportFinish()
        {
            _menu.ConfirmPopup.Show("Importing will overwrite current save.", () => OnImportConfirm());
        }

        protected void OnImportConfirm()
        {
            MapScript script = new MapScript();
            try
            {
                script.Deserialize(_menu.ImportPopup.ImportSetting.Value);
                BuiltinLevels.SaveCustomMap(_currentMap.Value, script);
                SceneLoader.LoadScene(SceneName.MapEditor);
            }
            catch (Exception e)
            {
                _menu.ImportPopup.ShowError("Error importing: " + e.Message);
            }
        }

        protected void OnImportPresetFinish()
        {
            _menu.ConfirmPopup.Show("Loading preset will overwrite current save.", () => OnImportPresetConfirm());
        }

        protected void OnImportAutosaveFinish()
        {
            _menu.ConfirmPopup.Show("Loading autosave will overwrite current save.", () => OnImportAutosaveConfirm());
        }

        protected void OnImportPresetConfirm()
        {
            string[] strArr = _menu.SelectListPopup.FinishSetting.Value.Split('/');
            string category = strArr[0];
            string map = strArr[1];
            MapScript script = new MapScript();
            script.Deserialize(BuiltinLevels.LoadMap(category, map));
            BuiltinLevels.SaveCustomMap(_currentMap.Value, script);
            SceneLoader.LoadScene(SceneName.MapEditor);
        }

        protected void OnImportAutosaveConfirm()
        {
            string map = _menu.SelectListPopup.FinishSetting.Value;
            MapScript script = new MapScript();
            script.Deserialize(BuiltinLevels.LoadAutosave(map));
            BuiltinLevels.SaveCustomMap(_currentMap.Value, script);
            SceneLoader.LoadScene(SceneName.MapEditor);
        }
    }
}
