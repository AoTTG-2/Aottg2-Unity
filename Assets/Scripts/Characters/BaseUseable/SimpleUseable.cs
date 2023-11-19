using Settings;
using UnityEngine;

namespace Characters
{
    /// <summary>
    /// A simple useable that is only triggered on button down and performs all logic on activation.
    /// </summary>
    abstract class SimpleUseable: BaseUseable
    {
        public SimpleUseable(BaseCharacter owner): base(owner)
        {
        }

        public override void ReadInput(KeybindSetting keybind)
        {
            SetInput(keybind.GetKeyDown());
        }

        public override void SetInput(bool key)
        {
            if (key && CanUse())
            {
                Activate();
                OnUse();
            }
        }
    }
}
