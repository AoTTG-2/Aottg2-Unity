using System.Collections.Generic;
using Utility;
using UnityEngine;

namespace Map
{
    class MapScript
    {
        protected string HeaderPrefix = "/// ";
        protected char Delimiter = '\n';
        public MapScriptOptions Options  = new MapScriptOptions();
        public MapScriptObjects Objects = new MapScriptObjects();
        public string Logic = string.Empty;

        public static MapScript CreateDefault()
        {
            var mapScript = new MapScript();
            var sceneObject = new MapScriptSceneObject();
            sceneObject.Asset = "Geometry/Cuboid";
            sceneObject.SetScale(new Vector3(100f, 5f, 100f));
            sceneObject.SetPosition(new Vector3(0f, -25f, 0f));
            sceneObject.Id = 0;
            var material = new MapScriptBasicMaterial();
            material.Shader = "Basic";
            material.Texture = BuiltinMapTextures.AllTextures["Grass6"].Texture;
            material.Tiling = new Vector2(25f, 25f);
            material.Color = new Color255(new Color(1f, 1f, 1f, 1f));
            sceneObject.Material = material;
            mapScript.Objects.Objects.Add(sceneObject);
            sceneObject = new MapScriptSceneObject();
            sceneObject.Copy(BuiltinMapPrefabs.AllPrefabs["Daylight"]);
            sceneObject.SetRotation(new Quaternion(-0.2f, -0.8f, 0.4f, -0.4f));
            sceneObject.Id = 1;
            mapScript.Objects.Objects.Add(sceneObject);
            return mapScript;
        }

        public virtual string Serialize()
        {
            List<string> items = new List<string>();
            items.Add(CreateHeader("Options"));
            items.Add(Options.Serialize());
            items.Add(CreateHeader("Objects"));
            items.Add(Objects.Serialize());
            items.Add(CreateHeader("Logic"));
            items.Add(Logic);
            return string.Join(Delimiter.ToString(), items.ToArray());
        }

        private string CreateHeader(string name)
        {
            return HeaderPrefix + name;
        }

        public virtual void Deserialize(string csv)
        {
            if (MapConverter.IsLegacy(csv))
            {
                Objects = MapConverter.Convert(csv).Objects;
                return;
            }
            string[] items = csv.Split(Delimiter);
            List<string> currentSectionItems = new List<string>();
            string currentSection = "";
            for (int i = 0; i < items.Length; i++)
            {
                string trimmed = items[i].Trim();
                if (trimmed.StartsWith(HeaderPrefix))
                {
                    DeserializeSection(currentSection, currentSectionItems);
                    currentSection = trimmed.Substring(HeaderPrefix.Length);
                    currentSectionItems.Clear();
                }
                else
                    currentSectionItems.Add(trimmed);
            }
            DeserializeSection(currentSection, currentSectionItems);
        }

        private void DeserializeSection(string currentSection, List<string> currentSectionItems)
        {
            string currentSectionCSV = string.Join(Delimiter.ToString(), currentSectionItems.ToArray());
            if (currentSection == "Options")
                Options.Deserialize(currentSectionCSV);
            else if (currentSection == "Objects")
                Objects.Deserialize(currentSectionCSV);
            else if (currentSection == "Logic")
                Logic = currentSectionCSV;
        }
    }
}
