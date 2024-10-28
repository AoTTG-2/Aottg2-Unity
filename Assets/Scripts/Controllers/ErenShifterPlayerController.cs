using Settings;
using Characters;
using UnityEngine;
using GameManagers;
using UI;

namespace Controllers
{
    class ErenShifterPlayerController: BasePlayerController
    {
        protected ErenShifter _shifter;
        protected ErenShifterInputSettings _shifterInput;

        protected override void Awake()
        {
            base.Awake();
            _shifter = GetComponent<ErenShifter>();
            _shifterInput = SettingsManager.InputSettings.ErenShifter;
            _shifter.BaseTitanCache.HandLHitbox.ScaleSphereCollider(1.5f);
            _shifter.BaseTitanCache.HandRHitbox.ScaleSphereCollider(1.5f);
            _shifter.BaseTitanCache.FootLHitbox.ScaleSphereCollider(1.5f);
            _shifter.BaseTitanCache.FootRHitbox.ScaleSphereCollider(1.5f);
        }

        protected override void UpdateActionInput(bool inMenu)
        {
            base.UpdateActionInput(inMenu);
            if (inMenu)
                return;
            _shifter.IsWalk = _shifterInput.Walk.GetKey();
            if (_shifter.CanAction())
            {
                if (_shifterInput.Jump.GetKeyDown())
                {
                    if (_shifter.HasDirection)
                        _shifter.Jump(Vector3.up + _shifter.Cache.Transform.forward);
                    else
                        _shifter.Jump(Vector3.up);
                }
                else if (_shifterInput.Kick.GetKeyDown())
                    _shifter.Kick();
                else
                {
                    foreach (string settingName in _shifterInput.Settings.Keys)
                    {
                        if (settingName.StartsWith("Attack"))
                        {
                            if (((KeybindSetting)_shifterInput.Settings[settingName]).GetKeyDown())
                                _shifter.Attack(settingName);
                        }
                    }
                }
            }
        }
    }
}
