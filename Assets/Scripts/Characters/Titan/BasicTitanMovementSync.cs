using Photon.Pun;
using UnityEngine;
using Utility;

namespace Characters
{
    class BasicTitanMovementSync : BaseMovementSync
    {
        protected BasicTitan _titan;
        private int _lastAnimationId;
        private byte _lastAnimationSeq;

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
            stream.SendNext(_titan.SyncAnimationId);
            stream.SendNext(_titan.SyncAnimationSeq);
            stream.SendNext(_titan.SyncAnimationFadeTime);
            stream.SendNext(_titan.SyncAnimationStartTime);
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
                        _titan.Animation.CrossFade(animation, fadeTime, startTime);
                    else
                        _titan.Animation.Play(animation, startTime);
                }
            }
        }
    }
}