using System.Collections.Generic;
using UnityEngine;
using Map;
using GameManagers;
using Utility;
using Photon.Pun;

namespace CustomLogic
{
    class CustomLogicNetworkViewBuiltin: CustomLogicBaseBuiltin
    {
        public MapObject MapObject;
        public CustomLogicPhotonSync Sync;
        public int OwnerId = -1;
        List<CustomLogicComponentInstance> _classInstances = new List<CustomLogicComponentInstance>();
        List<object> _streamObjs;

        public CustomLogicNetworkViewBuiltin(MapObject obj): base("NetworkView")
        {
            MapObject = obj;
        }

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

        public void RegisterComponentInstance(CustomLogicComponentInstance instance)
        {
            _classInstances.Add(instance);
        }

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

        public void SendNetworkStream(PhotonStream stream)
        {
            _streamObjs = new List<object>();            
            foreach (var instance in _classInstances)
            {
                CustomLogicManager.Evaluator.EvaluateMethod(instance, "SendNetworkStream");
            }
            stream.SendNext(_streamObjs.ToArray());
        }

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

        public void OnNetworkMessage(CustomLogicPlayerBuiltin player, string message, double sentServerTime)
        {
            if (CustomLogicManager.Evaluator == null || MapObject.GameObject == null)
                return;
            foreach (var instance in _classInstances)
            {
                CustomLogicManager.Evaluator.EvaluateMethod(instance, "OnNetworkMessage", new List<object>() { player, message, sentServerTime });
            }
        }

        public override object CallMethod(string methodName, List<object> parameters)
        {
            if (methodName == "Transfer")
            {
                if (Sync.photonView.IsMine)
                {
                    var player = (CustomLogicPlayerBuiltin)parameters[0];
                    if (player.Player != PhotonNetwork.LocalPlayer)
                    {
                        RPCManager.PhotonView.RPC("TransferNetworkViewRPC", player.Player, new object[] { MapObject.ScriptObject.Id });
                        PhotonNetwork.Destroy(Sync.gameObject);
                    }
                }
                return null;
            }
            if (methodName == "SendMessage")
            {
                var target = (CustomLogicPlayerBuiltin)parameters[0];
                string msg = (string)parameters[1];
                Sync.SendMessage(target.Player, msg);
                return null;
            }
            if (methodName == "SendMessageAll")
            {
                string msg = (string)parameters[0];
                Sync.SendMessageAll(msg);
                return null;
            }
            if (methodName == "SendMessageOthers")
            {
                string msg = (string)parameters[0];
                Sync.SendMessageOthers(msg);
                return null;
            }
            if (methodName == "SendStream")
            {
                var obj = parameters[0];
                obj = SerializeStreamObj(obj);
                _streamObjs.Add(obj);
                return null;
            }
            if (methodName == "ReceiveStream")
            {
                var obj = _streamObjs[0];
                obj = DeserializeStreamObj(obj);
                _streamObjs.RemoveAt(0);
                return obj;
            }
            return base.CallMethod(methodName, parameters);
        }

        public override object GetField(string name)
        {
            if (name == "Owner")
            {
                if (Sync == null)
                    return null;
                return new CustomLogicPlayerBuiltin(Sync.photonView.Owner);
            }
            return base.GetField(name);
        }

        protected object SerializeStreamObj(object obj)
        {
            if (obj is CustomLogicVector3Builtin)
                return ((CustomLogicVector3Builtin)obj).Value;
            if (obj is CustomLogicQuaternionBuiltin)
                return ((CustomLogicQuaternionBuiltin)obj).Value;
            return obj;
        }

        protected object DeserializeStreamObj(object obj)
        {
            if (obj is Vector3)
                return new CustomLogicVector3Builtin((Vector3)obj);
            if (obj is CustomLogicQuaternionBuiltin)
                return new CustomLogicQuaternionBuiltin((Quaternion)obj);
            return obj;
        }
    }
}
