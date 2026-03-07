using Photon.Pun;

namespace Characters
{
    class HumanAnimationSync : BaseAnimationSync
    {
        private Human _human;
        private byte _lastVisualFlags;

        protected override void Awake()
        {
            base.Awake();
            _human = GetComponent<Human>();
        }

        protected override void SendCustomStream(PhotonStream stream)
        {
            stream.SendNext(_human.VisualFlags);
        }

        protected override void ReceiveCustomStream(PhotonStream stream)
        {
            byte flags = (byte)stream.ReceiveNext();
            if (flags != _lastVisualFlags)
            {
                _lastVisualFlags = flags;
                _human.ApplyVisualFlags(flags);
            }
        }
    }
}
