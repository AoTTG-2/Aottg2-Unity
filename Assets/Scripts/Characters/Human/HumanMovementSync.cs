using Photon.Pun;
using UnityEngine;

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
                else
                {
                    _transform.position = Vector3.Lerp(_transform.position, _correctPosition, Time.deltaTime * SmoothingDelay);
                    _transform.rotation = Quaternion.Lerp(_transform.rotation, _correctRotation, Time.deltaTime * SmoothingDelay);
                    _correctPosition += _correctVelocity * Time.deltaTime;
                }
            }
        }
    }
}