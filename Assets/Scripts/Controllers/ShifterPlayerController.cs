using Settings;
using Characters;
using UnityEngine;
using GameManagers;
using UI;

namespace Controllers
{
    class ShifterPlayerController: BasePlayerController
    {
        protected BaseShifter _shifter;
        protected ShifterInputSettings _shifterInput;

        protected override void Awake()
        {
            base.Awake();
            _shifter = GetComponent<BaseShifter>();
            _shifterInput = SettingsManager.InputSettings.Shifter;
        }

        protected override void UpdateActionInput(bool inMenu)
        {
            base.UpdateActionInput(inMenu);
            if (inMenu)
                return;
            _shifter.IsWalk = _shifterInput.Walk.GetKey();
            if (_shifter.CanAction())
            {
                if (_shifterInput.Attack.GetKeyDown())
                    _shifter.Attack("AttackCombo");
                else if (_shifterInput.Jump.GetKeyDown())
                {
                    if (_shifter.HasDirection)
                        _shifter.Jump(Vector3.up + _shifter.Cache.Transform.forward);
                    else
                        _shifter.Jump(Vector3.up);
                }
                else if (_shifterInput.Kick.GetKeyDown())
                    _shifter.Kick();
            }
        }
    }
}
