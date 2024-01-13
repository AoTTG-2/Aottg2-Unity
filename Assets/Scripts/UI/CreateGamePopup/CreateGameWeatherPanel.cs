using Map;
using Settings;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using Weather;
using Utility;

namespace UI
{
    class CreateGameWeatherPanel : CreateGameCategoryPanel
    {
        protected override bool ScrollBar => true;
        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(titleWidth: 180f, themePanel: ThemePanel);
            ColorPickPopup colorPickPopup = UIManager.CurrentMenu.ColorPickPopup;
            WeatherSettings settings = SettingsManager.WeatherSettings;
            settings.WeatherSets.SelectedSetIndex.Value = SettingsManager.InGameUI.WeatherIndex.Value;
            ElementFactory.CreateDropdownSetting(DoublePanelLeft, new ElementStyle(titleWidth: 140f, themePanel: ThemePanel), settings.WeatherSets.GetSelectedSetIndex(),
                "Weather set", settings.WeatherSets.GetSetNames(), elementWidth: 205f, onDropdownOptionSelect: () => OnWeatherSetSelected(),
                tooltip: "* = preset and cannot be modified or deleted. Create a new set to use custom settings.");
            GameObject group = ElementFactory.CreateHorizontalGroup(DoublePanelLeft, 10f, TextAnchor.UpperLeft);
            foreach (string button in new string[] { "Create", "Delete", "Rename", "Copy" })
            {
                GameObject obj = ElementFactory.CreateDefaultButton(group.transform, style, UIManager.GetLocaleCommon(button),
                                                                    onClick: () => OnWeatherPanelButtonClick(button));
            }
            group = ElementFactory.CreateHorizontalGroup(DoublePanelLeft, 10f, TextAnchor.UpperLeft);
            foreach (string button in new string[] { "Import", "Export" })
            {
                GameObject obj = ElementFactory.CreateDefaultButton(group.transform, style, UIManager.GetLocaleCommon(button),
                                                                    onClick: () => OnWeatherPanelButtonClick(button));
            }
            WeatherSet set = (WeatherSet)settings.WeatherSets.GetSelectedSet();
            CreateHorizontalDivider(DoublePanelLeft);
            ElementStyle toggleStyle = new ElementStyle(titleWidth: 150f, themePanel: ThemePanel);
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, set.UseSchedule, "Use schedule",
                tooltip: "Follow a programmed weather schedule.");
            ElementFactory.CreateToggleSetting(DoublePanelLeft, style, set.ScheduleLoop, "Loop schedule");
            group = ElementFactory.CreateHorizontalGroup(DoublePanelLeft, 10f, TextAnchor.UpperLeft);
            foreach (string button in new string[] { "Import", "Export" })
            {
                GameObject obj = ElementFactory.CreateDefaultButton(group.transform, style, UIManager.GetLocaleCommon(button),
                                                                    onClick: () => OnWeatherPanelButtonClick(button + "Schedule"));
            }
            if (set.Preset.Value)
                ElementFactory.CreateDefaultLabel(DoublePanelRight, style, "*Weather presets cannot be modified. Create a new set to use custom settings.",
                    FontStyle.Normal, TextAnchor.MiddleCenter);
            ElementFactory.CreateDropdownSetting(DoublePanelRight, style, set.Skybox, "Skybox", Util.EnumToStringArray<WeatherSkybox>());
            ElementFactory.CreateColorSetting(DoublePanelRight, style, set.SkyboxColor, "Skybox color", colorPickPopup);
            ElementFactory.CreateColorSetting(DoublePanelRight, style, set.Daylight, "Daylight", colorPickPopup);
            ElementFactory.CreateSliderSetting(DoublePanelRight, style, set.DaylightIntensity, "Daylight intensity");
            ElementFactory.CreateColorSetting(DoublePanelRight, style, set.AmbientLight, "Ambient light", colorPickPopup);
            ElementFactory.CreateColorSetting(DoublePanelRight, style, set.Flashlight, "Flashlight", colorPickPopup);
            ElementFactory.CreateColorSetting(DoublePanelRight, style, set.FogColor, "Fog color", colorPickPopup);
            ElementFactory.CreateSliderSetting(DoublePanelRight, style, set.FogDensity, "Fog density");
            ElementFactory.CreateSliderSetting(DoublePanelRight, style, set.Rain, "Rain");
            ElementFactory.CreateSliderSetting(DoublePanelRight, style, set.Thunder, "Thunder");
            ElementFactory.CreateSliderSetting(DoublePanelRight, style, set.Snow, "Snow");
            ElementFactory.CreateSliderSetting(DoublePanelRight, style, set.Wind, "Wind");
        }

        private void OnWeatherSetSelected()
        {
            SettingsManager.InGameUI.WeatherIndex.Value = SettingsManager.WeatherSettings.WeatherSets.SelectedSetIndex.Value;
            Parent.RebuildCategoryPanel();
        }

        private void OnWeatherPanelButtonClick(string name)
        {
            WeatherSettings settings = SettingsManager.WeatherSettings;
            SetNamePopup setNamePopup = UIManager.CurrentMenu.SetNamePopup;
            switch (name)
            {
                case "Create":
                    setNamePopup.Show("New set", () => OnWeatherSetOperationFinish(name), UIManager.GetLocaleCommon("Create"));
                    break;
                case "Delete":
                    if (settings.WeatherSets.CanDeleteSelectedSet())
                        UIManager.CurrentMenu.ConfirmPopup.Show(UIManager.GetLocaleCommon("DeleteWarning"), () => OnWeatherSetOperationFinish(name),
                            UIManager.GetLocaleCommon("Delete"));
                    break;
                case "Rename":
                    if (settings.WeatherSets.CanEditSelectedSet())
                    {
                        string currentSetName = settings.WeatherSets.GetSelectedSet().Name.Value;
                        setNamePopup.Show(currentSetName, () => OnWeatherSetOperationFinish(name), UIManager.GetLocaleCommon("Rename"));
                    }
                    break;
                case "Copy":
                    setNamePopup.Show("New set", () => OnWeatherSetOperationFinish(name), UIManager.GetLocaleCommon("Copy"));
                    break;
                case "Import":
                    if (settings.WeatherSets.CanEditSelectedSet())
                        UIManager.CurrentMenu.ImportPopup.Show(onSave: () => OnWeatherSetOperationFinish(name));
                    break;
                case "Export":
                    var set = (WeatherSet)settings.WeatherSets.GetSelectedSet();
                    var json = set.SerializeToJsonObject();
                    if (json.HasKey("Preset"))
                        json["Preset"] = false;
                    UIManager.CurrentMenu.ExportPopup.Show(json.ToString(aIndent: 4));
                    break;
                case "ImportSchedule":
                    UIManager.CurrentMenu.ImportPopup.Show(onSave: () => OnWeatherSetOperationFinish(name));
                    break;
                case "ExportSchedule":
                    UIManager.CurrentMenu.ExportPopup.Show(((WeatherSet)settings.WeatherSets.GetSelectedSet()).Schedule.Value);
                    break;
            }
        }

        private void OnWeatherSetOperationFinish(string name)
        {
            SetNamePopup setNamePopup = UIManager.CurrentMenu.SetNamePopup;
            SetSettingsContainer<WeatherSet> settings = SettingsManager.WeatherSettings.WeatherSets;
            ImportPopup importPopup = UIManager.CurrentMenu.ImportPopup;
            switch (name)
            {
                case "Create":
                    settings.CreateSet(setNamePopup.NameSetting.Value);
                    settings.GetSelectedSetIndex().Value = settings.GetSets().GetCount() - 1;
                    break;
                case "Delete":
                    settings.DeleteSelectedSet();
                    settings.GetSelectedSetIndex().Value = 0;
                    break;
                case "Rename":
                    settings.GetSelectedSet().Name.Value = setNamePopup.NameSetting.Value;
                    break;
                case "Copy":
                    settings.CopySelectedSet(setNamePopup.NameSetting.Value);
                    settings.GetSelectedSetIndex().Value = settings.GetSets().GetCount() - 1;
                    break;
                case "Import":
                    try
                    {
                        var setName = settings.GetSelectedSet().Name.Value;
                        settings.GetSelectedSet().DeserializeFromJsonString(importPopup.ImportSetting.Value);
                        settings.GetSelectedSet().Preset.Value = false;
                        settings.GetSelectedSet().Name.Value = setName;
                        importPopup.Hide();
                    }
                    catch
                    {
                        importPopup.ShowError("Invalid weather preset.");
                    }
                    break;
                case "ImportSchedule":
                    string error = (new WeatherSchedule()).DeserializeFromCSV(importPopup.ImportSetting.Value);
                    if (error != string.Empty)
                        importPopup.ShowError(error);
                    else
                    {
                        ((WeatherSet)settings.GetSelectedSet()).Schedule.Value = importPopup.ImportSetting.Value;
                        importPopup.Hide();
                    }
                    break;
            }
            Parent.RebuildCategoryPanel();
        }
    }
}
