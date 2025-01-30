using Photon.Pun;
using UnityEngine;
using Utility;

namespace Characters
{
    class BasicTitanMovementSync : BaseMovementSync
    {
        protected BasicTitan _titan;

        protected override void Awake()
        {
            base.Awake();
            _titan = GetComponent<BasicTitan>();
        }

        protected override void SendCustomStream(PhotonStream stream)
        {
            if (_titan.AI)
            {
                stream.SendNext(_titan.TargetViewId);
                stream.SendNext(_titan.LookAtTarget);
            }
            else
            {
                var rotation = _titan.LateUpdateHeadRotation;
                if (rotation.HasValue)
                {
                    var rotationValue = rotation.Value;
                    stream.SendNext(QuaternionCompression.CompressQuaternion(ref rotationValue));
                }
                else
                    stream.SendNext(null);
            }
        }

        protected override void ReceiveCustomStream(PhotonStream stream)
        {
            if (_titan.AI)
            {
                _titan.TargetViewId = (int)stream.ReceiveNext();
                _titan.LookAtTarget = (bool)stream.ReceiveNext();
            }
            else
            {
                int? compressed = (int?)stream.ReceiveNext();
                if (compressed.HasValue)
                {
                    Quaternion recv = Quaternion.identity;
                    QuaternionCompression.DecompressQuaternion(ref recv, compressed.Value);
                    _titan.LateUpdateHeadRotationRecv = recv;
                }
                else
                    _titan.LateUpdateHeadRotationRecv = null;
            }
        }
    }
}