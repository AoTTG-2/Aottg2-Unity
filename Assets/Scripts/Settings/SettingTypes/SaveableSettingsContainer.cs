using UnityEngine;
using System.IO;
using System;
using Utility;

namespace Settings
{
    abstract class SaveableSettingsContainer : BaseSettingsContainer
    {
        protected virtual string FolderPath { get { return FolderPaths.Settings; } }
        protected abstract string FileName { get; }
        protected virtual bool Encrypted => false;

        protected override void Setup()
        {
            RegisterSettings();
            Load();
            Apply();
        }

        public virtual void Save()
        {
            Directory.CreateDirectory(FolderPath);
            string text = SerializeToJsonString();
            if (Encrypted)
            {
                text = new SimpleAES().Encrypt(text);
            }
            File.WriteAllText(GetFilePath(), text);
        }

        public virtual void Load()
        {
            string path = GetFilePath();
            if (File.Exists(path))
            {
                string text = File.ReadAllText(path);
                if (Encrypted)
                {
                    text = new SimpleAES().Decrypt(text);
                }
                DeserializeFromJsonString(text);
            }
            else
            {
                try
                {
                    LoadLegacy();
                }
                catch
                {
                    Debug.Log("Exception occurred while loading legacy settings.");
                }
            }
        }

        protected virtual void LoadLegacy()
        {
        }

        protected virtual string GetFilePath()
        {
            return FolderPath + "/" + FileName;
        }
    }
}
