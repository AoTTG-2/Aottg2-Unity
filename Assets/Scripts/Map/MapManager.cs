using ApplicationManagers;
using Events;
using GameManagers;
using Settings;
using UnityEngine;
using Utility;
using SimpleJSONFixed;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using CustomLogic;

namespace Map
{
    class MapManager: Photon.Pun.MonoBehaviourPunCallbacks
    {
        public static bool MapLoaded;
        public static MapScript MapScript;
        private static MapManager _instance;
        public static bool NeedsNavMeshUpdate = true;
        public static string LastMapHash = string.Empty;
        public static string LastGameMode = string.Empty;

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
            MapTransfer.Init();
            MapLoader.Init();
            BuiltinLevels.Init();
            BuiltinMapPrefabs.Init();
            BuiltinMapTextures.Init();
            EventManager.OnLoadScene += OnLoadScene;
            EventManager.OnPreLoadScene += OnPreLoadScene;
        }

        public static bool TryGetRandomTagXform(string tag, out Transform xform)
        {
            var go = GetRandomTag(tag);
            if (go)
            {
                xform = go.transform;
                return true;
            }

            xform = null;
            return false;
        }
        
        public static bool TryGetRandomTagsXform(List<string> tags, out Transform xform)
        {
            foreach (string tag in tags)
            {
                var go = GetRandomTag(tag);
                if (go)
                {
                    xform = go.transform;
                    return true;
                }
            }

            xform = null;
            return false;
        }

        /// <summary>
        /// <para>Outputs a list of <paramref name="count"/> randomly chosen objects with a given tag within avoidance parameters.</para>
        /// </summary>
        /// <returns>False if no transforms have the specified tag.</returns>
        public static bool TryGetRandomTagXforms(string tag, Vector3 avoidPosition, float avoidRadius, int count, out List<Transform> xforms)
        {
            xforms = null;
            if (!MapLoader.Tags.TryGetValue(tag, out List<MapObject> objs))
                return false;

            List<Transform> allXforms = new List<Transform>();
            xforms = new List<Transform>();
            foreach (var obj in objs)
                allXforms.Add(obj.GameObject.transform);

            List<Transform> currentXforms = new List<Transform>(allXforms);
            int avoids = 0;
            for (int i = 0; i < count; i++)
            {
                if (currentXforms.Count <= 0)
                    currentXforms = new List<Transform>(allXforms);
                int index = Random.Range(0, currentXforms.Count);
                var position = currentXforms[index].position;
                if (avoidRadius <= 0f || Vector3.Distance(position, avoidPosition) > avoidRadius || avoids > 100)
                    xforms.Add(currentXforms[index]);
                else
                {
                    i--;
                    avoids++;
                }
                currentXforms.RemoveAt(index);
            }
            return true;
        }
        
        public static GameObject GetRandomTag(string tag)
        {
            if (MapLoader.Tags.ContainsKey(tag))
            {
                if (MapLoader.Tags[tag].Count > 0)
                {
                    return MapLoader.Tags[tag].GetRandomItem().GameObject;
                }
            }
            return null;
        }

        private static void OnPreLoadScene(SceneName sceneName)
        {
            MapLoaded = false;
        }

        private static void OnLoadScene(SceneName sceneName)
        {
            if (sceneName == SceneName.InGame)
                StartInGame();
            else if (sceneName == SceneName.MapEditor)
                StartMapEditor();
            else
                MapLoader.StartLoadObjects(new List<string>(), new List<MapScriptBaseObject>(), null, null, false);
        }

        private static void StartInGame()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                InGameGeneralSettings settings = SettingsManager.InGameCurrent.General;
                if (settings.MapCategory.Value == "Custom")
                {
                    string map = BuiltinLevels.LoadMap("Custom", settings.MapName.Value);
                    MapScript = new MapScript();
                    MapScript.Deserialize(map);
                    MapTransfer.Start();
                    OnLoadCachedMapRPC(Util.CreateLocalPhotonInfo());
                }
                else
                {
                    MapTransfer.MapTransferReady = true;
                    RPCManager.PhotonView.RPC("LoadBuiltinMapRPC", RpcTarget.All, new object[] { settings.MapCategory.Value, settings.MapName.Value });
                }
            }
        }

        private static void StartMapEditor()
        {
            var current = SettingsManager.MapEditorSettings.CurrentMap;
            var maps = BuiltinLevels.GetMapNames("Custom").ToList();
            if (current.Value == string.Empty || !maps.Contains(current.Value))
            {
                if (maps.Count > 0)
                    current.Value = maps[0];
                else
                {
                    current.Value = "Untitled";
                    BuiltinLevels.SaveCustomMap(current.Value, MapScript.CreateDefault());
                }
            }
            MapScript = new MapScript();
            MapScript.Deserialize(BuiltinLevels.LoadMap("Custom", current.Value));
            MapLoader.StartLoadObjects(MapScript.CustomAssets.CustomAssets, MapScript.Objects.Objects, MapScript.Options, MapScript.Weather, true);
        }

        public static void OnLoadBuiltinMapRPC(string category, string name, PhotonMessageInfo info)
        {
            if (!info.Sender.IsMasterClient)
                return;
            string source = BuiltinLevels.LoadMap(category, name);
            MapScript = new MapScript();
            MapScript.Deserialize(source);
            MapTransfer.MapHash = string.Empty;
            bool mapChanged = LastMapHash != MapScript.MapHash || LastGameMode != SettingsManager.InGameCurrent.General.GameMode.Value;
            LastMapHash = MapScript.MapHash;
            LastGameMode = SettingsManager.InGameCurrent.General.GameMode.Value;
            LoadMap(mapChanged);
        }

        public static void OnLoadCachedMapRPC(PhotonMessageInfo info)
        {
            if (info.Sender != null && !info.Sender.IsMasterClient)
                return;
            bool mapChanged = LastMapHash != MapScript.MapHash || LastGameMode != SettingsManager.InGameCurrent.General.GameMode.Value;
            LastMapHash = MapScript.MapHash;
            LastGameMode = SettingsManager.InGameCurrent.General.GameMode.Value;
            LoadMap(mapChanged);
        }

        public static void LoadMap(bool mapChanged)
        {
            NeedsNavMeshUpdate = mapChanged;
            PhotonNetwork.LocalPlayer.SetCustomProperty("CustomMapHash", MapTransfer.MapHash);
            MapLoader.StartLoadObjects(MapScript.CustomAssets.CustomAssets, MapScript.Objects.Objects, MapScript.Options, MapScript.Weather);
        }

        public override void OnPlayerEnteredRoom(Player player)
        {
            if (PhotonNetwork.IsMasterClient && MapTransfer.MapTransferReady)
            {
                InGameGeneralSettings settings = SettingsManager.InGameCurrent.General;
                if (settings.MapCategory.Value == "Custom")
                    MapTransfer.Transfer(player);
                else
                    RPCManager.PhotonView.RPC("LoadBuiltinMapRPC", player, new object[] { settings.MapCategory.Value, settings.MapName.Value });
            }
        }
    }
}
