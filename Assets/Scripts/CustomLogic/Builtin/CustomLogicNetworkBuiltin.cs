using ApplicationManagers;
using GameManagers;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    [CLType(Name = "Network", Static = true)]
    partial class CustomLogicNetworkBuiltin : CustomLogicClassInstanceBuiltin
    {
        public CustomLogicNetworkBuiltin() : base("Network")
        {
        }

        [CLProperty(description: "Is the player the master client")]
        public bool IsMasterClient => PhotonNetwork.IsMasterClient;

        [CLProperty(description: "The list of players in the room")]
        public CustomLogicListBuiltin Players
        {
            get
            {
                CustomLogicListBuiltin list = new CustomLogicListBuiltin();
                foreach (var player in PhotonNetwork.PlayerList)
                {
                    list.List.Add(new CustomLogicPlayerBuiltin(player));
                }
                return list;
            }
        }

        [CLProperty(description: "The master client")]
        public CustomLogicPlayerBuiltin MasterClient => new CustomLogicPlayerBuiltin(PhotonNetwork.MasterClient);

        [CLProperty(description: "The local player")]
        public CustomLogicPlayerBuiltin MyPlayer => new CustomLogicPlayerBuiltin(PhotonNetwork.LocalPlayer);

        [CLProperty(description: "The network time")]
        public double NetworkTime => PhotonNetwork.Time;

        [CLProperty(description: "The local player's ping")]
        public int Ping => PhotonNetwork.GetPing();

        [CLMethod(description: "Send a message to a player")]
        public void SendMessage(CustomLogicPlayerBuiltin player, string message)
        {
            RPCManager.PhotonView.RPC("SendMessageRPC", player.Player, new object[] { message });
        }

        [CLMethod(description: "Send a message to all players")]
        public void SendMessageAll(string message)
        {
            RPCManager.PhotonView.RPC("SendMessageRPC", RpcTarget.All, new object[] { message });
        }

        [CLMethod(description: "Send a message to all players except the sender")]
        public void SendMessageOthers(string message)
        {
            RPCManager.PhotonView.RPC("SendMessageRPC", RpcTarget.Others, new object[] { message });
        }

        [CLMethod(description: "Get the difference between two photon timestamps")]
        public double GetTimestampDifference(double timestamp1, double timestamp2)
        {
            // Handle the wrap around case photon timestamps have for the user since most will likely ignore it otherwise.
            return Util.GetPhotonTimestampDifference(timestamp1, timestamp2);
        }

        [CLMethod(description: "Kick the given player by id or player reference.")]
        public void KickPlayer(object target, string reason = ".")
        {
            Photon.Realtime.Player player = null;
            if (target is int)
                player = PhotonNetwork.CurrentRoom.GetPlayer(target.UnboxToInt(), true);
            else if (target is CustomLogicPlayerBuiltin)
                player = ((CustomLogicPlayerBuiltin)target).Player;
            else
                throw new ArgumentException($"Invalid player parameter type {target.GetType()}. Valid types are {nameof(CustomLogicPlayerBuiltin)}, int (id).");

            if (PhotonNetwork.IsMasterClient)
                ChatManager.KickPlayer(player, reason: reason);
            else
                throw new Exception("Only the master client can kick players.");
        }


    }
}
