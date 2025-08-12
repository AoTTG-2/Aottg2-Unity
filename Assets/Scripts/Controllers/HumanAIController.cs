using UnityEngine;
using ApplicationManagers;
using Settings;
using Characters;
using UI;
using System.Collections.Generic;
using Utility;
using Map;
using UnityEngine.AI;
using Photon.Pun;
using UnityEditor.UI;
using System;
using CustomLogic;

namespace Controllers
{
    class HumanAIController : BaseAIController, IHumanController
    {

        protected Human _human;
        public float DetectRange = 10000f;
        protected static LayerMask HookMask = PhysicsLayer.GetMask(PhysicsLayer.TitanMovebox, PhysicsLayer.TitanPushbox,
            PhysicsLayer.MapObjectEntities, PhysicsLayer.MapObjectProjectiles, PhysicsLayer.MapObjectAll);

        protected float _movingLeft = 0;
        protected Vector3 AimDirection;
        protected Vector3 AimPoint;
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
        public HumanAIState AIState;
        public HumanAICallback Callbacks = new();
        public Dictionary<string, HumanAIState> AIStates = new();
        public bool _usePathfinding = true;
        protected NavMeshAgent _agent;
        protected float _moveAngle;
        protected bool _setTargetThisFrame = false;

        protected override void Awake()
        {
            base.Awake();
            _human = GetComponent<Human>();
            _usePathfinding = SettingsManager.InGameUI.Titan.TitanSmartMovement.Value;

            if (_usePathfinding)
            {
                if (NavMesh.SamplePosition(_human.Cache.Transform.position, out NavMeshHit hit, 100f, NavMesh.AllAreas))
                {
                    _human.Cache.Transform.position = hit.position;
                }
            }
        }

        protected override void Start()
        {
            SetAIState("MoveTo", new HumanAIStates.MoveTo().Init(_human));
            // set agent size
            if (_usePathfinding)
            {
                if (NavMesh.SamplePosition(_human.Cache.Transform.position, out NavMeshHit hit, 100f, NavMesh.AllAreas))
                {
                    _human.Cache.Transform.position = hit.position;
                }

                NavMeshBuildSettings agentSettings = Util.GetAgentSettingsCorrected(1f);
                _agent = gameObject.AddComponent<NavMeshAgent>();
                _agent.agentTypeID = agentSettings.agentTypeID;
                // _agent.agentTypeID = 1;
                _agent.speed = _human.Cache.Rigidbody.velocity.magnitude;
                _agent.angularSpeed = 10;
                _agent.acceleration = 100;
                _agent.autoRepath = true;
                _agent.stoppingDistance = 1.1f;
                _agent.autoBraking = false;
                // _agent.radius = 0.5f;
                // _agent.height = 1.0f;
                _agent.radius = agentSettings.agentRadius;
                _agent.height = agentSettings.agentHeight;
                _agent.updatePosition = false;
                _agent.updateRotation = false;
                _agent.obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;
                _agent.avoidancePriority = 0;
            }

        }

        protected void BeforeFixedUpdate()
        {
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
            if (!_human.FinishSetup)
                return;
            BeforeFixedUpdate();
            FixedUpdateTargetStatus();

            Callbacks.PreAction?.Invoke();
            if (AIState is not null)
            {
                AIState.Action();
            }
            else
            {
                Callbacks.NullAIState?.Invoke();
            }
            Callbacks.PostAction?.Invoke();
        }

        protected bool CanMove()
        {
            if (_human.Dead || _human.State == HumanState.Stun)
            {
                return false;
            }
            if (_human.MountState != HumanMountState.Horse)
            {
                if (_human.Grounded && _human.State != HumanState.Idle)
                    return false;
                if (!_human.Grounded && (_human.State == HumanState.EmoteAction || (_human.State == HumanState.SpecialAttack && _human.Special is not DownStrikeSpecial && _human.Special is not StockSpecial) ||
                    _human.Animation.IsPlaying("dash") || _human.Animation.IsPlaying("jump") || _human.IsFiringThunderspear()))
                    return false;
            }
            return true;
        }

        private HashSet<HumanState> _illegalWeaponStates = new HashSet<HumanState>() { HumanState.Grab, HumanState.SpecialAction, HumanState.EmoteAction, HumanState.Reload,
            HumanState.SpecialAttack, HumanState.Stun };

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

        public void Move(Vector3? direction)
        {
            if (!CanMove())
            {
                return;
            }
            if (direction is Vector3 moveDirection)
            {
                _movingLeft = AimDirection.x * moveDirection.z - AimDirection.z * moveDirection.x;
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

        public void Jump()
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

        public void HorseMount(bool mount = true)
        {
            if (_human.Dead || _human.State == HumanState.Stun)
                return;
            if (_human.MountState == HumanMountState.None && mount)
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
            else if (_human.MountState == HumanMountState.Horse && !mount)
            {
                if (_human.State == HumanState.Idle)
                    _human.Unmount(false);
            }

        }

        public void Dodge()
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

        public void Reload()
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

        public void UseGas(bool useGas)
        {
            _usingGas = useGas;
        }

        public void HorseWalk(bool isWalk)
        {
            _human.IsWalk = isWalk;
        }

        public void Dash(Vector3 direction)
        {
            if (!_human.Grounded && _human.State != HumanState.AirDodge && _human.MountState == HumanMountState.None && _human.State != HumanState.Grab && _human.CarryState != HumanCarryState.Carry
                && _human.State != HumanState.Stun && _human.State != HumanState.EmoteAction && _human.State != HumanState.SpecialAction && !_human.Dead)
            {
                _human.Dash(GetTargetAngle(direction));
            }
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

        /***
        NavMesh
        ***/

        public float GetAgentNavAngle(Vector3 target)
        {
            Vector3 resultDirection = _agent.velocity.normalized;
            // Good ol' power cycle fix
            // if agent gets desynced (position is not equal to titan position), disable the component and re-enable it
            Vector3 agentPositionXY = new(_agent.transform.position.x, 0, _agent.transform.position.z);
            Vector3 titanPositionXY = new(_human.Cache.Transform.position.x, 0, _human.Cache.Transform.position.z);
            if (_agent.isOnNavMesh && Vector3.Distance(agentPositionXY, titanPositionXY) > 1f)
            {
                resultDirection = (_agent.transform.position - _human.Cache.Transform.position).normalized;
            }
            else if (_agent.isOnNavMesh && _agent.pathPending == false)
            {
                SetAgentDestination(target);
            }
            else if (_agent.isOnNavMesh == false)
            {
                resultDirection = GetDirectionTowardsNavMesh();
            }

            if (resultDirection == Vector3.zero)
                return _human.TargetAngle;

            return GetChaseAngleGivenDirection(resultDirection, true);
        }

        protected Vector3 GetDirectionTowardsNavMesh()
        {
            // Find a point on the navmesh closest to the titan
            if (NavMesh.SamplePosition(_human.Cache.Transform.position, out NavMeshHit hit, 100f, NavMesh.AllAreas))
            {
                return (hit.position - _human.Cache.Transform.position).normalized;
            }

            // Return a random direction if the navmesh is not found
            Vector3 randDir = UnityEngine.Random.onUnitSphere;
            randDir.y = 0;
            return randDir.normalized;
        }

        protected float GetChaseAngleGivenDirection(Vector3 direction, bool useMoveAngle)
        {
            float angle;
            if (direction == Vector3.zero)
                angle = _human.TargetAngle;
            else
                angle = GetTargetAngle(direction);
            if (useMoveAngle)
                angle += _moveAngle;
            if (angle > 360f)
                angle -= 360f;
            if (angle < 0f)
                angle += 360f;
            return angle;
        }

        public void SetAgentDestination(Vector3 position)
        {
            if (!_setTargetThisFrame)
            {
                _agent.SetDestination(position);
                _setTargetThisFrame = true;
            }
        }

        public void RefreshAgent()
        {
            if (_usePathfinding)
            {
                // Good ol' power cycle fix
                // if agent gets desynced (position is not equal to titan position), disable the component and re-enable it
                Vector3 agentPositionXY = new(_agent.transform.position.x, 0, _agent.transform.position.z);
                Vector3 titanPositionXY = new(_human.Cache.Transform.position.x, 0, _human.Cache.Transform.position.z);
                if (_usePathfinding && Vector3.Distance(agentPositionXY, titanPositionXY) > 0.1f)
                {
                    // debug log
                    _agent.enabled = false;
                    _agent.enabled = true;
                }

                _agent.nextPosition = _human.Cache.Transform.position;
            }
        }

        public float GetChaseAngle(Vector3 position, bool useMoveAngle)
        {
            return GetChaseAngleGivenDirection((position - _character.Cache.Transform.position).normalized, useMoveAngle);
        }

        public void MoveToPosition()
        {
            _human.HasDirection = true;
            _human.TargetMagnitude = 1.0f;
            if (_usePathfinding)
            {
                _setTargetThisFrame = false;
                _agent.speed = _human.Cache.Rigidbody.velocity.magnitude;
                _moveAngle = UnityEngine.Random.Range(-5f, 5f);
                _human.TargetAngle = GetAgentNavAngle(TargetPosition);
            }
            else
            {
                _moveAngle = UnityEngine.Random.Range(-45f, 45f);
                _human.TargetAngle = GetChaseAngle(TargetPosition, true);
            }
            RefreshAgent();
        }

        /***
        AI State
        ***/

        public void SwitchAIState(HumanAIState aiState)
        {
            if (aiState == AIState)
                return;
            aiState?.PreSwitchState();
            var oldAIState = AIState;
            AIState = aiState;
            oldAIState?.PostSwitchState();
        }

        public void SetAIState(string name, HumanAIState aiState)
        {
            AIStates[name] = aiState;
        }

        public bool HasAIState(string name)
        {
            return AIStates.ContainsKey(name);
        }

        public HumanAIState GetAIState(string name)
        {
            return AIStates[name];
        }

        public void MoveTo(Vector3 position, float range)
        {
            Target = null;
            TargetPosition = position;
            var moveToState = (HumanAIStates.MoveTo)AIStates["MoveTo"];
            moveToState.MoveToRange = range;
            SwitchAIState(moveToState);
        }

        public void MoveToTarget(ITargetable targetable, float range)
        {
            Target = targetable;
            var moveToState = (HumanAIStates.MoveTo)AIStates["MoveTo"];
            moveToState.MoveToRange = range;
            SwitchAIState(moveToState);
        }

        public void Idle()
        {
            Callbacks.PreIdle?.Invoke();
            Target = null;
            _human.HasDirection = false;
            _human.TargetMagnitude = 0;
            SwitchAIState(null);
            Callbacks.PostIdle?.Invoke();
        }



    }

    class HumanAIState
    {
        protected Human Human;
        protected HumanAIController Controller;

        public virtual string Name
        {
            get => "";
        }
        public virtual HumanAIState Init(Human human)
        {
            Human = human;
            Controller = (HumanAIController)human.Controller;
            return this;
        }

        public virtual void PreSwitchState() { }
        public virtual void Action() { }
        public virtual void PostSwitchState() { }
    }

    class HumanAICallback
    {

        public Action NullAIState;
        public Action PreIdle;
        public Action PostIdle;

        public Action PreAction;

        public Action PostAction;

        public Action MoveToCallback;
    }

    namespace HumanAIStates
    {
        class MoveTo : HumanAIState
        {

            public float MoveToRange;
            public override string Name
            {
                get => "MoveTo";
            }
            public override void Action()
            {
                float distance = Vector3.Distance(Human.Cache.Transform.position, Controller.TargetPosition);

                if (distance <= MoveToRange)
                {
                    Controller.Callbacks.MoveToCallback?.Invoke();
                    Controller.Idle();
                }
                else
                    Controller.MoveToPosition();
            }

            public override void PostSwitchState()
            {
                Controller.Callbacks.MoveToCallback = null;
            }
        }

        class Custom : HumanAIState
        {

            protected UserClassInstance _instance;

            protected UserMethod _preSwitchState;

            protected UserMethod _postSwitchState;

            protected UserMethod _action;

            public override string Name
            {
                get => _name;
            }
            private string _name;

            public void Init(string name, UserClassInstance instance)
            {
                _name = name;
                _instance = instance;
                _preSwitchState = null;
                _action = null;
                _postSwitchState = null;
                if (_instance.Variables.ContainsKey("PreSwitchState"))
                {
                    _preSwitchState = (UserMethod)_instance.GetVariable("PreSwitchState");
                }
                if (_instance.Variables.ContainsKey("Action"))
                {
                    _action = (UserMethod)_instance.GetVariable("Action");
                }

                if (_instance.Variables.ContainsKey("PostSwitchState"))
                {
                    _postSwitchState = (UserMethod)_instance.GetVariable("PostSwitchState");
                }
            }

            public override void PreSwitchState()
            {
                if (_preSwitchState is not null)
                    CustomLogicManager.Evaluator.EvaluateMethod(_preSwitchState, new object[] { });
            }
            public override void Action()
            {
                if (_action is not null)
                    CustomLogicManager.Evaluator.EvaluateMethod(_action, new object[] { });
            }

            public override void PostSwitchState()
            {
                if (_action is not null)
                    CustomLogicManager.Evaluator.EvaluateMethod(_postSwitchState, new object[] { });
            }
        }
    }


}
