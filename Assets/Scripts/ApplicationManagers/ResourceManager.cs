using System.Collections.Generic;
using UnityEngine;

namespace ApplicationManagers
{
    class ResourceManager : MonoBehaviour
    {
        static Dictionary<string, Object> _cache = new Dictionary<string, Object>();

        public static void ClearCache()
        {
            _cache.Clear();
        }

        public static string LoadText(string path, string name)
        {
            return ((TextAsset)LoadAsset(path, name)).text;
        }

        public static string TryLoadText(string path, string name)
        {
            try
            {
                return LoadText(path, name);
            }
            catch (System.Exception e)
            {
                Debug.Log(string.Format("Error loading text from asset bundle: {0}, {1}", name, e.Message));
            }
            return string.Empty;
        }

        public static Object LoadAsset(string path, string name, bool cached = false)
        {
            if (path != "")
                name = path + "/" + name;
            if (cached)
            {
                if (!_cache.ContainsKey(name))
                    _cache.Add(name, Resources.Load(name));
                return _cache[name];
            }
            return Resources.Load(name);
        }

        public static T InstantiateAsset<T>(string path, string name, bool cached = false) where T : Object
        {
            return (T)Instantiate(LoadAsset(path, name, cached));
        }

        public static T InstantiateAsset<T>(string path, string name, Vector3 position, Quaternion rotation, bool cached = false) where T : Object
        {
            return (T)Instantiate(LoadAsset(path, name, cached), position, rotation);
        }
    }
}