using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utility;
using Weather;

namespace Settings
{
    class WeatherSet : BaseSetSetting
    {
        public StringSetting Skybox = new StringSetting("Day1");
        public ColorSetting SkyboxColor = new ColorSetting(new Color255(128, 128, 128, 255));
        public ColorSetting Daylight = new ColorSetting(new Color255(255, 255, 255, 255));
        public FloatSetting DaylightIntensity = new FloatSetting(1f, minValue: 0f, maxValue: 2f);
        public Vector3Setting DaylightDirection = new Vector3Setting(new Vector3(45f, 45f, 0f));
        public ColorSetting AmbientLight = new ColorSetting(new Color255(126, 122, 114, 255));
        public ColorSetting Flashlight = new ColorSetting(new Color255(255, 255, 255, 0));
        public FloatSetting FogDensity = new FloatSetting(0f, minValue: 0f, maxValue: 1f);
        public ColorSetting FogColor = new ColorSetting(new Color255(128, 128, 128, 255));
        public FloatSetting Rain = new FloatSetting(0f, minValue: 0f, maxValue: 1f);
        public FloatSetting Thunder = new FloatSetting(0f, minValue: 0f, maxValue: 1f);
        public FloatSetting Snow = new FloatSetting(0f, minValue: 0f, maxValue: 1f);
        public FloatSetting Wind = new FloatSetting(0f, minValue: 0f, maxValue: 1f);
        public Vector3Setting WindDirection = new Vector3Setting(new Vector3(1f, 0f, 0f));
        public FloatSetting RainForce = new FloatSetting(0f, minValue: 0f, maxValue: 100000f);
        public FloatSetting SnowForce = new FloatSetting(0f, minValue: 0f, maxValue: 100000f);
        public FloatSetting WindForce = new FloatSetting(0f, minValue: 0f, maxValue: 100000f);
        public BoolSetting UseSchedule = new BoolSetting(false);
        public BoolSetting ScheduleLoop = new BoolSetting(false);
        public StringSetting Schedule = new StringSetting(string.Empty);
    }
}
