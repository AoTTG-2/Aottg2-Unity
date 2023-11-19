using Settings;
using UnityEngine;

namespace Characters
{
    /// <summary>
    /// A useable that is only triggered on button down and performs logic over extended update frames.
    /// </summary>
    abstract class ExtendedUseable : BaseUseable
    {
        protected virtual float ActiveTime => 0f;
        protected float _activeTimeLeft;

        public ExtendedUseable(BaseCharacter owner) : base(owner)
        {
        }

        public override void ReadInput(KeybindSetting keybind)
        {
            SetInput(keybind.GetKeyDown());
        }

        public override void SetInput(bool key)
        {
            if (key && CanUse() && !IsActive)
            {
                IsActive = true;
                _activeTimeLeft = ActiveTime;
                Activate();
                OnUse();
            }
        }

        public override void OnFixedUpdate()
        {
            if (IsActive)
            {
                _activeTimeLeft -= Time.fixedDeltaTime;
                if (_activeTimeLeft <= 0f)
                {
                    IsActive = false;
                    Deactivate();
                }
                else
                    ActiveFixedUpdate();
            }
        }

        protected virtual void ActiveFixedUpdate()
        {
        }
    }
}
