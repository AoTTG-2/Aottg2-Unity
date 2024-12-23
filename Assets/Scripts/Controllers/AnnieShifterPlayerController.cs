using Settings;
using Characters;
using UnityEngine;
using GameManagers;
using UI;

namespace Controllers
{
    class AnnieShifterPlayerController: BasePlayerController
    {
        protected AnnieShifter _shifter;
        protected AnnieShifterInputSettings _shifterInput;

        protected override void Awake()
        {
            base.Awake();
            _shifter = GetComponent<AnnieShifter>();
            _shifterInput = SettingsManager.InputSettings.AnnieShifter;
            _shifter.BaseTitanCache.HandLHitbox.ScaleSphereCollider(1.5f);
            _shifter.BaseTitanCache.HandRHitbox.ScaleSphereCollider(1.5f);
            _shifter.BaseTitanCache.FootLHitbox.ScaleSphereCollider(1.5f);
            _shifter.BaseTitanCache.FootRHitbox.ScaleSphereCollider(1.5f);
            _shifter.BaseTitanCache.MouthHitbox.ScaleSphereCollider(1.5f);
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
                else if (_shifterInput.AttackBrushFront.GetKeyDown())
                    AttackBrushFront();
                else if (_shifterInput.AttackBrushHead.GetKeyDown())
                    AttackBrushHead();
                else if (_shifterInput.AttackGrabBottom.GetKeyDown())
                    AttackGrabBottom();
                else if (_shifterInput.AttackGrabMid.GetKeyDown())
                    AttackGrabMid();
                else if (_shifterInput.AttackGrabUp.GetKeyDown())
                    AttackGrabUp();
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

        protected void AttackBrushFront()
        {
            float x = GetAimAngles()[0];
            string attack;
            if (x > 0)
                attack = "AttackBrushFrontR";
            else
                attack = "AttackBrushFrontL";
            _shifter.Attack(attack);
        }

        protected void AttackBrushHead()
        {
            float x = GetAimAngles()[0];
            string attack;
            if (x > 0)
                attack = "AttackBrushHeadR";
            else
                attack = "AttackBrushHeadL";
            _shifter.Attack(attack);
        }

        protected void AttackGrabBottom()
        {
            float x = GetAimAngles()[0];
            string attack;
            if (x > 0)
                attack = "AttackGrabBottomRight";
            else
                attack = "AttackGrabBottomLeft";
            _shifter.Attack(attack);
        }

        protected void AttackGrabMid()
        {
            float x = GetAimAngles()[0];
            string attack;
            if (x > 0)
                attack = "AttackGrabMidRight";
            else
                attack = "AttackGrabMidLeft";
            _shifter.Attack(attack);
        }

        protected void AttackGrabUp()
        {
            float x = GetAimAngles()[0];
            string attack;
            if (x > 0)
                attack = "AttackGrabUpRight";
            else
                attack = "AttackGrabUpLeft";
            _shifter.Attack(attack);
        }
    }
}
