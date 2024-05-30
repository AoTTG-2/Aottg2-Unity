using System;
using UnityEngine;
using ApplicationManagers;
using GameManagers;
using UnityEngine.UI;
using Utility;
using System.Collections;
using SimpleJSONFixed;
using Effects;
using UI;
using System.Collections.Generic;
using Settings;
using Photon.Pun;
using Photon.Realtime;

namespace Characters
{
    abstract class BaseTitan : BaseCharacter
    {
        public TitanState State;
        public BaseTitanComponentCache BaseTitanCache;
        public TitanColliderToggler TitanColliderToggler;
        public bool IsWalk;
        public bool IsSprint;
        public bool IsSit;
        public Human HoldHuman = null;
        public bool HoldHumanLeft;
        public float Size = 1f;
        public virtual float DefaultCrippleTime => 8f;
        public virtual bool CanWallClimb => false;
        public virtual bool CanSprint => false;
        public float StunTime = 0.3f;
        public float ActionPause = 0.2f;
        public float AttackPause = 0.2f;
        public float TurnPause = 0.2f;
        public float MaxSprintStamina = 5f;
        public float SprintStaminaRecover = 0.5f;
        public float SprintStaminaConsumption = 1f;
        public float CurrentSprintStamina = 5f;
        public BaseCharacter TargetEnemy = null;
        protected BaseTitanAnimations BaseTitanAnimations;
        protected override float GroundDistance => 1f;
        protected virtual float DefaultRunSpeed => 15f;
        protected virtual float DefaultWalkSpeed => 5f;
        protected virtual float DefaultJumpForce => 150f;
        protected virtual float DefaultRotateSpeed => 1f;
        protected virtual float SizeMultiplier => 1f;
        public float AttackSpeedMultiplier = 1f;
        public Dictionary<string, float> AttackSpeeds = new Dictionary<string, float>();
        public float RunSpeedBase;
        public float WalkSpeedBase;
        public float RunSpeedPerLevel;
        public float WalkSpeedPerLevel;
        public float JumpForce;
        public float RotateSpeed;
        public float TurnSpeed;
        protected override Vector3 Gravity => Vector3.down * 100f;
        protected Vector3 LastTargetDirection;
        protected Vector3 _wallClimbForward;
        protected Quaternion _turnStartRotation;
        protected Quaternion _turnTargetRotation;
        protected Vector3 _jumpDirection;
        protected float _maxTurnTime;
        protected float _currentTurnTime;
        protected float _currentGroundDistance;
        protected float _currentCrippleTime;
        protected float _currentFallTime;
        protected LayerMask MapObjectMask => PhysicsLayer.GetMask(PhysicsLayer.MapObjectEntities);

        // attacks
        public float _stateTimeLeft;
        protected string _currentAttackAnimation;
        protected string _currentAttack;
        protected string _currentStateAnimation;
        protected float _currentAttackSpeed;
        protected int _currentAttackStage;
        protected bool _needFreshCore;
        protected Vector3 _startCoreAttackPosition;
        protected Vector3 _previousCoreLocalPosition;
        protected Vector3 _furthestCoreLocalPosition;
        protected Dictionary<string, float> _rootMotionAnimations = new Dictionary<string, float>();
        public Dictionary<string, string> AttackAnimations = new Dictionary<string, string>();

        public virtual void Init(bool ai, string team, JSONNode data)
        {
            base.Init(ai, team);
            if (data != null)
            {
                if (data.HasKey("RunSpeedBase"))
                    RunSpeedBase = data["RunSpeedBase"].AsFloat;
                if (data.HasKey("RunSpeedPerLevel"))
                    RunSpeedPerLevel = data["RunSpeedPerLevel"].AsFloat;
                if (data.HasKey("WalkSpeedBase"))
                    WalkSpeedBase = data["WalkSpeedBase"].AsFloat;
                if (data.HasKey("WalkSpeedPerLevel"))
                    WalkSpeedPerLevel = data["WalkSpeedPerLevel"].AsFloat;
                if (data.HasKey("JumpForce"))
                    JumpForce = data["JumpForce"].AsFloat;
                if (data.HasKey("RotateSpeed"))
                    RotateSpeed = data["RotateSpeed"].AsFloat;
                if (data.HasKey("ActionPause"))
                    ActionPause = data["ActionPause"].AsFloat;
                if (data.HasKey("TurnPause"))
                    TurnPause = data["TurnPause"].AsFloat;
                if (data.HasKey("AttackPause"))
                    AttackPause = data["AttackPause"].AsFloat;
                if (data.HasKey("Health"))
                    SetHealth(data["Health"].AsInt);
                if (data.HasKey("AttackSpeedMultiplier"))
                    AttackSpeedMultiplier = data["AttackSpeedMultiplier"].AsFloat;
                if (data.HasKey("AttackSpeeds"))
                {
                    foreach (string attack in data["AttackSpeeds"].Keys)
                        AttackSpeeds.Add(attack, data["AttackSpeeds"][attack].AsFloat);
                }
                if (data.HasKey("TurnSpeed"))
                {
                    TurnSpeed = data["TurnSpeed"].AsFloat;
                    if (BaseTitanAnimations.Turn90L != "")
                        Cache.Animation[BaseTitanAnimations.Turn90L].speed *= TurnSpeed;
                    if (BaseTitanAnimations.Turn90R != "")
                        Cache.Animation[BaseTitanAnimations.Turn90R].speed *= TurnSpeed;
                }
            }
        }

        protected virtual Dictionary<string, float> GetRootMotionAnimations()
        {
            return new Dictionary<string, float>();
        }

        public virtual bool IsGrabAttack()
        {
            return false;
        }

        public float GetCurrentSpeed()
        {
            if (IsWalk)
                return WalkSpeedBase + WalkSpeedPerLevel * Size;
            else
                return RunSpeedBase + RunSpeedPerLevel * Size;
        }

        public virtual bool CanAction()
        {
            return !Dead && (State == TitanState.Idle && _stateTimeLeft <= 0f) || State == TitanState.Run || State == TitanState.Walk || State == TitanState.Sprint;
        }

        public virtual bool CanStun()
        {
            return State != TitanState.Stun && !Dead;
        }

        public virtual void Jump(Vector3 direction)
        {
            if (BaseTitanAnimations.Jump == "")
                return;
            _jumpDirection = direction;
            StateAction(TitanState.PreJump, BaseTitanAnimations.Jump);
        }

        public virtual void StartJump()
        {
            State = TitanState.StartJump;
            _stateTimeLeft = 0.2f;
            Cache.Rigidbody.AddForce(_jumpDirection.normalized * JumpForce, ForceMode.VelocityChange);
        }

        public virtual void Attack(string attack)
        {
            ResetAttackState(attack);
            StateAttack(_currentAttackAnimation);
        }

        public virtual bool CanAttack()
        {
            return CanAction();
        }

        public virtual void ResetAttackState(string attack)
        {
            Cache.Rigidbody.velocity = Vector3.zero;
            _currentAttack = attack;
            _currentAttackAnimation = AttackAnimations[attack];
            _currentAttackSpeed = GetAttackSpeed(attack);
            _currentAttackStage = 0;
        }

        public float GetAttackSpeed(string attack)
        {
            float speed = 1f;
            if (AttackSpeeds.ContainsKey(attack))
                speed = AttackSpeeds[attack];
            speed *= AttackSpeedMultiplier;
            if (speed <= 0f)
                speed = 1f;
            return speed;
        }

        public virtual void Kick()
        {
        }

        public virtual void Stun()
        {
            if (CanStun())
                StateActionWithTime(TitanState.Stun, BaseTitanAnimations.Stun, StunTime, 0.1f);
        }

        public virtual void Run()
        {
            _stepPhase = 0;
            StateActionWithTime(TitanState.Run, BaseTitanAnimations.Run, 0f, 0.5f);
        }

        public virtual void Sprint()
        {
            _stepPhase = 0;
            StateActionWithTime(TitanState.Sprint, BaseTitanAnimations.Sprint, 0f, 0.2f);
        }

        public virtual void WallClimb()
        {
            if (!CanWallClimb)
                return;
            _stepPhase = 0;
            StateActionWithTime(TitanState.WallClimb, BaseTitanAnimations.Run, 0f, 0.1f);
        }

        public virtual void Eat()
        {
        }

        public virtual void Walk()
        {
            _stepPhase = 0;
            StateActionWithTime(TitanState.Walk, BaseTitanAnimations.Walk, 0f, 0.5f);
        }

        public virtual void Idle()
        {
            if (AI)
                Idle(0.1f);
            else
                Idle(0f);
        }
        
        public virtual void Idle(float fadeTime)
        {
            StateActionWithTime(TitanState.Idle, BaseTitanAnimations.Idle, 0f, fadeTime);
        }

        public virtual void IdleWait(float waitTime)
        {
            Idle(Mathf.Clamp(waitTime, 0.1f, 2f));
            _stateTimeLeft = waitTime;
        }

        public virtual void Land()
        {
            StateAction(TitanState.Land, BaseTitanAnimations.Land);
            EffectSpawner.Spawn(EffectPrefabs.Boom2, Cache.Transform.position + Vector3.down * _currentGroundDistance, 
                Quaternion.Euler(270f, 0f, 0f), Size * SizeMultiplier);
        }

        public virtual void Fall()
        {
            StateActionWithTime(TitanState.Fall, BaseTitanAnimations.Fall, 0f, 0.1f);
        }

        public virtual void Turn(Vector3 targetDirection)
        {
            if (!CanAction())
                return;
            float angle = GetAngleToTarget(targetDirection);
            string animation;
            if (angle > 0f)
                animation = BaseTitanAnimations.Turn90R;
            else
                animation = BaseTitanAnimations.Turn90L;
            if (animation == "")
                return;
            targetDirection = Vector3.RotateTowards(Cache.Transform.forward, targetDirection, 120f * Mathf.Deg2Rad, float.MaxValue);
            _turnStartRotation = Cache.Transform.rotation;
            _turnTargetRotation = Quaternion.LookRotation(targetDirection);
            _currentTurnTime = 0f;
            _maxTurnTime = Cache.Animation[animation].length / Cache.Animation[animation].speed;
            StateActionWithTime(TitanState.Turn, animation, _maxTurnTime, 0.1f);
        }

        public virtual void Blind()
        {
            if (State != TitanState.Blind && State != TitanState.SitBlind && AI)
            {
                if (State == TitanState.SitCripple || State == TitanState.SitIdle)
                {
                    if (BaseTitanAnimations.SitBlind != "")
                    {
                        StateAction(TitanState.SitBlind, BaseTitanAnimations.SitBlind);
                        DamagedGrunt();
                    }
                }
                else
                {
                    if (BaseTitanAnimations.Blind != "")
                    {
                        StateAction(TitanState.Blind, BaseTitanAnimations.Blind);
                        DamagedGrunt();
                    }
                }
            }
        }

        public virtual void Cripple(float time = 0f)
        {
            if (BaseTitanAnimations.SitFall != "" && State != TitanState.SitCripple && AI)
            {
                _currentCrippleTime = time > 0f? time: DefaultCrippleTime;
                StateAction(TitanState.SitFall, BaseTitanAnimations.SitFall);
                DamagedGrunt();
            }
        }

        public override void Emote(string emote)
        {
        }

        protected override IEnumerator WaitAndDie()
        {
            StateActionWithTime(TitanState.Dead, BaseTitanAnimations.Die, 0f, 0.1f);
            yield return new WaitForSeconds(2f);
            EffectSpawner.Spawn(EffectPrefabs.TitanDie1, BaseTitanCache.Hip.position, Quaternion.Euler(-90f, 0f, 0f), GetSpawnEffectSize(), false);
            yield return new WaitForSeconds(3f);
            EffectSpawner.Spawn(EffectPrefabs.TitanDie2, BaseTitanCache.Hip.position, Quaternion.Euler(-90f, 0f, 0f), GetSpawnEffectSize(), false);
            PhotonNetwork.Destroy(gameObject);
        }

        protected float GetSpawnEffectSize()
        {
            return Size * SizeMultiplier;
        }

        [PunRPC]
        public virtual void GrabRPC(int viewId, PhotonMessageInfo info)
        {
            var view = PhotonView.Find(viewId);
            if (view.Owner != info.Sender)
                return;
            HoldHuman = view.GetComponent<Human>();
        }

        [PunRPC]
        public virtual void UngrabRPC(PhotonMessageInfo info)
        {
            HoldHuman = null;
        }

        public virtual void Ungrab()
        {
            if (HoldHuman != null)
            {
                HoldHuman.Cache.PhotonView.RPC("UngrabRPC", HoldHuman.Cache.PhotonView.Owner, new object[0]);
                HoldHuman = null;
            }
        }

        public override void OnPlayerEnteredRoom(Player player)
        {
            base.OnPlayerEnteredRoom(player);
            if (Cache.PhotonView.IsMine)
                Cache.PhotonView.RPC("SetSizeRPC", player, new object[] { Size });
        }

        protected void StateAction(TitanState state, string animation, float fade = 0.1f, bool deactivateHitboxes = true)
        {
            StateActionWithTime(state, animation, Cache.Animation[animation].length, fade, deactivateHitboxes);
        }

        protected void StateAttack(string animation, float fade = 0.1f, bool deactivateHitboxes = true)
        {
            _needFreshCore = true;
            SetAnimationUpdateMode(true);
            Ungrab();
            if (deactivateHitboxes)
                DeactivateAllHitboxes();
            CrossFadeWithSpeed(animation, _currentAttackSpeed, fade);
            State = TitanState.Attack;
            _currentStateAnimation = animation;
            _stateTimeLeft = Cache.Animation[animation].length / _currentAttackSpeed;
        }

        protected void StateActionWithTime(TitanState state, string animation, float stateTime, float fade = 0.1f, bool deactivateHitboxes = true)
        {
            _needFreshCore = true;
            SetAnimationUpdateMode(state == TitanState.Jump);
            if (state != TitanState.Eat)
                Ungrab();
            if (deactivateHitboxes)
                DeactivateAllHitboxes();
            if (state != TitanState.Idle || _currentStateAnimation != animation)
                CrossFade(animation, fade);
            State = state;
            _currentStateAnimation = animation;
            _stateTimeLeft = stateTime;
        }

        protected void SetAnimationUpdateMode(bool always)
        {
            if (always)
                Cache.Animation.cullingType = AnimationCullingType.AlwaysAnimate;
            else
                Cache.Animation.cullingType = AnimationCullingType.BasedOnRenderers;
        }

        protected override void Awake()
        {
            base.Awake();
            CreateAnimations(null);
            Cache.Rigidbody.freezeRotation = true;
            Cache.Rigidbody.useGravity = false;
            TitanColliderToggler = TitanColliderToggler.Create(this);
            RunSpeedBase = DefaultRunSpeed;
            WalkSpeedBase = DefaultWalkSpeed;
            JumpForce = DefaultJumpForce;
            RotateSpeed = DefaultRotateSpeed;
            SetAnimationUpdateMode(false);
        }

        protected override void CreateCache(BaseComponentCache cache)
        {
            BaseTitanCache = (BaseTitanComponentCache)cache;
            base.CreateCache(cache);
        }

        protected virtual void CreateAnimations(BaseTitanAnimations animations)
        {
            if (animations == null)
                animations = new BaseTitanAnimations();
            BaseTitanAnimations = animations;
            _rootMotionAnimations = GetRootMotionAnimations();
            foreach (var fieldInfo in BaseTitanAnimations.GetType().GetFields())
            {
                if (fieldInfo.Name.StartsWith("Attack"))
                {
                    AttackAnimations.Add(fieldInfo.Name, (string)fieldInfo.GetValue(BaseTitanAnimations));
                }
            }
        }

        public override Transform GetCameraAnchor()
        {
            return BaseTitanCache.Head;
        }

        protected virtual void UpdateDisableArm()
        {
        }

        public virtual void DisableArm(bool left)
        {
        }

        protected virtual void UpdateTurn()
        {
            _currentTurnTime += Time.deltaTime;
            Cache.Transform.rotation = Quaternion.Slerp(_turnStartRotation, _turnTargetRotation, Mathf.Clamp(_currentTurnTime / _maxTurnTime, 0f, 1f));
        }

        protected virtual string GetSitIdleAniamtion()
        {
            return BaseTitanAnimations.SitIdle;
        }

        protected virtual string GetSitFallAnimation()
        {
            return BaseTitanAnimations.SitFall;
        }

        protected virtual string GetSitUpAnimation()
        {
            return BaseTitanAnimations.SitUp;
        }

        protected virtual void Update()
        {
            UpdateDisableArm();
            UpdateAnimationColliders();
            if (IsMine())
            {
                if (State == TitanState.Fall || State == TitanState.Jump)
                {
                    if (!AI && HasDirection && IsSprint && CurrentSprintStamina > 1f)
                    {
                        RaycastHit hit;
                        if (Physics.Raycast(Cache.Transform.position + Vector3.up * 3f * Size, Cache.Transform.forward, out hit, Size * 5f, MapObjectMask.value))
                        {
                            WallClimb();
                        }
                    }
                }
                if (State == TitanState.Sprint || State == TitanState.WallClimb)
                    CurrentSprintStamina -= SprintStaminaConsumption * Time.deltaTime;
                else
                    CurrentSprintStamina += SprintStaminaRecover * Time.deltaTime;
                CurrentSprintStamina = Mathf.Clamp(CurrentSprintStamina, 0f, MaxSprintStamina);
                if (State == TitanState.WallClimb)
                {
                    if (CurrentSprintStamina <= 0f || !IsSprint)
                        Idle(0.2f);
                }
                if (State == TitanState.Fall || State == TitanState.Dead || State == TitanState.Jump || State == TitanState.WallClimb)
                    return;
                if (State == TitanState.Eat)
                    UpdateEat();
                else if (State == TitanState.Turn)
                    UpdateTurn();
                else if (State == TitanState.Attack)
                {
                    UpdateAttack();
                }
                _stateTimeLeft -= Time.deltaTime;
                if (_stateTimeLeft > 0f)
                    return;
                if (State == TitanState.Attack && HoldHuman != null)
                {
                    Eat();
                    return;
                }
                else if (State == TitanState.Idle)
                {
                    if (HasDirection)
                    {
                        if (IsWalk && !IsSprint)
                            Walk();
                        else
                        {
                            if (IsSprint && CurrentSprintStamina > 1f && CanSprint)
                                Sprint();
                            else
                                Run();
                        }
                    }
                    else if (IsSit && BaseTitanAnimations.SitDown != "")
                        StateAction(TitanState.SitDown, BaseTitanAnimations.SitDown);
                }
                else if (State == TitanState.SitDown)
                    StateActionWithTime(TitanState.SitIdle, GetSitIdleAniamtion(), 0f, 0.1f);
                else if (State == TitanState.SitIdle)
                {
                    if (HasDirection || !IsSit)
                        StateAction(TitanState.SitUp, GetSitUpAnimation());
                }
                else if (State == TitanState.SitFall)
                    StateActionWithTime(TitanState.SitCripple, GetSitIdleAniamtion(), _currentCrippleTime, 0.1f);
                else if (State == TitanState.SitCripple)
                    StateAction(TitanState.SitUp, GetSitUpAnimation());
                else if (State == TitanState.Run)
                {
                    if (!HasDirection)
                        Idle(0.2f);
                    else if (IsSprint)
                    {
                        if (CanSprint && CurrentSprintStamina > 1f)
                            Sprint();
                    }
                    else if (IsWalk)
                        Walk();
                }
                else if (State == TitanState.Walk)
                {
                    if (!HasDirection)
                        Idle(0.2f);
                    else if (IsSprint)
                    {
                        if (CurrentSprintStamina > 1f && CanSprint)
                            Sprint();
                    }
                    else if (!IsWalk)
                        Run();
                }
                else if (State == TitanState.Sprint)
                {
                    if (!AI)
                    {
                        RaycastHit hit;
                        if (Physics.Raycast(Cache.Transform.position + Vector3.up * 3f * Size, Cache.Transform.forward, out hit, Size * 5f, MapObjectMask.value))
                        {
                            WallClimb();
                        }
                    }
                    if (!HasDirection)
                        Idle(0.2f);
                    else if (IsSprint)
                    {
                        if (CurrentSprintStamina <= 0f)
                            Run();
                    }
                    else if (IsWalk)
                        Walk();
                    else
                        Run();
                }
                else if (State == TitanState.PreJump)
                    StartJump();
                else if (State == TitanState.StartJump)
                    State = TitanState.Jump;
                else if (State == TitanState.Attack || State == TitanState.Eat)
                    IdleWait(AttackPause);
                else if (State == TitanState.Land)
                    IdleWait(ActionPause);
                else if (State == TitanState.Turn)
                    IdleWait(TurnPause);
                else if (State == TitanState.Blind || State == TitanState.SitUp || State == TitanState.Emote)
                    IdleWait(0.3f);
                else if (State == TitanState.ArmHurt)
                    IdleWait(0.2f);
                else if (State == TitanState.SitBlind)
                    StateActionWithTime(TitanState.SitIdle, BaseTitanAnimations.SitIdle, 0.3f, 0.3f);
                else
                    Idle();
            }
        }

        public virtual void StopWallClimb()
        {
            Idle(0.2f);
        }

        protected virtual void FixedUpdate()
        {
            if (IsMine())
            {
                CheckGround();
                if (State != TitanState.Fall)
                    _currentFallTime = 0f;
                if (State == TitanState.Jump)
                {
                    if (Cache.Rigidbody.velocity.y <= 1f)
                        Fall();
                }
                else if (State == TitanState.Attack)
                {
                    FixedUpdateAttack();
                    if (Grounded)
                        SetDefaultVelocity();
                }
                else if (State == TitanState.Dead)
                {
                    if (Grounded)
                        SetDefaultVelocity();
                }
                else if (Grounded && State != TitanState.Jump && State != TitanState.StartJump && State != TitanState.WallClimb)
                {
                    SetDefaultVelocity();
                    LastTargetDirection = Vector3.zero;
                    if (State == TitanState.Fall)
                        Land();
                    else if (HasDirection && (State == TitanState.Run || State == TitanState.Walk || State == TitanState.Sprint))
                    {
                        LastTargetDirection = GetTargetDirection();
                        if (State == TitanState.Run)
                            Cache.Rigidbody.velocity += Cache.Transform.forward * (RunSpeedBase + RunSpeedPerLevel * Size);
                        else if (State == TitanState.Sprint)
                            Cache.Rigidbody.velocity += Cache.Transform.forward * (RunSpeedBase + RunSpeedPerLevel * Size) * 1.7f;
                        else if (State == TitanState.Walk)
                            Cache.Rigidbody.velocity += Cache.Transform.forward * (WalkSpeedBase + WalkSpeedPerLevel * Size);
                    }
                }
                else if (State == TitanState.Fall)
                {
                    if (Cache.Rigidbody.velocity.y >= -1f)
                    {
                        _currentFallTime += Time.fixedDeltaTime;
                        if (_currentFallTime > 0.5f)
                            Land();
                    }
                }
                else if (State == TitanState.WallClimb)
                {
                    Cache.Rigidbody.velocity = Vector3.zero;
                    var direction = GetTargetDirection();
                    if (!HasDirection || Vector3.Angle(direction, Cache.Transform.forward) >= 135f)
                        StopWallClimb();
                    else
                    {
                        RaycastHit hit;
                        if (Physics.Raycast(Cache.Transform.position + Vector3.up * 3f * Size, Cache.Transform.forward, out hit, 5f * Size, MapObjectMask.value))
                        {
                            Cache.Rigidbody.velocity += Vector3.up * (RunSpeedBase + RunSpeedPerLevel * Size * 0.5f);
                            if (hit.distance > 3.5f * Size)
                                Cache.Rigidbody.velocity += Cache.Transform.forward * Mathf.Min((hit.distance - 3.5f * Size) / Time.fixedDeltaTime, 10f);
                        }
                        else
                        {
                            Cache.Transform.position += Vector3.up * 3f * Size + Cache.Transform.forward * 2f * Size;
                            StopWallClimb();
                        }
                    }
                }
                if (_needFreshCore)
                {
                    _furthestCoreLocalPosition = BaseTitanCache.Core.position - BaseTitanCache.Transform.position;
                    _previousCoreLocalPosition = _furthestCoreLocalPosition;
                    _needFreshCore = false;
                }
                if (_rootMotionAnimations.ContainsKey(_currentStateAnimation) && Cache.Animation.IsPlaying(_currentStateAnimation)
                    && Cache.Animation[_currentStateAnimation].normalizedTime < _rootMotionAnimations[_currentStateAnimation])
                {
                    Vector3 coreLocalPosition = BaseTitanCache.Core.position - BaseTitanCache.Transform.position;
                    if (coreLocalPosition.magnitude >= _furthestCoreLocalPosition.magnitude)
                    {
                        Vector3 v = -1f * (coreLocalPosition - _previousCoreLocalPosition) / Time.fixedDeltaTime;
                        _furthestCoreLocalPosition = coreLocalPosition;
                        _previousCoreLocalPosition = coreLocalPosition;
                        v.y = Cache.Rigidbody.velocity.y;
                        Cache.Rigidbody.velocity = v;
                    }
                }
                if (State != TitanState.WallClimb)
                    Cache.Rigidbody.AddForce(Gravity, ForceMode.Acceleration);
            }
        }

        protected virtual void FixedUpdateAttack()
        {
        }

        protected void SetDefaultVelocity()
        {
            Cache.Rigidbody.velocity = Vector3.up * Cache.Rigidbody.velocity.y + Vector3.down * Mathf.Min(_currentGroundDistance * 100f, 100f);
        }

        protected override void LateUpdate()
        {
            if (IsMine())
            {
                if ((State == TitanState.Run || State == TitanState.Walk || State == TitanState.Sprint) && HasDirection)
                {
                    Cache.Transform.rotation = Quaternion.Lerp(Cache.Transform.rotation, GetTargetRotation(), Time.deltaTime * RotateSpeed);
                }
                else if (State == TitanState.StartJump || State == TitanState.Jump)
                {
                    if (_jumpDirection.x != 0f || _jumpDirection.z != 0f)
                    {
                        var forward = _jumpDirection;
                        forward.y = 0f;
                        Quaternion rotation = Quaternion.LookRotation(forward.normalized);
                        Cache.Transform.rotation = Quaternion.Lerp(Cache.Transform.rotation, rotation, Time.deltaTime * 10f);
                    }
                }
            }
            base.LateUpdate();
        }

        protected bool IsPlayingClip(string clip)
        {
            return clip != "" && Cache.Animation.IsPlaying(clip);
        }

        protected override void CheckGround()
        {
            float radius = BaseTitanCache.Movebox.transform.lossyScale.x * ((CapsuleCollider)(BaseTitanCache.Movebox)).radius;
            RaycastHit hit;
            JustGrounded = false;
            if (Physics.SphereCast(Cache.Transform.position + Vector3.up * (radius + 1f), radius, Vector3.down, 
                out hit, 1f + GroundDistance, GroundMask.value))
            {
                if (!Grounded)
                    Grounded = JustGrounded = true;
                _currentGroundDistance = Mathf.Clamp(hit.distance - 1f, 0f, GroundDistance);
            }
            else
            {
                Grounded = false;
                _currentGroundDistance = GroundDistance;
            }
        }

        protected virtual void UpdateAttack()
        {
        }

        protected virtual void UpdateEat()
        {
        }

        protected void DeactivateAllHitboxes()
        {
            foreach (var hitbox in BaseTitanCache.Hitboxes)
            {
                hitbox.Deactivate();
            }
        }

        [PunRPC]
        public void SetSizeRPC(float size, PhotonMessageInfo info)
        {
            if (info.Sender != Cache.PhotonView.Owner)
                return;
            size = Mathf.Clamp(size, 0.1f, 50f);
            transform.localScale = new Vector3(size, size, size);
            Size = size;
            SetSizeParticles(size);
        }

        protected virtual void SetSizeParticles(float size)
        {
            foreach (ParticleSystem system in GetComponentsInChildren<ParticleSystem>())
            {
                var main = system.main;
                main.startSizeMultiplier *= size;
                main.startSpeedMultiplier *= size;
            }
        }

        public void SetSize(float size)
        {
            Cache.PhotonView.RPC("SetSizeRPC", RpcTarget.All, new object[] { size });
        }

        protected virtual float GetAnimationTime()
        {
            return Cache.Animation[_currentStateAnimation].normalizedTime;
        }

        protected virtual float GetHitboxTime(float normalizedLength)
        {
            return Cache.Animation[_currentStateAnimation].length * normalizedLength / _currentAttackSpeed;
        }

        protected virtual void DamagedGrunt(float chance = 1f)
        {
            if (SettingsManager.SoundSettings.TitanVocalEffect.Value && RandomGen.Roll(chance))
            {
                var grunts = new List<string>() { "Grunt3", "Grunt5" };
                PlaySound(grunts.GetRandomItem());
            }
        }

        protected virtual void NormalGrunt(float chance = 1f)
        {
            if (SettingsManager.SoundSettings.TitanVocalEffect.Value && RandomGen.Roll(chance))
            {
                var grunts = new List<string>() { "Grunt1", "Grunt2", "Grunt4", "Grunt6", "Grunt7", "Grunt8", "Grunt9" };
                PlaySound(grunts.GetRandomItem());
            }
        }

        protected override void Start()
        {
            base.Start();
            if (IsMine())
            {
                if (!IsMainCharacter() || !(this is BaseShifter))
                    StartCoroutine(HandleSpawnCollisionCoroutine(2f, 20f));
                Idle();
            }
        }

        protected IEnumerator HandleSpawnCollisionCoroutine(float time, float maxSpeed)
        {
            while (time > 0f)
            {
                if (Cache.Rigidbody.velocity.magnitude > maxSpeed)
                    Cache.Rigidbody.velocity = Cache.Rigidbody.velocity.normalized * maxSpeed;
                time -= Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
        }

        protected virtual void ToggleSitPushbox(bool sit)
        {
            if (BaseTitanCache.SitPushbox == null)
                return;
            if (sit && !BaseTitanCache.SitPushbox.enabled)
                BaseTitanCache.SitPushbox.enabled = true;
            else if (!sit && BaseTitanCache.SitPushbox.enabled)
                BaseTitanCache.SitPushbox.enabled = false;
        }

        protected virtual void UpdateAnimationColliders()
        {
            ToggleSitPushbox(IsPlayingSitAnimation());
        }

        protected virtual bool IsPlayingSitAnimation()
        {
            return IsPlayingClip(BaseTitanAnimations.SitDown) || IsPlayingClip(BaseTitanAnimations.SitFall) || IsPlayingClip(BaseTitanAnimations.SitIdle)
                || IsPlayingClip(BaseTitanAnimations.SitUp) || IsPlayingClip(BaseTitanAnimations.DieSit);
        }

        protected override string GetFootstepAudio(int phase)
        {
            return phase == 0 ? TitanSounds.Footstep1 : TitanSounds.Footstep2;
        }

        protected override int GetFootstepPhase()
        {
            if (BaseTitanAnimations.Run != "" && Cache.Animation.IsPlaying(BaseTitanAnimations.Run))
            {
                float time = Cache.Animation[BaseTitanAnimations.Run].normalizedTime % 1f;
                return (time >= 0f && time < 0.5f) ? 0 : 1;
            }
            else if (BaseTitanAnimations.Walk != "" && Cache.Animation.IsPlaying(BaseTitanAnimations.Walk))
            {
                float time = Cache.Animation[BaseTitanAnimations.Walk].normalizedTime % 1f;
                return (time >= 0.1f && time < 0.6f) ? 1 : 0;
            }
            return _stepPhase;
        }

        protected virtual void SpawnShatter(Vector3 position)
        {
            RaycastHit hit;
            if (Physics.Raycast(position + Vector3.up * 1f, Vector3.down, out hit, 2f, GroundMask.value))
            {
                EffectSpawner.Spawn(EffectPrefabs.GroundShatter, hit.point + Vector3.up * 0.1f, Quaternion.identity, Size * SizeMultiplier);
            }
        }
    }

    public enum TitanState
    {
        Idle,
        Run,
        Sprint,
        Walk,
        PreJump,
        StartJump,
        Jump,
        Fall,
        Emote,
        Land,
        Attack,
        ArmHurt,
        Special,
        Stun,
        Block,
        Dead,
        Blind,
        SitCripple,
        SitBlind,
        SitFall,
        SitDown,
        SitUp,
        SitIdle,
        Eat,
        Turn,
        WallClimb
    }
}
