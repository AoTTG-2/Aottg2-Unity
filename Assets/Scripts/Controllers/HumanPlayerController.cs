using UnityEngine;
using ApplicationManagers;
using Settings;
using Characters;
using UI;
using System.Collections.Generic;
using Utility;
using Photon.Pun;

namespace Controllers
{
    class HumanPlayerController : BasePlayerController
    {
        public bool HideCursor;
        protected Human _human;
        protected float _reelOutScrollTimeLeft;
        protected float _reelInScrollCooldownLeft = 0f;
        protected float _reelInScrollCooldown = 0.2f;
        protected HumanInputSettings _humanInput;
        protected Dictionary<HumanDashDirection, KeybindSetting> _dashKeys;
        protected Dictionary<HumanDashDirection, float> _dashTimes;
        protected float _dashUpwardsTime = -1f; // added by Ata 21 May 24 for Double Tap Dash Upwards //
        protected static LayerMask HookMask = PhysicsLayer.GetMask(PhysicsLayer.TitanMovebox, PhysicsLayer.TitanPushbox,
            PhysicsLayer.MapObjectEntities, PhysicsLayer.MapObjectProjectiles, PhysicsLayer.MapObjectAll);

        private ZippsUIManager _zippsUIManager;

        protected override void Awake()
        {
            base.Awake();
            _human = GetComponent<Human>();
            _humanInput = SettingsManager.InputSettings.Human;
            SetupDash();
        }

        private void SetupDash()
        {
            GeneralInputSettings general = SettingsManager.InputSettings.General;
            _dashKeys = new Dictionary<HumanDashDirection, KeybindSetting>() {
                { HumanDashDirection.Forward, general.Forward },
                { HumanDashDirection.Back, general.Back },
                { HumanDashDirection.Left, general.Left },
                { HumanDashDirection.Right, general.Right }
            };
            _dashTimes = new Dictionary<HumanDashDirection, float>()
            {
                { HumanDashDirection.Forward, -1f },
                { HumanDashDirection.Back, -1f },
                { HumanDashDirection.Left, -1f },
                { HumanDashDirection.Right, -1f }
            };
        }

        protected override void Update()
        {
            if (!_human.FinishSetup)
                return;
            base.Update();
        }

        protected override void UpdateMovementInput(bool inMenu)
        {
            if (inMenu || _human.Dead || _human.State == HumanState.Stun)
            {
                _human.HasDirection = false;
                return;
            }
            _human.IsWalk = _humanInput.HorseWalk.GetKey();
            if (_human.MountState != HumanMountState.Horse)
            {
                if (_human.Grounded && _human.State != HumanState.Idle)
                    return;
                if (!_human.Grounded && (_human.State == HumanState.EmoteAction || _human.State == HumanState.SpecialAttack ||
                    _human.Cache.Animation.IsPlaying("dash") || _human.Cache.Animation.IsPlaying("jump") || _human.IsFiringThunderspear()))
                    return;
            }
            int forward = 0;
            int right = 0;
            if (_generalInput.Forward.GetKey())
                forward = 1;
            else if (_generalInput.Back.GetKey())
                forward = -1;
            if (_generalInput.Left.GetKey())
                right = -1;
            else if (_generalInput.Right.GetKey())
                right = 1;
            if (forward != 0 || right != 0)
            {
                _character.TargetAngle = GetTargetAngle(forward, right);
                _character.HasDirection = true;
                Vector3 v = new Vector3(right, 0f, forward);
                float magnitude = (v.magnitude <= 0.95f) ? ((v.magnitude >= 0.25f) ? v.magnitude : 0f) : 1f;
                _human.TargetMagnitude = magnitude;
            }
            else
            {
                _character.HasDirection = false;
                _human.TargetMagnitude = 0f;
            }
        }

        protected override void UpdateUI(bool inMenu)
        {
            Ray ray = SceneLoader.CurrentCamera.Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            string str = string.Empty;
            string distance = "???";
            float magnitude = 1000f;
            float speed = (_human.CarryState == HumanCarryState.Carry && _human.Carrier != null)
                ? _human.Carrier.CarryVelocity.magnitude
                : _human.Cache.Rigidbody.velocity.magnitude;
            if (Physics.Raycast(ray, out hit, 1000f, HookMask.value))
            {
                magnitude = (hit.point - _human.Cache.Transform.position).magnitude;
                distance = ((int)magnitude).ToString();
            }
            if (SettingsManager.UISettings.ShowCrosshairDistance.Value)
                str += distance;
            if (SettingsManager.UISettings.Speedometer.Value == (int)SpeedometerType.Speed)
            {
                if (str != string.Empty)
                    str += "\n";
                str += speed.ToString("F1") + " u/s";
            }
            else if (SettingsManager.UISettings.Speedometer.Value == (int)SpeedometerType.Damage)
            {
                if (str != string.Empty)
                    str += "\n";
                str += ((speed / 100f)).ToString("F1") + "K";
            }
            CursorManager.SetCrosshairText(str);
            if (magnitude > 120f)
                CursorManager.SetCrosshairColor(false);
            else
                CursorManager.SetCrosshairColor(true);
            if (SettingsManager.UISettings.ShowCrosshairArrows.Value)
            {
                Vector3 target = _human.GetAimPoint();
                float dist = Vector3.Distance(SceneLoader.CurrentCamera.Cache.Transform.position, target);
                float offset = dist <= 50f ? dist * 0.05f : dist * 0.3f;
                Vector3 leftTarget = target - offset * _human.Cache.Transform.right;
                Vector3 rightTarget = target + offset * _human.Cache.Transform.right;
                var leftPosition = SceneLoader.CurrentCamera.Camera.WorldToScreenPoint(leftTarget);
                leftPosition.z = 0f;
                var leftRotation = GetHookArrowRotation(true, leftPosition);
                CursorManager.SetHookArrow(true, leftPosition, leftRotation, Physics.Raycast(_human.Cache.Transform.position, (leftTarget - _human.Cache.Transform.position).normalized, 120f, HookMask.value));
                var rightPosition = SceneLoader.CurrentCamera.Camera.WorldToScreenPoint(rightTarget);
                rightPosition.z = 0f;
                var rightRotation = GetHookArrowRotation(false, rightPosition);
                CursorManager.SetHookArrow(false, rightPosition, rightRotation, Physics.Raycast(_human.Cache.Transform.position, (rightTarget - _human.Cache.Transform.position).normalized, 120f, HookMask.value));
            }
        }

        Quaternion GetHookArrowRotation(bool left, Vector3 position)
        {
            float x = position.x - Input.mousePosition.x;
            float y = position.y - Input.mousePosition.y;
            float z = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
            return Quaternion.Euler(0, 0, z);
        }

        void UpdateHookInput(bool inMenu)
        {
            if (inMenu)
                return;
            bool canHook = _human.State != HumanState.Grab && _human.State != HumanState.Stun && _human.CurrentGas > 0f 
                && _human.MountState != HumanMountState.MapObject && !_human.Dead;
            bool hookBoth = _humanInput.HookBoth.GetKey();
            bool hookLeft = _humanInput.HookLeft.GetKey();
            bool hookRight = _humanInput.HookRight.GetKey();
            bool hasHook = _human.HookLeft.HasHook() || _human.HookRight.HasHook();
            if (_human.CancelHookBothKey)
            {
                if (hookBoth)
                    hookBoth = false;
                else
                    _human.CancelHookBothKey = false;
            }
            if (_human.CancelHookLeftKey)
            {
                if (hookLeft)
                    hookLeft = false;
                else
                    _human.CancelHookLeftKey = false;
            }
            if (_human.CancelHookRightKey)
            {
                if (hookRight)
                    hookRight = false;
                else
                    _human.CancelHookRightKey = false;
            }
            _human.HookLeft.HookBoth = hookBoth && !hookLeft;
            _human.HookRight.HookBoth = hookBoth && !hookRight;
            _human.HookLeft.SetInput(canHook && (hookLeft || (hookBoth && (_human.HookLeft.IsHooked() || !hasHook))));
            _human.HookRight.SetInput(canHook && (hookRight || (hookBoth && (_human.HookRight.IsHooked() || !hasHook))));
            if (_human.CurrentGas <= 0f && (hookLeft || hookRight || hookBoth))
            {
                if (_humanInput.HookLeft.GetKeyDown() || _humanInput.HookRight.GetKeyDown() || _humanInput.HookBoth.GetKeyDown())
                _human.PlaySoundRPC(HumanSounds.NoGas, Util.CreateLocalPhotonInfo());
            }
            // TestScore();
        }

        private void TestScore()
        {
            if (_humanInput.HookLeft.GetKeyDown())
            {
                _inGameMenu.ShowKillFeed("test", "test", 100, "Thunderspear");
                _inGameMenu.ShowKillScore(100);
            }
            if (_humanInput.HookRight.GetKeyDown())
            {
                _inGameMenu.ShowKillFeed("test", "test", 3000, "Thunderspear");
                _inGameMenu.ShowKillScore(3000);
            }
        }

        protected override void UpdateActionInput(bool inMenu)
        {
            base.UpdateActionInput(inMenu);
            UpdateHookInput(inMenu);
            UpdateReelInput(inMenu);
            UpdateDashInput(inMenu);
            UpdateDashUpwardsInput(inMenu);
            UpdateDashDownwardsInput(inMenu);
            if (!inMenu)
            {
                if (SettingsManager.InputSettings.General.HideCursor.GetKeyDown())
                    HideCursor = !HideCursor;
            }

            
            var states = new HashSet<HumanState>() { HumanState.Grab, HumanState.SpecialAction, HumanState.EmoteAction, HumanState.Reload,
            HumanState.SpecialAttack, HumanState.Stun};
            bool canWeapon = (_human.MountState == HumanMountState.None /*|| _human.MountState == HumanMountState.Horse*/) && !states.Contains(_human.State) && !inMenu && !_human.Dead;
            var attackInput = _humanInput.AttackDefault;
            var specialInput = _humanInput.AttackSpecial;
            if (_human.Weapon is ThunderspearWeapon && SettingsManager.InputSettings.Human.SwapTSAttackSpecial.Value)
            {
                attackInput = _humanInput.AttackSpecial;
                specialInput = _humanInput.AttackDefault;
            }
            _human._gunArmAim = false;
            if (canWeapon)
            {
                if (_human.Weapon is AmmoWeapon && ((AmmoWeapon)_human.Weapon).RoundLeft == 0 && 
                    !(_human.Weapon is ThunderspearWeapon && ((ThunderspearWeapon)_human.Weapon).HasActiveProjectile()))
                {
                    if (attackInput.GetKeyDown() && _human.State == HumanState.Idle)
                        _human.Reload();
                }
                else
                {
                    if (_human.Weapon is AHSSWeapon)
                    {
                        bool isClick = attackInput.Contains(KeyCode.Mouse0);
                        if (isClick && _inGameMenu.SkipAHSSInput)
                            _inGameMenu.SkipAHSSInput = false;
                        else if (attackInput.GetKeyUp())
                        {
                            _human.Weapon.SetInput(true);
                            _inGameMenu.SkipAHSSInput = false;
                        }
                        else
                            _human.Weapon.SetInput(false);
                        _human._gunArmAim = attackInput.GetKey() || _human.Weapon.IsActive;
                    }
                    else
                        _human.Weapon.ReadInput(attackInput);
                }
            }
            else
                _human.Weapon.SetInput(false);

            if (_human.Special != null)
            {
                if (_humanInput.Ability1.GetKeyDown() && _human.CurrentSpecial != SettingsManager.InGameCharacterSettings.Special.Value && SettingsManager.InGameCharacterSettings.Special.Value != "None") // added by Ata 20 May 2024 for Ability Wheel//
                {
                    _human.SwitchCurrentSpecial(SettingsManager.InGameCharacterSettings.Special.Value, 1);
                    PlayAbilitySelectSound();
                }

                    bool canSpecial = _human.MountState == HumanMountState.None && 
                    (_human.Special is EscapeSpecial || _human.Special is ShifterTransformSpecial || _human.State != HumanState.Grab)
                    && _human.CarryState != HumanCarryState.Carry && _human.State != HumanState.EmoteAction && _human.State != HumanState.SpecialAttack && !inMenu && !_human.Dead;
                if (canSpecial)
                    _human.Special.ReadInput(specialInput);
                else
                    _human.Special.SetInput(false);
            }
            
            if (_human.Special_2 != null && _humanInput.Ability2.GetKeyDown() && _human.CurrentSpecial != SettingsManager.InGameCharacterSettings.Special_2.Value && SettingsManager.InGameCharacterSettings.Special_2.Value != "None")
            {
                _human.SwitchCurrentSpecial(SettingsManager.InGameCharacterSettings.Special_2.Value, 2);
                PlayAbilitySelectSound();
            }

            if (_human.Special_3 != null && _humanInput.Ability3.GetKeyDown() && _human.CurrentSpecial != SettingsManager.InGameCharacterSettings.Special_3.Value && SettingsManager.InGameCharacterSettings.Special_3.Value != "None")
            {
                _human.SwitchCurrentSpecial(SettingsManager.InGameCharacterSettings.Special_3.Value, 3);
                PlayAbilitySelectSound();
            }

            if (inMenu || _human.Dead || _human.State == HumanState.Stun)
                return;
            if (_human.MountState == HumanMountState.None)
            {   
                if (_humanInput.HorseAutorun.GetKeyDown())
                {
                    PlayHorseAutoSwitchSoundFromKeybind();
                    EmVariables.HorseAutorun = !EmVariables.HorseAutorun; // added by Snake 26 May 2024 for Horse Auto Running Bug Fixing //
                } 
                if (_human.CanJump())
                {
                    if (_humanInput.Jump.GetKeyDown())
                        _human.Jump();
                    else if (_humanInput.HorseMount.GetKeyDown() && _human.Horse != null && _human.MountState == HumanMountState.None &&
                    Vector3.Distance(_human.Horse.Cache.Transform.position, _human.Cache.Transform.position) < 15f)
                        _human.MountHorse();
                    else if (_humanInput.Dodge.GetKeyDown())
                    {
                        if (_human.HasDirection)
                            _human.Dodge(_human.TargetAngle + 180f);
                        else
                            _human.Dodge(_human.TargetAngle);
                    } 
                }
                if (_human.State == HumanState.Idle)
                {
                    if (_humanInput.Reload.GetKeyDown())
                        _human.Reload();
                }
                if(_human.CarryState == HumanCarryState.Carry)
                {
                    if (_humanInput.HorseMount.GetKeyDown())
                        _human.Cache.PhotonView.RPC("UncarryRPC", RpcTarget.All, new object[0]);
                }
            }
            else if (_human.MountState == HumanMountState.Horse)
            {
                if (_humanInput.HorseAutorun.GetKeyDown())
                {
                    PlayHorseAutoSwitchSoundFromKeybind();
                    EmVariables.HorseAutorun = !EmVariables.HorseAutorun; // added by Snake 24 May 2024 for Horse Auto Running Key Input //
                } 
                else  
                if (_humanInput.HorseMount.GetKeyDown())
                    _human.Unmount(false);
                else if (_humanInput.HorseJump.GetKeyDown())
                    _human.Horse.Jump();
            }
        }
        
        private void PlayHorseAutoSwitchSoundFromKeybind() // added by Ata 20 May 2024 for Ability Wheel //
        {
            _zippsUIManager = FindFirstObjectByType<ZippsUIManager>();
            if ( _zippsUIManager != null )
            {
                _zippsUIManager.PlayHorseAutoSwitchSoundFromKeybind();
            }

        }

        private void PlayAbilitySelectSound() // added by Ata 20 May 2024 for Ability Wheel //
        {
            _zippsUIManager = FindFirstObjectByType<ZippsUIManager>();
            if ( _zippsUIManager != null )
            {
                _zippsUIManager.PlayAbilitySelectSoundFromKeybind();
            }

        }

        private void ToggleUI()
        {
            GameObject defaultMenu = GameObject.Find("DefaultMenu(Clone)");
            if (defaultMenu != null)
            {
                defaultMenu.GetComponent<Canvas>().enabled = !defaultMenu.GetComponent<Canvas>().enabled;
            }
        }



        void UpdateReelInput(bool inMenu)
        {
            _reelOutScrollTimeLeft -= Time.deltaTime;
            if (_reelOutScrollTimeLeft <= 0f)
                _human.ReelOutAxis = 0f;
            if (_humanInput.ReelIn.GetKey())
            {
                if (!_human._reelInWaitForRelease)
                    _human.ReelInAxis = -1f;
                _reelInScrollCooldownLeft = _reelInScrollCooldown;
            }
            else
            {
                bool hasScroll = false;
                _reelInScrollCooldownLeft -= Time.deltaTime;
                foreach (InputKey inputKey in _humanInput.ReelIn.InputKeys)
                {
                    if (inputKey.IsWheel())
                        hasScroll = true;
                }
                foreach (InputKey inputKey in _humanInput.ReelIn.InputKeys)
                {
                    if (inputKey.IsWheel())
                    {
                        if (_reelInScrollCooldownLeft <= 0f)
                            _human._reelInWaitForRelease = false;
                    }
                    else
                    {
                        if (!hasScroll || inputKey.GetKeyUp())
                            _human._reelInWaitForRelease = false;
                    }
                }
            }
            foreach (InputKey inputKey in _humanInput.ReelOut.InputKeys)
            {
                if (inputKey.GetKey())
                {
                    _human.ReelOutAxis = 1f;
                    if (inputKey.IsWheel())
                        _reelOutScrollTimeLeft = SettingsManager.InputSettings.Human.ReelOutScrollSmoothing.Value;
                }
            }
        }

        void UpdateDashInput(bool inMenu)
        {
            if (!_human.Grounded && _human.State != HumanState.AirDodge && _human.MountState == HumanMountState.None && _human.State != HumanState.Grab && _human.CarryState != HumanCarryState.Carry
                && _human.State != HumanState.Stun && _human.State != HumanState.EmoteAction && _human.State != HumanState.SpecialAction
                && !inMenu && !_human.Dead)
            {
                HumanDashDirection currentDirection = HumanDashDirection.None;
                if (_humanInput.Dash.GetKeyDown())
                {
                    foreach (HumanDashDirection direction in _dashKeys.Keys)
                    {
                        if (_dashKeys[direction].GetKey())
                        {
                            currentDirection = direction;
                            break;
                        }
                    }
                }
                if (SettingsManager.InputSettings.Human.DashDoubleTap.Value)
                {
                    foreach (HumanDashDirection direction in _dashKeys.Keys)
                    {
                        if (_dashTimes[direction] >= 0f)
                        {
                            _dashTimes[direction] += Time.deltaTime;
                            if (_dashTimes[direction] > 0.2f)
                                _dashTimes[direction] = -1f;
                        }
                        if (_dashKeys[direction].GetKeyDown())
                        {
                            if (_dashTimes[direction] == -1f)
                                _dashTimes[direction] = 0f;
                            else if (_dashTimes[direction] > 0f)
                                currentDirection = direction;
                        }
                    }
                }
                if (currentDirection != HumanDashDirection.None)
                    _human.Dash(GetDashAngle(currentDirection));
            }
        }

        float GetDashAngle(HumanDashDirection direction)
        {
            float angle = 0f;
            if (direction == HumanDashDirection.Forward)
                angle = GetTargetAngle(1, 0);
            else if (direction == HumanDashDirection.Back)
                angle = GetTargetAngle(-1, 0);
            else if (direction == HumanDashDirection.Right)
                angle = GetTargetAngle(0, 1);
            else if (direction == HumanDashDirection.Left)
                angle = GetTargetAngle(0, -1);
            return angle;
        }

        #region Dashing Upwards by Ata 2 May 2024
        void UpdateDashUpwardsInput(bool inMenu)
        {
            if (/*!_human.Grounded && */_human.State != HumanState.AirDodge && _human.MountState == HumanMountState.None && _human.State != HumanState.Grab && _human.CarryState != HumanCarryState.Carry
                && _human.State != HumanState.Stun && _human.State != HumanState.EmoteAction && _human.State != HumanState.SpecialAction
                && !inMenu && !_human.Dead)
            {
                if (_humanInput.DashUpwards.GetKeyDown())
                {
                    _human.DashUpwards();
                }
                if (SettingsManager.InputSettings.Human.DashUpwardsDoubleTap.Value)
                {
                    if (_dashUpwardsTime >= 0f)
                    {
                        _dashUpwardsTime += Time.deltaTime;
                        if (_dashUpwardsTime > 0.2f)
                            _dashUpwardsTime = -1f;
                    }
                    if (_humanInput.Jump.GetKeyDown())
                    {
                        if (_dashUpwardsTime == -1f)
                            _dashUpwardsTime = 0f;
                        else if (_dashUpwardsTime > 0f)
                            _human.DashUpwards();
                    }
                }
            }
        }
        #endregion

        #region Dashing Downwards by Ata 2 May 2024
        void UpdateDashDownwardsInput(bool inMenu)
        {
            if (!_human.Grounded && _human.State != HumanState.AirDodge && _human.MountState == HumanMountState.None && _human.State != HumanState.Grab && _human.CarryState != HumanCarryState.Carry
                && _human.State != HumanState.Stun && _human.State != HumanState.EmoteAction && _human.State != HumanState.SpecialAction
                && !inMenu && !_human.Dead)
            {
                if (_humanInput.DashDownwards.GetKeyDown())
                {
                    _human.DashDownwards();
                }
            }
        }
        #endregion
    }
}
