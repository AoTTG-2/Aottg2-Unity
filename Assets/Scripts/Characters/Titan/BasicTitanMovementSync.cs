using Photon.Pun;
using UnityEngine;

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
                stream.SendNext(_titan.LateUpdateHeadRotation);
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
                _titan.LateUpdateHeadRotationRecv = (Quaternion?)stream.ReceiveNext();
            }
        }
    }
}