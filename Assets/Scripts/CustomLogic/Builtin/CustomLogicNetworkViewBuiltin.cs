using System.Collections.Generic;
using UnityEngine;
using Map;
using GameManagers;
using Utility;
using Photon.Pun;

namespace CustomLogic
{
    [CLType(Abstract = true)]
    class CustomLogicNetworkViewBuiltin : CustomLogicClassInstanceBuiltin
    {
        public MapObject MapObject;
        public CustomLogicPhotonSync Sync;
        public int OwnerId = -1;
        List<CustomLogicComponentInstance> _classInstances = new List<CustomLogicComponentInstance>();
        List<object> _streamObjs;

        public CustomLogicNetworkViewBuiltin(MapObject obj) : base("NetworkView")
        {
            MapObject = obj;
        }

        [CLProperty("Gets the owner of the network view.")]
        public CustomLogicPlayerBuiltin Owner
        {
            get
            {
                if (Sync == null)
                    return null;
                return new CustomLogicPlayerBuiltin(Sync.photonView.Owner);
            }
        }

        [CLMethod("Handles per-second updates for the network view.")]
        public void OnSecond()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (OwnerId >= 0 && OwnerId != PhotonNetwork.LocalPlayer.ActorNumber)
                {
                    var player = Util.FindPlayerById(OwnerId);
                    if (player == null)
                    {
                        var go = PhotonNetwork.Instantiate("Game/CustomLogicPhotonSyncPrefab", Vector3.zero, Quaternion.identity, 0);
                        var photonView = go.GetComponent<CustomLogicPhotonSync>();
                        photonView.Init(MapObject.ScriptObject.Id);
                    }
                }
            }
        }

        [CLMethod("Registers a component instance with the network view.")]
        public void RegisterComponentInstance(CustomLogicComponentInstance instance)
        {
            _classInstances.Add(instance);
        }

        [CLMethod("Sets the sync object for the network view.")]
        public void SetSync(CustomLogicPhotonSync sync)
        {
            int oldId = OwnerId;
            Sync = sync;
            OwnerId = sync.photonView.Owner.ActorNumber;
            if (oldId >= 0)
            {
                var oldPlayer = Util.FindPlayerById(oldId);
                CustomLogicPlayerBuiltin oldOwner = null;
                if (oldPlayer != null)
                    oldOwner = new CustomLogicPlayerBuiltin(oldPlayer);
                var newOwner = new CustomLogicPlayerBuiltin(Sync.photonView.Owner);
                if (MapObject.GameObject != null)
                {
                    foreach (var instance in _classInstances)
                        CustomLogicManager.Evaluator.EvaluateMethod(instance, "OnNetworkTransfer", new List<object>() { oldOwner, newOwner });
                }
            }
        }

        [CLMethod("Sends the network stream to the specified Photon stream.")]
        public void SendNetworkStream(PhotonStream stream)
        {
            _streamObjs = new List<object>();
            foreach (var instance in _classInstances)
            {
                CustomLogicManager.Evaluator.EvaluateMethod(instance, "SendNetworkStream");
            }
            stream.SendNext(_streamObjs.ToArray());
        }

        [CLMethod("Handles the network stream received from the specified objects.")]
        public void OnNetworkStream(object[] objs)
        {
            _streamObjs = new List<object>(objs);
            if (MapObject.GameObject == null)
                return;
            foreach (var instance in _classInstances)
            {
                CustomLogicManager.Evaluator.EvaluateMethod(instance, "OnNetworkStream");
            }
        }

        [CLMethod("Handles a network message from the specified player.")]
        public void OnNetworkMessage(CustomLogicPlayerBuiltin player, string message, double sentServerTime)
        {
            if (CustomLogicManager.Evaluator == null || MapObject.GameObject == null)
                return;
            foreach (var instance in _classInstances)
            {
                CustomLogicManager.Evaluator.EvaluateMethod(instance, "OnNetworkMessage", new List<object>() { player, message, sentServerTime });
            }
        }

        [CLMethod("Transfers the network view to the specified player.")]
        public void Transfer(CustomLogicPlayerBuiltin player)
        {
            if (Sync.photonView.IsMine)
            {
                if (player.Player != PhotonNetwork.LocalPlayer)
                {
                    RPCManager.PhotonView.RPC("TransferNetworkViewRPC", player.Player, new object[] { MapObject.ScriptObject.Id });
                    PhotonNetwork.Destroy(Sync.gameObject);
                }
            }
        }

        [CLMethod("Sends a message to the specified player.")]
        public void SendMessage(CustomLogicPlayerBuiltin target, string msg)
        {
            Sync.SendMessage(target.Player, msg);
        }

        [CLMethod("Sends a message to all players.")]
        public void SendMessageAll(string msg)
        {
            Sync.SendMessageAll(msg);
        }

        [CLMethod("Sends a message to all players except the sender.")]
        public void SendMessageOthers(string msg)
        {
            Sync.SendMessageOthers(msg);
        }

        [CLMethod("Sends an object through the network stream.")]
        public void SendStream(object obj)
        {
            obj = SerializeStreamObj(obj);
            _streamObjs.Add(obj);
        }

        [CLMethod("Receives an object from the network stream.")]
        public object ReceiveStream()
        {
            var obj = _streamObjs[0];
            obj = DeserializeStreamObj(obj);
            _streamObjs.RemoveAt(0);
            return obj;
        }

        protected object SerializeStreamObj(object obj)
        {
            if (obj is CustomLogicVector3Builtin)
                return ((CustomLogicVector3Builtin)obj).Value;
            return obj;
        }

        protected object DeserializeStreamObj(object obj)
        {
            if (obj is Vector3)
                return new CustomLogicVector3Builtin((Vector3)obj);
            return obj;
        }
    }
}
