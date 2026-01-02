using ApplicationManagers;
using GameManagers;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Diagnostics;
using Utility;

namespace CustomLogic
{
    [CLType(Name = "Network", Static = true, Abstract = true, Description = "Networking functions.")]
    partial class CustomLogicNetworkBuiltin : BuiltinClassInstance
    {
        [CLConstructor("Creates a new Network instance.")]
        public CustomLogicNetworkBuiltin()
        {
        }

        [CLProperty(Static = true, Description = "Is the player the master client.")]
        public bool IsMasterClient => PhotonNetwork.IsMasterClient;

        [CLProperty(Static = true, TypeArguments = new[] { "Player" }, Description = "The list of players in the room.")]
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

        [CLProperty(Static = true, Description = "The master client.")]
        public CustomLogicPlayerBuiltin MasterClient => new CustomLogicPlayerBuiltin(PhotonNetwork.MasterClient);

        [CLProperty(Static = true, Description = "The local player.")]
        public CustomLogicPlayerBuiltin MyPlayer => new CustomLogicPlayerBuiltin(PhotonNetwork.LocalPlayer);

        [CLProperty(Static = true, Description = "The network time.")]
        public double NetworkTime => PhotonNetwork.Time;

        [CLProperty(Static = true, Description = "The local player's ping.")]
        public int Ping => PhotonNetwork.GetPing();

        [CLMethod(Static = true, Description = "Send a message to a player.")]
        public void SendMessage(
            [CLParam("The player to send the message to.")]
            CustomLogicPlayerBuiltin player,
            [CLParam("The message to send.")]
            string message)
        {
            RPCManager.PhotonView.RPC(nameof(RPCManager.SendMessageRPC), player.Player, new object[] { message });
        }

        [CLMethod(Static = true, Description = "Send a message to all players.")]
        public void SendMessageAll(
            [CLParam("The message to send.")]
            string message)
        {
            RPCManager.PhotonView.RPC(nameof(RPCManager.SendMessageRPC), RpcTarget.All, new object[] { message });
        }

        [CLMethod(Static = true, Description = "Send a message to all players except the sender.")]
        public void SendMessageOthers(
            [CLParam("The message to send.")]
            string message)
        {
            RPCManager.PhotonView.RPC(nameof(RPCManager.SendMessageRPC), RpcTarget.Others, new object[] { message });
        }

        [CLMethod(Static = true, Description = "Finds a player in the room by id.")]
        public CustomLogicPlayerBuiltin FindPlayer(
            [CLParam("The player ID to find.")]
            int id)
        {
            Player player = Util.FindPlayerById(id);
            if (player != null)
                return new CustomLogicPlayerBuiltin(player);
            return null;
        }

        [CLMethod(Static = true, Description = "Get the difference between two photon timestamps.")]
        public double GetTimestampDifference(
            [CLParam("The first timestamp.")]
            double timestamp1,
            [CLParam("The second timestamp.")]
            double timestamp2)
        {
            // Handle the wrap around case photon timestamps have for the user since most will likely ignore it otherwise.
            return Util.GetPhotonTimestampDifference(timestamp1, timestamp2);
        }

        [CLMethod(Static = true, Description = "Kick the given player by id or player reference.")]
        public void KickPlayer(
            [CLParam("The player to kick (can be Player object or int ID).", Type = "Player|int")]
            object target,
            [CLParam("The reason for kicking the player (default: '.').")]
            string reason = ".")
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
