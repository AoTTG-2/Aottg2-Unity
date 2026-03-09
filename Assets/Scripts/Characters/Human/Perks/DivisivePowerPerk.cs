using Characters;
using UnityEngine;

namespace Assets.Scripts.Characters.Human.Perks
{
    class DivisivePowerPerk : BasePerk
    {
        public bool PerkEnabled => CurrPoints == MaxPoints;
        
        protected virtual float MaxPower => 1f;
        protected virtual float MinPower => 0f;
        protected virtual float PowerUsageDivisor => 2f;
        protected virtual float LinearRecoveryRate => 0.1f;
        
        protected float _currentPower;
        protected float _lastUpdateTime;
        
        protected virtual float Cooldown => 0f;
        protected float _lastUseTime = -1000f;
        
        public DivisivePowerPerk()
        {
            _currentPower = MaxPower;
            _lastUpdateTime = Time.time;
        }
        
        public virtual void Reset()
        {
            _currentPower = MaxPower;
            _lastUpdateTime = Time.time;
        }
        
        public float GetCurrentPower()
        {
            UpdatePower();
            return _currentPower;
        }
        
        public void SetCurrentPower(float power)
        {
            _currentPower = Mathf.Clamp(power, MinPower, MaxPower);
            _lastUpdateTime = Time.time;
        }
        
        public void RecoverPower(float amount)
        {
            _currentPower = Mathf.Clamp(_currentPower + amount, MinPower, MaxPower);
            _lastUpdateTime = Time.time;
        }
        
        public void RecoverPowerBySeconds(float seconds)
        {
            if (seconds > 0f)
            {
                float recovery = GetRecoveryAmount(seconds);
                _currentPower = Mathf.Clamp(_currentPower + recovery, MinPower, MaxPower);
                _lastUpdateTime = Time.time;
            }
        }
        
        public float GetPowerRatio()
        {
            UpdatePower();
            if (MaxPower == MinPower)
                return 1f;
            return (_currentPower - MinPower) / (MaxPower - MinPower);
        }
        
        protected void UpdatePower()
        {
            float deltaTime = Time.time - _lastUpdateTime;
            if (deltaTime > 0f && _currentPower < MaxPower)
            {
                float recovery = GetRecoveryAmount(deltaTime);
                _currentPower = Mathf.Clamp(_currentPower + recovery, MinPower, MaxPower);
                _lastUpdateTime = Time.time;
            }
        }
        
        protected virtual float GetRecoveryAmount(float deltaTime)
        {
            return LinearRecoveryRate * deltaTime;
        }
        
        public virtual bool CanUse()
        {
            UpdatePower();
            bool hasPower = _currentPower > MinPower;
            bool cooldownReady = Cooldown == 0f || (Time.time - _lastUseTime) >= Cooldown;
            return hasPower && cooldownReady;
        }
        
        public virtual void OnUse()
        {
            UpdatePower();
            _currentPower = Mathf.Clamp(_currentPower / PowerUsageDivisor, MinPower, MaxPower);
            _lastUseTime = Time.time;
            _lastUpdateTime = Time.time;
        }

        public virtual void OnUse(float percent)
        {
            UpdatePower();
            _currentPower = Mathf.Clamp(_currentPower - (MaxPower * percent), MinPower, MaxPower);
            _lastUseTime = Time.time;
            _lastUpdateTime = Time.time;
        }

        public void SetCooldownLeft(float cooldownLeft)
        {
            _lastUseTime = Time.time - Cooldown + cooldownLeft;
        }
        
        public float GetCooldownLeft()
        {
            return Mathf.Clamp(Cooldown - (Time.time - _lastUseTime), 0f, Cooldown);
        }
        
        public float GetCooldownRatio()
        {
            if (Cooldown == 0f)
                return 1f;
            return Mathf.Clamp((Time.time - _lastUseTime) / Cooldown, 0f, 1f);
        }
    }
}
