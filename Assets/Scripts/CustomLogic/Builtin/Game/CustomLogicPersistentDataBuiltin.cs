using System.Collections.Generic;
using System.IO;
using Utility;
using SimpleJSONFixed;
using System.Linq;

namespace CustomLogic
{
    /// <summary>
    /// Store and retrieve persistent data. Persistent data can be saved and loaded from file.
    /// Supports float, int, string, and bool types. Note that any game mode may use the same file names,
    /// so it is recommended that you choose unique file names when saving and loading.
    /// Saved files are located in Documents/Aottg2/PersistentData.
    /// </summary>
    [CLType(Name = "PersistentData", Static = true, Abstract = true)]
    partial class CustomLogicPersistentDataBuiltin : BuiltinClassInstance
    {
        [CLConstructor]
        public CustomLogicPersistentDataBuiltin(){}

        /// <summary>
        /// Sets the property with given name to the object value. Valid value types are float, string, bool, and int.
        /// </summary>
        /// <param name="property">The name of the property.</param>
        /// <param name="value">The value to set (must be float, string, bool, int, or null).</param>
        [CLMethod]
        public static void SetProperty(
            string property,
            [CLParam(Type = "float|int|string|bool|null")]
            object value)
        {
            if (value is not (null or float or int or string or bool))
                throw new System.Exception("PersistentData.SetProperty only supports null, float, int, string, or bool values.");

            CustomLogicManager.PersistentData[property] = value;
        }

        /// <summary>
        /// Gets the property with given name.
        /// </summary>
        /// <param name="property">The name of the property.</param>
        /// <param name="defaultValue">The default value to return if the property does not exist.</param>
        /// <returns>The property value, or defaultValue if the property does not exist.</returns>
        [CLMethod]
        public static object GetProperty(
            string property,
            object defaultValue)
        {
            return CustomLogicManager.PersistentData.GetValueOrDefault(property, defaultValue);
        }

        /// <summary>
        /// Loads persistent data from given file name.
        /// </summary>
        /// <param name="fileName">The name of the file to load from.</param>
        /// <param name="encrypted">Whether the file is encrypted. If true, will treat the file as having been saved as encrypted.</param>
        [CLMethod]
        public static void LoadFromFile(
            string fileName,
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

        /// <summary>
        /// Saves current persistent data to given file name.
        /// </summary>
        /// <param name="fileName">The name of the file to save to.</param>
        /// <param name="encrypted">Whether to encrypt the file. If true, will also encrypt the file instead of using plaintext.</param>
        [CLMethod]
        public static void SaveToFile(
            string fileName,
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

        /// <summary>
        /// Clears current persistent data.
        /// </summary>
        [CLMethod]
        public static void Clear()
        {
            CustomLogicManager.PersistentData.Clear();
        }

        /// <summary>
        /// Determines whether or not the given fileName will be allowed for use when saving/loading a file.
        /// </summary>
        /// <param name="fileName">The file name to validate.</param>
        /// <returns>True if the file name is valid, false otherwise.</returns>
        [CLMethod]
        public static bool IsValidFileName(
            string fileName)
        {
            return Util.IsValidFileName(fileName);
        }

        /// <summary>
        /// Determines whether the file given already exists.
        /// </summary>
        /// <param name="fileName">The file name to check.</param>
        /// <returns>True if the file exists, false otherwise. Throws an error if given an invalid file name.</returns>
        [CLMethod]
        public static bool FileExists(
            string fileName)
        {
            if (!Util.IsValidFileName(fileName))
                throw new System.Exception("PersistentData.FileExists only supports legal fileName characters.");
            return File.Exists(Path.Combine(FolderPaths.PersistentData, fileName + ".txt"));
        }
    }
}
