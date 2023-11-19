using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace Settings
{
    abstract class PresetSettingsContainer : SaveableSettingsContainer
    {
        protected virtual string PresetFolderPath { get { return Application.streamingAssetsPath + "/Presets"; } }

        public override void Load()
        {
            string path = GetPresetFilePath();
            Dictionary<string, string> presets = new Dictionary<string, string>();
            if (File.Exists(path))
            {
                DeserializeFromJsonString(File.ReadAllText(path));
                foreach (string key in Settings.Keys)
                {
                    presets.Add(key, ((BaseSetting)Settings[key]).SerializeToJsonString());
                }
            }
            SetDefault();
            base.Load();
            foreach (KeyValuePair<string, string> entry in presets)
            {
                BaseSetting setting = (BaseSetting)Settings[entry.Key];
                if (setting is ISetSettingsContainer)
                {
                    ISetSettingsContainer setSetting = (ISetSettingsContainer)setting;
                    setSetting.SetPresetsFromJsonString(entry.Value);
                }
            }
        }

        protected virtual string GetPresetFilePath()
        {
            return PresetFolderPath + "/" + FileName;
        }
    }
}
