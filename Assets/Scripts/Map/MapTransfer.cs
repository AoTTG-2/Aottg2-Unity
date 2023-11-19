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
        private static MapTransfer _instance;
        private static readonly byte MsgMapStart = 0;
        private static readonly byte MsgMapBody = 1;
        private static readonly byte MsgMapEnd = 2;
        private static int CompressDeltaRows = 22;
        private static JSONNode _mapScriptSymbolTable;
        public static List<byte> _mapScriptCompressed;
        private static List<byte[][]> _mapTransferData;

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
            MapHash = Util.CreateMD5(serialize);
            object[] compress = CSVCompression.Compress(serialize, CompressDeltaRows);
            _mapScriptCompressed = new List<byte>((byte[])compress[0]);
            _mapScriptSymbolTable = (JSONNode)compress[1];
            _mapTransferData = new List<byte[][]>();
            _mapTransferData.Add(new byte[][] { new byte[] { MsgMapStart }, 
                StringCompression.Compress(MapManager.MapScript.Options.Serialize()),
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
            if (chunks.Count == 0)
                _mapTransferData.Add(new byte[][] { new byte[] { MsgMapEnd }, new byte[0], Encoding.UTF8.GetBytes(MapHash) });
            for (int i = 0; i < chunks.Count; i++)
            {
                if (i == chunks.Count - 1)
                    _mapTransferData.Add(new byte[][] { new byte[] { MsgMapEnd }, chunks[i], Encoding.UTF8.GetBytes(MapHash) });
                else
                    _mapTransferData.Add(new byte[][] { new byte[] {MsgMapBody}, chunks[i] });
            }
        }

        public static void OnTransferMapRPC(byte[][] byteArr, int msgNumber, int msgTotal, PhotonMessageInfo info)
        {
            if (!info.Sender.IsMasterClient)
                return;
            UIManager.LoadingMenu.UpdateLoading(0.9f * ((float)msgNumber / (float)msgTotal));
            byte msgType = byteArr[0][0];
            if (msgType == MsgMapStart)
            {
                MapManager.MapScript = new MapScript();
                MapManager.MapScript.Options.Deserialize(StringCompression.Decompress(byteArr[1]));
                _mapScriptSymbolTable = JSON.Parse(StringCompression.Decompress(byteArr[2]));
                _mapScriptCompressed = new List<byte>();

            }
            else if (msgType == MsgMapBody || msgType == MsgMapEnd)
            {
                _mapScriptCompressed.AddRange(byteArr[1]);
                if (msgType == MsgMapEnd)
                {
                    if (_mapScriptCompressed.Count > 0)
                    {
                        string decompress = CSVCompression.Decompress(_mapScriptCompressed.ToArray(), _mapScriptSymbolTable, CompressDeltaRows);
                        MapManager.MapScript.Objects.Deserialize(decompress);
                    }
                    MapHash = Encoding.UTF8.GetString(byteArr[2]);
                    MapManager.LoadMap();
                }
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
