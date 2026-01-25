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
            stream.SendNext((byte)_wallColossal.SteamState);
            stream.SendNext((byte)_wallColossal.StunState);
        }

        protected override void ReceiveCustomStream(PhotonStream stream)
        {
            if (stream.PeekNext() is byte)
            {
                var receivedSteamState = (ColossalSteamState)stream.ReceiveNext();
                
                if (_wallColossal.SteamState != receivedSteamState)
                {
                    _wallColossal.ApplySteamState(receivedSteamState);
                }
            }
            
            if (stream.PeekNext() is byte)
            {
                var receivedStunState = (ColossalStunState)stream.ReceiveNext();
                
                if (_wallColossal.StunState != receivedStunState)
                {
                    _wallColossal.ApplyStunState(receivedStunState);
                }
            }
        }
    }
}
