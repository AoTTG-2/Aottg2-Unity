using Map;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using Utility;

namespace CustomLogic
{
    class CustomLogicPhotonSync : Photon.Pun.MonoBehaviourPunCallbacks, Photon.Pun.IPunObservable
    {
        public bool SyncTransforms = true;
        protected Vector3 _correctPosition = Vector3.zero;
        protected Quaternion _correctRotation = Quaternion.identity;
        protected Vector3 _correctVelocity = Vector3.zero;
        protected bool _syncVelocity = false;
        protected float SmoothingDelay => 5f;
        protected MapObject _mapObject;
        protected PhotonView _photonView;
        protected CustomLogicNetworkViewBuiltin _networkView;
        protected bool _inited = false;
        protected object[] _streamObjs;
        protected bool _synced = false;

        protected virtual void Awake()
        {
            _photonView = photonView;
        }

        public void Init(int mapObjectId, bool rigidbody)
        {
            _mapObject = MapLoader.IdToMapObject[mapObjectId];
            _photonView.RPC("InitRPC", RpcTarget.AllBuffered, new object[] { mapObjectId, rigidbody });
        }

        public void Init(int mapObjectId)
        {
            _mapObject = MapLoader.IdToMapObject[mapObjectId];
            bool rigidbody = _mapObject.GameObject.GetComponent<Rigidbody>() != null;
            _photonView.RPC("InitRPC", RpcTarget.AllBuffered, new object[] { mapObjectId, rigidbody });
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (_inited && SyncTransforms)
                _photonView.RPC("SyncRPC", newPlayer, new object[] { GetPosition(), GetRotation() });
        }

        [PunRPC]
        public void InitRPC(int mapObjectId, bool syncVelocity, PhotonMessageInfo info)
        {
            if (info.Sender != _photonView.Owner)
                return;
            _syncVelocity = syncVelocity;
            StartCoroutine(WaitAndFinishInit(mapObjectId));
        }

        [PunRPC]
        public void SyncRPC(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
        {
            if (info.Sender != _photonView.Owner)
                return;
            StartCoroutine(WaitAndFinishSync(position, rotation));
        }

        public IEnumerator WaitAndFinishInit(int mapObjectId)
        {
            while (CustomLogicManager.Evaluator == null || !CustomLogicManager.Evaluator.IdToNetworkView.ContainsKey(mapObjectId))
                yield return null;
            FinishInit(mapObjectId);
        }

        public IEnumerator WaitAndFinishSync(Vector3 position, Quaternion rotation)
        {
            while (!_inited)
                yield return null;
            _correctPosition = position;
            _correctRotation = rotation;
        }

        private void FinishInit(int mapObjectId)
        {
            _mapObject = MapLoader.IdToMapObject[mapObjectId];
            _networkView = CustomLogicManager.Evaluator.IdToNetworkView[mapObjectId];
            _networkView.SetSync(this);
            _correctPosition = _mapObject.GameObject.transform.position;
            _correctRotation = _mapObject.GameObject.transform.rotation;
            _inited = true;
        }

        [PunRPC]
        public void SendMessageRPC(string message, PhotonMessageInfo info)
        {
            var player = info.Sender;
            if (_networkView != null)
                _networkView.OnNetworkMessage(new CustomLogicPlayerBuiltin(player), message, info.SentServerTime);
        }

        public void SendMessage(Player player, string message)
        {
            _photonView.RPC("SendMessageRPC", player, new object[] { message });
        }

        public void SendMessageAll(string message)
        {
            _photonView.RPC("SendMessageRPC", RpcTarget.All, new object[] { message });
        }

        public void SendMessageOthers(string message)
        {
            _photonView.RPC("SendMessageRPC", RpcTarget.Others, new object[] { message });
        }

        protected virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                if (_inited && _mapObject.GameObject != null)
                {
                    if (SyncTransforms)
                    {
                        stream.SendNext(GetPosition());
                        var rotation = GetRotation();
                        stream.SendNext(QuaternionCompression.CompressQuaternion(ref rotation));
                        if (_syncVelocity)
                        {
                            stream.SendNext(GetVelocity());
                        }
                    }
                    _networkView.SendNetworkStream(stream);
                }
            }
            else
            {
                if (_inited)
                {
                    if (SyncTransforms)
                    {
                        _correctPosition = (Vector3)stream.ReceiveNext();
                        QuaternionCompression.DecompressQuaternion(ref _correctRotation, (int)stream.ReceiveNext());
                        if (_syncVelocity)
                            _correctVelocity = (Vector3)stream.ReceiveNext();
                    }
                    _streamObjs = (object[])stream.ReceiveNext();
                    if (_streamObjs != null && _streamObjs.Length > 0)
                        _networkView.OnNetworkStream(_streamObjs);
                }
            }
        }

        protected virtual void Update()
        {
            if (!_photonView.IsMine && _inited)
            {
                if (_mapObject.GameObject == null || !SyncTransforms)
                    return;
                var transform = _mapObject.GameObject.transform;
                transform.position = Vector3.Lerp(transform.position, _correctPosition, Time.deltaTime * SmoothingDelay);
                transform.rotation = Quaternion.Lerp(transform.rotation, _correctRotation, Time.deltaTime * SmoothingDelay);
                if (_syncVelocity)
                {
                    _mapObject.GameObject.GetComponent<Rigidbody>().velocity = _correctVelocity;
                }
            }
        }

        private Vector3 GetPosition()
        {
            if (_mapObject.GameObject != null)
                return _mapObject.GameObject.transform.position;
            return Vector3.zero;
        }

        private Quaternion GetRotation()
        {
            if (_mapObject.GameObject != null)
                return _mapObject.GameObject.transform.rotation;
            return Quaternion.identity;
        }

        private Vector3 GetVelocity()
        {
            if (_mapObject.GameObject != null)
                return _mapObject.GameObject.GetComponent<Rigidbody>().velocity;
            return Vector3.zero;
        }

        void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            OnPhotonSerializeView(stream, info);
        }
    }
}
