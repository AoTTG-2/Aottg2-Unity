using ApplicationManagers;

using Events;
using GameManagers;
using Photon.Pun;
using Photon.Realtime;
using SimpleJSONFixed;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UI;
using UnityEngine;
using Utility;

namespace Map
{
    class MapTransfer: MonoBehaviour
    {
        public static bool MapTransferReady;
        public static string MapHash;
        public static string MapName;
        private static MapTransfer _instance;
        private static readonly byte MsgMapStart = 0;
        private static readonly byte MsgMapBody = 1;
        private static readonly byte MsgLogicBody = 2;
        private static readonly byte MsgMapEnd = 3;
        private static int CompressDeltaRows = 22;
        private static JSONNode _mapScriptSymbolTable;
        public static List<byte> _mapScriptCompressed;
        private static List<byte[][]> _mapTransferData;
        public static List<byte> _logicScriptCompressed;

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
            EventManager.OnPreLoadScene += OnPreLoadScene;
        }

        private static void OnPreLoadScene(SceneName sceneName)
        {
            _instance.StopAllCoroutines();
            MapTransferReady = false;
        }

        public static void Start()
        {
            CreateTransferData();
            foreach (Player player in PhotonNetwork.PlayerListOthers)
            {
                string mapHash = player.GetStringProperty(PlayerProperty.CustomMapHash);
                if (mapHash != MapHash)
                    Transfer(player);
                else
                    RPCManager.PhotonView.RPC("LoadCachedMapRPC", player, new object[0]);
            }
            MapTransferReady = true;
        }

        public static void Transfer(Player player)
        {
            _instance.StartCoroutine(_instance.TransferMapData(player));
        }

        private static void CreateTransferData()
        {
            string serialize = MapManager.MapScript.Objects.Serialize();
            string logic = MapManager.MapScript.Logic;
            MapHash = Util.CreateMD5(serialize);
            if (logic.Trim() != string.Empty)
                MapHash += Util.CreateMD5(logic);
            object[] compress = CSVCompression.Compress(serialize, CompressDeltaRows);
            _mapScriptCompressed = new List<byte>((byte[])compress[0]);
            _mapScriptSymbolTable = (JSONNode)compress[1];
            _mapTransferData = new List<byte[][]>();
            _mapTransferData.Add(new byte[][] { new byte[] { MsgMapStart }, 
                StringCompression.Compress(MapManager.MapScript.Options.Serialize()),
                StringCompression.Compress(MapManager.MapScript.CustomAssets.Serialize()),
                StringCompression.Compress(MapManager.MapScript.Weather.SerializeToJsonString()),
                StringCompression.Compress(_mapScriptSymbolTable.ToString())
            });
            int chunkSize = 10000;
            int totalSize = _mapScriptCompressed.Count;
            List<byte[]> chunks = new List<byte[]>();
            for (int i = 0; i < totalSize; i += chunkSize)
            {
                if (i + chunkSize > totalSize)
                    chunkSize = totalSize - i;
                chunks.Add(_mapScriptCompressed.GetRange(i, chunkSize).ToArray());
            }
            for (int i = 0; i < chunks.Count; i++)
            {
                _mapTransferData.Add(new byte[][] { new byte[] { MsgMapBody }, chunks[i] });
            }
            if (logic.Trim() != string.Empty)
            {
                byte[] logicCompress = StringCompression.Compress(logic);
                _logicScriptCompressed = new List<byte>(logicCompress);
                totalSize = _logicScriptCompressed.Count;
                chunks = new List<byte[]>();
                for (int i = 0; i < totalSize; i += chunkSize)
                {
                    if (i + chunkSize > totalSize)
                        chunkSize = totalSize - i;
                    chunks.Add(_logicScriptCompressed.GetRange(i, chunkSize).ToArray());
                }
                for (int i = 0; i < chunks.Count; i++)
                {
                    _mapTransferData.Add(new byte[][] { new byte[] { MsgLogicBody }, chunks[i] });
                }
            }
            _mapTransferData.Add(new byte[][] { new byte[] { MsgMapEnd }, Encoding.UTF8.GetBytes(MapHash) });
        }

        public static void OnTransferMapRPC(byte[][] byteArr, int msgNumber, int msgTotal, PhotonMessageInfo info)
        {
            if (!info.Sender.IsMasterClient)
                return;
            UIManager.LoadingMenu.UpdateLoading(0.5f * ((float)msgNumber / (float)msgTotal));
            byte msgType = byteArr[0][0];
            if (msgType == MsgMapStart)
            {
                MapManager.MapScript = new MapScript();
                MapManager.MapScript.Options.Deserialize(StringCompression.Decompress(byteArr[1]));
                MapManager.MapScript.CustomAssets.Deserialize(StringCompression.Decompress(byteArr[2]));
                MapManager.MapScript.Weather.DeserializeFromJsonString(StringCompression.Decompress(byteArr[3]));
                _mapScriptSymbolTable = JSON.Parse(StringCompression.Decompress(byteArr[4]));
                _mapScriptCompressed = new List<byte>();
                _logicScriptCompressed = new List<byte>();
            }
            else if (msgType == MsgMapBody)
            {
                _mapScriptCompressed.AddRange(byteArr[1]);
            }
            else if (msgType == MsgLogicBody)
            {
                _logicScriptCompressed.AddRange(byteArr[1]);
            }
            else if (msgType == MsgMapEnd)
            {
                if (_mapScriptCompressed.Count > 0)
                {
                    string decompress = CSVCompression.Decompress(_mapScriptCompressed.ToArray(), _mapScriptSymbolTable, CompressDeltaRows);
                    MapManager.MapScript.Objects.Deserialize(decompress);
                }
                if (_logicScriptCompressed.Count > 0)
                {
                    string decompress = StringCompression.Decompress(_logicScriptCompressed.ToArray());
                    MapManager.MapScript.Logic = decompress;
                }
                MapHash = Encoding.UTF8.GetString(byteArr[1]);
                MapManager.LoadMap(true);
            }
        }

        private IEnumerator TransferMapData(Player player)
        {
            for (int i = 0; i < _mapTransferData.Count; i++)
            {
                RPCManager.PhotonView.RPC("TransferMapRPC", player, new object[] { _mapTransferData[i], i, _mapTransferData.Count });
                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}
