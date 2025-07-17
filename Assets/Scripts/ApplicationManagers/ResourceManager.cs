using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ApplicationManagers
{
    class ResourceManager : MonoBehaviour
    {
        static Dictionary<string, Object> _cache = new Dictionary<string, Object>();
        private static Dictionary<string, Texture2D> _externalTextureCache = new Dictionary<string, Texture2D>();
        private static HashSet<string> _persistentTextures = new HashSet<string>();

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
            TextAsset asset = (TextAsset)LoadAsset(path, name);
            if (asset != null)
                return asset.text;
            return "";
        }

        public static Object LoadAsset(string path, string name, bool cached = true)
        {
            if (path != "")
                name = path + "/" + name;
            Texture2D externalTexture = GetExternalTexture(name);
            if (externalTexture != null)
            {
                if (cached && !_cache.ContainsKey(name))
                    _cache[name] = externalTexture;
                return externalTexture;
            }
            if (cached)
            {
                if (!_cache.ContainsKey(name))
                    _cache[name] = Resources.Load(name);
                return _cache[name];
            }
            return Resources.Load(name);
        }

        public static T LoadAsset<T>(string path, string name, bool cached = true) where T : Object
        {
            return (T)LoadAsset(path, name, cached);
        }

        public static T InstantiateAsset<T>(string path, string name, bool cached = true) where T : Object
        {
            return (T)Instantiate(LoadAsset(path, name, cached));
        }

        public static T InstantiateAsset<T>(string path, string name, Vector3 position, Quaternion rotation, bool cached = true) where T : Object
        {
            return (T)Instantiate(LoadAsset(path, name, cached), position, rotation);
        }

        public static Texture2D GetExternalTexture(string key)
        {
            return _externalTextureCache.ContainsKey(key) ? _externalTextureCache[key] : null;
        }

        public static void SetExternalTexture(string key, Texture2D texture, bool persistent = true)
        {
            _externalTextureCache[key] = texture;
            if (persistent && texture != null)
            {
                Object.DontDestroyOnLoad(texture);
                _persistentTextures.Add(key);
            }
        }
        
        public static void RemoveExternalTexture(string key)
        {
            if (_externalTextureCache.ContainsKey(key))
            {
                _externalTextureCache.Remove(key);
                _persistentTextures.Remove(key);
            }
        }
        
        public static Texture2D LoadExternalTexture(string filePath, string cacheKey = null, bool persistent = true)
        {
            if (string.IsNullOrEmpty(cacheKey))
                cacheKey = filePath;
            if (_externalTextureCache.ContainsKey(cacheKey))
            {
                return _externalTextureCache[cacheKey];
            }
            try
            {
                if (File.Exists(filePath))
                {
                    byte[] fileData = File.ReadAllBytes(filePath);
                    Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                    if (texture.LoadImage(fileData))
                    {
                        SetExternalTexture(cacheKey, texture, persistent);
                        return texture;
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Failed to load external texture: {filePath}, {e.Message}");
            }
            return null;
        }
        
        public static void ClearExternalTextureCache()
        {
            foreach (var texture in _externalTextureCache.Values)
            {
                if (texture != null)
                    Object.DestroyImmediate(texture);
            }
            _externalTextureCache.Clear();
            _persistentTextures.Clear();
        }
        
        public static void ClearNonPersistentTextures()
        {
            var keysToRemove = new List<string>();
            foreach (var kvp in _externalTextureCache)
            {
                if (!_persistentTextures.Contains(kvp.Key))
                {
                    if (kvp.Value != null)
                        Object.DestroyImmediate(kvp.Value);
                    keysToRemove.Add(kvp.Key);
                }
            }
            foreach (var key in keysToRemove)
            {
                _externalTextureCache.Remove(key);
            }
        }
        
        public static int GetExternalTextureCacheCount()
        {
            return _externalTextureCache.Count;
        }
        
        public static int GetPersistentTextureCacheCount()
        {
            return _persistentTextures.Count;
        }
    }
}