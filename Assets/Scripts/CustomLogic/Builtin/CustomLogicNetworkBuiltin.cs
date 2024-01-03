using ApplicationManagers;
using GameManagers;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

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
            return base.GetField(name);
        }

        public override void SetField(string name, object value)
        {
            base.SetField(name, value);
        }
    }
}
