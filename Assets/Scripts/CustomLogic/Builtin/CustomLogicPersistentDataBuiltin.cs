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
    class CustomLogicPersistentDataBuiltin: CustomLogicBaseBuiltin
    {
        public CustomLogicPersistentDataBuiltin(): base("PersistentData")
        {
        }

        public override object CallMethod(string name, List<object> parameters)
        {
            if (name == "SetProperty")
            {
                string property = (string)parameters[0];
                object value = parameters[1];
                if (!(value == null || value is float || value is int || value is string || value is bool))
                    throw new System.Exception("PersistentData.SetProperty only supports null, float, int, string, or bool values.");
                CustomLogicManager.PersistentData[property] = value;
                return null;
            }
            if (name == "GetProperty")
            {
                string property = (string)parameters[0];
                object value = parameters[1];
                if (CustomLogicManager.PersistentData.ContainsKey(property))
                    return CustomLogicManager.PersistentData[property];
                return value;
            }
            if (name == "LoadFromFile")
            {
                Directory.CreateDirectory(FolderPaths.PersistentData);
                CustomLogicManager.PersistentData.Clear();
                string fileName = (string)parameters[0];
                bool encrypted = (bool)parameters[1];
                string path = FolderPaths.PersistentData + "/" + fileName;
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
                return null;
            }
            if (name == "SaveToFile")
            {
                Directory.CreateDirectory(FolderPaths.PersistentData);
                string fileName = (string)parameters[0];
                bool encrypted = (bool)parameters[1];
                string path = FolderPaths.PersistentData + "/" + fileName;
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
                return null;
            }
            if (name == "Clear")
            {
                CustomLogicManager.PersistentData.Clear();
                return null;
            }
            return base.CallMethod(name, parameters);
        }
    }
}
