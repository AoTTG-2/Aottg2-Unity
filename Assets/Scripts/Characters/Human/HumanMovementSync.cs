using ApplicationManagers;
using Photon.Pun;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using Utility;


namespace Characters
{
    class HumanMovementSync : BaseMovementSync
    {
        protected Human _human;
        protected override void Awake()
        {
            base.Awake();
            _human = GetComponent<Human>();
        }

        protected override void SendCustomStream(PhotonStream stream)
        {
            if (_human.LateUpdateHeadRotation.HasValue)
            {
                var rotation = _human.LateUpdateHeadRotation.Value;
                stream.SendNext(unchecked((int)QuaternionCompression.CompressQuaternion(ref rotation)));
            }
            else
                stream.SendNext(null);
        }

        protected override void ReceiveCustomStream(PhotonStream stream)
        {
            int? compressed = (int?)stream.ReceiveNext();
            if (compressed.HasValue)
            {
                var rotation = Quaternion.identity;
                QuaternionCompression.DecompressQuaternion(ref rotation, unchecked((uint)compressed.Value));
                _human.LateUpdateHeadRotationRecv = rotation;
            }
            else
                _human.LateUpdateHeadRotationRecv = null;
        }

        protected override void Update()
        {
            if (!Disabled && !_photonView.IsMine)
            {
                _rigidbody.velocity = Vector3.zero;
                if (_human.MountState == HumanMountState.MapObject && _human.MountedTransform != null)
                {
                    _transform.position = _human.MountedTransform.TransformPoint(_human.MountedPositionOffset);
                    _transform.rotation = Quaternion.Euler(_human.MountedTransform.rotation.eulerAngles + _human.MountedRotationOffset);
                }
                else if (_human.CarryState == HumanCarryState.Carry && _human.Carrier != null)
                {
                    Vector3 offset = _human.Carrier.Cache.Transform.forward * -0.4f + _human.Carrier.Cache.Transform.up * 0.5f;
                    _transform.position = _human.Carrier.Cache.Transform.position + offset;
                    _transform.rotation = _human.Carrier.Cache.Transform.rotation;
                }
                else
                {
                    _transform.position = Vector3.Lerp(_transform.position, _correctPosition, Time.deltaTime * SmoothingDelay);
                    _transform.rotation = Quaternion.Lerp(_transform.rotation, _correctRotation, Time.deltaTime * SmoothingDelay);
                    if(_human.BackHuman != null)
                        _human.CarryVelocity = _correctVelocity;
                    if (_timeSinceLastMessage < MaxPredictionTime)
                    {
                        _correctPosition += _correctVelocity * Time.deltaTime;
                        _timeSinceLastMessage += Time.deltaTime;
                    }
                }
            }
        }
    }
}