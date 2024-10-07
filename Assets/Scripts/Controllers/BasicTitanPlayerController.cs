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
            _titan.BasicCache.HandLHitbox.ScaleSphereCollider(1.5f);
            _titan.BasicCache.HandRHitbox.ScaleSphereCollider(1.5f);
            _titan.BasicCache.FootLHitbox.ScaleSphereCollider(1.5f);
            _titan.BasicCache.FootRHitbox.ScaleSphereCollider(1.5f);
            _titan.BasicCache.MouthHitbox.ScaleSphereCollider(1.2f);
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
                else if (_titanInput.CoverNape.GetKeyDown())
                    _titan.CoverNape();
                else if (_titanInput.Kick.GetKeyDown())
                    _titan.Attack("AttackKick");
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
        }
    }
}
