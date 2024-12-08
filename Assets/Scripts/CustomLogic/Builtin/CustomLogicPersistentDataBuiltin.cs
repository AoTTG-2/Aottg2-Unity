using ApplicationManagers;
using GameManagers;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utility;
using SimpleJSONFixed;
using System.Linq;

namespace CustomLogic
{
    [CLType(Static = true)]
    class CustomLogicPersistentDataBuiltin : CustomLogicClassInstanceBuiltin
    {
        public CustomLogicPersistentDataBuiltin() : base("PersistentData")
        {
        }

        [CLMethod("Sets a property in the persistent data.")]
        public static void SetProperty(string property, object value)
        {
            if (!(value == null || value is float || value is int || value is string || value is bool))
                throw new System.Exception("PersistentData.SetProperty only supports null, float, int, string, or bool values.");
            CustomLogicManager.PersistentData[property] = value;
        }

        [CLMethod("Gets a property from the persistent data.")]
        public static object GetProperty(string property, object defaultValue)
        {
            if (CustomLogicManager.PersistentData.ContainsKey(property))
                return CustomLogicManager.PersistentData[property];
            return defaultValue;
        }

        [CLMethod("Loads persistent data from a file.")]
        public static void LoadFromFile(string fileName, bool encrypted)
        {
            Directory.CreateDirectory(FolderPaths.PersistentData);
            CustomLogicManager.PersistentData.Clear();

            if (!Util.IsValidFileName(fileName))
                throw new System.Exception("PersistentData.LoadFromFile only supports legal fileName characters.");

            string path = Path.Combine(FolderPaths.PersistentData, fileName + ".txt");
            if (File.Exists(path))
            {
                string text = File.ReadAllText(path);
                if (encrypted)
                    text = new SimpleAES().Decrypt(text);
                var json = JSON.Parse(text);
                foreach (string key in json.Keys)
                {
                    string value = json[key].Value;
                    string type = value.Split(':')[0];
                    if (type == "float")
                        CustomLogicManager.PersistentData[key] = float.Parse(value.Substring(6));
                    else if (type == "int")
                        CustomLogicManager.PersistentData[key] = int.Parse(value.Substring(4));
                    else if (type == "string")
                        CustomLogicManager.PersistentData[key] = value.Substring(7);
                    else if (type == "bool")
                        CustomLogicManager.PersistentData[key] = value.Substring(5) == "1";
                }
            }
        }

        [CLMethod("Saves persistent data to a file.")]
        public static void SaveToFile(string fileName, bool encrypted)
        {
            Directory.CreateDirectory(FolderPaths.PersistentData);

            if (!Util.IsValidFileName(fileName))
                throw new System.Exception("PersistentData.LoadFromFile only supports legal fileName characters.");

            string path = Path.Combine(FolderPaths.PersistentData, fileName + ".txt");
            JSONNode node = new JSONObject();
            foreach (string key in CustomLogicManager.PersistentData.Keys)
            {
                object value = CustomLogicManager.PersistentData[key];
                if (value == null)
                    continue;
                if (value is float)
                    node.Add(key, new JSONString("float:" + value.ToString()));
                else if (value is int)
                    node.Add(key, new JSONString("int:" + value.ToString()));
                else if (value is string)
                    node.Add(key, new JSONString("string:" + value.ToString()));
                else if (value is bool)
                    node.Add(key, new JSONString("bool:" + (((bool)value == true) ? "1" : "0")));
            }
            string text = node.ToString(aIndent: 4);
            if (encrypted)
                text = new SimpleAES().Encrypt(text);
            if (text.Length > (1000 * 1000))
                throw new System.Exception("PersistentData.SaveToFile exceeded 1 mb limit.");
            foreach (var fi in new DirectoryInfo(FolderPaths.PersistentData).GetFiles().OrderByDescending(x => x.LastWriteTime).Skip(100))
                fi.Delete();
            File.WriteAllText(path, text);
        }

        [CLMethod("Clears all properties from the persistent data.")]
        public static void Clear()
        {
            CustomLogicManager.PersistentData.Clear();
        }

        [CLMethod("Checks if the specified file name is valid.")]
        public static bool IsValidFileName(string fileName)
        {
            return Util.IsValidFileName(fileName);
        }

        [CLMethod("Checks if the specified file exists.")]
        public static bool FileExists(string fileName)
        {
            if (!Util.IsValidFileName(fileName))
                throw new System.Exception("PersistentData.FileExists only supports legal fileName characters.");
            return File.Exists(Path.Combine(FolderPaths.PersistentData, fileName + ".txt"));
        }
    }
}
