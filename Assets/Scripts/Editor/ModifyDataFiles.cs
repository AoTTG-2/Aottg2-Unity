using Map;
using SimpleJSONFixed;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    class ModifyDataFiles : EditorWindow
    {
        [MenuItem("AoTTG2/Editor/UpdateDataFiles")]
        public static void ShowWindow()
        {
            GetWindow(typeof(ModifyDataFiles));
        }

        public void OnGUI()
        {
            if (GUILayout.Button("Update Experiences"))
            {
                UpdateExperiences();
            }
        }

        string ExperiencesPath = "Assets/Resources/Data/Experiences";
        string InfoPath = "Assets/Resources/Data/Info";

        public void UpdateExperiences()
        {
            BuiltinLevels.Init();
            string experiencesFolder = ExperiencesPath;
            string[] experienceFiles = Directory.GetFiles(experiencesFolder, "*Exp.json", SearchOption.TopDirectoryOnly);

            var mapCategories = new HashSet<string>();
            var gameModes = new HashSet<string>();
            var experiencesArray = new JSONArray();

            foreach (string filePath in experienceFiles)
            {
                Debug.Log(filePath);
                string jsonText = File.ReadAllText(filePath);
                var json = JSON.Parse(jsonText);

                if (json == null)
                {
                    Debug.LogWarning($"Failed to parse JSON in file: {filePath}");
                    continue;
                }

                string name = json["Name"];
                string category = json["Category"];
                string subcategory = json["SubCategory"];
                string description = json["Description"];
                //string mapCategory = json["MapCategory"];
                //string gameMode = json["GameMode"];

                var experienceObj = new JSONObject();
                experienceObj["Name"] = name;
                experienceObj["Category"] = category;
                experienceObj["SubCategory"] = subcategory;
                experienceObj["Description"] = description;

                experiencesArray.Add(experienceObj);
            }

            var outputJson = BuiltinLevels.GetInfo();
            outputJson["Experiences"] = experiencesArray;

            string path = InfoPath + "/BuiltinMapInfo.json";//Path.Combine(DataPath, ResourcePaths.Info, "BuiltinMapInfo");
            File.WriteAllText(path, outputJson.ToString(aIndent: 4));

            AssetDatabase.Refresh();
            Debug.Log($"Experiences summary written to: {path}");
        }
    }
}
