using ApplicationManagers;
using Settings;
using SimpleJSONFixed;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Utility;

namespace Map
{
    class Experience
    {
        public string Category;
        public string SubCategory;
        public string Name;
        public string Description;

        public Experience(JSONNode node)
        {
            // Category, SubCategory, Name, Description
            Category = node["Category"];
            SubCategory = node["SubCategory"];
            Name = node["Name"];
            Description = node["Description"];
        }

        public JSONNode Serialize()
        {
            JSONObject json = new JSONObject();
            json.Add("Name", Name);
            json.Add("Category", Category);
            json.Add("SubCategory", Category);
            json.Add("Description", Description);
            return json;
        }
    }

    class BuiltinLevels
    {
        private static JSONNode _info;
        public static string CustomMapFolderPath = FolderPaths.CustomMap;
        public static string CustomMapAutosaveFolderPath = FolderPaths.CustomMapAutosave;
        public static string CustomLogicFolderPath = FolderPaths.CustomLogic;
        public static string UseMapLogic = "Map Logic";

        public static void Init()
        {
            Directory.CreateDirectory(CustomMapFolderPath);
            Directory.CreateDirectory(CustomLogicFolderPath);
            Directory.CreateDirectory(CustomMapAutosaveFolderPath);
            _info = JSON.Parse(((TextAsset)ResourceManager.LoadAsset(ResourcePaths.Info, "BuiltinMapInfo")).text);
        }

        public static JSONNode GetInfo()
        {
            return _info;
        }


        public static string LoadMap(string category, string name)
        {
            if (category == "Custom")
            {
                string path = CustomMapFolderPath + "/" + name + ".txt";
                if (File.Exists(path))
                    return File.ReadAllText(path);
            }
            else
            {
                var map = GetMap(category, name);
                if (map.HasKey("Category"))
                    category = map["Category"];
                return ResourceManager.TryLoadText(ResourcePaths.BuiltinMaps, category + "/" + name + "Map");
            }
            return string.Empty;
        }

        public static string LoadAutosave(string name)
        {
            string path = CustomMapAutosaveFolderPath + "/" + name + ".txt";
            if (File.Exists(path))
                return File.ReadAllText(path);
            return string.Empty;
        }

        public static string LoadLogic(string name)
        {
            if (name == "")
                return string.Empty;
            if (name == UseMapLogic)
                return UseMapLogic;
            foreach (JSONNode gameMode in _info["GameModes"])
            {
                if (gameMode["Name"] == name)
                    return ResourceManager.TryLoadText(ResourcePaths.Modes, name + "Logic");
            }
            string path = CustomLogicFolderPath + "/" + name + ".cl";
            if (File.Exists(path))
                return File.ReadAllText(path);
            path = CustomLogicFolderPath + "/" + name + ".txt";
            if (File.Exists(path))
                return File.ReadAllText(path);
            return string.Empty;
        }

        public static bool IsLogicBuiltin(string name)
        {
            if (name == UseMapLogic)
                return true;
            foreach (JSONNode gameMode in _info["GameModes"])
            {
                if (gameMode["Name"] == name)
                    return true;
            }
            return false;
        }

        // Optimally only extend once or twice.
        public static PresetDefinition Resolve(string name)
        {
            string text = ResourceManager.TryLoadText(ResourcePaths.Experiences, name + "Exp");
            PresetDefinition preset = JsonUtility.FromJson<PresetDefinition>(text);

            if (preset.Extends == null)
            {
                return preset;
            }
            else
            {
                return PresetMerger.Merge(Resolve(preset.Extends), preset);
            }
        }

        public static PresetDefinition LoadExperience(string name)
        {
            if (name == "")
                return null;
            foreach (JSONNode experience in _info["Experiences"])
            {
                if (experience["Name"] == name)
                {
                    return Resolve(experience["Name"]); // dont need looping since all we care about is name.
                }
            }
            return null;    // determine later if we want custom experiences that aren't bundled with their own content.
        }

        public static List<Experience> GetExperiences()
        {
            List<Experience> experiences = new List<Experience>();
            foreach (JSONNode experience in _info["Experiences"])
                experiences.Add(new Experience(experience));
            return experiences;
        }

        public static string[] GetMapCategories()
        {
            List<string> categories = new List<string>();
            foreach (JSONNode category in _info["MapCategories"])
                categories.Add(category["Name"]);
            categories.Add("Custom");
            return categories.ToArray();
        }

        public static string[] GetMapNames(string category)
        {
            if (category == "Custom")
            {
                string[] files = GetTxtFiles(CustomMapFolderPath);
                for (int i = 0; i < files.Length; i++)
                    files[i] = files[i].Replace(".txt", "");
                return files;
            }
            else
            {
                List<string> mapNames = new List<string>();
                foreach (JSONNode mapCategory in _info["MapCategories"])
                {
                    if (mapCategory["Name"] == category)
                    {
                        foreach (JSONNode map in mapCategory["Maps"])
                            mapNames.Add(map["Name"]);
                    }
                }
                return mapNames.ToArray();
            }
        }

        public static string[] QueryMapsNames(UIRule query)
        {
            HashSet<string> mapNames = new HashSet<string>();
            foreach (JSONNode mapCategory in _info["MapCategories"])
            {
                foreach (JSONNode map in mapCategory["Maps"])
                {
                    if (query.MatchesAllowedValues(map["name"]) || query.AllowedValues.Count == 0)
                        mapNames.Add(map["Name"]);
                }

            }
            return mapNames.ToArray();
        }

        public static string[] QueryModeNames(UIRule query)
        {
            HashSet<string> mapNames = new HashSet<string>();
            foreach (JSONNode mode in _info["GameModes"])
            {
                if (query.MatchesAllowedValues(mode["name"]) || query.AllowedValues.Count == 0)
                    mapNames.Add(mode["Name"]);

            }
            return mapNames.ToArray();
        }

        public static string[] GetAutosaveNames()
        {
            string[] files = GetTxtFiles(CustomMapAutosaveFolderPath);
            for (int i = 0; i < files.Length; i++)
                files[i] = files[i].Replace(".txt", "");
            return files;
        }

        public static void DeleteCustomMap(string name)
        {
            File.Delete(CustomMapFolderPath + "/" + name + ".txt");
        }

        public static void DeleteCustomLogic(string name)
        {
            File.Delete(CustomLogicFolderPath + "/" + name + ".txt");
            File.Delete(CustomLogicFolderPath + "/" + name + ".cl");
        }

        public static void SaveCustomMap(string name, MapScript script)
        {
            File.WriteAllText(CustomMapFolderPath + "/" + name + ".txt", script.Serialize());
        }

        public static void AutosaveCustomMap(string name, MapScript script)
        {
            File.WriteAllText(CustomMapAutosaveFolderPath + "/" + name + ".txt", script.Serialize());
        }

        public static void SaveCustomLogic(string name, string script)
        {
            File.WriteAllText(CustomLogicFolderPath + "/" + name + ".cl", script);
        }

        public static string[] GetGameModes(string category, string mapName, bool hasMapLogic)
        {
            List<string> gameModes = new List<string>();
            if (hasMapLogic)
                gameModes.Add(UseMapLogic);
            foreach (JSONNode gameMode in _info["GameModes"])
                gameModes.Add(gameMode["Name"].Value);
            JSONNode cat = GetCategory(category);
            JSONNode map = GetMap(category, mapName);
            if (cat != null && map != null)
            {
                if (map.HasKey("IncludedModes"))
                {
                    gameModes.Clear();
                    foreach (JSONNode node in map["IncludedModes"])
                        gameModes.Add(node.Value);
                }
                else if (map.HasKey("ExcludedModes"))
                {
                    foreach (JSONNode node in map["ExcludedModes"])
                    {
                        if (gameModes.Contains(node.Value))
                            gameModes.Remove(node.Value);
                    }
                }
                else if (cat.HasKey("IncludedModes"))
                {
                    gameModes.Clear();
                    foreach (JSONNode node in cat["IncludedModes"])
                        gameModes.Add(node.Value);
                }
                else if (cat.HasKey("ExcludedModes"))
                {
                    foreach (JSONNode node in cat["ExcludedModes"])
                    {
                        if (gameModes.Contains(node.Value))
                            gameModes.Remove(node.Value);
                    }
                }
            }
            string[] names = GetCustomModes();
            foreach (string name in names)
            {
                if (!gameModes.Contains(name))
                    gameModes.Add(name);
            }
            return gameModes.ToArray();
        }

        public static void LoadMiscSettings(string category, string mapName, string gameMode, InGameMiscSettings settings)
        {
            Dictionary<string, JSONNode> defaultSettings = GetMiscSettings(category, mapName, gameMode);
            foreach (string key in defaultSettings.Keys)
            {
                JSONNode value = defaultSettings[key];
                if (settings.Settings.Contains(key))
                {
                    BaseSetting setting = (BaseSetting)settings.Settings[key];
                    if (setting is BoolSetting)
                        ((BoolSetting)setting).Value = value.AsBool;
                    else if (setting is IntSetting)
                        ((IntSetting)setting).Value = value.AsInt;
                    else if (setting is FloatSetting)
                        ((FloatSetting)setting).Value = value.AsFloat;
                    else if (setting is StringSetting)
                        ((StringSetting)setting).Value = value.ToString();
                }
            }
        }

        private static string[] GetTxtFiles(string path)
        {
            if (Directory.Exists(path))
                return Directory.GetFiles(path, "*.txt", SearchOption.TopDirectoryOnly).Select(f => Path.GetFileName(f)).ToArray();
            return new string[0];
        }

        private static string[] GetClFiles(string path)
        {
            if (Directory.Exists(path))
                return Directory.GetFiles(path, "*.cl", SearchOption.TopDirectoryOnly).Select(f => Path.GetFileName(f)).ToArray();
            return new string[0];
        }

        public static string[] GetCustomModes()
        {
            List<string> modes = new List<string>();
            HashSet<string> foundModes = new HashSet<string>();
            string[] clFiles = GetClFiles(CustomLogicFolderPath);
            string[] txtFiles = GetTxtFiles(CustomLogicFolderPath);
            foreach (string file in clFiles)
            {
                string name = file.Replace(".cl", "");
                modes.Add(name);
                foundModes.Add(name);
            }
            foreach (string file in txtFiles)
            {
                string name = file.Replace(".txt", "");
                if (!foundModes.Contains(name))
                {
                    modes.Add(name);
                    foundModes.Add(name);
                }
            }
            return modes.ToArray();
        }

        private static Dictionary<string, JSONNode> GetMiscSettings(string category, string mapName, string gameMode)
        {
            JSONNode map = GetMap(category, mapName);
            JSONNode mode = GetGameMode(gameMode);
            Dictionary<string, JSONNode> settings = new Dictionary<string, JSONNode>();
            if (mode != null && mode.HasKey("MiscSettings"))
                LoadSettings(settings, mode["MiscSettings"]);
            if (map != null && map.HasKey("MiscSettings"))
                LoadSettings(settings, map["MiscSettings"]);
            return settings;
        }

        private static JSONNode GetMap(string category, string mapName)
        {
            foreach (JSONNode node in _info["MapCategories"])
            {
                if (node["Name"] == category)
                {
                    foreach (JSONNode map in node["Maps"])
                    {
                        if (map["Name"] == mapName)
                            return map;
                    }
                }
            }
            return null;
        }

        private static JSONNode GetCategory(string category)
        {
            foreach (JSONNode node in _info["MapCategories"])
            {
                if (node["Name"] == category)
                    return node;
            }
            return null;
        }

        private static JSONNode GetGameMode(string gameMode)
        {
            foreach (JSONNode node in _info["GameModes"])
            {
                if (node["Name"] == gameMode)
                    return node;
            }
            return null;
        }

        private static void LoadSettings(Dictionary<string, JSONNode> current, JSONNode node)
        {
            foreach (string key in node.Keys)
            {
                if (current.ContainsKey(key))
                    current[key] = node[key];
                else
                    current.Add(key, node[key]);
            }
        }
    }
}
