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
            stream.SendNext(_titan.TargetViewId);
        }

        protected override void ReceiveCustomStream(PhotonStream stream)
        {
            _titan.TargetViewId = (int)stream.ReceiveNext();
        }
    }
}