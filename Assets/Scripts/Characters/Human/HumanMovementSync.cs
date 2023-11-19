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
            base.Update();
            if (!Disabled && !_photonView.IsMine && _human.MountState == HumanMountState.MapObject && _human.MountedTransform != null)
            {
                _transform.position = _human.MountedTransform.TransformPoint(_human.MountedPositionOffset);
                _transform.rotation = Quaternion.Euler(_human.MountedTransform.rotation.eulerAngles + _human.MountedRotationOffset);
                _rigidbody.velocity = Vector3.zero;
            }
        }
    }
}