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
using UnityEngine.AI;
using Cameras;

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
        public float SprintStaminaRecover = 0.7f;
        public float SprintStaminaConsumption = 1f;
        public float CurrentSprintStamina = 5f;
        public ITargetable TargetEnemy = null;
        protected BaseTitanAnimations BaseTitanAnimations;
        protected override float GroundDistance => 1f;
        private LayerMask TitanGroundMaskLayers = PhysicsLayer.GetMask(PhysicsLayer.TitanMovebox, PhysicsLayer.MapObjectEntities,
                PhysicsLayer.MapObjectTitans, PhysicsLayer.MapObjectCharacters, PhysicsLayer.MapObjectAll);
        public override LayerMask GroundMask => TitanGroundMaskLayers;
        protected virtual float DefaultRunSpeed => 15f;
        protected virtual float DefaultWalkSpeed => 5f;
        protected virtual float DefaultJumpForce => 150f;
        protected virtual float DefaultRotateSpeed => 1f;
        protected virtual float SizeMultiplier => 1f;
        protected virtual float DisableCooldown => 0f;
        public float AttackSpeedMultiplier = 1f;
        public float ConfusedTime = 0;
        public float PreviousAttackSpeedMultiplier = -1f;
        public Dictionary<string, float> AttackSpeeds = new Dictionary<string, float>();
        public float RunSpeedBase;
        public float WalkSpeedBase;
        public float RunSpeedPerLevel;
        public float WalkSpeedPerLevel;
        public float JumpForce;
        public float RotateSpeed;
        public float TurnSpeed;
        public bool LeftArmDisabled;
        public bool RightArmDisabled;
        protected override Vector3 Gravity => Vector3.down * 100f;
        protected virtual float CheckGroundTime => 0.4f;
        protected Vector3 LastTargetDirection;
        protected Vector3 _wallClimbForward;
        protected Quaternion _turnStartRotation;
        protected Quaternion _turnTargetRotation;
        public Vector3 _jumpDirection;
        protected float _maxTurnTime;
        protected float _currentTurnTime;
        protected float _currentGroundDistance;
        protected float _currentCrippleTime;
        protected float _currentFallTotalTime;
        protected float _currentFallStuckTime;
        protected float _disableCooldownLeft;
        protected float _checkGroundTimeLeft;
        protected Vector3 _startPosition;
        protected LayerMask MapObjectMask => PhysicsLayer.GetMask(PhysicsLayer.MapObjectEntities);

        // attacks
        public float _stateTimeLeft;
        protected string _currentAttackAnimation;
        protected string _currentAttack;
        protected string _currentStateAnimation;
        protected float _currentAttackSpeed;
        protected int _currentAttackStage;
        protected bool _needFreshCore;
        protected Vector3 _attackVelocity;
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
                        Animation.SetSpeed(BaseTitanAnimations.Turn90L, Animation.GetSpeed(BaseTitanAnimations.Turn90L) * TurnSpeed);
                    if (BaseTitanAnimations.Turn90R != "")
                        Animation.SetSpeed(BaseTitanAnimations.Turn90R, Animation.GetSpeed(BaseTitanAnimations.Turn90R) * TurnSpeed);
                }
            }
        }

        protected override void CreateDetection()
        {
            Detection = new TitanDetection(this);
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
            SetKinematic(false);
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
            SetKinematic(false);
            if (AI)
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
            _maxTurnTime = Animation.GetTotalTime(animation);
            StateActionWithTime(TitanState.Turn, animation, _maxTurnTime, 0.1f);
        }

        public virtual void Blind()
        {
            if (State != TitanState.Blind && State != TitanState.SitBlind && AI && _disableCooldownLeft <= 0f)
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
            if (BaseTitanAnimations.SitFall != "" && State != TitanState.SitCripple && AI && _disableCooldownLeft <= 0f)
            {
                _currentCrippleTime = time > 0f ? time : DefaultCrippleTime;
                StateAction(TitanState.SitFall, BaseTitanAnimations.SitFall);
                DamagedGrunt();
                _disableCooldownLeft = _currentCrippleTime + DisableCooldown;
            }
        }

        public override void Emote(string emote)
        {
        }

        public override void ForceAnimation(string animation, float fade)
        {
            StateAction(TitanState.Emote, animation, fade);
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
        public virtual void GrabRPC(int viewId, bool left, PhotonMessageInfo info)
        {
            var view = PhotonView.Find(viewId);
            if (view?.Owner != info.Sender)
                return;
            var human = view.GetComponent<Human>();
            if (this.IsMine())
                HoldHuman = human;
            human.Grabber = this;
            if (left)
                human.GrabHand = this.BaseTitanCache.GrabLSocket;
            else
                human.GrabHand = this.BaseTitanCache.GrabRSocket;
        }

        [PunRPC]
        public virtual void UngrabRPC(int viewId, PhotonMessageInfo info)
        {
            var view = PhotonView.Find(viewId);
            if (view?.Owner != info.Sender)
                return;
            var human = view.GetComponent<Human>();
            if (this.IsMine())
                HoldHuman = null;
            human.Grabber = null;
            human.GrabHand = null;
        }

        public virtual void Ungrab()
        {
            if (HoldHuman != null)
            {
                HoldHuman.Cache.PhotonView.RPC("UngrabRPC", RpcTarget.All, new object[0]);
                HoldHuman.GrabHand = null;
                HoldHuman = null;
            }
        }

        [PunRPC]
        public virtual void DecreaseAttackSpeedRPC(PhotonMessageInfo info)
        {
            if (ConfusedTime <= 0)
            {
                PreviousAttackSpeedMultiplier = AttackSpeedMultiplier;
                AttackSpeedMultiplier = AttackSpeedMultiplier * 0.67f;
            }
            ConfusedTime = 10;
        }

        public virtual void Confuse()
        {
            Cache.PhotonView.RPC("DecreaseAttackSpeedRPC", Cache.PhotonView.Owner, new object[0]);
        }

        protected void ResetAttackSpeed()
        {
            if (PreviousAttackSpeedMultiplier >= 0)
            {
                AttackSpeedMultiplier = PreviousAttackSpeedMultiplier;
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
            StateActionWithTime(state, animation, Animation.GetLength(animation), fade, deactivateHitboxes);
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
            _stateTimeLeft = Animation.GetLength(animation) / _currentAttackSpeed;
        }

        protected void StateActionWithTime(TitanState state, string animation, float stateTime, float fade = 0.1f, bool deactivateHitboxes = true)
        {
            _needFreshCore = true;
            SetAnimationUpdateMode(state == TitanState.Jump || state == TitanState.Attack);
            if (state != TitanState.Eat && state != TitanState.HumanThrow)
            {
                Ungrab();
            }
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
            Animation.SetCullingType(always);
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
            ScaleSounds(1f);
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

        protected void SetDefaultVelocityLerp()
        {
            float value = 1f;
            if (this._currentAttack != "AttackBellyFlop" && this._currentAttack != "AttackRockThrow")
                value = 1.47f;
            Vector3 targetVelocity = Vector3.up * Cache.Rigidbody.velocity.y + Vector3.down * Mathf.Min(_currentGroundDistance * 100f, 100f);
            Cache.Rigidbody.velocity = Vector3.Lerp(Cache.Rigidbody.velocity, targetVelocity, Time.deltaTime * value);
        }
        protected virtual void Update()
        {
            UpdateDisableArm();
            UpdateAnimationColliders();
            if (IsMine())
            {
                _disableCooldownLeft -= Time.deltaTime;

                if (!AI && (State == TitanState.Sprint || State == TitanState.Run || State == TitanState.Walk) && IsSit && State != TitanState.SitDown && State != TitanState.SitIdle && State != TitanState.SitUp)
                {
                    StateAction(TitanState.SitDown, BaseTitanAnimations.SitDown);
                    return;
                }
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
                if (State == TitanState.Eat || State == TitanState.HumanThrow)
                    UpdateEat();
                else if (State == TitanState.Turn)
                    UpdateTurn();
                else if (State == TitanState.Attack)
                {
                    UpdateAttack();
                }
                else if (State == TitanState.PreJump && !AI)
                {
                    Vector3 to = GetAimPoint() - BaseTitanCache.Head.position;
                    float time = to.magnitude / JumpForce;
                    float down = 0.5f * Gravity.magnitude * time * time;
                    to.y += down;
                    _jumpDirection = to;
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
                else if (State == TitanState.Land || State == TitanState.CoverNape)
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

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (IsMine())
            {
                _checkGroundTimeLeft -= Time.fixedDeltaTime;
                bool isKinematic = Cache.Rigidbody.isKinematic;
                if ((State == TitanState.Idle || State == TitanState.SitIdle) && AI && Grounded && CurrentSpeed <= 0.1f && _disableKinematicTimeLeft <= 0f)
                {
                    if (!isKinematic)
                        SetKinematic(true);
                    return;
                }
                else
                {
                    if (isKinematic)
                        SetKinematic(false);
                }
                if (Cache.Rigidbody.velocity.y >= 0f)
                    _currentFallTotalTime = 0f;
                else
                    _currentFallTotalTime += Time.fixedDeltaTime;
                if (AI & _currentFallTotalTime >= 10f && Cache.Rigidbody.velocity.y <= Gravity.y * 10f)
                {
                    GetKilledRPC("Gravity");
                }
                if (_checkGroundTimeLeft <= 0f || !AI || State == TitanState.Fall || State == TitanState.StartJump)
                {
                    CheckGround();
                    _checkGroundTimeLeft = CheckGroundTime;
                }
                if (State != TitanState.Fall)
                    _currentFallStuckTime = 0f;
                if (!AI && (State == TitanState.PreJump || State == TitanState.CoverNape || State == TitanState.SitDown || State == TitanState.Dead))
                {
                    SetDefaultVelocityLerp();
                }
                else if (State == TitanState.Jump)
                {
                    if (Cache.Rigidbody.velocity.y <= 1f)
                        Fall();
                }
                else if (State == TitanState.Attack)
                {
                    FixedUpdateAttack();
                    if (Grounded && AI)
                        SetDefaultVelocity();
                    else if (Grounded && !AI)
                        SetDefaultVelocityLerp();
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
                        _currentFallStuckTime += Time.fixedDeltaTime;
                        if (_currentFallStuckTime > 0.5f)
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
                if (_rootMotionAnimations.ContainsKey(_currentStateAnimation) && Animation.IsPlaying(_currentStateAnimation)
                    && Animation.GetCurrentNormalizedTime() < _rootMotionAnimations[_currentStateAnimation])
                {
                    Vector3 coreLocalPosition = BaseTitanCache.Core.position - BaseTitanCache.Transform.position;
                    if (coreLocalPosition.magnitude >= _furthestCoreLocalPosition.magnitude)
                    {
                        Vector3 v = -1f * (coreLocalPosition - _previousCoreLocalPosition) / Time.fixedDeltaTime;
                        _furthestCoreLocalPosition = coreLocalPosition;
                        _previousCoreLocalPosition = coreLocalPosition;

                        if (AI)
                        {
                            Cache.Rigidbody.velocity = v;
                        }
                        else
                        {
                            v = Vector3.Lerp(Vector3.zero, v, 0.0435f);
                            v += Cache.Rigidbody.velocity;
                            Cache.Rigidbody.velocity = v;
                        }
                    }
                }
                if (State != TitanState.WallClimb)
                    Cache.Rigidbody.AddForce(Gravity, ForceMode.Acceleration);
                if (ConfusedTime > 0)
                    ConfusedTime -= Time.fixedDeltaTime;
                else
                    ResetAttackSpeed();
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
            base.LateUpdate();
            if (IsMine())
            {
                if ((State == TitanState.Run || State == TitanState.Walk || State == TitanState.Sprint || State == TitanState.Jump || State == TitanState.Fall) && HasDirection)
                {
                    Cache.Transform.rotation = Quaternion.Lerp(Cache.Transform.rotation, GetTargetRotation(), Time.deltaTime * RotateSpeed);
                }
            }
        }

        protected bool IsPlayingClip(string clip)
        {
            return clip != "" && Animation.IsPlaying(clip);
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
            ScaleSounds(size);
            var camera = (InGameCamera)SceneLoader.CurrentCamera;
            if (camera._follow == this)
                camera.SetFollow(this);
        }

        protected virtual void ScaleSounds(float size)
        {
            if (this is BaseShifter)
                size *= 3.5f;
            float stepVolume = Mathf.Clamp(size * 0.125f, 0.1f, 0.5f);
            Cache.AudioSources[TitanSounds.Fall].volume = Mathf.Clamp(size * 0.3f, 0.1f, 1f);
            Cache.AudioSources[TitanSounds.Footstep1].volume = stepVolume;
            Cache.AudioSources[TitanSounds.Footstep2].volume = stepVolume;
            Cache.AudioSources[TitanSounds.Footstep3].volume = stepVolume;
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
            return Animation.GetCurrentNormalizedTime();
        }

        protected virtual float GetHitboxTime(float normalizedLength)
        {
            return Animation.GetLength(_currentStateAnimation) * normalizedLength / _currentAttackSpeed;
        }

        protected virtual void DamagedGrunt(float chance = 1f)
        {
            if (SettingsManager.SoundSettings.TitanVocalEffect.Value && RandomGen.Roll(chance))
            {
                PlaySound(TitanSounds.GetRandomHurt());
            }
        }

        protected virtual void GrabGrunt(float chance = 1f)
        {
            if (SettingsManager.SoundSettings.TitanVocalEffect.Value && RandomGen.Roll(chance))
            {
                PlaySound(TitanSounds.GetRandomGrabGrunt());
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
            _startPosition = Cache.Transform.position;
        }

        protected IEnumerator HandleSpawnCollisionCoroutine(float time, float maxSpeed)
        {
            while (time > 0f)
            {
                if (!Cache.Rigidbody.isKinematic && Cache.Rigidbody.velocity.magnitude > maxSpeed)
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
            return TitanSounds.GetRandomFootstep();
        }

        protected override int GetFootstepPhase()
        {
            if (BaseTitanAnimations.Run != "" && Animation.IsPlaying(BaseTitanAnimations.Run))
            {
                float time = Animation.GetCurrentNormalizedTime() % 1f;
                return (time >= 0f && time < 0.5f) ? 0 : 1;
            }
            else if (BaseTitanAnimations.Walk != "" && Animation.IsPlaying(BaseTitanAnimations.Walk))
            {
                float time = Animation.GetCurrentNormalizedTime() % 1f;
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

        public virtual bool CheckNapeAngle(Vector3 hitPosition, float maxAngle)
        {
            var nape = BaseTitanCache.Head.transform;
            Vector3 direction = (hitPosition - nape.position).normalized;
            return Vector3.Angle(-nape.forward, direction) < maxAngle;
        }

        public override Vector3 GetCenterPosition()
        {
            return BaseTitanCache.Hip.position;
        }

        public virtual float GetColliderToggleRadius()
        {
            return Size * SizeMultiplier * 20f;
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
        WallClimb,
        CoverNape,
        HumanThrow
    }
}
