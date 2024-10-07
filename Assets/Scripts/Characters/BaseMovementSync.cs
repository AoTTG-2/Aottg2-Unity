




using ApplicationManagers;
using Photon;
using Photon.Pun;
using System;
using UnityEngine;

namespace Characters
{
    public class BaseMovementSync : MonoBehaviourPun, IPunObservable
    {
        public bool Disabled;
        protected Vector3 _correctPosition = Vector3.zero;
        protected Quaternion _correctRotation = Quaternion.identity;
        public Vector3 _correctVelocity = Vector3.zero;
        public Quaternion _correctCamera = Quaternion.identity;
        protected bool _syncVelocity = false;
        protected bool _syncCamera = false;
        protected float SmoothingDelay => 10f;
        protected Transform _transform;
        protected Rigidbody _rigidbody;
        protected PhotonView _photonView;

        protected virtual void Awake()
        {
            _transform = transform;
            _photonView = photonView;
            _correctPosition = _transform.position;
            _correctRotation = transform.rotation;
            _rigidbody = GetComponent<Rigidbody>();
            var character = _transform.GetComponent<BaseCharacter>();
            if (_rigidbody != null)
            {
                _syncVelocity = true;
                _correctVelocity = _rigidbody.velocity;
            }
            if (character != null && !character.AI)
                _syncCamera = true;
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
                stream.SendNext(_transform.rotation);
                if (_syncVelocity)
                    stream.SendNext(_rigidbody.velocity);
                if (_syncCamera)
                    stream.SendNext(SceneLoader.CurrentCamera.Cache.Transform.rotation);
                SendCustomStream(stream);
            }
            else
            {
                _correctPosition = (Vector3)stream.ReceiveNext();
                _correctRotation = (Quaternion)stream.ReceiveNext();
                if (_syncVelocity)
                    _correctVelocity = (Vector3)stream.ReceiveNext();
                if (_syncCamera)
                    _correctCamera = (Quaternion)stream.ReceiveNext();
                ReceiveCustomStream(stream);
            }
        }

        protected virtual void Update()
        {
            if (!Disabled && !_photonView.IsMine)
            {
                _rigidbody.velocity = Vector3.zero;
                _transform.position = Vector3.Lerp(_transform.position, _correctPosition, Time.deltaTime * SmoothingDelay);
                _transform.rotation = Quaternion.Lerp(_transform.rotation, _correctRotation, Time.deltaTime * SmoothingDelay);
                if (_syncVelocity)
                    _correctPosition += _correctVelocity * Time.deltaTime;
            }
        }

        void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            OnPhotonSerializeView(stream, info);
        }
    }
}