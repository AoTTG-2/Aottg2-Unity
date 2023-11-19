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

namespace Map
{
    class MapManager: Photon.Pun.MonoBehaviourPunCallbacks
    {
        public static bool MapLoaded;
        public static MapScript MapScript;
        private static MapManager _instance;

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

        public static Vector3 GetRandomTagPosition(string tag, Vector3 defaultPosition)
        {
            GameObject go = GetRandomTag(tag);
            if (go != null)
                return go.transform.position;
            return defaultPosition;
        }

        public static List<Vector3> GetRandomTagPositions(string tag, Vector3 avoidPosition, float avoidRadius, Vector3 defaultPosition, int count)
        {
            List<Vector3> allPositions = new List<Vector3>();
            List<Vector3> finalPositions = new List<Vector3>();
            if (MapLoader.Tags.ContainsKey(tag))
            {
                foreach (var obj in MapLoader.Tags[tag])
                    allPositions.Add(obj.GameObject.transform.position);
            }
            else
                allPositions.Add(defaultPosition);
            List<Vector3> currentPositions = new List<Vector3>(allPositions);
            int avoids = 0;
            for (int i = 0; i < count; i++)
            {
                if (currentPositions.Count <= 0)
                    currentPositions = new List<Vector3>(allPositions);
                int index = Random.Range(0, currentPositions.Count);
                var position = currentPositions[index];
                if (avoidRadius <= 0f || Vector3.Distance(position, avoidPosition) > avoidRadius || avoids > 100)
                    finalPositions.Add(currentPositions[index]);
                else
                {
                    i--;
                    avoids++;
                }
                currentPositions.RemoveAt(index);
            }
            return finalPositions;
        }

        public static Vector3 GetRandomTagsPosition(List<string> tags, Vector3 defaultPosition)
        {
            foreach (string tag in tags)
            {
                GameObject go = GetRandomTag(tag);
                if (go != null)
                    return go.transform.position;
            }
            return defaultPosition;
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
                MapLoader.StartLoadObjects(new List<MapScriptBaseObject>(), false);
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
            MapLoader.StartLoadObjects(MapScript.Objects.Objects, true);
        }

        public static void OnLoadBuiltinMapRPC(string category, string name, PhotonMessageInfo info)
        {
            if (!info.Sender.IsMasterClient)
                return;
            string source = BuiltinLevels.LoadMap(category, name);
            MapScript = new MapScript();
            MapScript.Deserialize(source);
            MapTransfer.MapHash = string.Empty;
            LoadMap();
        }

        public static void OnLoadCachedMapRPC(PhotonMessageInfo info)
        {
            if (!info.Sender.IsMasterClient)
                return;
            LoadMap();
        }

        public static void LoadMap()
        {
            PhotonNetwork.LocalPlayer.SetCustomProperty("CustomMapHash", MapTransfer.MapHash);
            MapLoader.StartLoadObjects(MapScript.Objects.Objects);
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
