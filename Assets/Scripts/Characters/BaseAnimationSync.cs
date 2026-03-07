using Photon.Pun;

namespace Characters
{
    class BaseAnimationSync : MonoBehaviourPun, IPunObservable
    {
        protected BaseCharacter _character;
        private int _lastAnimationId;
        private byte _lastAnimationSeq;

        protected virtual void Awake()
        {
            _character = GetComponent<BaseCharacter>();
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
                stream.SendNext(_character.SyncAnimationId);
                stream.SendNext(_character.SyncAnimationSeq);
                stream.SendNext(_character.SyncAnimationFadeTime);
                stream.SendNext(_character.SyncAnimationStartTime);
                SendCustomStream(stream);
            }
            else
            {
                int animId = (int)stream.ReceiveNext();
                byte animSeq = (byte)stream.ReceiveNext();
                float fadeTime = (float)stream.ReceiveNext();
                float startTime = (float)stream.ReceiveNext();
                if (animSeq != _lastAnimationSeq && animId != 0)
                {
                    _lastAnimationId = animId;
                    _lastAnimationSeq = animSeq;
                    if (NetworkAnimationId.TryGetName(animId, out string animation))
                    {
                        if (fadeTime > 0f)
                            _character.Animation.CrossFade(animation, fadeTime, startTime);
                        else
                            _character.Animation.Play(animation, startTime);
                    }
                }
                ReceiveCustomStream(stream);
            }
        }

        void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            OnPhotonSerializeView(stream, info);
        }
    }
}
