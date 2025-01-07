using ApplicationManagers;
using GameManagers;
using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    class CustomLogicNetworkBuiltin: CustomLogicBaseBuiltin
    {
        public CustomLogicNetworkBuiltin(): base("Network")
        {
        }

        public override object CallMethod(string name, List<object> parameters)
        {
            if (name == "SendMessage")
            {
                var player = (CustomLogicPlayerBuiltin)parameters[0];
                RPCManager.PhotonView.RPC("SendMessageRPC", player.Player, new object[] { (string)parameters[1] });
                return null;
            }
            if (name == "SendMessageAll")
            {
                RPCManager.PhotonView.RPC("SendMessageRPC", RpcTarget.All, new object[] { (string)parameters[0] });
                return null;
            }
            if (name == "SendMessageOthers")
            {
                RPCManager.PhotonView.RPC("SendMessageRPC", RpcTarget.Others, new object[] { (string)parameters[0] });
                return null;
            }
            if (name == "GetTimestampDifference")
            {
                // Handle the wrap around case photon timestamps have for the user since most will likely ignore it otherwise.
                return Util.GetPhotonTimestampDifference(parameters[0].UnboxToFloat(), parameters[1].UnboxToFloat());
            }
            if (name == "KickPlayer")
            {
                Photon.Realtime.Player player = null;
                if (parameters[0] is int)
                    player = PhotonNetwork.CurrentRoom.GetPlayer(parameters[0].UnboxToInt(), true);
                else if (parameters[0] is CustomLogicPlayerBuiltin)
                    player = ((CustomLogicPlayerBuiltin)parameters[0]).Player;
                else
                    throw new ArgumentException($"Invalid player parameter type {parameters[0].GetType()}. Valid types are {nameof(CustomLogicPlayerBuiltin)}, int (id).");

                string reason = ".";
                if (parameters.Count >= 2)
                    reason = (string)parameters[1];

                if (PhotonNetwork.IsMasterClient)
                    ChatManager.KickPlayer(player, reason: reason);
                else
                    throw new Exception("Only the master client can kick players.");
                return null;
            }
            return base.CallMethod(name, parameters);
        }

        public override object GetField(string name)
        {
            if (name == "IsMasterClient")
                return PhotonNetwork.IsMasterClient;
            if (name == "Players")
            {
                CustomLogicListBuiltin list = new CustomLogicListBuiltin();
                foreach (var player in PhotonNetwork.PlayerList)
                {
                    list.List.Add(new CustomLogicPlayerBuiltin(player));
                }
                return list;
            }
            if (name == "MasterClient")
                return new CustomLogicPlayerBuiltin(PhotonNetwork.MasterClient);
            if (name == "MyPlayer")
                return new CustomLogicPlayerBuiltin(PhotonNetwork.LocalPlayer);
            if (name == "NetworkTime")
                return (float)PhotonNetwork.Time;
            if (name == "Ping")
                return PhotonNetwork.GetPing();
            return base.GetField(name);
        }

        public override void SetField(string name, object value)
        {
            base.SetField(name, value);
        }
    }
}
