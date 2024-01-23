using System.Collections.Generic;
using UnityEngine;
using Weather;
using UI;
using Utility;
using GameManagers;
using Events;
using Cameras;
using Map;
using Projectiles;
using Characters;
using Settings;
using CustomSkins;
using Photon.Pun;
using UnityEngine.SceneManagement;

namespace ApplicationManagers
{
    /// <summary>
    /// Manager used by other classes to load and setup scenes with proper game managers, maps, and cameras.
    /// </summary>
    class SceneLoader : MonoBehaviour
    {
        static SceneLoader _instance;
        public static SceneName SceneName = SceneName.Startup;
        public static BaseGameManager CurrentGameManager;
        public static VoiceChatManager VoiceChatManager;
        public static BaseCamera CurrentCamera;

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
            SceneManager.sceneLoaded += _instance.OnSceneWasLoaded;
            LoadScene(SceneName.MainMenu);
        }

        public static void LoadScene(SceneName sceneName)
        {
            SkyboxCustomSkinLoader.SkyboxMaterial = null;
            Time.timeScale = 1f;
            EventManager.InvokePreLoadScene(sceneName);
            SceneName = sceneName;
            ClothFactory.DisposeAllObjects();
            ClothFactory.ClearClothCache();
            ResourceManager.ClearCache();
            AssetBundleManager.Clear();
            if (sceneName == SceneName.InGame)
                PhotonNetwork.LoadLevel(0);
            else
                SceneManager.LoadScene(0);
            FullscreenHandler.UpdateFPS();
            CharacterData.Init(); // remove this after testing is done
        }

        private static void CreateGameManager()
        {
            if (CurrentGameManager != null)
                Debug.Log("Warning: game manager already exists.");
            if (SceneName == SceneName.MainMenu)
                CurrentGameManager = Util.CreateObj<MainMenuGameManager>();
            else if (SceneName == SceneName.InGame)
            { 
                CurrentGameManager = Util.CreateObj<InGameManager>();
                VoiceChatManager = Util.CreateObj<VoiceChatManager>();
            }
            else if (SceneName == SceneName.CharacterEditor)
                CurrentGameManager = Util.CreateObj<CharacterEditorGameManager>();
            else if (SceneName == SceneName.MapEditor)
                CurrentGameManager = Util.CreateObj<MapEditorGameManager>();
            else if (SceneName == SceneName.SnapshotViewer || SceneName == SceneName.Gallery)
                CurrentGameManager = Util.CreateObj<BaseGameManager>();
        }

        private static void CreateCamera()
        {
            if (CurrentCamera != null)
                Debug.Log("Warning: Camera already exists.");
            if (SceneName == SceneName.Startup)
                return;
            var go = ResourceManager.InstantiateAsset<GameObject>("", "Game/MainCamera");
            if (SceneName == SceneName.InGame)
                CurrentCamera = go.AddComponent<InGameCamera>();
            else if (SceneName == SceneName.MapEditor)
                CurrentCamera = go.AddComponent<MapEditorCamera>();
            else if (SceneName == SceneName.CharacterEditor)
                CurrentCamera = go.AddComponent<CharacterEditorCamera>();
            else if (SceneName == SceneName.DebugTest)
                CurrentCamera = go.AddComponent<TestCamera>();
            else
            {
                CurrentCamera = go.AddComponent<StaticCamera>();
                CurrentCamera.Camera.nearClipPlane = 0.3f;
                if (SceneName == SceneName.SnapshotViewer)
                    CurrentCamera.GetComponent<StaticCamera>().SetSkybox(true);
                else
                    CurrentCamera.GetComponent<StaticCamera>().SetSkybox(false);
            }
        }

        private void OnSceneWasLoaded(Scene scene, LoadSceneMode mode)
        {
            CreateGameManager();
            CreateCamera();
            EventManager.InvokeLoadScene(SceneName);
        }
    }

    public enum SceneName
    {
        Startup,
        MainMenu,
        InGame,
        MapEditor,
        CharacterEditor,
        SnapshotViewer,
        Gallery,
        DebugTest
    }
}
