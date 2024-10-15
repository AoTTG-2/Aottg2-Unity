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
        static System.Type[] AllowedComponents = new System.Type[] {
            typeof(UnityEngine.Transform),
            typeof(UnityEngine.Collider),
            typeof(UnityEngine.MeshFilter),
            typeof(UnityEngine.Animation),
            typeof(UnityEngine.Animator),
            typeof(UnityEngine.AudioSource),
            typeof(UnityEngine.AudioClip),
            typeof(UnityEngine.AudioChorusFilter),
            typeof(UnityEngine.AudioDistortionFilter),
            typeof(UnityEngine.AudioEchoFilter),
            typeof(UnityEngine.AudioHighPassFilter),
            typeof(UnityEngine.AudioListener),
            typeof(UnityEngine.AudioLowPassFilter),
            typeof(UnityEngine.AudioReverbFilter),
            typeof(UnityEngine.AudioReverbZone),
            typeof(UnityEngine.ParticleSystem),
            typeof(UnityEngine.LensFlare),
            typeof(UnityEngine.LineRenderer),
            typeof(UnityEngine.Projector),
            typeof(UnityEngine.TrailRenderer),
            typeof(UnityEngine.Renderer),
            typeof(UnityEngine.Terrain),
            typeof(UnityEngine.ArticulationBody),
            typeof(UnityEngine.CharacterController),
            typeof(UnityEngine.Cloth),
            typeof(UnityEngine.ConstantForce),
            typeof(UnityEngine.Joint),
            typeof(UnityEngine.Rigidbody),
            typeof(UnityEngine.Light),
            typeof(UnityEngine.LODGroup),
            typeof(UnityEngine.Video.VideoPlayer)
        };

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
            bool isValid = Util.IsValidFileName(bundle);
            if (!isValid)
                yield break;

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
                    string showUrl = url;
                    if (showUrl.Length > 50)
                        showUrl = showUrl.Substring(0, 50);
                    menu._customAssetUrlPopup.Show(showUrl);
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
                                if (_bundles[bundle] == null)
                                {
                                    File.Delete(path);
                                }
                            }
                        }
                    }
                }
            }
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

        public static Object LoadAsset(string bundle, string name)
        {
            if (!_bundles.ContainsKey(bundle) || _bundles[bundle] == null)
                throw new System.Exception("Custom bundle not loaded: " + bundle);
            var assetBundle = _bundles[bundle];
            var prefab = assetBundle.LoadAsset(name);
            ValidateCustomPrefab((GameObject)prefab);
            return prefab;
        }

        public static void ValidateCustomPrefab(GameObject prefab)
        {
            foreach (var component in prefab.GetComponentsInChildren(typeof(Component)))
            {
                bool inAllowlist = false;
                foreach (var type in AllowedComponents)
                {
                    if (type.IsAssignableFrom(component.GetType()))
                    {
                        inAllowlist = true;
                        break;
                    }
                }
                if (!inAllowlist)
                {
                    throw new System.Exception("Disallowed component (" + component.GetType().Name + ")");
                }
                if (component is Animation)
                {
                    var animation = (Animation)component;
                    foreach (AnimationState state in animation)
                        state.clip.events = new AnimationEvent[0];
                }
                else if (component is Animator)
                {
                    var animator = (Animator)component;
                    foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
                        clip.events = new AnimationEvent[0];
                }
            }
        }
    }
}