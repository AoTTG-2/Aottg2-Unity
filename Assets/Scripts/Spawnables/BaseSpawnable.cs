using Photon.Realtime;
using Settings;
using UnityEngine;

namespace Spawnables
{
    class BaseSpawnable: MonoBehaviour
    {
        protected Player _owner;
        protected float _timeLeft;
        protected bool _expires;

        public virtual void Setup(Player owner, float liveTime, object[] settings)
        {
            _owner = owner;
            _timeLeft = liveTime;
            _expires = _timeLeft > 0f;
            SetupSettings(settings);
        }

        protected virtual void SetupSettings(object[] settings)
        {
        }

        protected virtual void Update()
        {
            _timeLeft -= Time.deltaTime;
            if (_timeLeft <= 0f && _expires)
                Destroy(gameObject);
        }
    }
}
