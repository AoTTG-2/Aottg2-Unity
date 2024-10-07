using System.Collections;
using UnityEngine;

namespace Utility
{
    class RateLimit
    {
        private int _currentUsage;
        private int _maxUsage;
        private float _resetDelay;
        private float _lastResetTime;

        public RateLimit Copy()
        {
            return new RateLimit(_maxUsage, _resetDelay);
        }

        public RateLimit(int maxUsage, float resetDelay)
        {
            _currentUsage = 0;
            _lastResetTime = Time.realtimeSinceStartup;
            _maxUsage = maxUsage;
            _resetDelay = resetDelay;
        }

        public bool Peek(int usage = 1)
        {
            TryReset();
            if (_currentUsage + usage <= _maxUsage)
            {
                return true;
            }
            return false;
        }

        public bool Use(int usage = 1)
        {
            if (Peek(usage))
            {
                _currentUsage += usage;
                return true;
            }
            return false;
        }

        private void TryReset()
        {
            if (Time.realtimeSinceStartup >= _lastResetTime + _resetDelay)
            {
                _currentUsage = 0;
                _lastResetTime = Time.realtimeSinceStartup;
            }
        }
    }
}
