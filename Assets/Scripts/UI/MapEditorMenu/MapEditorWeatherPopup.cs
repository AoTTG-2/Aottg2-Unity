using ApplicationManagers;
using GameManagers;
using Photon.Pun.Demo.PunBasics;
using Settings;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utility;
using Weather;

namespace UI
{
    class MapEditorWeatherPopup: BasePopup
    {
        protected override string Title => "Weather";
        protected override float Width => 1010f;
        protected override float Height => 630f;
        protected override bool DoublePanel => true;
        protected override bool DoublePanelDivider => true;
        protected override bool ScrollBar => true;
        private BoolSetting _hasWeather = new BoolSetting(false);
        private ToggleSettingElement _hasWeatherElement;
        protected List<GameObject> _elements = new List<GameObject>();

        public override void Setup(BasePanel parent = null)
        {
            base.Setup(parent);
            ElementStyle style = new ElementStyle(titleWidth: 180f, themePanel: ThemePanel);
            _hasWeatherElement = ElementFactory.CreateToggleSetting(DoublePanelLeft, style, _hasWeather, "Map has weather", 
                tooltip: "Make the Map Default selection use these weather settings.").GetComponent<ToggleSettingElement>();
            CreateHorizontalDivider(DoublePanelLeft);
            GameObject group = ElementFactory.CreateHorizontalGroup(DoublePanelLeft, 10f, TextAnchor.UpperLeft);
            group = ElementFactory.CreateHorizontalGroup(DoublePanelLeft, 10f, TextAnchor.UpperLeft);
            foreach (string button in new string[] { "Import", "Export" })
            {
                GameObject obj = ElementFactory.CreateDefaultButton(group.transform, style, UIManager.GetLocaleCommon(button),
                                                                    onClick: () => OnWeatherButtonClick(button));
            }
            ElementFactory.CreateTextButton(BottomBar, style, UIManager.GetLocaleCommon("Save"), onClick: () => OnWeatherButtonClick("Save"));
        }

        public override void Show()
        {
            base.Show();
            foreach (var go in _elements)
                Destroy(go);
            
            _elements.Clear();
            var gameManager = (MapEditorGameManager)SceneLoader.CurrentGameManager;
            ElementStyle style = new ElementStyle(titleWidth: 180f, themePanel: ThemePanel);
            ColorPickPopup colorPickPopup = UIManager.CurrentMenu.ColorPickPopup;
            _hasWeather.Value = gameManager.MapScript.Options.HasWeather;
            _hasWeatherElement.SyncElement();
            WeatherSet set = gameManager.MapScript.Weather;
            _elements.Add(ElementFactory.CreateToggleSetting(DoublePanelLeft, style, set.UseSchedule, "Use schedule",
                tooltip: "Follow a programmed weather schedule."));
            _elements.Add(ElementFactory.CreateToggleSetting(DoublePanelLeft, style, set.ScheduleLoop, "Loop schedule"));
            var group = ElementFactory.CreateHorizontalGroup(DoublePanelLeft, 10f, TextAnchor.UpperLeft);
            _elements.Add(group);
            foreach (string button in new string[] { "Import", "Export" })
            {
                GameObject obj = ElementFactory.CreateDefaultButton(group.transform, style, UIManager.GetLocaleCommon(button),
                                                                    onClick: () => OnWeatherButtonClick(button + "Schedule"));
            }
            var vector3Popup = UIManager.CurrentMenu.Vector3Popup;
            var dropdown = ElementFactory.CreateDropdownSetting(DoublePanelRight, style, set.Skybox, "Skybox", Util.EnumToStringArray<WeatherSkybox>());
            dropdown.GetComponent<DropdownSettingElement>().FixScale();
            _elements.Add(dropdown);
            _elements.Add(ElementFactory.CreateColorSetting(DoublePanelRight, style, set.SkyboxColor, "Skybox color", colorPickPopup));
            _elements.Add(ElementFactory.CreateColorSetting(DoublePanelRight, style, set.Daylight, "Daylight", colorPickPopup));
            _elements.Add(ElementFactory.CreateSliderSetting(DoublePanelRight, style, set.DaylightIntensity, "Daylight intensity"));
            _elements.Add(ElementFactory.CreateVector3Setting(DoublePanelRight, style, set.DaylightDirection, "Daylight direction", vector3Popup));
            _elements.Add(ElementFactory.CreateColorSetting(DoublePanelRight, style, set.AmbientLight, "Ambient light", colorPickPopup));
            _elements.Add(ElementFactory.CreateColorSetting(DoublePanelRight, style, set.Flashlight, "Flashlight", colorPickPopup));
            _elements.Add(ElementFactory.CreateColorSetting(DoublePanelRight, style, set.FogColor, "Fog color", colorPickPopup));
            _elements.Add(ElementFactory.CreateSliderInputSetting(DoublePanelRight, style, set.FogDensity, "Fog density", sliderWidth: 130f, decimalPlaces: 3));
            _elements.Add(ElementFactory.CreateSliderSetting(DoublePanelRight, style, set.Rain, "Rain"));
            _elements.Add(ElementFactory.CreateSliderSetting(DoublePanelRight, style, set.Thunder, "Thunder"));
            _elements.Add(ElementFactory.CreateSliderSetting(DoublePanelRight, style, set.Snow, "Snow"));
            _elements.Add(ElementFactory.CreateSliderSetting(DoublePanelRight, style, set.Wind, "Wind"));
            _elements.Add(ElementFactory.CreateVector3Setting(DoublePanelRight, style, set.WindDirection, "Wind direction", vector3Popup));
            _elements.Add(ElementFactory.CreateInputSetting(DoublePanelRight, style, set.RainForce, "Rain force"));
            _elements.Add(ElementFactory.CreateInputSetting(DoublePanelRight, style, set.SnowForce, "Snow force"));
            _elements.Add(ElementFactory.CreateInputSetting(DoublePanelRight, style, set.WindForce, "Wind force"));
        }

        private void OnWeatherButtonClick(string name)
        {
            var gameManager = (MapEditorGameManager)SceneLoader.CurrentGameManager;
            WeatherSet set = gameManager.MapScript.Weather;
            SetNamePopup setNamePopup = UIManager.CurrentMenu.SetNamePopup;
            switch (name)
            {
                case "Import":
                    UIManager.CurrentMenu.ImportPopup.Show(onSave: () => OnWeatherSetOperationFinish(name));
                    break;
                case "Export":
                    var json = set.SerializeToJsonObject();
                    if (json.HasKey("Preset"))
                        json["Preset"] = false;
                    UIManager.CurrentMenu.ExportPopup.Show(json.ToString(aIndent: 4));
                    break;
                case "ImportSchedule":
                    UIManager.CurrentMenu.ImportPopup.Show(onSave: () => OnWeatherSetOperationFinish(name));
                    break;
                case "ExportSchedule":
                    UIManager.CurrentMenu.ExportPopup.Show(set.Schedule.Value);
                    break;
                case "Save":
                    gameManager.MapScript.Options.HasWeather = _hasWeather.Value;
                    ((MapEditorMenu)UIManager.CurrentMenu)._topPanel.Save();
                    Hide();
                    break;
            }
        }

        private void OnWeatherSetOperationFinish(string name)
        {
            ImportPopup importPopup = UIManager.CurrentMenu.ImportPopup;
            var gameManager = (MapEditorGameManager)SceneLoader.CurrentGameManager;
            WeatherSet set = gameManager.MapScript.Weather;
            switch (name)
            {
                case "Import":
                    try
                    {
                        var setName = set.Name.Value;
                        set.DeserializeFromJsonString(importPopup.ImportSetting.Value);
                        set.Preset.Value = false;
                        set.Name.Value = setName;
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
                        set.Schedule.Value = importPopup.ImportSetting.Value;
                        importPopup.Hide();
                    }
                    break;
            }
            Show();
        }
    }
}
