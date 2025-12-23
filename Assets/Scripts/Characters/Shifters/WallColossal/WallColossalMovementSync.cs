using Photon.Pun;
using UnityEngine;
using Utility;

namespace Characters
{
    class WallColossalMovementSync : BaseMovementSync
    {
        protected WallColossalShifter _wallColossal;

        protected override void Awake()
        {
            base.Awake();
            _wallColossal = GetComponent<WallColossalShifter>();
        }

        protected override void SendCustomStream(PhotonStream stream)
        {
            // Only sync steam state - hand health/state uses RPCs like other shifters (Annie, Eren)
            stream.SendNext((byte)_wallColossal.SteamState);
        }

        protected override void ReceiveCustomStream(PhotonStream stream)
        {
            if (stream.PeekNext() is byte)
            {
                var receivedSteamState = (ColossalSteamState)stream.ReceiveNext();
                
                // Apply steam state if changed
                if (_wallColossal.SteamState != receivedSteamState)
                {
                    _wallColossal.ApplySteamState(receivedSteamState);
                }
            }
        }
    }
}
