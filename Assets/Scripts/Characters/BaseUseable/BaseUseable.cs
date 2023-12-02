using Settings;
using UnityEngine;

namespace Characters
{
    /// <summary>
    /// Base class for useables (items, weapons, hooks, specials)
    /// </summary>
    abstract class BaseUseable
    {
        public float Cooldown;
        public int UsesLeft;
        public int MaxUses;
        public bool IsActive;
        public string Name;
        protected float _lastUseTime = -1000f;
        protected BaseCharacter _owner;

        public BaseUseable(BaseCharacter owner, float cooldown = 0f, int maxUses = -1)
        {
            _owner = owner;
            Cooldown = cooldown;
            UsesLeft = MaxUses = maxUses;
        }

        public virtual void Reload()
        {
        }

        public virtual void Reset()
        {
            UsesLeft = MaxUses;
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
            if (!HasUsesLeft())
                return 0f;
            if (Cooldown == 0f)
                return 1f;
            return Mathf.Clamp((Time.time - _lastUseTime) / Cooldown, 0f, 1f);
        }

        public bool HasUsesLeft()
        {
            return UsesLeft != 0;
        }

        public float GetUsesRatio()
        {
            if (MaxUses <= 0 || UsesLeft < 0)
                return 0f;
            return UsesLeft / MaxUses;
        }

        public virtual bool CanUse()
        {
            return (Time.time - _lastUseTime) >= Cooldown && (UsesLeft == -1 || UsesLeft > 0);
        }

        protected virtual void OnUse()
        {
            if (UsesLeft > 0)
                UsesLeft--;
            _lastUseTime = Time.time;
        }

        protected virtual void Activate()
        {
        }

        protected virtual void Deactivate()
        {
        }

        public virtual void ReadInput(KeybindSetting keybind)
        {
        }

        public virtual void SetInput(bool key)
        {
        }

        public virtual void OnFixedUpdate()
        {
        }
    }
}
