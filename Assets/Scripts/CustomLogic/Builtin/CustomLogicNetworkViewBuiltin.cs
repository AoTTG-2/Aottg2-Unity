using GameManagers;
using Map;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    /// <summary>
    /// Represents a network view on a map object that has the "networked" flag.
    /// Note1: messages sent from a mapobjects network view are not component scoped, all components will receive the same message.
    /// If you intend for a mapobject to have multiple message sending components, preface the message with the component name to determine scope.
    /// 
    /// Note2: Rooms and Players have bandwidth limits, exceeding the limits via CL will result in either the player being kicked or the room being shut down.
    /// When possible, use basic message passing for state sync and then run logic locally instead of repeatedly sending state over the network. Also
    /// avoid cases where message sending increases heavily with the number of players in the room.
    /// </summary>
    /// <code>
    /// # The following is for a component scoped object, in general this is bad practice if the component is widely used.
    /// # OnPlayerJoin, every object with this component will send a message to the player that joined, if you use 100 objects with this, 100 messages will be sent.
    /// # Preferred practice for this sort of case is to have a either Main handle the single message pass or have a single ManagerComponent that handles the message pass
    /// # and defers the value to all registered components.
    /// KillCount = 0;
    /// 
    /// function OnNetworkMessage(player, message, sentServerTime) {
    ///     if (player.ID == Network.MasterClient.ID) {
    ///         self.KillCount == Convert.ToInt(message);
    ///     }
    /// }
    /// 
    /// function OnCharacterDie(victim, killer, killerName) {
    ///     self.KillCount += 1;
    /// }
    /// 
    /// function OnPlayerJoined(player) {
    ///     if (Network.IsMasterClient) {
    ///         self.NetworkView.SendMessage(player, Convert.ToString(self.KillCount));
    ///     }
    /// }
    /// 
    /// # Good Practice would be to have a single component that handles the message pass and defers the value to all registered components.
    /// TODO: Bother someone for good practice example - maybe move this into Networking Summary Page.
    /// </code>
    [CLType(Name = "NetworkView", Abstract = true)]
    partial class CustomLogicNetworkViewBuiltin : BuiltinClassInstance
    {
        public readonly MapObject MapObject;
        public CustomLogicPhotonSync Sync;
        public int OwnerId = -1;

        private List<object> _streamObjects;
        private readonly List<CustomLogicComponentInstance> _classInstances = new List<CustomLogicComponentInstance>();
        private bool _isTransformSynced = true;


        [CLConstructor]
        public CustomLogicNetworkViewBuiltin(MapObject obj)
        {
            MapObject = obj;
        }

        [CLProperty("Whether or not the object's Transform is synced. If PhotonSync is not initialized yet, it will defer until it is set.")]
        public bool SyncTransforms
        {
            get => _isTransformSynced;
            set
            {
                _isTransformSynced = value;
                if (Sync != null)
                    Sync.SyncTransforms = value;
            }
        }

        [CLProperty("The network view's owner.")]
        public CustomLogicPlayerBuiltin Owner
        {
            get
            {
                if (Sync == null)
                    return null;
                return new CustomLogicPlayerBuiltin(Sync.photonView.Owner);
            }
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
            Sync.SyncTransforms = _isTransformSynced;
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
                        CustomLogicManager.Evaluator.EvaluateMethod(instance, "OnNetworkTransfer", new object[] { oldOwner, newOwner });
                }
            }
        }

        public void SendNetworkStream(PhotonStream stream)
        {
            _streamObjects = new List<object>();
            foreach (var instance in _classInstances)
            {
                CustomLogicManager.Evaluator.EvaluateMethod(instance, "SendNetworkStream");
            }
            stream.SendNext(_streamObjects.ToArray());
        }

        public void OnNetworkStream(object[] objs)
        {
            _streamObjects = new List<object>(objs);
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
                CustomLogicManager.Evaluator.EvaluateMethod(instance, "OnNetworkMessage", new object[] { player, message, sentServerTime });
            }
        }

        [CLMethod("Owner only. Transfer ownership of this NetworkView to another player.")]
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

        [CLMethod("Send a message to a target player. This will be received in any of the MapObject attached components through the OnNetworkMessage callback.")]
        public void SendMessage(CustomLogicPlayerBuiltin target, string msg)
        {
            Sync.SendMessage(target.Player, msg);
        }

        [CLMethod("Send a message to all players including myself.")]
        public void SendMessageAll(string msg)
        {
            Sync.SendMessageAll(msg);
        }

        [CLMethod("Send a message to players excluding myself.")]
        public void SendMessageOthers(string msg)
        {
            Sync.SendMessageOthers(msg);
        }

        /// <summary>
        /// Send an object to the network sync stream.
        /// This represents sending data from the object owner to all non-owner observers,
        /// and should only be called in the SendNetworkStream callback in the attached component.
        /// It only works with some object types: primitives and Vector3.
        /// </summary>
        [CLMethod]
        public void SendStream(object obj)
        {
            obj = SerializeStreamObj(obj);
            _streamObjects.Add(obj);
        }

        /// <summary>
        /// Receive an object through the network sync stream.
        /// This represents receiving data from the object owner as a non-owner observer,
        /// and should only be called in the OnNetworkStream callback.
        /// </summary>
        [CLMethod]
        public object ReceiveStream()
        {
            var obj = _streamObjects[0];
            obj = DeserializeStreamObj(obj);
            _streamObjects.RemoveAt(0);
            return obj;
        }

        private static object SerializeStreamObj(object obj)
        {
            if (obj is CustomLogicVector3Builtin)
                return ((CustomLogicVector3Builtin)obj).Value;
            if (obj is CustomLogicQuaternionBuiltin)
                return ((CustomLogicQuaternionBuiltin)obj).Value;
            return obj;
        }

        private static object DeserializeStreamObj(object obj)
        {
            if (obj is Vector3)
                return new CustomLogicVector3Builtin((Vector3)obj);
            if (obj is CustomLogicQuaternionBuiltin)
                return new CustomLogicQuaternionBuiltin((Quaternion)obj);
            return obj;
        }
    }
}
