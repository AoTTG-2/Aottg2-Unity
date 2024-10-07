using ApplicationManagers;

using CustomLogic;
using Events;
using GameManagers;
using Photon.Pun;
using Photon.Realtime;
using SimpleJSONFixed;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UI;
using UnityEngine;
using Utility;

namespace Map
{
    class CustomLogicTransfer : MonoBehaviour
    {
        public static bool LogicTransferReady;
        public static string LogicHash;
        private static CustomLogicTransfer _instance;
        private static readonly byte MsgLogicStart = 0;
        private static readonly byte MsgLogicBody = 1;
        private static readonly byte MsgLogicEnd = 2;
        private static List<byte> _logicScriptCompressed;
        private static List<byte[][]> _logicTransferData;

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
            EventManager.OnPreLoadScene += OnPreLoadScene;
        }

        private static void OnPreLoadScene(SceneName sceneName)
        {
            _instance.StopAllCoroutines();
            LogicTransferReady = false;
        }

        public static void Start()
        {
            CreateTransferData();
            foreach (Player player in PhotonNetwork.PlayerListOthers)
            {
                string logicHash = player.GetStringProperty(PlayerProperty.CustomLogicHash);
                if (logicHash != LogicHash)
                    Transfer(player);
                else
                    RPCManager.PhotonView.RPC("LoadCachedLogicRPC", player, new object[0]);
            }
            LogicTransferReady = true;
        }

        public static void Transfer(Player player)
        {
            _instance.StartCoroutine(_instance.TransferLogicData(player));
        }

        private static void CreateTransferData()
        {
            LogicHash = Util.CreateMD5(CustomLogicManager.Logic);
            byte[] compress = StringCompression.Compress(CustomLogicManager.Logic);
            _logicScriptCompressed = new List<byte>(compress);
            _logicTransferData = new List<byte[][]>();
            int chunkSize = 10000;
            int totalSize = compress.Length;
            List<byte[]> chunks = new List<byte[]>();
            for (int i = 0; i < totalSize; i += chunkSize)
            {
                if (i + chunkSize > totalSize)
                    chunkSize = totalSize - i;
                chunks.Add(_logicScriptCompressed.GetRange(i, chunkSize).ToArray());
            }
            _logicTransferData.Add(new byte[][] { new byte[] { MsgLogicStart }, new byte[0] });
            if (chunks.Count == 0)
                _logicTransferData.Add(new byte[][] { new byte[] { MsgLogicEnd }, new byte[0], Encoding.UTF8.GetBytes(LogicHash) });
            for (int i = 0; i < chunks.Count; i++)
            {
                if (i == chunks.Count - 1)
                    _logicTransferData.Add(new byte[][] { new byte[] { MsgLogicEnd }, chunks[i], Encoding.UTF8.GetBytes(LogicHash) });
                else
                    _logicTransferData.Add(new byte[][] { new byte[] { MsgLogicBody }, chunks[i] });
            }
        }

        public static void OnTransferLogicRPC(byte[][] byteArr, int msgNumber, int msgTotal, PhotonMessageInfo info)
        {
            if (!info.Sender.IsMasterClient)
                return;
            byte msgType = byteArr[0][0];
            if (msgType == MsgLogicStart)
            {
                CustomLogicManager.Logic = string.Empty;
                _logicScriptCompressed = new List<byte>();
            }
            _logicScriptCompressed.AddRange(byteArr[1]);
            if (msgType == MsgLogicEnd)
            {
                CustomLogicManager.Logic = StringCompression.Decompress(_logicScriptCompressed.ToArray());
                LogicHash = Encoding.UTF8.GetString(byteArr[2]);
                CustomLogicManager.LogicHash = LogicHash;
                CustomLogicManager.FinishLoadLogic();
            }
        }

        private IEnumerator TransferLogicData(Player player)
        {
            for (int i = 0; i < _logicTransferData.Count; i++)
            {
                RPCManager.PhotonView.RPC("TransferLogicRPC", player, new object[] { _logicTransferData[i], i, _logicTransferData.Count });
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
