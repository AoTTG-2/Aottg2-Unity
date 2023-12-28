using UnityEngine;
using System.Collections;
using Utility;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using System.Threading;
using UI;

namespace ApplicationManagers
{
    public class AssetBundleManager : MonoBehaviour
    {
        static AssetBundleManager _instance;
        static Dictionary<string, Dictionary<string, Object>> _cache = new Dictionary<string, Dictionary<string, Object>>();
        static Dictionary<string, AssetBundle> _bundles = new Dictionary<string, AssetBundle>();

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
            ClearTemp();
            if (!Directory.Exists(FolderPaths.CustomAssetsLocal))
                Directory.CreateDirectory(FolderPaths.CustomAssetsLocal);
            if (!Directory.Exists(FolderPaths.CustomAssetsWeb))
                Directory.CreateDirectory(FolderPaths.CustomAssetsWeb);
        }

        private void OnApplicationQuit()
        {
            ClearTemp();
        }

        private static void ClearTemp()
        {
            if (Directory.Exists(FolderPaths.CustomAssetsWeb))
            {
                try
                {
                    Directory.Delete(FolderPaths.CustomAssetsWeb, true);
                }
                catch (System.Exception e)
                {
                    Debug.Log(string.Format("Error deleting custom asset web folder: {0}", e.Message));
                }
            }
        }

        public static void Clear()
        {
            _cache.Clear();
            foreach (var bundle in _bundles.Values)
            {
                if (bundle != null)
                    bundle.Unload(true);
            }
            _bundles.Clear();
        }

        public static IEnumerator LoadBundle(string bundle, string url, bool editor)
        {
            if (_bundles.ContainsKey(bundle) && _bundles[bundle] != null)
                yield break;
            _bundles[bundle] = null;
            if (editor || File.Exists(FolderPaths.CustomAssetsLocal + "/" + bundle))
            {
                _bundles[bundle] = AssetBundle.LoadFromFile(FolderPaths.CustomAssetsLocal + "/" + bundle);
                yield break;
            }
            else
            {
                string path = FolderPaths.CustomAssetsWeb + "/" + bundle;
                if (File.Exists(path))
                {
                    _bundles[bundle] = AssetBundle.LoadFromFile(path);
                    yield break;
                }
                else if (url == string.Empty)
                    yield break;
                else
                {
                    var menu = (InGameMenu)UIManager.CurrentMenu;
                    menu._customAssetUrlPopup.Show(url);
                    while (!menu._customAssetUrlPopup.Done)
                        yield return null;
                    if (menu._customAssetUrlPopup.Confirmed)
                    {
                        using (UnityWebRequest dlreq = new UnityWebRequest(url))
                        {
                            using (dlreq.downloadHandler = new DownloadHandlerFile(path))
                            {
                                UnityWebRequestAsyncOperation op = dlreq.SendWebRequest();
                                uint maxBytes = 1000 * 1000 * 1000;
                                while (!op.isDone)
                                {
                                    if (op.webRequest.downloadedBytes > maxBytes)
                                        yield break;
                                    else
                                        yield return null;
                                }
                                _bundles[bundle] = AssetBundle.LoadFromFile(path);
                            }
                        }
                    }
                }
            }
        }

        public static string TryLoadText(string bundle, string name)
        {
            try
            {
                return ((TextAsset)LoadAsset(bundle, name)).text;
            }
            catch (System.Exception e)
            {
                Debug.Log(string.Format("Error loading text from asset bundle: {0}, {1}", bundle, e.Message));
            }
            return string.Empty;
        }

        public static List<string> GetAssetListFromBundle(string bundle)
        {
            var list = new List<string>();
            if (!_bundles.ContainsKey(bundle) || _bundles[bundle] == null)
                return list;
            foreach (string str in _bundles[bundle].GetAllAssetNames())
            {
                string trimmed = str.Trim();
                if (trimmed.EndsWith(".prefab"))
                {
                    if (trimmed.Contains('/'))
                    {
                        string[] strArr = trimmed.Split('/');
                        trimmed = strArr[strArr.Length - 1];
                    }
                    list.Add("Custom/" + bundle + "/" + trimmed.Substring(0, trimmed.Length - 7));
                }
            }
            return list;
        }

        public static List<string> GetAssetList()
        {
            var list = new List<string>();
            foreach (string bundle in _bundles.Keys)
                list.AddRange(GetAssetListFromBundle(bundle));
            return list;
        }

        public static bool LoadedBundle(string bundle)
        {
            return _bundles.ContainsKey(bundle) && _bundles[bundle] != null;
        }

        public static Object LoadAsset(string bundle, string name, bool cached = false)
        {
            if (!_bundles.ContainsKey(bundle) || _bundles[bundle] == null)
                throw new System.Exception("Custom bundle not loaded: " + bundle);
            var assetBundle = _bundles[bundle];
            if (cached)
            {
                if (!_cache.ContainsKey(bundle))
                    _cache.Add(bundle, new Dictionary<string, Object>());
                var bundleCache = _cache[bundle];
                if (!bundleCache.ContainsKey(name))
                    bundleCache.Add(name, assetBundle.LoadAsset(name));
                return bundleCache[name];
            }
            return assetBundle.LoadAsset(name);
        }

        public static T InstantiateAsset<T>(string bundle, string name, bool cached = false) where T : Object
        {
            return (T)Instantiate(LoadAsset(bundle, name, cached));
        }

        public static T InstantiateAsset<T>(string bundle, string name, Vector3 position, Quaternion rotation, bool cached = false) where T : Object
        {
            return (T)Instantiate(LoadAsset(bundle, name, cached), position, rotation);
        }
    }
}