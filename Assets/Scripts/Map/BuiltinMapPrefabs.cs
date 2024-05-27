using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using SimpleJSONFixed;
using ApplicationManagers;
using Settings;
using Events;
using System.IO;
using System.Linq;
using Utility;

namespace Map
{
    class BuiltinMapPrefabs
    {
        private static JSONNode _prefabList;
        public static Dictionary<string, MapScriptBaseObject> AllPrefabs = new Dictionary<string, MapScriptBaseObject>();
        public static Dictionary<string, List<MapScriptBaseObject>> PrefabCategories = new Dictionary<string, List<MapScriptBaseObject>>();
        public static Dictionary<string, MapScriptBaseObject> AllPrefabsLower = new Dictionary<string, MapScriptBaseObject>();
        public static Dictionary<string, string> PrefabPreviews = new Dictionary<string, string>();
        public static Dictionary<string, List<string>> PrefabVariants = new Dictionary<string, List<string>>();
        public static Dictionary<string, string> VariantToBasePrefab = new Dictionary<string, string>();


        public static void Init()
        {
            _prefabList = JSON.Parse(((TextAsset)ResourceManager.LoadAsset(ResourcePaths.Info, "MapPrefabList")).text);
            foreach (string category in _prefabList.Keys)
            {
                JSONNode categoryNode = _prefabList[category];
                JSONNode info = categoryNode["Info"];
                PrefabCategories.Add(category, new List<MapScriptBaseObject>());
                foreach (JSONNode prefabNode in categoryNode["Prefabs"])
                {
                    MapScriptSceneObject sceneObject = new MapScriptSceneObject();
                    sceneObject.Type = info["Type"].Value;
                    sceneObject.Name = prefabNode["Name"].Value;
                    string asset = string.Empty;
                    if (info.HasKey("AssetSameAsName") && info["AssetSameAsName"].AsBool)
                        asset = prefabNode["Name"].Value;
                    if (prefabNode.HasKey("Asset"))
                        asset = prefabNode["Asset"].Value;
                    if (asset == string.Empty || asset == "None")
                        asset = "None";
                    else if (info.HasKey("AssetPrefix"))
                        asset = info["AssetPrefix"].Value + asset;
                    sceneObject.Asset = asset;
                    if (prefabNode.HasKey("Static"))
                        sceneObject.Static = prefabNode["Static"].AsBool;
                    if (prefabNode.HasKey("Networked"))
                        sceneObject.Networked = prefabNode["Networked"].AsBool;
                    if (prefabNode.HasKey("Visible"))
                        sceneObject.Visible = prefabNode["Visible"].AsBool;
                    if (prefabNode.HasKey("CollideMode"))
                        sceneObject.CollideMode = prefabNode["CollideMode"].Value;
                    if (prefabNode.HasKey("CollideWith"))
                        sceneObject.CollideWith = prefabNode["CollideWith"].Value;
                    if (prefabNode.HasKey("Material"))
                        sceneObject.Material = MapScriptSceneObject.DeserializeMaterial(prefabNode["Material"].Value);
                    if (prefabNode.HasKey("Components"))
                    {
                        foreach (JSONNode componentNode in prefabNode["Components"])
                        {
                            MapScriptComponent component = new MapScriptComponent();
                            component.Deserialize(componentNode.Value);
                            sceneObject.Components.Add(component);
                        }
                    }
                    if (prefabNode.HasKey("ScaleX"))
                        sceneObject.ScaleX = prefabNode["ScaleX"].AsFloat;
                    if (prefabNode.HasKey("ScaleY"))
                        sceneObject.ScaleX = prefabNode["ScaleY"].AsFloat;
                    if (prefabNode.HasKey("ScaleZ"))
                        sceneObject.ScaleX = prefabNode["ScaleZ"].AsFloat;
                    if (prefabNode.HasKey("Preview"))
                        PrefabPreviews.Add(sceneObject.Name, prefabNode["Preview"].Value);
                    if (prefabNode.HasKey("Variant"))
                    {
                        var variantBase = prefabNode["Variant"].Value;
                        if (variantBase != sceneObject.Name)
                        {
                            if (!PrefabVariants.ContainsKey(variantBase))
                                PrefabVariants.Add(variantBase, new List<string>());
                            PrefabVariants[variantBase].Add(sceneObject.Name);
                            VariantToBasePrefab[sceneObject.Name] = variantBase;
                        }
                    }
                    AllPrefabs.Add(sceneObject.Name, sceneObject);
                    AllPrefabsLower.Add(sceneObject.Name.ToLower(), sceneObject);
                    PrefabCategories[category].Add(sceneObject);
                }
            }
        }
    }
}
