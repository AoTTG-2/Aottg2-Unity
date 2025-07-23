using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomSkins;
using Settings;
using UI;
using ApplicationManagers;

namespace Utility
{
    public static class CharacterPreviewGenerator
    {
        private static readonly HashSet<string> _currentlyGenerating = new HashSet<string>();
        private static Dictionary<string, PreviewCameraData> _persistentCameras = new Dictionary<string, PreviewCameraData>();
        private static Dictionary<string, Coroutine> _activeDebounceCoroutines = new Dictionary<string, Coroutine>();
        private static HashSet<string> _generatedPreviewKeys = new HashSet<string>();

        private class PreviewCameraData
        {
            public Camera Camera;
            public RenderTexture RenderTexture;
            public bool IsInitialized;
            public bool IsCleanedUp;
        }

        public static void CleanupOrphanedPreviews()
        {
            CleanupOrphanedPreviewsInFolder(true);
            CleanupOrphanedPreviewsInFolder(false);
        }

        private static void CleanupOrphanedPreviewsInFolder(bool isHuman)
        {
            string subFolder = isHuman ? "Human" : "Titans";
            string folder = Path.Combine(FolderPaths.CharacterPreviews, subFolder);
            if (!Directory.Exists(folder))
                return;
            HashSet<string> validSetIds = new HashSet<string>();
            if (isHuman)
            {
                var humanSets = SettingsManager.HumanCustomSettings.CustomSets.GetSets().GetItems();
                foreach (var baseSetting in humanSets)
                {
                    var set = (Settings.HumanCustomSet)baseSetting;
                    validSetIds.Add("Preset" + set.UniqueId.Value);
                }
            }
            else
            {
                var titanSets = SettingsManager.TitanCustomSettings.TitanCustomSets.GetSets().GetItems();
                foreach (var baseSetting in titanSets)
                {
                    var set = (Settings.TitanCustomSet)baseSetting;
                    validSetIds.Add("Preset" + set.UniqueId.Value);
                }
            }
            string[] previewFiles = Directory.GetFiles(folder, "Preset*.png");
            foreach (string filePath in previewFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                if (!validSetIds.Contains(fileName))
                {
                    File.Delete(filePath);
                }
            }
        }

        public static void SetLayerRecursively(GameObject obj, int newLayer)
        {
            obj.layer = newLayer;
            foreach (Transform child in obj.transform)
                SetLayerRecursively(child.gameObject, newLayer);
        }

        private static PreviewCameraData GetOrCreatePersistentCamera(string cameraId, Transform parent = null)
        {
            if (_persistentCameras.ContainsKey(cameraId) && _persistentCameras[cameraId].IsInitialized && !_persistentCameras[cameraId].IsCleanedUp)
            {
                var existingData = _persistentCameras[cameraId];
                if (existingData.Camera != null && existingData.RenderTexture != null && existingData.Camera.gameObject != null)
                {
                    return existingData;
                }
                else
                {
                    _persistentCameras.Remove(cameraId);
                }
            }
            var cameraData = new PreviewCameraData();
            GameObject cameraObject = new GameObject($"PersistentPreviewCamera_{cameraId}");
            Object.DontDestroyOnLoad(cameraObject);
            if (parent != null)
            {
                if (parent.gameObject.scene.name == "DontDestroyOnLoad" || parent.gameObject.scene.name == null)
                {
                    cameraObject.transform.SetParent(parent);
                }
            }
            cameraData.Camera = cameraObject.AddComponent<Camera>();
            Camera mainCamera = Camera.main ?? Object.FindObjectOfType<Camera>();
            if (mainCamera != null)
            {
                cameraData.Camera.clearFlags = mainCamera.clearFlags;
                cameraData.Camera.backgroundColor = mainCamera.backgroundColor;
                cameraData.Camera.nearClipPlane = mainCamera.nearClipPlane;
                cameraData.Camera.farClipPlane = mainCamera.farClipPlane;
                var mainSkybox = mainCamera.GetComponent<Skybox>();
                if (mainSkybox != null)
                {
                    var skybox = cameraObject.AddComponent<Skybox>();
                    skybox.material = mainSkybox.material;
                }
            }
            cameraData.Camera.fieldOfView = 60f;
            cameraData.Camera.orthographic = false;
            cameraData.Camera.cullingMask = ~0;
            cameraData.Camera.enabled = false;
            int renderWidth = 128;
            int renderHeight = 128;
            cameraData.RenderTexture = new RenderTexture(renderWidth, renderHeight, 16);
            if (cameraData.RenderTexture == null)
            {
                return null;
            }
            cameraData.RenderTexture.antiAliasing = 1;
            cameraData.RenderTexture.Create();
            cameraData.Camera.targetTexture = cameraData.RenderTexture;
            cameraData.IsInitialized = true;
            cameraData.IsCleanedUp = false;
            _persistentCameras[cameraId] = cameraData;
            return cameraData;
        }

        private static void PositionCameraForCharacter(Camera camera, GameObject character, bool isHuman = true)
        {
            Vector3 anchorPoint = new Vector3(0f, 1.18f, 0f);
            float defaultDistance = isHuman ? 3f : 3.5f;
            Vector3 characterPosition = character.transform.position;
            Vector3 cameraPosition = characterPosition + new Vector3(0f, 1.23f, defaultDistance);
            Vector3 targetAnchorPoint = characterPosition + anchorPoint;
            camera.transform.position = cameraPosition;
            camera.transform.LookAt(targetAnchorPoint);
            camera.fieldOfView = 20f;
        }

        private static Texture2D CapturePreviewWithCamera(PreviewCameraData cameraData, GameObject character, int size = 128, bool isHuman = true)
        {
            if (cameraData?.Camera == null || cameraData.RenderTexture == null)
            {
                return null;
            }
            Vector3 originalPosition = character.transform.position;
            Quaternion originalRotation = character.transform.rotation;
            Vector3 originalScale = character.transform.localScale;
            try
            {
                SetLayerRecursively(character, LayerMask.NameToLayer("Default"));
                var renderers = character.GetComponentsInChildren<Renderer>();
                foreach (var renderer in renderers)
                {
                    renderer.enabled = true;
                    if (renderer is SkinnedMeshRenderer skinnedRenderer)
                    {
                        skinnedRenderer.updateWhenOffscreen = true;
                    }
                }
                PositionCameraForCharacter(cameraData.Camera, character, isHuman);
                RenderTexture originalActive = RenderTexture.active;
                cameraData.Camera.Render();
                RenderTexture.active = cameraData.RenderTexture;
                int renderWidth = cameraData.RenderTexture.width;
                int renderHeight = cameraData.RenderTexture.height;
                Texture2D texture = new Texture2D(renderWidth, renderHeight, TextureFormat.RGB24, false);
                texture.ReadPixels(new Rect(0, 0, renderWidth, renderHeight), 0, 0);
                texture.Apply();
                RenderTexture.active = originalActive;
                if (size != renderWidth)
                {
                    TextureScaler.ScaleBlocking(texture, size, size);
                }
                return texture;
            }
            finally
            {
                character.transform.position = originalPosition;
                character.transform.rotation = originalRotation;
                character.transform.localScale = originalScale;
            }
        }

        public static void CleanupPersistentCamera(string cameraId)
        {
            if (!_persistentCameras.ContainsKey(cameraId))
                return;
            var cameraData = _persistentCameras[cameraId];
            if (cameraData.Camera != null)
            {
                cameraData.Camera.targetTexture = null;
                cameraData.Camera.enabled = false;
                Object.DestroyImmediate(cameraData.Camera.gameObject);
            }
            if (cameraData.RenderTexture != null)
            {
                cameraData.RenderTexture.Release();
                Object.DestroyImmediate(cameraData.RenderTexture);
            }
            cameraData.IsCleanedUp = true;
            _persistentCameras.Remove(cameraId);
        }

        public static void CleanupAllPersistentCameras()
        {
            var cameraIds = new List<string>(_persistentCameras.Keys);
            foreach (var cameraId in cameraIds)
            {
                CleanupPersistentCamera(cameraId);
            }
        }

        public static void GeneratePreviewWithPersistentCamera(string cameraId, GameObject character, string fileName, int size = 128, bool isHuman = true, Transform cameraParent = null)
        {
            lock (_currentlyGenerating)
            {
                if (_currentlyGenerating.Contains(fileName))
                {
                    return;
                }
                _currentlyGenerating.Add(fileName);
            }
            try
            {
                var cameraData = GetOrCreatePersistentCamera(cameraId, cameraParent);
                Texture2D texture = CapturePreviewWithCamera(cameraData, character, size, isHuman);
                if (texture == null)
                {
                    return;
                }
                string setId = fileName.Replace("Preset", "");
                string cacheKey = "CharacterPreview_" + (isHuman ? "Human" : "Titans") + "_" + setId;
                ResourceManager.SetExternalTexture(cacheKey, texture, persistent: true);
                _generatedPreviewKeys.Add(cacheKey);
            }
            finally
            {
                lock (_currentlyGenerating)
                {
                    _currentlyGenerating.Remove(fileName);
                }
            }
        }

        public static void SaveCachedPreviewsToDisk()
        {
            string humanFolder = System.IO.Path.Combine(FolderPaths.CharacterPreviews, "Human");
            string titanFolder = System.IO.Path.Combine(FolderPaths.CharacterPreviews, "Titans");
            foreach (string cacheKey in _generatedPreviewKeys)
            {
                Texture2D texture = ResourceManager.GetExternalTexture(cacheKey);
                if (texture != null)
                {
                    string[] parts = cacheKey.Split('_');
                    if (parts.Length >= 3)
                    {
                        bool isHuman = parts[1] == "Human";
                        string setId = string.Join("_", parts.Skip(2));
                        string folder = isHuman ? humanFolder : titanFolder;
                        if (!System.IO.Directory.Exists(folder))
                        {
                            System.IO.Directory.CreateDirectory(folder);
                        }
                        string path = System.IO.Path.Combine(folder, "Preset" + setId + ".png");
                        byte[] pngData = texture.EncodeToPNG();
                        System.IO.File.WriteAllBytes(path, pngData);
                    }
                }
            }
            CleanupOrphanedPreviews();
        }

        public static void ClearSessionGeneratedPreviews()
        {
            foreach (string cacheKey in _generatedPreviewKeys)
            {
                Texture2D texture = ResourceManager.GetExternalTexture(cacheKey);
                if (texture != null)
                {
                    Object.DestroyImmediate(texture);
                }
                ResourceManager.RemoveExternalTexture(cacheKey);
            }
            _generatedPreviewKeys.Clear();
        }

        public static void ClearNonPersistentPreviews()
        {
            ResourceManager.ClearNonPersistentTextures();
        }

        public static void GetCacheInfo(out int totalCached, out int persistent)
        {
            totalCached = ResourceManager.GetExternalTextureCacheCount();
            persistent = ResourceManager.GetPersistentTextureCacheCount();
        }

        public static void CaptureCurrentCharacterPreview(bool isHuman = true)
        {
            var gameManager = (GameManagers.CharacterEditorGameManager)ApplicationManagers.SceneLoader.CurrentGameManager;
            if (gameManager == null) return;
            if (isHuman && gameManager.Human != null)
            {
                var currentSet = (Settings.HumanCustomSet)Settings.SettingsManager.HumanCustomSettings.CustomSets.GetSelectedSet();
                GeneratePreviewWithPersistentCamera("HumanPreview", gameManager.Human.gameObject, "Preset" + currentSet.UniqueId.Value, 128, true);
            }
            else if (!isHuman && gameManager.Titan != null)
            {
                var currentSet = (Settings.TitanCustomSet)Settings.SettingsManager.TitanCustomSettings.TitanCustomSets.GetSelectedSet();
                GeneratePreviewWithPersistentCamera("TitanPreview", gameManager.Titan.gameObject, "Preset" + currentSet.UniqueId.Value, 128, false);
            }
        }

        public static void InitializePreviewSystem()
        {
            CleanupOrphanedPreviews();
        }
        
        internal static void GeneratePreviewForHumanSet(UI.CharacterEditorHumanMenu humanMenu, bool isRebuild = false)
        {
            var currentSet = (Settings.HumanCustomSet)Settings.SettingsManager.HumanCustomSettings.CustomSets.GetSelectedSet();
            var gameManager = (GameManagers.CharacterEditorGameManager)ApplicationManagers.SceneLoader.CurrentGameManager;
            var dummyHuman = gameManager.Human;
            var currentWeapon = humanMenu != null ? (Characters.HumanWeapon)humanMenu.Weapon.Value : Characters.HumanWeapon.Blade;
            if (isRebuild)
            {
                dummyHuman.Setup.Load(currentSet, currentWeapon, false);
                if (humanMenu != null)
                {
                    humanMenu.StartCoroutine(GeneratePreviewForHumanSetCoroutine(humanMenu, false));
                    return;
                }
            }
            if (humanMenu != null)
            {
                GeneratePreviewWithPersistentCamera("HumanPreview", dummyHuman.gameObject, "Preset" + currentSet.UniqueId.Value, 128, true, humanMenu.transform);
            }
        }
        
        internal static System.Collections.IEnumerator GeneratePreviewForHumanSetCoroutine(UI.CharacterEditorHumanMenu humanMenu, bool isRebuild = false)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            GeneratePreviewForHumanSet(humanMenu, isRebuild);
        }
        
        internal static System.Collections.IEnumerator GeneratePreviewForTitanSetCoroutine(UI.CharacterEditorTitanMenu titanMenu, bool isRebuild = false)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            GeneratePreviewForTitanSet(titanMenu, isRebuild);
        }
        
        public static void GeneratePreviewWithDebounce(MonoBehaviour coroutineRunner, string debounceKey, System.Action generateAction, float delaySeconds = 0.1f)
        {
            if (_activeDebounceCoroutines.ContainsKey(debounceKey) && _activeDebounceCoroutines[debounceKey] != null)
            {
                coroutineRunner.StopCoroutine(_activeDebounceCoroutines[debounceKey]);
            }
            _activeDebounceCoroutines[debounceKey] = coroutineRunner.StartCoroutine(DebouncedPreviewCoroutine(debounceKey, generateAction, delaySeconds));
        }
        
        private static System.Collections.IEnumerator DebouncedPreviewCoroutine(string debounceKey, System.Action generateAction, float delaySeconds)
        {
            yield return new WaitForSeconds(delaySeconds);
            generateAction?.Invoke();
            if (_activeDebounceCoroutines.ContainsKey(debounceKey))
            {
                _activeDebounceCoroutines[debounceKey] = null;
            }
        }

        internal static void GeneratePreviewForTitanSet(UI.CharacterEditorTitanMenu titanMenu, bool isRebuild = false)
        {
            var currentSet = (Settings.TitanCustomSet)Settings.SettingsManager.TitanCustomSettings.TitanCustomSets.GetSelectedSet();
            var gameManager = (GameManagers.CharacterEditorGameManager)ApplicationManagers.SceneLoader.CurrentGameManager;
            var dummyTitan = gameManager.Titan;
            if (isRebuild)
            {
                dummyTitan.Setup.Load(currentSet);
                if (titanMenu != null)
                {
                    titanMenu.StartCoroutine(GeneratePreviewForTitanSetCoroutine(titanMenu, false));
                    return;
                }
            }
            if (titanMenu != null)
            {
                GeneratePreviewWithPersistentCamera("TitanPreview", dummyTitan.gameObject, "Preset" + currentSet.UniqueId.Value, 128, false, titanMenu.transform);
            }
        }

        public static System.Collections.IEnumerator GeneratePreviewWithPersistentCameraCoroutine(string cameraId, GameObject character, string fileName, int size = 128, bool isHuman = true, Transform cameraParent = null)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            GeneratePreviewWithPersistentCamera(cameraId, character, fileName, size, isHuman, cameraParent);
        }
    }
}
