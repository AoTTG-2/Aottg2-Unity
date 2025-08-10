using UnityEngine;
using ApplicationManagers;
using Settings;
using Characters;
using UI;
using System.Collections.Generic;
using Utility;
using Map;
using Photon.Pun;
using UnityEditor.UI;

namespace Controllers
{
    class HumanAIController : BaseAIController, IHumanController
    {

        protected Human _human;
        public float DetectRange = 10000f;
        protected static LayerMask HookMask = PhysicsLayer.GetMask(PhysicsLayer.TitanMovebox, PhysicsLayer.TitanPushbox,
            PhysicsLayer.MapObjectEntities, PhysicsLayer.MapObjectProjectiles, PhysicsLayer.MapObjectAll);

        protected float _movingLeft = 0;
        protected Vector3? MoveDirection;
        protected Vector3 AimDirection;
        protected Vector3 AimPoint;

        protected bool IsWalk = false;
        protected bool _usingGas = false;
        protected bool _hookingLeft = false;
        protected bool _hookingRight = false;

        protected ITargetable _target;

        public ITargetable Target
        {
            get
            {
                return _target;
            }
            set
            {
                _target = value;
                if (_target != null)
                {
                    TargetVelocity = Vector3.zero;
                    _targetLastPosition = _target.GetPosition();
                }
            }
        }

        public Vector3 TargetPosition;
        public Vector3 TargetDirection;

        protected Vector3? _targetLastPosition;
        public Vector3 TargetVelocity;

        protected override void Awake()
        {
            base.Awake();
            _human = GetComponent<Human>();
        }

        protected void Update()
        {
            if (!_human.FinishSetup)
                return;
            UpdateMovementInput();
        }

        protected void BeforeFixedUpdate()
        {
            MoveDirection = null;
            if (Target != null)
            {
                if (Target is BaseTitan titan)
                {
                    var napeBox = (CapsuleCollider)titan.BaseTitanCache.NapeHurtbox;
                    var globalScale = napeBox.transform.lossyScale;
                    var radius = napeBox.radius * Mathf.Max(globalScale.x, globalScale.y);
                    TargetPosition = napeBox.transform.position - 1.5f * radius * napeBox.transform.forward + 0.5f * radius * napeBox.transform.up;
                    titan.TitanColliderToggler.RegisterLook();
                }
                else
                {
                    TargetPosition = Target.GetPosition();
                }
                TargetDirection = TargetPosition - _human.Cache.Transform.position;
            }
        }

        public ITargetable FindNearestEnemy()
        {
            Vector3 position = _human.Cache.Transform.position;
            float nearestDistance = float.PositiveInfinity;
            ITargetable nearestCharacter = null;
            var character = _human.Detection.ClosestEnemy;
            if (character != null && !character.Dead)
            {
                float distance = Vector3.Distance(character.Cache.Transform.position, position);
                if (distance < nearestDistance && distance < DetectRange)
                {
                    nearestDistance = distance;
                    nearestCharacter = character;
                }
            }
            foreach (MapTargetable targetable in MapLoader.MapTargetables)
            {
                if (targetable == null || !targetable.ValidTarget())
                    continue;
                float distance = Vector3.Distance(targetable.GetPosition(), position);
                if (distance < nearestDistance && distance < DetectRange)
                {
                    nearestDistance = distance;
                    nearestCharacter = targetable;
                }
            }
            return nearestCharacter;
        }

        protected void FixedUpdateTargetStatus()
        {
            if (Target != null)
            {
                var targetCurrentPosition = Target.GetPosition();
                if (_targetLastPosition is Vector3 p)
                {
                    TargetVelocity = (targetCurrentPosition - p) / Time.deltaTime;
                }
                else
                {
                    TargetVelocity = Vector3.zero;
                }
                _targetLastPosition = targetCurrentPosition;
            }
            else
            {
                TargetVelocity = Vector3.zero;
                _targetLastPosition = null;
            }
        }

        protected override void FixedUpdate()
        {
            BeforeFixedUpdate();
            FixedUpdateTargetStatus();
        }

        protected void UpdateMovementInput()
        {
            if (_human.Dead || _human.State == HumanState.Stun)
            {
                return;
            }
            _human.IsWalk = IsWalk;
            if (_human.MountState != HumanMountState.Horse)
            {
                if (_human.Grounded && _human.State != HumanState.Idle)
                    return;
                if (!_human.Grounded && (_human.State == HumanState.EmoteAction || (_human.State == HumanState.SpecialAttack && _human.Special is not DownStrikeSpecial && _human.Special is not StockSpecial) ||
                    _human.Animation.IsPlaying("dash") || _human.Animation.IsPlaying("jump") || _human.IsFiringThunderspear()))
                    return;
            }

            if (MoveDirection is Vector3 moveDirection)
            {
                float _movingLeft = AimDirection.x * moveDirection.z - AimDirection.z * moveDirection.x;
                _character.TargetAngle = GetTargetAngle(moveDirection);
                _character.HasDirection = true;
                Vector3 v = new(moveDirection.x, 0f, moveDirection.z);
                float magnitude = (v.magnitude <= 0.95f) ? ((v.magnitude >= 0.25f) ? v.magnitude : 0f) : 1f;
                _human.TargetMagnitude = magnitude;
            }
            else
            {
                _character.HasDirection = false;
                _human.TargetMagnitude = 0f;
            }
        }

        private HashSet<HumanState> _illegalWeaponStates = new HashSet<HumanState>() { HumanState.Grab, HumanState.SpecialAction, HumanState.EmoteAction, HumanState.Reload,
            HumanState.SpecialAttack, HumanState.Stun };

        public void JumpInput()
        {
            if (_human.Dead || _human.State == HumanState.Stun)
                return;
            if (_human.MountState == HumanMountState.None)
            {
                if (_human.CanJump())
                {
                    _human.Jump();
                }
            }
            else if (_human.MountState == HumanMountState.Horse)
            {
                _human.Horse.Jump();
            }
        }

        public void HorseMountInput()
        {
            if (_human.Dead || _human.State == HumanState.Stun)
                return;
            if (_human.MountState == HumanMountState.None)
            {
                if (_human.CanJump())
                {
                    if (_human.Horse != null && _human.MountState == HumanMountState.None &&
                                        Vector3.Distance(_human.Horse.Cache.Transform.position, _human.Cache.Transform.position) < 15f && !_human.HasDirection)
                        _human.MountHorse();
                }
                if (_human.CarryState == HumanCarryState.Carry)
                {
                    _human.Cache.PhotonView.RPC("UncarryRPC", RpcTarget.All, new object[0]);
                }
            }
            else if (_human.MountState == HumanMountState.Horse)
            {
                if (_human.State == HumanState.Idle)
                    _human.Unmount(false);
            }

        }

        public void DodgeInput()
        {
            if (_human.Dead || _human.State == HumanState.Stun)
                return;
            if (_human.CanJump())
            {
                if (_human.HasDirection)
                    _human.Dodge(_human.TargetAngle + 180f);
                else
                    _human.Dodge(_human.TargetAngle);
            }
        }

        public void ReloadInput()
        {
            if (_human.Dead || _human.State == HumanState.Stun)
                return;
            if (_human.MountState == HumanMountState.None)
            {
                if (_human.State == HumanState.Idle)
                {
                    _human.Reload();
                }
            }
            else if (_human.MountState == HumanMountState.Horse)
            {
                if (_human.State == HumanState.Idle && _human.IsAttackableState)
                {
                    _human.Reload();
                }
            }
        }

        bool IsSpin3Special()
        {
            return _human.State == HumanState.SpecialAttack && _human.Special is Spin3Special;
        }

        public bool MovingLeft()
        {
            return _movingLeft < 0;
        }
        public bool MovingRight()
        {
            return _movingLeft > 0;
        }

        public bool UsingGas()
        {
            return _usingGas;
        }

        public bool HookingLeft()
        {
            return _hookingLeft;
        }

        public bool HookingRight()
        {
            return _hookingRight;
        }

        public bool HookingBoth()
        {
            return false;
        }

        public Vector3 GetAimPoint()
        {
            return AimPoint;
        }

        public void AimAt(Vector3? position)
        {
            if (position is Vector3 pos)
            {
                AimPoint = pos;
                AimDirection = pos - _human.transform.position;
            }
            else
            {
                AimDirection = Vector3.zero;
            }
        }

        public void Move(Vector3? direction)
        {
            MoveDirection = direction;
        }

        public void UseGas(bool useGas)
        {
            _usingGas = useGas;
        }

        public void HorseWalk(bool isWalk)
        {
            IsWalk = isWalk;
        }

        public void Reel(int reelAxis)
        {
            if (reelAxis < 0)
            {
                _human.ReelInAxis = -1f;
            }
            if (reelAxis > 0)
            {
                _human.ReelOutAxis = 1f;
            }
            else
            {
                _human.ReelOutAxis = 0f;
            }
        }

        public void Dash(Vector3 direction)
        {
            if (!_human.Grounded && _human.State != HumanState.AirDodge && _human.MountState == HumanMountState.None && _human.State != HumanState.Grab && _human.CarryState != HumanCarryState.Carry
                && _human.State != HumanState.Stun && _human.State != HumanState.EmoteAction && _human.State != HumanState.SpecialAction && !_human.Dead)
            {
                _human.Dash(GetTargetAngle(direction));
            }
        }

        public void LaunchHookLeft(Vector3 aimPoint)
        {
            _hookingLeft = true;
            AimAt(aimPoint);
            UpdateHookInput();
        }

        public void ReleaseHookLeft()
        {
            _hookingLeft = false;
            UpdateHookInput();
        }

        public void LaunchHookRight(Vector3 aimPoint)
        {
            _hookingRight = true;
            AimAt(aimPoint);
            UpdateHookInput();
        }

        public void ReleaseHookRight()
        {
            _hookingRight = false;
            UpdateHookInput();
        }

        public void ReleaseHookAll()
        {
            _hookingLeft = false;
            _hookingRight = false;
            UpdateHookInput();
        }

        protected void UpdateHookInput()
        {
            bool hookLeft = _hookingLeft;
            bool hookRight = _hookingRight;
            bool hookBoth = false;
            bool canHook = _human.State != HumanState.Grab && _human.State != HumanState.Stun && _human.Stats.CurrentGas > 0f
                && !(_human.MountState == HumanMountState.MapObject && !_human.CanMountedAttack) && !_human.Dead;
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
            _human.HookLeft.SetInput(canHook && !IsSpin3Special() && (hookLeft || (hookBoth && (_human.HookLeft.IsHooked() || !hasHook))));
            _human.HookRight.SetInput(canHook && !IsSpin3Special() && (hookRight || (hookBoth && (_human.HookRight.IsHooked() || !hasHook))));

            if (_human.Stats.CurrentGas <= 0f && (hookLeft || hookRight || hookBoth))
            {
                _human.PlaySoundRPC(HumanSounds.NoGas, Util.CreateLocalPhotonInfo());
            }
        }

        public void Attack(bool attackOn)
        {
            bool canWeapon = _human.IsAttackableState && !_illegalWeaponStates.Contains(_human.State) && !_human.Dead;
            _human._gunArmAim = false;
            if (canWeapon)
            {
                _human.Weapon.SetInput(attackOn);
                _human._gunArmAim = _human.Weapon is AHSSWeapon && (attackOn || _human.Weapon.IsActive);
            }
            else
                _human.Weapon.SetInput(false);
        }

        public void ActivateSpecial(bool activate)
        {
            if (_human.Special != null)
            {
                bool canSpecial = _human.IsAttackableState &&
                    (_human.Special is EscapeSpecial || _human.Special is ShifterTransformSpecial || _human.State != HumanState.Grab) && _human.CarryState != HumanCarryState.Carry
                    && _human.State != HumanState.EmoteAction && _human.State != HumanState.Attack && _human.State != HumanState.SpecialAttack && !_human.Dead;
                bool canSpecialHold = _human.Special is BaseHoldAttackSpecial && _human.IsAttackableState && _human.State != HumanState.Grab && (_human.State != HumanState.Attack || _human.Special is StockSpecial) &&
                    _human.State != HumanState.EmoteAction && _human.State != HumanState.Grab && _human.CarryState != HumanCarryState.Carry && !_human.Dead;
                if (canSpecial || canSpecialHold)
                {
                    // Makes AHSSTwinShot activate on key up instead of key down
                    _human.Special.SetInput(activate);
                }
                else
                    _human.Special.SetInput(false);
            }
        }

    }
}
