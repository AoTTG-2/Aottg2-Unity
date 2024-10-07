using System.Collections.Generic;
using UnityEngine;
using ApplicationManagers;

namespace CustomSkins
{
    class MaterialCache
    {
        private static Dictionary<string, Material> _IdToMaterial = new Dictionary<string, Material>();
        private static int MaxItems = 200;
        public static Material TransparentMaterial;

        public static void Init()
        {
            TransparentMaterial = ResourceManager.InstantiateAsset<Material>("Map/Materials", "TransparentMaterial");
            TransparentMaterial.color = new Color(0f, 0f, 0f, 0f);
        }

        public static void Clear()
        {
            _IdToMaterial.Clear();
        }

        public static bool ContainsKey(string rendererId, string url)
        {
            return _IdToMaterial.ContainsKey(GetId(rendererId, url));
        }

        public static Material GetMaterial(string rendererId, string url)
        {
            return _IdToMaterial[GetId(rendererId, url)];
        }

        public static void SetMaterial(string rendererId, string url, Material material)
        {
            if (_IdToMaterial.Count > MaxItems)
                _IdToMaterial.Clear();
            string id = GetId(rendererId, url);
            if (_IdToMaterial.ContainsKey(id))
                _IdToMaterial[id] = material;
            else
                _IdToMaterial.Add(id, material);
        }

        private static string GetId(string rendererId, string url)
        {
            return rendererId + "," + url;
        }
    }
}
