using System.Collections.Generic;
using System.IO;
using Utility;
using SimpleJSONFixed;
using System.Linq;

namespace CustomLogic
{
    [CLType(Name = "PersistentData", Static = true, Abstract = true, Description = "Store and retrieve persistent data. Persistent data can be saved and loaded from file. Supports float, int, string, and bool types. Note that any game mode may use the same file names, so it is recommended that you choose unique file names when saving and loading. Saved files are located in Documents/Aottg2/PersistentData.")]
    partial class CustomLogicPersistentDataBuiltin : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicPersistentDataBuiltin(){}

        [CLMethod("Sets the property with given name to the object value. Valid value types are float, string, bool, and int.")]
        public static void SetProperty(
            [CLParam("The name of the property.")]
            string property,
            [CLParam("The value to set (must be float, string, bool, int, or null).", Type = "float|int|string|bool|null")]
            object value)
        {
            if (value is not (null or float or int or string or bool))
                throw new System.Exception("PersistentData.SetProperty only supports null, float, int, string, or bool values.");

            CustomLogicManager.PersistentData[property] = value;
        }

        [CLMethod("Gets the property with given name. If property does not exist, returns defaultValue.")]
        public static object GetProperty(
            [CLParam("The name of the property.")]
            string property,
            [CLParam("The default value to return if the property does not exist.")]
            object defaultValue)
        {
            return CustomLogicManager.PersistentData.GetValueOrDefault(property, defaultValue);
        }

        [CLMethod("Loads persistent data from given file name. If encrypted is true, will treat the file as having been saved as encrypted.")]
        public static void LoadFromFile(
            [CLParam("The name of the file to load from.")]
            string fileName,
            [CLParam("Whether the file is encrypted.")]
            bool encrypted)
        {
            Directory.CreateDirectory(FolderPaths.PersistentData);
            CustomLogicManager.PersistentData.Clear();

            if (!Util.IsValidFileName(fileName))
                throw new System.Exception("PersistentData.LoadFromFile only supports legal fileName characters.");

            var path = Path.Combine(FolderPaths.PersistentData, fileName + ".txt");
            if (File.Exists(path))
            {
                var text = File.ReadAllText(path);
                if (encrypted)
                    text = new SimpleAES().Decrypt(text);
                var json = JSON.Parse(text);
                foreach (var key in json.Keys)
                {
                    var value = json[key].Value;
                    var type = value.Split(':')[0];
                    CustomLogicManager.PersistentData[key] = type switch
                    {
                        "float" => float.Parse(value[6..]),
                        "int" => int.Parse(value[4..]),
                        "string" => value[7..],
                        "bool" => value[5..] == "1",
                        _ => CustomLogicManager.PersistentData[key]
                    };
                }
            }
        }

        [CLMethod("Saves current persistent data to given file name. If encrypted is true, will also encrypt the file instead of using plaintext.")]
        public static void SaveToFile(
            [CLParam("The name of the file to save to.")]
            string fileName,
            [CLParam("Whether to encrypt the file.")]
            bool encrypted)
        {
            Directory.CreateDirectory(FolderPaths.PersistentData);

            if (!Util.IsValidFileName(fileName))
                throw new System.Exception("PersistentData.SaveToFile only supports legal fileName characters.");

            var path = Path.Combine(FolderPaths.PersistentData, fileName + ".txt");
            JSONNode node = new JSONObject();
            foreach (var key in CustomLogicManager.PersistentData.Keys)
            {
                var value = CustomLogicManager.PersistentData[key];
                if (value == null)
                    continue;
                if (value is float)
                    node.Add(key, new JSONString("float:" + value));
                else if (value is int)
                    node.Add(key, new JSONString("int:" + value));
                else if (value is string)
                    node.Add(key, new JSONString("string:" + value));
                else if (value is bool b)
                    node.Add(key, new JSONString("bool:" + (b ? "1" : "0")));
            }
            var text = node.ToString(aIndent: 4);
            if (encrypted)
                text = new SimpleAES().Encrypt(text);
            if (text.Length > (1000 * 1000))
                throw new System.Exception("PersistentData.SaveToFile exceeded 1 mb limit.");
            foreach (var fi in new DirectoryInfo(FolderPaths.PersistentData).GetFiles().OrderByDescending(x => x.LastWriteTime).Skip(100))
                fi.Delete();
            File.WriteAllText(path, text);
        }

        [CLMethod("Clears current persistent data.")]
        public static void Clear()
        {
            CustomLogicManager.PersistentData.Clear();
        }

        [CLMethod("Determines whether or not the given fileName will be allowed for use when saving/loading a file.")]
        public static bool IsValidFileName(
            [CLParam("The file name to validate.")]
            string fileName)
        {
            return Util.IsValidFileName(fileName);
        }

        [CLMethod("Determines whether the file given already exists. Throws an error if given an invalid file name.")]
        public static bool FileExists(
            [CLParam("The file name to check.")]
            string fileName)
        {
            if (!Util.IsValidFileName(fileName))
                throw new System.Exception("PersistentData.FileExists only supports legal fileName characters.");
            return File.Exists(Path.Combine(FolderPaths.PersistentData, fileName + ".txt"));
        }
    }
}
