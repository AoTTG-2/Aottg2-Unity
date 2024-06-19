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
            _titan.JumpForce = 240f;
            _titan.AttackPause = 0.1f;
            _titan.ActionPause = 0.1f;
            _titan.RockThrow1Speed = 500f;
        }

        protected override void UpdateUI(bool inMenu)
        {
            CursorManager.SetCrosshairText(string.Empty);
            CursorManager.SetCrosshairColor(true);
        }

        protected override void UpdateActionInput(bool inMenu)
        {
            base.UpdateActionInput(inMenu);
            if (inMenu)
                return;
            _titan.IsWalk = _titanInput.Walk.GetKey();
            _titan.IsSit = _titanInput.Sit.GetKey();
            _titan.IsSprint = _titanInput.Sprint.GetKey();
            _enemyTimeLeft -= Time.deltaTime;
            if (_titan.CanAction())
            {
                if (_titanInput.Jump.GetKeyDown())
                    _titan.Attack("AttackJump");
                else if (_titanInput.AttackPunch.GetKeyDown())
                    _titan.Attack("AttackPunch");
                else if (_titanInput.AttackBody.GetKeyDown())
                    _titan.Attack("AttackBellyFlop");
                else if (_titanInput.Kick.GetKeyDown())
                    _titan.Attack("AttackKick");
                else if (_titanInput.AttackRockThrow.GetKeyDown())
                    _titan.Attack("AttackRockThrow");
                else if (_titanInput.AttackGrab.GetKeyDown())
                    AttackGrab();
                else if (_titanInput.AttackSlap.GetKeyDown())
                    AttackSlap();
            }
        }

        protected void AttackGrab()
        {
            float[] angles = GetAimAngles();
            float x = angles[0];
            float y = angles[1];
            string attack;
            if (x > -90f && x < 90f)
            {
                if (y > 15f)
                    attack = x < 0 ? "AttackGrabHighL" : "AttackGrabHighR";
                else if (y > -15f)
                    attack = x < 0 ? "AttackGrabAirFarL" : "AttackGrabAirFarR";
                else if (y > -45f)
                    attack = x < 0 ? "AttackGrabGroundFrontL" : "AttackGrabGroundFrontR";
                else
                    attack = x < 0 ? "AttackGrabCoreL" : "AttackGrabCoreR";
            }
            else
            {
                if (y > 0f)
                    attack = x < 0 ? "AttackGrabHeadBackL" : "AttackGrabHeadBackR";
                else
                    attack = x < 0 ? "AttackGrabGroundBackL" : "AttackGrabGroundBackR";
            }
            _titan.Attack(attack);
        }

        protected void AttackSlap()
        {
            float[] angles = GetAimAngles();
            float x = angles[0];
            float y = angles[1];
            string attack;
            if (y > 10f)
            {
                if (x > 0)
                    attack = "AttackSlapHighR";
                else
                    attack = "AttackSlapHighL";
            }
            else if (y < -20f)
            {
                if (x > 0)
                    attack = "AttackSlapLowR";
                else
                    attack = "AttackSlapLowL";
            }
            else
            {
                if (x > 0)
                    attack = "AttackSlapR";
                else
                    attack = "AttackSlapL";
            }
            _titan.Attack(attack);
        }
    }
}
