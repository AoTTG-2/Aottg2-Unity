using Settings;
using Characters;
using UnityEngine;
using GameManagers;
using UI;
using CustomLogic;
using SimpleJSONFixed;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Utility;
using ApplicationManagers;

namespace Controllers
{
    class BasicTitanPlayerController: BasePlayerController
    {
        protected BasicTitan _titan;
        protected TitanInputSettings _titanInput;
        protected float _enemyTimeLeft;
        protected float RockCooldown = 5f;
        protected float _rockCooldownLeft;

        protected override void Awake()
        {
            base.Awake();
            _titan = GetComponent<BasicTitan>();
            _titanInput = SettingsManager.InputSettings.Titan;
            _titan.RotateSpeed = 5f;
            _titan.RunSpeedBase = 20f;
            _titan.RunSpeedPerLevel = 20f;
            _titan.WalkSpeedBase = 5f;
            _titan.WalkSpeedPerLevel = 1f;
            _titan.BellyFlopTime = 2.6f;
            _titan.AttackSpeedMultiplier = 1.2f;
            _titan.JumpForce = 200f;
            _titan.AttackPause = 0.1f;
            _titan.ActionPause = 0.1f;
            _titan.RockThrow1Speed = 500f;
            _titan.BasicCache.HandLHitbox.ScaleSphereCollider(1.5f);
            _titan.BasicCache.HandRHitbox.ScaleSphereCollider(1.5f);
            _titan.BasicCache.FootLHitbox.ScaleSphereCollider(1.5f);
            _titan.BasicCache.FootRHitbox.ScaleSphereCollider(1.5f);
            _titan.BasicCache.MouthHitbox.ScaleSphereCollider(1.2f);
            _titan.AttackSpeeds["AttackGrabAirL"] = 0.7f;
        }

        protected override void UpdateUI(bool inMenu)
        {
            CursorManager.SetCrosshairText(string.Empty);
            CursorManager.SetCrosshairColor(true);
        }

        protected override void UpdateActionInput(bool inMenu)
        {
            _rockCooldownLeft -= Time.deltaTime;
            base.UpdateActionInput(inMenu);
            if (inMenu)
                return;
            _titan.IsWalk = _titanInput.Walk.GetKey();
            _titan.IsSit = _titanInput.Sit.GetKey();
            _titan.IsSprint = _titanInput.Sprint.GetKey();
            _enemyTimeLeft -= Time.deltaTime;
            if (_titanInput.CoverNape1.GetKeyUp())
                _titan.UncoverNape();
            if (_titan.CanAction())
            {
                if (_titanInput.Jump.GetKeyDown())
                {
                    _titan.JumpForce = 200f;
                    _titan.Attack("AttackJump");
                }
                else if (_titanInput.CoverNape1.GetKeyDown())
                    _titan.CoverNape();
                else if (_titanInput.Kick.GetKeyDown())
                    _titan.Attack("AttackKick");
                else if (_titanInput.AttackSwing.GetKeyDown())
                    AttackSwing();
                else if (_titanInput.AttackGrabAirFar.GetKeyDown())
                    AttackGrabAirFar();
                else if (_titanInput.AttackGrabAir.GetKeyDown())
                    AttackGrabAir();
                else if (_titanInput.AttackGrabBody.GetKeyDown())
                    AttackGrabBody();
                else if (_titanInput.AttackGrabCore.GetKeyDown())
                    AttackGrabCore();
                else if (_titanInput.AttackGrabGround.GetKeyDown())
                    AttackGrabGround();
                else if (_titanInput.AttackGrabHead.GetKeyDown())
                    AttackGrabHead();
                else if (_titanInput.AttackGrabHigh.GetKeyDown())
                    AttackGrabHigh();
                else if (_titanInput.AttackBrushChest.GetKeyDown())
                    AttackBrushChest();
                else if (_titanInput.AttackRockThrow.GetKeyDown())
                {
                    if (_rockCooldownLeft <= 0f && _titan.Grounded)
                    {
                        _titan.Attack("AttackRockThrow");
                        _rockCooldownLeft = RockCooldown;
                    }
                }
                else
                {
                    foreach (string settingName in _titanInput.Settings.Keys)
                    {
                        if (settingName.StartsWith("Attack"))
                        {
                            if (((KeybindSetting)_titanInput.Settings[settingName]).GetKeyDown())
                                _titan.Attack(settingName);
                        }
                    }
                }
            }
            else if (_titan.State == TitanState.PreJump && _titanInput.Jump.GetKeyUp())
            {
                _titan._jumpDirection = Vector3.up;
                _titan.JumpForce = 120f;
                if (_titan.HasDirection)
                {
                    _titan._jumpDirection += _titan.Cache.Transform.forward * 0.5f;
                    _titan._jumpDirection.Normalize();
                }
                _titan.JumpImmediate();
            }
        }

        protected void AttackSwing()
        {
            float x = GetAimAngles()[0];
            string attack;
            if (x > 0)
                attack = "AttackSwingL";
            else
                attack = "AttackSwingR";
            _titan.Attack(attack);
        }

        protected void AttackGrabAirFar()
        {
            float x = GetAimAngles()[0];
            string attack;
            if (x > 0)
                attack = "AttackGrabAirFarR";
            else
                attack = "AttackGrabAirFarL";
            _titan.Attack(attack);
        }

        protected void AttackGrabAir()
        {
            float x = GetAimAngles()[0];
            string attack;
            if (x > 0)
                attack = "AttackGrabAirR";
            else
                attack = "AttackGrabAirL";
            _titan.Attack(attack);
        }

        protected void AttackGrabBody()
        {
            float x = GetAimAngles()[0];
            string attack;
            if (x > 0 && x <= 90)
                attack = "AttackGrabStomachR";
            else if (x > 90)
                attack = "AttackGrabBackR";
            else if (x < 0 && x >= -90)
                attack = "AttackGrabStomachL";
            else
                attack = "AttackGrabBackL";
            _titan.Attack(attack);
        }

        protected void AttackGrabCore()
        {
            float x = GetAimAngles()[0];
            string attack;
            if (x > 0)
                attack = "AttackGrabCoreR";
            else
                attack = "AttackGrabCoreL";
            _titan.Attack(attack);
        }

        protected void AttackGrabGround()
        {
            float x = GetAimAngles()[0];
            string attack;
            if (x > 0 && x <= 90)
                attack = "AttackGrabGroundFrontR";
            else if (x > 90)
                attack = "AttackGrabGroundBackR";
            else if (x < 0 && x >= -90)
                attack = "AttackGrabGroundFrontL";
            else
                attack = "AttackGrabGroundBackL";
            _titan.Attack(attack);
        }

        protected void AttackGrabHead()
        {
            float x = GetAimAngles()[0];
            string attack;
            if (x > 0 && x <= 90)
                attack = "AttackGrabHeadFrontR";
            else if (x > 90)
                attack = "AttackGrabHeadBackR";
            else if (x < 0 && x >= -90)
                attack = "AttackGrabHeadFrontL";
            else
                attack = "AttackGrabHeadBackL";
            _titan.Attack(attack);
        }

        protected void AttackGrabHigh()
        {
            float x = GetAimAngles()[0];
            string attack;
            if (x > 0)
                attack = "AttackGrabHighR";
            else
                attack = "AttackGrabHighL";
            _titan.Attack(attack);
        }

        protected void AttackBrushChest()
        {
            float x = GetAimAngles()[0];
            string attack;
            if (x > 0)
                attack = "AttackBrushChestL";
            else
                attack = "AttackBrushChestR";
            _titan.Attack(attack);
        }
    }
}
