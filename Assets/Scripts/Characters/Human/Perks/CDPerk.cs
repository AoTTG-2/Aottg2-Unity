using Characters;
using UnityEngine;

namespace Assets.Scripts.Characters.Human.Perks
{
    class CDPerk : BasePerk
    {
        public bool PerkEnabled => CurrPoints == MaxPoints;
        public float Cooldown => 3.5f;
        protected float _lastUseTime = -1000f;

        public virtual void Reset()
        {
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

        public virtual bool CanUse()
        {
            return (Time.time - _lastUseTime) >= Cooldown;
        }

        public virtual void OnUse()
        {
            _lastUseTime = Time.time;
        }

    }
}
