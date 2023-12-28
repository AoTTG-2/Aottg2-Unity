using Map;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using System;
using CustomLogic;

namespace UI
{
    class CreateGameCustomPanel : CreateGameCategoryPanel
    {
        protected static IntSetting SelectedMap = new IntSetting(0);
        protected static IntSetting SelectedLogic = new IntSetting(0);
        protected string[] CurrentMapNames;
        protected string[] CurrentLogicNames;

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(titleWidth: 120f, themePanel: ThemePanel);
            string cat = "CreateGamePopup";
            string sub = "Custom";
            var maps = BuiltinLevels.GetMapNames("Custom");
            ElementFactory.CreateDefaultLabel(DoublePanelLeft, style, UIManager.GetLocale(cat, sub, "CustomMap"), FontStyle.Bold, alignment: TextAnchor.MiddleLeft);
            CreateHorizontalDivider(DoublePanelLeft);
            CurrentMapNames = maps;
            if (maps.Length > 0)
            {
                if (SelectedMap.Value >= maps.Length)
                    SelectedMap.Value = 0;
                ElementFactory.CreateDropdownSetting(DoublePanelLeft, new ElementStyle(titleWidth: 53f, themePanel: ThemePanel), SelectedMap,
                UIManager.GetLocale(cat, sub, "File"), maps, elementWidth: 269f);
                GameObject group = ElementFactory.CreateHorizontalGroup(DoublePanelLeft, 10f, TextAnchor.UpperLeft);
                foreach (string button in new string[] { "New", "Delete", "Import", "Export" })
                {
                    GameObject obj = ElementFactory.CreateDefaultButton(group.transform, style, UIManager.GetLocaleCommon(button),
                                                                        onClick: () => OnCustomButtonClick(button, true));
                }
            }
            else
            {
                GameObject group = ElementFactory.CreateHorizontalGroup(DoublePanelLeft, 10f, TextAnchor.UpperLeft);
                foreach (string button in new string[] { "New" })
                {
                    GameObject obj = ElementFactory.CreateDefaultButton(group.transform, style, UIManager.GetLocaleCommon(button),
                                                                        onClick: () => OnCustomButtonClick(button, true));
                }
            }
            CreateHorizontalDivider(DoublePanelLeft);
            ElementFactory.CreateDefaultLabel(DoublePanelLeft, new ElementStyle(fontSize: 20, themePanel: ThemePanel), 
                "Custom maps can be found in Documents/Aottg2/CustomMap. ", alignment: TextAnchor.MiddleLeft);
            ElementFactory.CreateDefaultLabel(DoublePanelLeft, new ElementStyle(fontSize: 20, themePanel: ThemePanel),
               "To use a custom map, select the Custom map category under the General section.", alignment: TextAnchor.MiddleLeft);
            var logics = BuiltinLevels.GetCustomGameModes();
            ElementFactory.CreateDefaultLabel(DoublePanelRight, style, UIManager.GetLocale(cat, sub, "CustomLogic"), FontStyle.Bold, alignment: TextAnchor.MiddleLeft);
            CreateHorizontalDivider(DoublePanelRight);
            CurrentLogicNames = logics;
            if (logics.Length > 0)
            {
                if (SelectedLogic.Value >= logics.Length)
                    SelectedLogic.Value = 0;
                ElementFactory.CreateDropdownSetting(DoublePanelRight, new ElementStyle(titleWidth: 53f, themePanel: ThemePanel), SelectedLogic,
                UIManager.GetLocale(cat, sub, "File"), logics, elementWidth: 269f);
                GameObject group = ElementFactory.CreateHorizontalGroup(DoublePanelRight, 10f, TextAnchor.UpperLeft);
                foreach (string button in new string[] { "New", "Delete", "Import", "Export" })
                {
                    GameObject obj = ElementFactory.CreateDefaultButton(group.transform, style, UIManager.GetLocaleCommon(button),
                                                                        onClick: () => OnCustomButtonClick(button, false));
                }
            }
            else
            {
                GameObject group = ElementFactory.CreateHorizontalGroup(DoublePanelRight, 10f, TextAnchor.UpperLeft);
                foreach (string button in new string[] { "New" })
                {
                    GameObject obj = ElementFactory.CreateDefaultButton(group.transform, style, UIManager.GetLocaleCommon(button),
                                                                        onClick: () => OnCustomButtonClick(button, false));
                }
            }
            CreateHorizontalDivider(DoublePanelRight);
            ElementFactory.CreateDefaultLabel(DoublePanelRight, new ElementStyle(fontSize: 20, themePanel: ThemePanel),
                "Custom logic can be found in Documents/Aottg2/CustomLogic", alignment: TextAnchor.MiddleLeft);
            ElementFactory.CreateDefaultLabel(DoublePanelRight, new ElementStyle(fontSize: 20, themePanel: ThemePanel),
                "To use a custom script, select it in the Game mode dropdown under the General section.", alignment: TextAnchor.MiddleLeft);
        }

        private void OnCustomButtonClick(string name, bool isMap)
        {
            switch (name)
            {
                case "New":
                    UIManager.CurrentMenu.NewImportPopup.Show(onSave: () => OnCustomOperationFinish(name, isMap));
                    break;
                case "Delete":
                    UIManager.CurrentMenu.ConfirmPopup.Show(UIManager.GetLocaleCommon("DeleteWarning"), () => OnCustomOperationFinish(name, isMap),
                        UIManager.GetLocaleCommon("Delete"));
                    break;
                case "Import":
                    UIManager.CurrentMenu.ImportPopup.Show(onSave: () => OnCustomOperationFinish(name, isMap));
                    break;
                case "Export":
                    if (isMap)
                        UIManager.CurrentMenu.ExportPopup.Show(BuiltinLevels.LoadMap("Custom", CurrentMapNames[SelectedMap.Value]));
                    else
                        UIManager.CurrentMenu.ExportPopup.Show(BuiltinLevels.LoadLogic(CurrentLogicNames[SelectedLogic.Value]));
                    break;
            }
        }

        private void OnCustomOperationFinish(string name, bool isMap)
        {
            switch (name)
            {
                case "New":
                    var newImportPopup = UIManager.CurrentMenu.NewImportPopup;
                    string fileName = newImportPopup.FileName.Value;
                    if (fileName == "")
                        newImportPopup.ShowError("Name cannot be empty.");
                    else if (isMap)
                    {
                        if (CurrentMapNames.Contains(fileName))
                            newImportPopup.ShowError("File name already exists.");
                        else
                        {
                            MapScript script = new MapScript();
                            try
                            {
                                script.Deserialize(UIManager.CurrentMenu.NewImportPopup.ImportSetting.Value);
                                BuiltinLevels.SaveCustomMap(UIManager.CurrentMenu.NewImportPopup.FileName.Value, script);
                                newImportPopup.Hide();
                            }
                            catch (Exception e)
                            {
                                newImportPopup.ShowError("Error importing: " + e.Message);
                            }
                        }
                    }
                    else
                    {
                        if (CurrentLogicNames.Contains(fileName))
                            newImportPopup.ShowError("File name already exists.");
                        else
                        {
                            string logic = UIManager.CurrentMenu.NewImportPopup.ImportSetting.Value;
                            string logicError = string.Empty;
                            if (logic != string.Empty)
                                logicError = CustomLogicManager.TryParseLogic(logic);
                            if (logicError != string.Empty)
                                newImportPopup.ShowError(logicError);
                            else
                            {
                                BuiltinLevels.SaveCustomLogic(fileName, logic);
                                newImportPopup.Hide();
                            }
                        }
                    }
                    break;
                case "Delete":
                    if (isMap)
                        BuiltinLevels.DeleteCustomMap(CurrentMapNames[SelectedMap.Value]);
                    else
                        BuiltinLevels.DeleteCustomLogic(CurrentLogicNames[SelectedLogic.Value]);
                    break;
                case "Import":
                    var importPopup = UIManager.CurrentMenu.ImportPopup;
                    if (isMap)
                    {
                        MapScript script = new MapScript();
                        try
                        {
                            script.Deserialize(importPopup.ImportSetting.Value);
                            BuiltinLevels.SaveCustomMap(CurrentMapNames[SelectedMap.Value], script);
                            importPopup.Hide();
                        }
                        catch (Exception e)
                        {
                            importPopup.ShowError("Error importing: " + e.Message);
                        }
                    }
                    else
                    {
                        string logic = importPopup.ImportSetting.Value;
                        string logicError = string.Empty;
                        if (logic != string.Empty)
                            logicError = CustomLogicManager.TryParseLogic(logic);
                        if (logicError != string.Empty)
                            importPopup.ShowError(logicError);
                        else
                        {
                            BuiltinLevels.SaveCustomLogic(CurrentLogicNames[SelectedLogic.Value], logic);
                            importPopup.Hide();
                        }
                    }
                    break;
            }
            Parent.RebuildCategoryPanel();
        }
    }
}
