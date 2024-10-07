using System;
using UnityEngine;

namespace Settings
{
    class WeatherSettings : PresetSettingsContainer
    {
        protected override string FileName { get { return "Weather.json"; } }
        public SetSettingsContainer<WeatherSet> WeatherSets = new SetSettingsContainer<WeatherSet>();
    }
}
