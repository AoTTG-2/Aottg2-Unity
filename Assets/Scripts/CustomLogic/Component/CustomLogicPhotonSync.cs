using Map;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    public enum SpawnIntent
    {
        LocalOnly,        // Create a MapObject only (no networking)
        PreplacedBind,    // Bind a PhotonView to an existing MapObject by id
        NetworkedRuntime  // Networked spawn that creates the MapObject from script data
    }

    class CustomLogicPhotonSync : Photon.Pun.MonoBehaviourPunCallbacks, Photon.Pun.IPunObservable, IOnPhotonViewOwnerChange, IPunInstantiateMagicCallback
    {
        public bool SyncTransforms
        {
            get => _syncTransforms;
            set
            {
                if (PhotonView.IsMine)
                    _syncTransforms = value;
            }
        }
        public bool SyncVelocity
        {
            get => _syncVelocity;
            set
            {
                if (PhotonView.IsMine)
                    _syncVelocity = value;
            }
        }

        protected bool _syncTransforms = true;
        protected bool _syncVelocity = true;

        public int ObjectId;
        public PhotonView PhotonView;
        public MapObject MapObject { get; protected set; }
        public CustomLogicMapObjectBuiltin CustomLogicMapObjectBuiltin { get; protected set; }
        public CustomLogicNetworkViewBuiltin NetworkView { get; protected set; }
        public float SmoothingDelay => 5f;


        protected Vector3 _correctPosition = Vector3.zero;
        protected Quaternion _correctRotation = Quaternion.identity;
        protected Vector3 _correctVelocity = Vector3.zero;


        protected bool _inited = false;
        protected object[] _streamObjs;
        protected bool _synced = false;
        protected bool _persistsOwnership = false;
        protected SpawnIntent _spawnIntent = SpawnIntent.LocalOnly;

        protected virtual void Awake()
        {
            PhotonView = photonView;
        }
        public virtual void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        public virtual void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }
        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            // Expecting: SpawnIntent intent [, int id, bool syncVelocity] | [, bool persistsOwner, string csvScript]
            object[] data = (object[])info.photonView.InstantiationData;

            _spawnIntent = (SpawnIntent)data[0];

            if (_spawnIntent == SpawnIntent.PreplacedBind)
            {
                ObjectId = (int)data[1];
                _syncVelocity = (bool)data[2];
                // StartCoroutine(WaitAndFinishInit(ObjectId)); // use legacy method for now, this CAN improve bandwidth usage but we would have to reorder component loading.
            }
            else if (_spawnIntent == SpawnIntent.NetworkedRuntime)
            {
                //bool persistsOwnership = (bool)data[1];
                //string csvScript = (string)data[2];
                //if (string.IsNullOrEmpty(csvScript))
                //{
                //    // This is likely a placeholder object, so we don't need to do anything.
                //    return;
                //}

                //ObjectId = MapLoader.ROOT_OBJECT_ID - MapLoader.NETWORK_OFFSET - photonView.ViewID;

                //// Deserialize the CSV/script to create the MapObject.
                //var prefab = string.Join("", csvScript.Split('\n'));
                //var script = new MapScriptSceneObject();
                //script.Deserialize(prefab);
                //script.Id = ObjectId;
                //script.Parent = 0;
                //script.Networked = true;
                //var mapObject = MapLoader.LoadObject(script, false);
                //mapObject.RuntimeCreated = true;
                //MapLoader.SetParent(mapObject);

                //NetworkView = new CustomLogicNetworkViewBuiltin(mapObject);
                //CustomLogicMapObjectBuiltin = new CustomLogicMapObjectBuiltin(mapObject);
                //NetworkView.Sync = this;

                //if (CustomLogicManager.Evaluator.IdToMapObjectBuiltin.ContainsKey(ObjectId))
                //{
                //    Debug.LogWarning($"PhotonSync: MapObject for id {ObjectId} already exists, overwriting.");
                //}
                //CustomLogicManager.Evaluator.IdToMapObjectBuiltin[ObjectId] = CustomLogicMapObjectBuiltin;

                //if (CustomLogicManager.Evaluator.IdToNetworkView.ContainsKey(ObjectId))
                //{
                //    Debug.LogWarning($"PhotonSync: NetworkView for id {ObjectId} already exists, overwriting.");
                //}
                //CustomLogicManager.Evaluator.IdToNetworkView[ObjectId] = NetworkView;

                //CustomLogicManager.Evaluator.LoadRuntimeMapObjectComponents(mapObject, true);

            }
        }

        #region Legacy Init
        public void Init(int mapObjectId, bool rigidbody)
        {
            MapObject = MapLoader.IdToMapObject[mapObjectId];
            PhotonView.RPC("InitRPC", RpcTarget.AllBuffered, new object[] { mapObjectId, rigidbody });
        }

        public void Init(int mapObjectId)
        {
            MapObject = MapLoader.IdToMapObject[mapObjectId];
            bool rigidbody = MapObject.GameObject.GetComponent<Rigidbody>() != null;
            PhotonView.RPC("InitRPC", RpcTarget.AllBuffered, new object[] { mapObjectId, rigidbody });
        }

        public void InitDynamic(bool persistsOwnership, string csvScript)
        {
            PhotonView.RPC("InitDynamicRPC", RpcTarget.OthersBuffered, new object[] { persistsOwnership, csvScript });
            CreateAndSetupObject(persistsOwnership, csvScript);
        }

        [PunRPC]
        public void InitRPC(int mapObjectId, bool syncVelocity, PhotonMessageInfo info)
        {
            if (info.Sender != PhotonView.Owner)
                return;
            _syncVelocity = syncVelocity;
            StartCoroutine(WaitAndFinishInit(mapObjectId));
        }

        [PunRPC]
        public void InitDynamicRPC(bool persistsOwnership, string csvScript, PhotonMessageInfo info)
        {
            if (info.Sender != PhotonView.Owner)
                return;
            CreateAndSetupObject(persistsOwnership, csvScript);
        }

        public void CreateAndSetupObject(bool persistsOwnership, string csvScript)
        {
            if (string.IsNullOrEmpty(csvScript))
            {
                // This is likely a placeholder object, so we don't need to do anything.
                return;
            }
            _persistsOwnership = persistsOwnership;
            ObjectId = MapLoader.ROOT_OBJECT_ID - MapLoader.NETWORK_OFFSET - photonView.ViewID;



            // Deserialize the CSV/script to create the MapObject.
            var prefab = string.Join("", csvScript.Split('\n'));
            var script = new MapScriptSceneObject();
            script.Deserialize(prefab);
            script.Id = ObjectId;
            script.Parent = 0;
            script.Networked = true;
            var mapObject = MapLoader.LoadObject(script, false);
            mapObject.RuntimeCreated = true;
            MapLoader.SetParent(mapObject);

            NetworkView = new CustomLogicNetworkViewBuiltin(mapObject);
            CustomLogicMapObjectBuiltin = new CustomLogicMapObjectBuiltin(mapObject);
            MapObject = mapObject;

            if (CustomLogicManager.Evaluator.IdToMapObjectBuiltin.ContainsKey(ObjectId))
            {
                Debug.LogWarning($"PhotonSync: MapObject for id {ObjectId} already exists, overwriting.");
            }
            CustomLogicManager.Evaluator.IdToMapObjectBuiltin[ObjectId] = CustomLogicMapObjectBuiltin;

            if (CustomLogicManager.Evaluator.IdToNetworkView.ContainsKey(ObjectId))
            {
                Debug.LogWarning($"PhotonSync: NetworkView for id {ObjectId} already exists, overwriting.");
            }
            CustomLogicManager.Evaluator.IdToNetworkView[ObjectId] = NetworkView;
            NetworkView.SetSyncDynamic(this);
            CustomLogicManager.Evaluator.LoadRuntimeMapObjectComponents(mapObject, true);
            _correctPosition = MapObject.GameObject.transform.position;
            _correctRotation = MapObject.GameObject.transform.rotation;
            _inited = true;
        }

        public IEnumerator WaitAndFinishInit(int mapObjectId)
        {
            while (CustomLogicManager.Evaluator == null || !CustomLogicManager.Evaluator.IdToNetworkView.ContainsKey(mapObjectId))
                yield return null;
            FinishInit(mapObjectId);
        }

        private void FinishInit(int mapObjectId)
        {
            MapObject = MapLoader.IdToMapObject[mapObjectId];
            NetworkView = CustomLogicManager.Evaluator.IdToNetworkView[mapObjectId];
            NetworkView.SetSync(this);
            _correctPosition = MapObject.GameObject.transform.position;
            _correctRotation = MapObject.GameObject.transform.rotation;
            _inited = true;
        }

        #endregion
        [PunRPC]
        public void SendMessageRPC(string message, PhotonMessageInfo info)
        {
            var player = info.Sender;
            if (NetworkView != null)
                NetworkView.OnNetworkMessage(new CustomLogicPlayerBuiltin(player), message, info.SentServerTime);
        }

        public void SendMessage(Player player, string message)
        {
            PhotonView.RPC("SendMessageRPC", player, new object[] { message });
        }

        public void SendMessageAll(string message)
        {
            PhotonView.RPC("SendMessageRPC", RpcTarget.All, new object[] { message });
        }

        public void SendMessageOthers(string message)
        {
            PhotonView.RPC("SendMessageRPC", RpcTarget.Others, new object[] { message });
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (_inited && SyncTransforms)
                PhotonView.RPC("SyncRPC", newPlayer, new object[] { GetPosition(), GetRotation() });
        }

        [PunRPC]
        public void SyncRPC(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
        {
            if (info.Sender != PhotonView.Owner)
                return;
            StartCoroutine(WaitAndFinishSync(position, rotation));
        }

        public IEnumerator WaitAndFinishSync(Vector3 position, Quaternion rotation)
        {
            while (!_inited)
                yield return null;
            _correctPosition = position;
            _correctRotation = rotation;
        }

        protected virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                if (_inited && MapObject.GameObject != null)
                {
                    if (_syncTransforms)
                    {
                        stream.SendNext(GetPosition());
                        var rotation = GetRotation();
                        stream.SendNext(QuaternionCompression.CompressQuaternion(ref rotation));
                        if (_syncVelocity)
                        {
                            stream.SendNext(GetVelocity());
                        }
                    }
                    NetworkView.SendNetworkStream(stream);
                }
            }
            else
            {
                if (_inited)
                {
                    _syncTransforms = stream.PeekNext() is not object[];    // We cannot effectively sync "SyncTransforms" state so this is the next best thing.
                    if (_syncTransforms)
                    {
                        _correctPosition = (Vector3)stream.ReceiveNext();
                        QuaternionCompression.DecompressQuaternion(ref _correctRotation, (int)stream.ReceiveNext());
                        _syncVelocity = stream.PeekNext() is not object[];   // Also allows toggling of velocity sync without destroy/reinstantiate.
                        if (_syncVelocity)
                            _correctVelocity = (Vector3)stream.ReceiveNext();
                    }
                    _streamObjs = (object[])stream.ReceiveNext();
                    if (_streamObjs != null && _streamObjs.Length > 0)
                        NetworkView.OnNetworkStream(_streamObjs);
                }
            }
        }

        protected virtual void Update()
        {
            if (!PhotonView.IsMine && _inited)
            {
                if (MapObject.GameObject == null || !_syncVelocity)
                    return;
                var transform = MapObject.GameObject.transform;
                transform.position = Vector3.Lerp(transform.position, _correctPosition, Time.deltaTime * SmoothingDelay);
                transform.rotation = Quaternion.Lerp(transform.rotation, _correctRotation, Time.deltaTime * SmoothingDelay);
                if (_syncVelocity)
                {
                    MapObject.GameObject.GetComponent<Rigidbody>().velocity = _correctVelocity;
                }
            }
        }

        private Vector3 GetPosition()
        {
            if (MapObject.GameObject != null)
                return MapObject.GameObject.transform.position;
            return Vector3.zero;
        }

        private Quaternion GetRotation()
        {
            if (MapObject.GameObject != null)
                return MapObject.GameObject.transform.rotation;
            return Quaternion.identity;
        }

        private Vector3 GetVelocity()
        {
            if (MapObject.GameObject != null)
                return MapObject.GameObject.GetComponent<Rigidbody>().velocity;
            return Vector3.zero;
        }

        void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            OnPhotonSerializeView(stream, info);
        }

        public void Transfer(CustomLogicPlayerBuiltin player)
        {
            if (PhotonView.IsMine && player.Player != PhotonNetwork.LocalPlayer)
            {
                PhotonView.TransferOwnership(player.Player.ActorNumber);
            }
        }

        void IOnPhotonViewOwnerChange.OnOwnerChange(Player newOwner, Player previousOwner)
        {
            var oldOwnerBuiltin = previousOwner != null ? new CustomLogicPlayerBuiltin(previousOwner) : null;
            var newOwnerBuiltin = new CustomLogicPlayerBuiltin(newOwner);

            if (NetworkView != null)
                NetworkView.OnNetworkTransfer(oldOwnerBuiltin, newOwnerBuiltin);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (PhotonView.Owner == otherPlayer)
            {
                if (_persistsOwnership)
                {
                    // Transfer to MasterClient
                    if (PhotonNetwork.IsMasterClient)
                        PhotonView.TransferOwnership(PhotonNetwork.MasterClient);
                }
            }
        }

        public void DestroyMe()
        {
            // Destroy the self, the object, the controller, and the network view.
            if (PhotonView.IsMine)
                PhotonNetwork.Destroy(this.gameObject);
        }

        public void OnDestroy()
        {
            CustomLogicMapBuiltin.DestroyMapObjectBuiltin(CustomLogicMapObjectBuiltin, true);
        }
    }
}
