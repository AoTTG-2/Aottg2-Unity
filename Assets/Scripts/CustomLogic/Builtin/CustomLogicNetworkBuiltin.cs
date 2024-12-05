using ApplicationManagers;
using GameManagers;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    class CustomLogicNetworkBuiltin: CustomLogicClassInstanceBuiltin
    {
        public CustomLogicNetworkBuiltin(): base("Network")
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
    }
}
