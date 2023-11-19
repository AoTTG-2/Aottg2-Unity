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
    class BuiltinMapTextures
    {
        private static JSONNode _textureList;
        public static Dictionary<string, MapScriptBasicMaterial> AllTextures = new Dictionary<string, MapScriptBasicMaterial>();
        public static Dictionary<string, List<MapScriptBasicMaterial>> TextureCategories = new Dictionary<string, List<MapScriptBasicMaterial>>();
        public static Dictionary<string, MapScriptBasicMaterial> AllTexturesLower = new Dictionary<string, MapScriptBasicMaterial>();
        public static Dictionary<string, MapScriptBasicMaterial> AllTexturesNoLegacy = new Dictionary<string, MapScriptBasicMaterial>();
        public static Dictionary<string, string> LegacyTexturePaths = new Dictionary<string, string>();

        public static void Init()
        {
            _textureList = JSON.Parse(((TextAsset)ResourceManager.LoadAsset(ResourcePaths.Info, "MapTextureList")).text);
            foreach (string category in _textureList.Keys)
            {
                JSONNode categoryNode = _textureList[category];
                TextureCategories.Add(category, new List<MapScriptBasicMaterial>());
                foreach (JSONNode textureNode in categoryNode)
                {
                    var material = new MapScriptBasicMaterial();
                    var name = textureNode["Name"].Value;
                    material.Texture = category + "/" + name;
                    float tilingX = 1f;
                    float tilingY = 1f;
                    if (textureNode.HasKey("TilingX"))
                        tilingX = textureNode["TilingX"].AsFloat;
                    if (textureNode.HasKey("TilingY"))
                        tilingY = textureNode["TilingY"].AsFloat;
                    material.Tiling = new Vector2(tilingX, tilingY);
                    AllTextures.Add(name, material);
                    AllTexturesLower.Add(name.ToLower(), material);
                    TextureCategories[category].Add(material);
                    if (category == "Legacy")
                        LegacyTexturePaths.Add(name, textureNode["Texture"].Value);
                    else
                        AllTexturesNoLegacy.Add(name, material);
                }
            }
        }
    }
}
