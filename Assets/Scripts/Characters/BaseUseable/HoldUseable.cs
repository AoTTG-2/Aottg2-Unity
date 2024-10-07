using Settings;
using UnityEngine;

namespace Characters
{
    /// <summary>
    /// An extended useable that is triggered while button is held down and performs logic over extended update frames.
    /// </summary>
    class HoldUseable: ExtendedUseable
    {
        public HoldUseable(BaseCharacter owner): base(owner)
        {
        }

        public override void OnFixedUpdate()
        {
            if (IsActive)
            {
                ActiveFixedUpdate();
            }
        }

        public override void SetInput(bool key)
        {
            if (key)
            {
                if (!IsActive && CanUse())
                {
                    IsActive = true;
                    _activeTimeLeft = ActiveTime;
                    Activate();
                    OnUse();
                }
            }
            else if (IsActive)
            {
                IsActive = false;
                Deactivate();
            }
        }

        public override void ReadInput(KeybindSetting keybind)
        {
            SetInput(keybind.GetKey());
        }
    }
}
