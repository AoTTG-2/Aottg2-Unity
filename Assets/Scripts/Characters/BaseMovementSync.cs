




using ApplicationManagers;
using Photon;
using Photon.Pun;
using System;
using UI;
using UnityEngine;
using Utility;

namespace Characters
{
    class BaseMovementSync : MonoBehaviourPun, IPunObservable
    {
        public bool Disabled;
        protected Vector3 _correctPosition = Vector3.zero;
        protected Quaternion _correctRotation = Quaternion.identity;
        public Vector3 _correctVelocity = Vector3.zero;
        public Quaternion _correctCamera = Quaternion.identity;
        protected bool _syncVelocity = false;
        protected bool _syncCamera = false;
        protected float SmoothingDelay => 10f;
        protected float MaxPredictionTime = 0.5f;
        protected Transform _transform;
        protected Rigidbody _rigidbody;
        protected PhotonView _photonView;
        protected BaseCharacter _character;
        protected float _timeSinceLastMessage = 0f;

        protected virtual void Awake()
        {
            _transform = transform;
            _photonView = photonView;
            _correctPosition = _transform.position;
            _correctRotation = transform.rotation;
            _rigidbody = GetComponent<Rigidbody>();
            _character = GetComponent<BaseCharacter>();
            if (_rigidbody != null)
            {

                _syncVelocity = true;
                _correctVelocity = _rigidbody.velocity;
            }
        }

        protected virtual void SendCustomStream(PhotonStream stream)
        {
        }

        protected virtual void ReceiveCustomStream(PhotonStream stream)
        {
        }

        protected virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_transform.position);
                var rotation = _transform.rotation;
                stream.SendNext(QuaternionCompression.CompressQuaternion(ref rotation));
                if (_syncVelocity)
                    stream.SendNext(_rigidbody.velocity);
                if (!_character.AI)
                {
                    if (((InGameMenu)UIManager.CurrentMenu)._spectateCount > 0)
                    {
                        var camRotation = SceneLoader.CurrentCamera.Cache.Transform.rotation;
                        stream.SendNext(QuaternionCompression.CompressQuaternion(ref camRotation));
                    }
                    else
                        stream.SendNext(null);
                }
                SendCustomStream(stream);
            }
            else
            {
                _correctPosition = (Vector3)stream.ReceiveNext();
                QuaternionCompression.DecompressQuaternion(ref _correctRotation, (int)stream.ReceiveNext());
                if (_syncVelocity)
                    _correctVelocity = (Vector3)stream.ReceiveNext();
                if (!_character.AI)
                {
                    int? compressed = (int?)stream.ReceiveNext();
                    if (compressed.HasValue)
                        QuaternionCompression.DecompressQuaternion(ref _correctCamera, compressed.Value);
                }
                ReceiveCustomStream(stream);
                _timeSinceLastMessage = 0f;
            }
        }

        protected virtual void Update()
        {
            if (!Disabled && !_photonView.IsMine)
            {
                _transform.position = Vector3.Lerp(_transform.position, _correctPosition, Time.deltaTime * SmoothingDelay);
                _transform.rotation = Quaternion.Lerp(_transform.rotation, _correctRotation, Time.deltaTime * SmoothingDelay);
                if (_syncVelocity && _timeSinceLastMessage < MaxPredictionTime)
                {
                    _correctPosition += _correctVelocity * Time.deltaTime;
                    _timeSinceLastMessage += Time.deltaTime;
                }
            }
        }

        void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            OnPhotonSerializeView(stream, info);
        }
    }
}