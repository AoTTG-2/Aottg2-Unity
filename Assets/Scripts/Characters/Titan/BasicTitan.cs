using UnityEngine;
using ApplicationManagers;
using Utility;
using Controllers;
using System.Collections.Generic;
using SimpleJSONFixed;
using System.Collections;
using Effects;
using UI;
using Settings;
using CustomLogic;
using Photon.Pun;
using Projectiles;
using Spawnables;

namespace Characters
{
    class BasicTitan : BaseTitan
    {
        public BasicTitanComponentCache BasicCache;
        protected BasicTitanAnimations BasicAnimations;
        public bool IsCrawler;
        protected string _runAnimation;
        public BasicTitanSetup Setup;
        public Quaternion _oldHeadRotation;
        public Quaternion? LateUpdateHeadRotation = Quaternion.identity;
        public Quaternion? LateUpdateHeadRotationRecv = Quaternion.identity;
        public Vector2 LastGoodHeadAngle = Vector2.zero;
        public float BellyFlopTime = 5.5f;
        protected float _leftArmDisabledTimeLeft;
        protected float _rightArmDisabledTimeLeft;
        protected float ArmDisableTime = 12f;
        public float RockThrow1Speed = 150f;
        protected Vector3 _rockThrowTarget;
        protected float _originalCapsuleValue;
        public int TargetViewId = -1;
        public bool LookAtTarget = false;
        public override bool CanSprint => true;
        public override bool CanWallClimb => true;

        public override List<string> EmoteActions => new List<string>() { "Laugh", "Nod", "Shake", "Roar" };
        private Vector3 _cutHandSize = new Vector3(0.01f, 0.01f, 0.01f);
        private TitanCustomSet _customSet;

        public void Init(bool ai, string team, JSONNode data, TitanCustomSet customSet)
        {
            _customSet = customSet;
            if (ai)
            {
                var controller = gameObject.AddComponent<BaseTitanAIController>();
                controller.Init(data);
                Name = data["Name"].Value;
                IsCrawler = data["IsCrawler"].AsBool;
            }
            else
                gameObject.AddComponent<BasicTitanPlayerController>();
            if (IsCrawler)
                _runAnimation = BasicAnimations.RunCrawler;
            else
            {
                int runAnimationType = 1;
                if (data != null && data.HasKey("RunAnimation"))
                    runAnimationType = data["RunAnimation"].AsInt;
                if (runAnimationType == 0)
                    _runAnimation = BasicAnimations.Runs[UnityEngine.Random.Range(0, BasicAnimations.Runs.Length)];
                else
                    _runAnimation = BasicAnimations.Runs[runAnimationType - 1];
            }
            Cache.PhotonView.RPC("SetCrawlerRPC", RpcTarget.AllBuffered, new object[] { IsCrawler });
            base.Init(ai, team, data);
            Animation.SetSpeed(BasicAnimations.CoverNape, 1.5f);
        }

        public override bool IsGrabAttack()
        {
            return _currentAttack.StartsWith("AttackGrab");
        }

        protected override Dictionary<string, float> GetRootMotionAnimations()
        {
            return new Dictionary<string, float>() { {BasicAnimations.AttackBellyFlop, 1f },
                { BasicAnimations.AttackBellyFlopGetup, 1f }, { BasicAnimations.AttackPunch, 1f }, { BasicAnimations.AttackPunchCombo, 1f} };
        }

        protected override void SetSizeParticles(float size)
        {
            base.SetSizeParticles(size);
            foreach (ParticleSystem system in new ParticleSystem[] { BasicCache.ForearmSmokeL, BasicCache.ForearmSmokeR })
            {
                system.startSize *= size;
                system.startSpeed *= size;
            }
            BasicCache.ForearmSmokeL.transform.localScale = Vector3.one * Size;
            BasicCache.ForearmSmokeR.transform.localScale = Vector3.one * Size;
        }

        [PunRPC]
        public void SetCrawlerRPC(bool crawler, PhotonMessageInfo info)
        {
            if (info.Sender == Cache.PhotonView.Owner)
            {
                IsCrawler = crawler;
                var capsule = (CapsuleCollider)BasicCache.Movebox;
                if (crawler)
                {
                    capsule.direction = 2;
                    capsule.radius *= 0.5f;
                    capsule.center = new Vector3(0f, capsule.radius, 0f);
                    _originalCapsuleValue = capsule.height;
                    var nape = (CapsuleCollider)BasicCache.NapeHurtbox;
                    nape.radius *= 1f;
                }
                else
                    _originalCapsuleValue = capsule.radius;
            }
        }

        [PunRPC]
        public void ClearRockRPC(PhotonMessageInfo info)
        {
            if (info.Sender == Cache.PhotonView.Owner)
            {
                foreach (Rock1Spawnable rock in BasicCache.HandRHitbox.GetComponentsInChildren<Rock1Spawnable>())
                    Destroy(rock.gameObject);
            }
        }

        public override Transform GetCameraAnchor()
        {
            return Cache.Transform;
        }

        protected override void Start()
        {
            _inGameManager.RegisterCharacter(this);
            base.Start();
            if (IsMine())
            {
                string setup = _customSet.SerializeToJsonString();
                Cache.PhotonView.RPC("SetupRPC", RpcTarget.AllBuffered, new object[] { setup });
                EffectSpawner.Spawn(EffectPrefabs.TitanSpawn, Cache.Transform.position, Quaternion.Euler(-90f, 0f, 0f), GetSpawnEffectSize());
            }
        }

        [PunRPC]
        public void SetupRPC(string json, PhotonMessageInfo info)
        {
            if (info.Sender != Cache.PhotonView.Owner)
                return;
            var set = new TitanCustomSet();
            set.DeserializeFromJsonString(json);
            Setup.Load(set);
        }

        protected override void CreateCache(BaseComponentCache cache)
        {
            BasicCache = new BasicTitanComponentCache(gameObject);
            base.CreateCache(BasicCache);
        }

        protected override void CreateAnimations(BaseTitanAnimations animations)
        {
            BasicAnimations = new BasicTitanAnimations();
            base.CreateAnimations(BasicAnimations);
        }

        public override void Emote(string emote)
        {
            if (CanAction())
            {
                string anim = string.Empty;
                if (emote == "Laugh")
                {
                    anim = BasicAnimations.EmoteLaugh;
                    StartCoroutine(WaitAndPlaySound(TitanSounds.GetRandomLaugh(), 0.5f));
                }
                else if (emote == "Nod")
                    anim = BasicAnimations.EmoteNod;
                else if (emote == "Shake")
                    anim = BasicAnimations.EmoteShake;
                else if (emote == "Roar")
                {
                    anim = BasicAnimations.EmoteRoar;
                    StartCoroutine(WaitAndPlaySound(TitanSounds.Roar, 1.4f));
                }
                StateAction(TitanState.Emote, anim);
            }
        }

        public void CoverNape()
        {
            if (CanAction())
            {
                StateActionWithTime(TitanState.CoverNape, BasicAnimations.CoverNape, Animation.GetTotalTime(BasicAnimations.CoverNape));
            }
        }

        public override void DisableArm(bool left)
        {
            if (!AI)
                return;
            if (left && !LeftArmDisabled)
            {
                Cache.PhotonView.RPC("DisableArmRPC", RpcTarget.All, new object[] { left });
                if (HoldHuman != null && HoldHumanLeft)
                {
                    Ungrab();
                }
                bool isBellyFlop = _currentStateAnimation == BasicAnimations.AttackBellyFlop || _currentStateAnimation == BasicAnimations.AttackBellyFlopGetup;
                if (State != TitanState.SitDown && State != TitanState.SitUp && State != TitanState.SitFall && State != TitanState.SitIdle &&
                    State != TitanState.Fall && State != TitanState.Jump && State != TitanState.PreJump && State != TitanState.SitBlind &&
                    State != TitanState.SitCripple && State != TitanState.StartJump && !isBellyFlop)
                    StateAction(TitanState.ArmHurt, BasicAnimations.ArmHurtL);
                DamagedGrunt();
            }
            else if (!left && !RightArmDisabled)
            {
                Cache.PhotonView.RPC("DisableArmRPC", RpcTarget.All, new object[] { left });
                if (HoldHuman != null && !HoldHumanLeft)
                {
                    Ungrab();
                }
                bool isBellyFlop = _currentStateAnimation == BasicAnimations.AttackBellyFlop || _currentStateAnimation == BasicAnimations.AttackBellyFlopGetup;
                if (State != TitanState.SitDown && State != TitanState.SitUp && State != TitanState.SitFall && State != TitanState.SitIdle &&
                    State != TitanState.Fall && State != TitanState.Jump && State != TitanState.PreJump && State != TitanState.SitBlind &&
                    State != TitanState.SitCripple && State != TitanState.StartJump && !isBellyFlop)
                    StateAction(TitanState.ArmHurt, BasicAnimations.ArmHurtR);
                DamagedGrunt();
            }
        }

        public override bool CanAttack()
        {
            return base.CanAttack();
        }

        [PunRPC]
        public virtual void DisableArmRPC(bool left, PhotonMessageInfo info)
        {
            if (info.Sender != Cache.PhotonView.Owner)
                return;
            if (left)
            {
                BasicCache.ForearmBloodL.Play(true);
                _leftArmDisabledTimeLeft = ArmDisableTime;
                LeftArmDisabled = true;
            }
            else
            {
                BasicCache.ForearmBloodR.Play(true);
                _rightArmDisabledTimeLeft = ArmDisableTime;
                RightArmDisabled = true;
            }
        }

        public void Laugh(BaseCharacter character)
        {
            Cache.PhotonView.RPC("LaughRPC", Cache.PhotonView.Owner, new object[] { character.Cache.Transform.position });
        }

        public void Distract(BaseCharacter character)
        {
            Cache.PhotonView.RPC("DistractRPC", Cache.PhotonView.Owner, new object[] { character.Cache.PhotonView.ViewID });
        }

        [PunRPC]
        public virtual void LaughRPC(Vector3 source)
        {
            if (!AI || !Cache.PhotonView.IsMine)
                return;
            if (Vector3.Angle(Cache.Transform.forward, (source - Cache.Transform.position).normalized) < 80f)
                Emote("Laugh");
        }

        [PunRPC]
        public virtual void DistractRPC(int viewId)
        {
            if (!AI || !Cache.PhotonView.IsMine)
                return;
            var character = Util.FindCharacterByViewId(viewId);
            GetComponent<BaseTitanAIController>().SetEnemy(character, 10f);
        }

        protected override void UpdateDisableArm()
        {
            if (LeftArmDisabled)
            {
                _leftArmDisabledTimeLeft -= Time.deltaTime;
                if (ArmDisableTime - _leftArmDisabledTimeLeft > 2.5f && !BasicCache.ForearmSmokeL.isPlaying)
                    BasicCache.ForearmSmokeL.Play();
                if (_leftArmDisabledTimeLeft <= 0f)
                {
                    LeftArmDisabled = false;
                    BasicCache.ForearmSmokeL.Stop();
                }
            }
            if (RightArmDisabled)
            {
                _rightArmDisabledTimeLeft -= Time.deltaTime;
                if (ArmDisableTime - _rightArmDisabledTimeLeft > 2.5f && !BasicCache.ForearmSmokeR.isPlaying)
                    BasicCache.ForearmSmokeR.Play();
                if (_rightArmDisabledTimeLeft <= 0f)
                {
                    RightArmDisabled = false;
                    BasicCache.ForearmSmokeR.Stop();
                }
            }
        }

        public override void Run()
        {
            _stepPhase = 0;
            StateActionWithTime(TitanState.Run, _runAnimation, 0f, 0.5f);
            if (IsCrawler && !BasicCache.CrawlerHitbox.IsActive())
                BasicCache.CrawlerHitbox.Activate();
        }

        public override void WallClimb()
        {
            _stepPhase = 0;
            StateActionWithTime(TitanState.WallClimb, BasicAnimations.RunCrawler, 0f, 0.1f);
        }

        public override void Jump(Vector3 direction)
        {
            _jumpDirection = direction;
            if (IsCrawler)
            {
                StateAction(TitanState.PreJump, BasicAnimations.JumpCrawler);
            }
            else
            {
                float stateTime = Animation.GetLength(BasicAnimations.Jump) / 2f;
                StateActionWithTime(TitanState.PreJump, BaseTitanAnimations.Jump, stateTime, 0.1f);
            }
        }

        public virtual void StunDirectional(bool left)
        {
            if (CanStun())
            {
                if (left)
                    StateActionWithTime(TitanState.Stun, BasicAnimations.StunLeft, StunTime, 0.1f);
                else
                    StateActionWithTime(TitanState.Stun, BasicAnimations.StunRight, StunTime, 0.1f);
            }
        }

        public override void StartJump()
        {
            base.StartJump();
            BasicCache.MouthHitbox.Activate();
        }

        public void JumpImmediate()
        {
            StartJump();
            CrossFade(BasicAnimations.Jump, 0.1f, 0.85f);
        }

        public override void Eat()
        {
            if (HoldHuman == null)
                return;
            if (HoldHumanLeft)
                StateAction(TitanState.Eat, BasicAnimations.AttackEatL);
            else
                StateAction(TitanState.Eat, BasicAnimations.AttackEatR);
            GrabGrunt();
        }

        public override void Land()
        {
            if (IsCrawler)
                StateAction(TitanState.Land, BasicAnimations.LandCrawler);
            else
                StateAction(TitanState.Land, BaseTitanAnimations.Land);
            EffectSpawner.Spawn(EffectPrefabs.Boom2, Cache.Transform.position + Vector3.down * _currentGroundDistance,
                            Quaternion.Euler(270f, 0f, 0f), Size * SizeMultiplier);
        }

        public override void Fall()
        {
            if (IsCrawler)
                StateActionWithTime(TitanState.Fall, BasicAnimations.FallCrawler, 0f, 0.1f);
            else
                StateActionWithTime(TitanState.Fall, BaseTitanAnimations.Fall, 0f, 0.1f);
        }

        public override void Idle(float fadeTime)
        {
            if (IsCrawler)
                StateActionWithTime(TitanState.Idle, BasicAnimations.IdleCrawler, 0f, fadeTime);
            else
                StateActionWithTime(TitanState.Idle, BasicAnimations.Idle, 0f, fadeTime);
        }

        public override void Turn(Vector3 targetDirection)
        {
            if (!CanAction())
                return;
            float angle = GetAngleToTarget(targetDirection);
            string animation;
            if (IsCrawler)
            {
                if (angle > 0f)
                    animation = BasicAnimations.Turn90RCrawler;
                else
                    animation = BasicAnimations.Turn90LCrawler;
            }
            else
            {
                if (angle > 0f)
                    animation = BaseTitanAnimations.Turn90R;
                else
                    animation = BaseTitanAnimations.Turn90L;
            }
            targetDirection = Vector3.RotateTowards(Cache.Transform.forward, targetDirection, 120f * Mathf.Deg2Rad, float.MaxValue);
            _turnStartRotation = Cache.Transform.rotation;
            _turnTargetRotation = Quaternion.LookRotation(targetDirection);
            _currentTurnTime = 0f;
            _maxTurnTime = Animation.GetLength(animation) * 0.71f / Animation.GetSpeed(animation);
            StateActionWithTime(TitanState.Turn, animation, _maxTurnTime, 0.1f);
        }

        [PunRPC]
        public override void MarkDeadRPC(PhotonMessageInfo info)
        {
            if (info.Sender != Cache.PhotonView.Owner)
                return;
            Dead = true;
            if (SettingsManager.GraphicsSettings.NapeBloodEnabled.Value)
                BasicCache.NapeBlood.Play(true);
        }

        protected override IEnumerator WaitAndDie()
        {
            string dieAnimation = BasicAnimations.DieFront;
            if (State == TitanState.Stun)
                dieAnimation = BasicAnimations.DieBack;
            if (IsCrawler)
                dieAnimation = BasicAnimations.DieCrawler;
            else if (_currentStateAnimation == BasicAnimations.AttackBellyFlop || _currentStateAnimation == BasicAnimations.AttackBellyFlopGetup)
                dieAnimation = BasicAnimations.DieGround;
            else if (_currentStateAnimation == BasicAnimations.SitFall || _currentStateAnimation == BasicAnimations.SitIdle
                || _currentStateAnimation == BasicAnimations.SitBlind)
                dieAnimation = BasicAnimations.DieSit;
            StateActionWithTime(TitanState.Dead, dieAnimation, 0f, 0.05f);
            yield return new WaitForSeconds(1.4f);
            PlaySound(TitanSounds.Fall);
            yield return new WaitForSeconds(1f);
            EffectSpawner.Spawn(EffectPrefabs.TitanDie1, BaseTitanCache.Hip.position, Quaternion.Euler(-90f, 0f, 0f), GetSpawnEffectSize(), false);
            yield return new WaitForSeconds(2f);
            yield return new WaitForSeconds(2f);
            EffectSpawner.Spawn(EffectPrefabs.TitanDie2, BaseTitanCache.Hip.position, Quaternion.Euler(-90f, 0f, 0f), GetSpawnEffectSize(), false);
            PhotonNetwork.Destroy(gameObject);
        }

        protected override void Awake()
        {
            base.Awake();
            Setup = gameObject.AddComponent<BasicTitanSetup>();
            Animation.SetSpeed(BasicAnimations.Jump, 2f);
        }

        [PunRPC]
        public override void GetHitRPC(int viewId, string name, int damage, string type, string collider)
        {
            if (Dead)
                return;
            var settings = SettingsManager.InGameCurrent.Titan;
            if (type == "CannonBall" || type == "Rock")
            {
                base.GetHitRPC(viewId, name, damage, type, collider);
                return;
            }
            if (settings.TitanArmorEnabled.Value && (!IsCrawler || settings.TitanArmorCrawlerEnabled.Value))
            {
                if (damage < settings.TitanArmor.Value)
                    damage = 0;
            }
            if (type == "TitanStun" || type == "ShifterStun")
            {
                if (!IsCrawler)
                {
                    var killer = Util.FindCharacterByViewId(viewId);
                    if (killer != null)
                    {
                        Vector3 direction = killer.Cache.Transform.position - Cache.Transform.position;
                        direction.y = 0f;
                        Cache.Transform.forward = direction.normalized;
                        Vector3 local = Cache.Transform.InverseTransformPoint(killer.Cache.Transform.position);
                        if (local.x < 0f)
                            StunDirectional(true);
                        else
                            StunDirectional(false);
                    }
                    else
                        StunDirectional(true);
                }
                base.GetHitRPC(viewId, name, damage, type, collider);
            }
            else if (BaseTitanCache.EyesHurtbox != null && collider == BaseTitanCache.EyesHurtbox.name)
                Blind();
            else if (BaseTitanCache.LegLHurtbox != null && (collider == BaseTitanCache.LegLHurtbox.name || collider == BaseTitanCache.LegRHurtbox.name))
                Cripple();
            else if (collider == BasicCache.ForearmLHurtbox.name)
            {
                if (IsCrawler)
                    Cripple();
                else
                    DisableArm(true);
            }
            else if (collider == BasicCache.ForearmRHurtbox.name)
            {
                if (IsCrawler)
                    Cripple();
                else
                    DisableArm(false);
            }
            else if (collider == BaseTitanCache.NapeHurtbox.name)
                base.GetHitRPC(viewId, name, damage, type, collider);
        }

        public override void Kick()
        {
            Attack("AttackKick");
        }

        public override void Attack(string attack)
        {
            if (!Grounded && (attack == "AttackBellyFlop" || attack == "AttackRockThrow"))
                return;
            if (!AI)
                _attackVelocity = new Vector3(Cache.Rigidbody.velocity.x, 0f, Cache.Rigidbody.velocity.z);
            ResetAttackState(attack);
            if (_currentAttackAnimation == BasicAnimations.AttackBellyFlop)
                StateActionWithTime(TitanState.Attack, _currentAttackAnimation, BellyFlopTime, 0.1f);
            else if (_currentAttackAnimation == BasicAnimations.AttackRockThrow)
            {
                if (!AI)
                    _rockThrowTarget = GetAimPoint();
                StateAttack(_currentAttackAnimation);
            }
            else if (_currentAttackAnimation == BasicAnimations.Jump)
            {
                if (AI)
                {
                    if (TargetEnemy != null && TargetEnemy.ValidTarget())
                    {
                        Vector3 to = TargetEnemy.GetPosition() - BasicCache.Head.position;
                        float time = to.magnitude / JumpForce;
                        float down = 0.5f * Gravity.magnitude * time * time;
                        to.y += down;
                        Jump(to.normalized);
                    }
                    else
                        Jump(Vector3.up);
                }
                else
                {
                    Vector3 to = GetAimPoint() - BasicCache.Head.position;
                    float time = to.magnitude / JumpForce;
                    float down = 0.5f * Gravity.magnitude * time * time;
                    to.y += down;
                    Jump(to.normalized);
                }
            }
            else if (_currentAttackAnimation == BasicAnimations.JumpCrawler)
            {
                if (TargetEnemy != null && TargetEnemy.ValidTarget())
                {
                    Vector3 to = TargetEnemy.GetPosition() - BasicCache.Head.position;
                    float time = to.magnitude / JumpForce;
                    float down = 0.5f * Gravity.magnitude * time * time;
                    to.y += down;
                    Jump(to.normalized);
                }
                else
                    Jump(Cache.Transform.forward + Vector3.up);
            }
            else
                StateAttack(_currentAttackAnimation);
        }

        protected override void UpdateAttack()
        {
            float animationTime = GetAnimationTime();
            var rotation = Quaternion.Euler(270f, 0f, 0f);
            if (_currentAttackAnimation == BasicAnimations.AttackPunchCombo)
            {
                if (_currentAttackStage == 0 && animationTime > 0.115f)
                {
                    PlaySound(TitanSounds.Swing1);
                    BasicCache.HandRHitbox.Activate(GetHitboxTime(0.005f), GetHitboxTime(0.025f));
                    _currentAttackStage = 1;
                }
                else if (_currentAttackStage == 1 && animationTime > 0.265f)
                {
                    PlaySound(TitanSounds.Swing2);
                    BasicCache.HandLHitbox.Activate(GetHitboxTime(0.005f), GetHitboxTime(0.025f));
                    _currentAttackStage = 2;
                }
                else if (_currentAttackStage == 2 && animationTime > 0.48f)
                {
                    PlaySound(TitanSounds.Swing3);
                    BasicCache.HandLHitbox.Activate(GetHitboxTime(0.02f), GetHitboxTime(0.02f));
                    BasicCache.HandRHitbox.Activate(GetHitboxTime(0.02f), GetHitboxTime(0.02f));
                    _currentAttackStage = 3;
                }
                else if (_currentAttackStage == 3 && animationTime > 0.52f)
                {
                    var position = BasicCache.Transform.position + BasicCache.Transform.forward * 7f * Size;
                    EffectSpawner.Spawn(EffectPrefabs.Boom1, position, rotation, Size);
                    SpawnShatter(position);
                    _currentAttackStage = 4;
                }
            }
            else if (_currentAttackAnimation == BasicAnimations.AttackPunch)
            {
                if (AI)
                {
                    if (_currentAttackStage == 0 && animationTime > 0.22f)
                    {
                        PlaySound(TitanSounds.Swing1);
                        BasicCache.HandRHitbox.Activate(0f, GetHitboxTime(0.04f));
                        _currentAttackStage = 1;
                    }
                    else if (_currentAttackStage == 1 && animationTime > 0.505f)
                    {
                        PlaySound(TitanSounds.Swing2);
                        BasicCache.HandLHitbox.Activate(0f, GetHitboxTime(0.04f));
                        _currentAttackStage = 2;
                    }
                }
                else
                {
                    if (_currentAttackStage == 0 && animationTime > 0.2f)
                    {
                        PlaySound(TitanSounds.Swing1);
                        BasicCache.HandRHitbox.Activate(0f, GetHitboxTime(0.06f));
                        _currentAttackStage = 1;
                    }
                    else if (_currentAttackStage == 1 && animationTime > 0.49f)
                    {
                        PlaySound(TitanSounds.Swing2);
                        BasicCache.HandLHitbox.Activate(0f, GetHitboxTime(0.06f));
                        _currentAttackStage = 2;
                    }
                }
            }
            else if (_currentAttackAnimation == BasicAnimations.AttackSlam)
            {
                if (_currentAttackStage == 0 && animationTime > 0.42f)
                {
                    PlaySound(TitanSounds.Swing3);
                    BasicCache.HandLHitbox.Activate(GetHitboxTime(0.02f), GetHitboxTime(0.01f));
                    BasicCache.HandRHitbox.Activate(GetHitboxTime(0.02f), GetHitboxTime(0.01f));
                    _currentAttackStage = 1;
                }
                else if (_currentAttackStage == 1 && animationTime > 0.45f)
                {
                    var position = BasicCache.Transform.position + BasicCache.Transform.forward * 7f * Size;
                    EffectSpawner.Spawn(EffectPrefabs.Boom1, position, rotation, Size);
                    SpawnShatter(position);
                    _currentAttackStage = 2;
                }
            }
            else if (_currentAttackAnimation == BasicAnimations.AttackBellyFlop)
            {
                if (_currentAttackStage == 0 && animationTime > 0.65f)
                {
                    _currentAttackStage = 1;
                    BasicCache.BodyHitbox.Activate(0f, GetHitboxTime(0.155f));
                }
                else if (_currentAttackStage == 1 && animationTime > 0.805f)
                {
                    _currentAttackStage = 2;
                    var position = Cache.Transform.position + Cache.Transform.forward * 5f;
                    EffectSpawner.Spawn(EffectPrefabs.Boom4, position, Quaternion.Euler(270f, Cache.Transform.rotation.eulerAngles.y, 0f), Size);
                }
                else if (_currentAttackStage == 2 && _stateTimeLeft < 1.1f)
                {
                    _currentAttackStage = 3;
                    CrossFade(BasicAnimations.AttackBellyFlopGetup, 0.1f);
                }
            }
            else if (_currentStateAnimation == BasicAnimations.AttackHitBack)
            {
                if (_currentAttackStage == 0 && animationTime > 0.635f)
                {
                    _currentAttackStage = 1;
                    BasicCache.HandRHitbox.Activate(0f, GetHitboxTime(0.055f));
                }
                else if (_currentAttackStage == 1 && animationTime > 0.68f)
                {
                    _currentAttackStage = 2;
                    EffectSpawner.Spawn(EffectPrefabs.Boom3, BasicCache.HandRHitbox.transform.position, rotation, Size);
                }
            }
            else if (_currentStateAnimation == BasicAnimations.AttackHitFace)
            {
                if (_currentAttackStage == 0 && animationTime > 0.635f)
                {
                    _currentAttackStage = 1;
                    BasicCache.HandRHitbox.Activate(0f, GetHitboxTime(0.055f));
                }
                else if (_currentAttackStage == 1 && animationTime > 0.68f)
                {
                    _currentAttackStage = 2;
                    EffectSpawner.Spawn(EffectPrefabs.Boom3, BasicCache.HandRHitbox.transform.position, rotation, Size);
                }
            }
            else if (_currentAttack.StartsWith("AttackSlap"))
            {
                float t1 = 0.33f;
                float t2 = 0.14f;
                if (!AI)
                {
                    t1 = 0.3f;
                    t2 = 0.2f;
                }
                if (_currentAttackStage == 0 && animationTime > t1)
                {
                    PlaySound(TitanSounds.Swing1);
                    if (_currentStateAnimation == BasicAnimations.AttackSlapL || _currentStateAnimation == BasicAnimations.AttackSlapHighL ||
                        _currentStateAnimation == BasicAnimations.AttackSlapLowL)
                        BasicCache.HandLHitbox.Activate(0f, GetHitboxTime(t2));
                    else
                        BasicCache.HandRHitbox.Activate(0f, GetHitboxTime(t2));
                    _currentAttackStage = 1;
                }
            }
            else if (_currentAttackAnimation == BasicAnimations.AttackKick)
            {
                if (_currentAttackStage == 0 && animationTime > 0.39f)
                {
                    BasicCache.FootLHitbox.Activate(0f, GetHitboxTime(0.09f));
                    _currentAttackStage = 1;
                }
                else if (_currentAttackStage == 1 && animationTime > 0.43f)
                {
                    _currentAttackStage = 2;
                    var position = BasicCache.FootLHitbox.transform.position;
                    position.y = BasicCache.Transform.position.y;
                    EffectSpawner.Spawn(EffectPrefabs.Boom5, position, BasicCache.Transform.rotation, Size);
                    SpawnShatter(position);
                }
            }
            else if (_currentAttackAnimation == BasicAnimations.AttackStomp)
            {
                if (_currentAttackStage == 0 && animationTime > 0.38f)
                {
                    BasicCache.FootLHitbox.Activate(0f, GetHitboxTime(0.06f));
                    _currentAttackStage = 1;
                }
                else if (_currentAttackStage == 1 && animationTime > 0.43f)
                {
                    _currentAttackStage = 2;
                    var position = BasicCache.FootLHitbox.transform.position;
                    position.y = BasicCache.Transform.position.y;
                    EffectSpawner.Spawn(EffectPrefabs.Boom2, position, rotation, Size);
                    SpawnShatter(position);
                }
            }
            else if (_currentAttack.StartsWith("AttackSwing"))
            {
                if (_currentAttackStage == 0 && animationTime > 0.41f)
                {
                    PlaySound(TitanSounds.Swing1);
                    if (_currentStateAnimation == BasicAnimations.AttackSwingL)
                        BasicCache.HandLHitbox.Activate(GetHitboxTime(0.02f), GetHitboxTime(0.02f));
                    else
                        BasicCache.HandRHitbox.Activate(GetHitboxTime(0.02f), GetHitboxTime(0.02f));
                    _currentAttackStage = 1;
                }
                else if (_currentAttackStage == 1 && animationTime > 0.45f)
                {
                    Vector3 position;
                    if (_currentStateAnimation == BasicAnimations.AttackSwingL)
                    {
                        position = BasicCache.HandLHitbox.transform.position;
                        position.y = BasicCache.Transform.position.y;
                    }
                    else
                    {
                        position = BasicCache.HandRHitbox.transform.position;
                        position.y = BasicCache.Transform.position.y;
                    }
                    EffectSpawner.Spawn(EffectPrefabs.Boom1, position, rotation, Size);
                    SpawnShatter(position);
                    _currentAttackStage = 2;
                }
            }
            else if (_currentAttack.StartsWith("AttackBite"))
            {
                float stage1Time;
                float stage2Time;
                if (_currentStateAnimation == BasicAnimations.AttackBiteF)
                {
                    stage1Time = 0.53f;
                    stage2Time = 0.62f;
                    if (!AI)
                    {
                        stage1Time = 0.47f;
                        stage2Time = 0.63f;
                    }
                }
                else
                {
                    stage1Time = 0.33f;
                    stage2Time = 0.42f;
                    if (!AI)
                    {
                        stage1Time = 0.28f;
                        stage2Time = 0.43f;
                    }
                }
                if (_currentAttackStage == 0 && animationTime > stage1Time)
                {
                    BasicCache.MouthHitbox.Activate(0f, GetHitboxTime(0.09f));
                    _currentAttackStage = 1;
                }
                else if (_currentAttackStage == 1 && animationTime > stage2Time)
                {
                    var transform = BasicCache.MouthHitbox.transform;
                    var position = transform.position + transform.up * Size;
                    EffectSpawner.Spawn(EffectPrefabs.TitanBite, position, rotation, Size);
                    PlaySound(TitanSounds.GetRandomBite());
                    _currentAttackStage = 2;
                }
            }
            else if (_currentAttack.StartsWith("AttackBrush"))
            {
                float t1 = 0.55f;
                float t2 = 0.1f;
                if (!AI)
                {
                    t1 = 0.37f;
                    t2 = 0.28f;
                }
                if (_currentAttackStage == 0 && animationTime > t1)
                {
                    PlaySound(TitanSounds.Swing1);
                    if (_currentStateAnimation == BasicAnimations.AttackBrushChestL)
                        BasicCache.HandLHitbox.Activate(0f, GetHitboxTime(t2));
                    else
                        BasicCache.HandRHitbox.Activate(0f, GetHitboxTime(t2));
                    _currentAttackStage = 1;
                }
            }
            else if (_currentAttack.StartsWith("AttackGrab"))
            {
                if (_currentAttackStage == 0)
                {
                    _currentAttackStage = 1;
                    if (_currentStateAnimation == BasicAnimations.AttackGrabCoreL)
                        BasicCache.HandLHitbox.Activate(GetHitboxTime(0.41f), GetHitboxTime(0.12f));
                    else if (_currentStateAnimation == BasicAnimations.AttackGrabCoreR)
                        BasicCache.HandRHitbox.Activate(GetHitboxTime(0.41f), GetHitboxTime(0.12f));
                    else if (_currentStateAnimation == BasicAnimations.AttackGrabBackL)
                        BasicCache.HandLHitbox.Activate(GetHitboxTime(0.32f), GetHitboxTime(0.16f));
                    else if (_currentStateAnimation == BasicAnimations.AttackGrabBackR)
                        BasicCache.HandRHitbox.Activate(GetHitboxTime(0.38f), GetHitboxTime(0.3f));
                    else if (_currentStateAnimation == BasicAnimations.AttackGrabStomachL)
                        BasicCache.HandLHitbox.Activate(GetHitboxTime(0.35f), GetHitboxTime(0.19f));
                    else if (_currentStateAnimation == BasicAnimations.AttackGrabStomachR)
                        BasicCache.HandRHitbox.Activate(GetHitboxTime(0.35f), GetHitboxTime(0.19f));
                    else if (_currentStateAnimation == BasicAnimations.AttackGrabHeadBackL)
                        BasicCache.HandRHitbox.Activate(GetHitboxTime(0.46f), GetHitboxTime(0.05f));
                    else if (_currentStateAnimation == BasicAnimations.AttackGrabHeadBackR)
                        BasicCache.HandLHitbox.Activate(GetHitboxTime(0.46f), GetHitboxTime(0.05f));
                    else if (_currentStateAnimation == BasicAnimations.AttackGrabHeadFrontL)
                        BasicCache.HandLHitbox.Activate(GetHitboxTime(0.42f), GetHitboxTime(0.25f));
                    else if (_currentStateAnimation == BasicAnimations.AttackGrabHeadFrontR)
                        BasicCache.HandRHitbox.Activate(GetHitboxTime(0.42f), GetHitboxTime(0.25f));
                    else if (_currentStateAnimation == BasicAnimations.AttackGrabGroundFrontL)
                        BasicCache.HandLHitbox.Activate(GetHitboxTime(0.42f), GetHitboxTime(0.16f));
                    else if (_currentStateAnimation == BasicAnimations.AttackGrabGroundFrontR)
                        BasicCache.HandRHitbox.Activate(GetHitboxTime(0.42f), GetHitboxTime(0.16f));
                    else if (_currentStateAnimation == BasicAnimations.AttackGrabGroundBackL)
                        BasicCache.HandLHitbox.Activate(GetHitboxTime(0.35f), GetHitboxTime(0.05f));
                    else if (_currentStateAnimation == BasicAnimations.AttackGrabGroundBackR)
                        BasicCache.HandRHitbox.Activate(GetHitboxTime(0.35f), GetHitboxTime(0.05f));
                    else if (_currentStateAnimation == BasicAnimations.AttackGrabAirL)
                        BasicCache.HandLHitbox.Activate(GetHitboxTime(0.3f), GetHitboxTime(0.15f));
                    else if (_currentStateAnimation == BasicAnimations.AttackGrabAirR)
                        BasicCache.HandRHitbox.Activate(GetHitboxTime(0.38f), GetHitboxTime(0.17f));
                    else if (_currentStateAnimation == BasicAnimations.AttackGrabAirFarL)
                        BasicCache.HandLHitbox.Activate(GetHitboxTime(0.38f), GetHitboxTime(0.17f));
                    else if (_currentStateAnimation == BasicAnimations.AttackGrabAirFarR)
                        BasicCache.HandRHitbox.Activate(GetHitboxTime(0.38f), GetHitboxTime(0.17f));
                    else if (_currentStateAnimation == BasicAnimations.AttackGrabHighL)
                        BasicCache.HandLHitbox.Activate(GetHitboxTime(0.3f), GetHitboxTime(0.17f));
                    else if (_currentStateAnimation == BasicAnimations.AttackGrabHighR)
                        BasicCache.HandRHitbox.Activate(GetHitboxTime(0.3f), GetHitboxTime(0.17f));
                }
            }
            else if (_currentAttackAnimation == BasicAnimations.AttackRockThrow)
            {
                Vector3 hand = BasicCache.HandRHitbox.transform.position;
                if (AI)
                {
                    if (TargetEnemy != null && TargetEnemy.ValidTarget())
                    {
                        float distance = Vector3.Distance(hand, TargetEnemy.GetPosition());
                        float time = distance / RockThrow1Speed;
                        float predictivePower = Mathf.Clamp(distance / 120f, 0f, 1f);
                        _rockThrowTarget = TargetEnemy.GetPosition();
                        if (TargetEnemy is BaseCharacter)
                            _rockThrowTarget += ((BaseCharacter)TargetEnemy).GetVelocity() * time * predictivePower;
                    }
                    else
                        _rockThrowTarget = Cache.Transform.position + Cache.Transform.forward * 200f;
                }
                else
                    _rockThrowTarget = GetAimPoint();
                var flatTarget = _rockThrowTarget;
                flatTarget.y = Cache.Transform.position.y;
                var forward = (flatTarget - Cache.Transform.position).normalized;
                Cache.Transform.rotation = Quaternion.Lerp(Cache.Transform.rotation, Quaternion.LookRotation(forward), Time.deltaTime * 5f);
                if (_currentAttackStage == 0 && animationTime > 0.16f)
                {
                    _currentAttackStage = 1;
                    SpawnableSpawner.Spawn(SpawnablePrefabs.Rock1, hand, Quaternion.identity, Size * 1.5f, new object[] { Cache.PhotonView.ViewID });
                }
                else if (_currentAttackStage == 1 && animationTime > 0.61f)
                {
                    _currentAttackStage = 2;
                    Vector3 direction = (_rockThrowTarget - hand).normalized;
                    Cache.PhotonView.RPC("ClearRockRPC", RpcTarget.All, new object[0]);
                    ProjectileSpawner.Spawn(ProjectilePrefabs.Rock1, hand, Quaternion.LookRotation(direction), direction * RockThrow1Speed,
                        Vector3.zero, 10f, Cache.PhotonView.ViewID, "", new object[] { Size * 1.5f });
                }
            }
        }

        protected override void UpdateEat()
        {
            if (State != TitanState.HumanThrow && HoldHuman == null && _stateTimeLeft > 4.72f)
            {
                IdleWait(0.5f);
                return;
            }
            if (State != TitanState.HumanThrow && _stateTimeLeft <= 4.72f)
            {
                if (HoldHuman != null)
                {
                    int damage = 100;
                    if (CustomDamageEnabled)
                        damage = CustomDamage;
                    var tempHoldHuman = HoldHuman;
                    Ungrab();
                    tempHoldHuman.GetHit(this, damage, "TitanEat", "");
                }
            }
            if (!AI && HoldHuman && !HoldHuman.Dead && !HoldHumanLeft && (Input.anyKeyDown || State == TitanState.HumanThrow))
                UpdateThrowHuman();
        }

        private void UpdateThrowHuman()
        {
            if (State != TitanState.HumanThrow)
                StateAction(TitanState.HumanThrow, BasicAnimations.AttackRockThrow);
            var flatTarget = GetAimPoint();
            flatTarget.y = Cache.Transform.position.y;
            var forward = (flatTarget - Cache.Transform.position).normalized;
            Cache.Transform.rotation = Quaternion.Lerp(Cache.Transform.rotation, Quaternion.LookRotation(forward), Time.deltaTime * 5f);
            if (GetAnimationTime() > 0.61f)
            {
                Human temp = HoldHuman;
                Vector3 hand = BasicCache.HandRHitbox.transform.position;
                Vector3 pos = (GetAimPoint() - hand).normalized * 150;
                Ungrab();
                if (temp.photonView.gameObject != null)
                    temp.photonView.RPC("BlowAwayRPC", temp.photonView.Owner, new object[] { pos });
            }
        }

        public override void Blind()
        {
            if (IsCrawler)
            {
                if (State != TitanState.Blind && AI)
                {
                    StateAction(TitanState.Blind, BasicAnimations.BlindCrawler);
                    DamagedGrunt();
                }
            }
            else
                base.Blind();
        }

        protected override string GetSitIdleAniamtion()
        {
            if (IsCrawler)
                return BasicAnimations.SitIdleCrawler;
            return BaseTitanAnimations.SitIdle;
        }

        protected override string GetSitFallAnimation()
        {
            if (IsCrawler)
                return BasicAnimations.SitFallCrawler;
            return BaseTitanAnimations.SitFall;
        }

        protected override string GetSitUpAnimation()
        {
            if (IsCrawler)
                return BasicAnimations.SitUpCrawler;
            return BaseTitanAnimations.SitUp;
        }

        public override void Cripple(float time = 0f)
        {
            if (IsCrawler)
            {
                if (State != TitanState.SitCripple && AI)
                {
                    _currentCrippleTime = time > 0f ? time : DefaultCrippleTime;
                    StateAction(TitanState.SitFall, BasicAnimations.SitFallCrawler);
                    DamagedGrunt();
                }
            }
            else
            {
                bool isBellyFlop = _currentStateAnimation == BasicAnimations.AttackBellyFlop || _currentStateAnimation == BasicAnimations.AttackBellyFlopGetup;
                if (!isBellyFlop)
                    base.Cripple(time);
            }
        }

        public override void OnHit(BaseHitbox hitbox, object victim, Collider collider, string type, bool firstHit)
        {
            int damage = 100;
            if (CustomDamageEnabled)
                damage = CustomDamage;
            if (victim is CustomLogicCollisionHandler)
            {
                ((CustomLogicCollisionHandler)victim).GetHit(this, Name, damage, type, hitbox.transform.position);
                return;
            }
            var victimChar = (BaseCharacter)victim;
            if (State == TitanState.Attack && IsGrabAttack() && victim is Human)
            {
                var human = (Human)victim;
                if (HoldHuman == null && firstHit && !human.Dead)
                {
                    HoldHumanLeft = hitbox == BasicCache.HandLHitbox;
                    if (HoldHumanLeft)
                        human.GetHit(this, 0, "GrabLeft", collider.name);
                    else
                        human.GetHit(this, 0, "GrabRight", collider.name);
                }
            }
            else if (victim is BaseTitan)
            {
                if (firstHit)
                {
                    EffectSpawner.Spawn(EffectPrefabs.PunchHit, hitbox.transform.position, Quaternion.identity);
                    PlaySound(TitanSounds.Hit);
                    if (!victimChar.Dead)
                    {
                        if (IsMainCharacter())
                            ((InGameMenu)UIManager.CurrentMenu).ShowKillScore(damage);
                        victimChar.GetHit(this, damage, "TitanStun", collider.name);
                    }
                }
            }
            else
            {
                if (firstHit && !victimChar.Dead)
                {
                    if (IsMainCharacter())
                        ((InGameMenu)UIManager.CurrentMenu).ShowKillScore(damage);
                    victimChar.GetHit(this, damage, "Titan", collider.name);
                }
            }
        }

        private Vector2 GetLookAngle(Vector3 target)
        {
            Vector3 vector = target - Cache.Transform.position;
            float angle = -Mathf.Atan2(vector.z, vector.x) * Mathf.Rad2Deg;
            float verticalAngle = -Mathf.DeltaAngle(angle, Cache.Transform.rotation.eulerAngles.y - 90f);
            float y = BasicCache.Neck.position.y - target.y;
            float distance = Util.DistanceIgnoreY(target, BasicCache.Transform.position);
            float horizontalAngle = Mathf.Atan2(y, distance) * Mathf.Rad2Deg;
            return new Vector2(horizontalAngle, verticalAngle);
        }

        protected void LateUpdateHeadPosition(Vector3 position)
        {
            if (position != null)
            {
                Vector3 vector = position - Cache.Transform.position;
                Vector2 angle = GetLookAngle(position);

                // maintain horizontal angle if within buffer zone on left or right.
                bool isInLeftRange = angle.y > -120 && angle.y < -50;
                bool isInRightRange = angle.y > 50 && angle.y < 120;

                if (isInLeftRange || isInRightRange)
                {
                    // if we were in front of the character before hitting the buffer zone, use the camera for the yaw, otherwise it will use the the ray.
                    if (LastGoodHeadAngle.y < -50 || LastGoodHeadAngle.y > 50)
                    {
                        // set angle to look at the camera
                        position = SceneLoader.CurrentCamera.Camera.transform.position;
                        angle = GetLookAngle(position);
                    }

                    angle.y = LastGoodHeadAngle.y;
                    LastGoodHeadAngle.x = angle.x;
                }
                else if (Vector3.Dot(Cache.Transform.forward, vector.normalized) < 0)
                {
                    // set angle to look at the camera
                    position = SceneLoader.CurrentCamera.Camera.transform.position;
                    angle = GetLookAngle(position);
                    LastGoodHeadAngle = angle;
                }
                else
                {
                    LastGoodHeadAngle = angle;
                }

                angle.x = Mathf.Clamp(angle.x, -80f, 30f);
                angle.y = Mathf.Clamp(angle.y, -80f, 80f);

                BasicCache.Head.rotation = Quaternion.Euler(BasicCache.Head.rotation.eulerAngles.x + angle.x,
                    BasicCache.Head.rotation.eulerAngles.y + angle.y, BasicCache.Head.rotation.eulerAngles.z);
                BasicCache.Head.localRotation = Quaternion.Lerp(_oldHeadRotation, BasicCache.Head.localRotation, Time.deltaTime * 10f);
            }
            else
            {
                BasicCache.Head.localRotation = Quaternion.Lerp(_oldHeadRotation, BasicCache.Head.localRotation, Time.deltaTime * 10f);
                LastGoodHeadAngle = new Vector2(0, 0);
            }
            _oldHeadRotation = BasicCache.Head.localRotation;
            LateUpdateHeadRotation = BasicCache.Head.rotation;
        }


        protected void LateUpdateHead(BaseCharacter target)
        {
            if (target != null)
            {
                Vector3 targetPosition = target.Cache.Transform.position;
                if (target is BaseTitan)
                    targetPosition = ((BaseTitan)target).BaseTitanCache.Head.position;
                Vector3 vector = target.Cache.Transform.position - Cache.Transform.position;
                float angle = -Mathf.Atan2(vector.z, vector.x) * Mathf.Rad2Deg;
                float num = -Mathf.DeltaAngle(angle, Cache.Transform.rotation.eulerAngles.y - 90f);
                num = Mathf.Clamp(num, -40f, 40f);
                float y = (BasicCache.Neck.position.y + (Size * 2f)) - targetPosition.y;
                float distance = Util.DistanceIgnoreY(target.Cache.Transform.position, BasicCache.Transform.position);
                float num2 = Mathf.Atan2(y, distance) * Mathf.Rad2Deg;
                num2 = Mathf.Clamp(num2, -40f, 30f);
                BasicCache.Head.rotation = Quaternion.Euler(BasicCache.Head.rotation.eulerAngles.x + num2,
                    BasicCache.Head.rotation.eulerAngles.y + num, BasicCache.Head.rotation.eulerAngles.z);
                BasicCache.Head.localRotation = Quaternion.Lerp(_oldHeadRotation, BasicCache.Head.localRotation, Time.deltaTime * 10f);
            }
            else
                BasicCache.Head.localRotation = Quaternion.Lerp(_oldHeadRotation, BasicCache.Head.localRotation, Time.deltaTime * 10f);
            _oldHeadRotation = BasicCache.Head.localRotation;
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            if (IsMine())
            {
                bool canLook = State == TitanState.Idle || State == TitanState.Run || State == TitanState.Walk || State == TitanState.Turn;
                if (AI)
                {
                    var canTarget = false;
                    if (TargetEnemy != null && TargetEnemy.ValidTarget() && TargetEnemy is BaseCharacter)
                    {
                        BaseCharacter character = (BaseCharacter)TargetEnemy;
                        TargetViewId = character.Cache.PhotonView.ViewID;
                        canTarget = !IsCrawler && Util.DistanceIgnoreY(TargetEnemy.GetPosition(), BasicCache.Transform.position) < 100f && canLook;
                    }
                    else
                        TargetViewId = -1;
                    if (canTarget)
                    {
                        LookAtTarget = true;
                        LateUpdateHead((BaseCharacter)TargetEnemy);
                    }
                    else
                    {
                        LookAtTarget = false;
                        LateUpdateHead(null);
                    }
                }
                else
                {
                    if (canLook)
                        LateUpdateHeadPosition(GetAimPoint());
                    else
                    {
                        LateUpdateHeadRotation = null;
                        _oldHeadRotation = BasicCache.Head.localRotation;
                    }
                }

                if ((State == TitanState.Run || State == TitanState.Walk || State == TitanState.Sprint) && HasDirection)
                    Cache.Transform.rotation = Quaternion.Lerp(Cache.Transform.rotation, GetTargetRotation(), Time.deltaTime * RotateSpeed);
            }
            else
            {
                bool canLook = State == TitanState.Idle || State == TitanState.Run || State == TitanState.Walk || State == TitanState.Turn;
                if (AI)
                {
                    if (LookAtTarget && TargetViewId >= 0)
                    {
                        var character = Util.FindCharacterByViewId(TargetViewId);
                        LateUpdateHead(character);
                    }
                    else
                        LateUpdateHead(null);
                }
                else
                {
                    if (LateUpdateHeadRotationRecv != null && canLook)
                    {
                        BasicCache.Head.rotation = (Quaternion)LateUpdateHeadRotationRecv;
                        BasicCache.Head.localRotation = Quaternion.Lerp(_oldHeadRotation, BasicCache.Head.localRotation, Time.deltaTime * 10f);
                        _oldHeadRotation = BasicCache.Head.localRotation;
                    }
                }
            }
            if (LeftArmDisabled)
            {
                BasicCache.ForearmL.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                BasicCache.ForearmL.localRotation = Quaternion.identity;
            }
            else
                BasicCache.ForearmL.localScale = Vector3.one;
            if (RightArmDisabled)
            {
                BasicCache.ForearmR.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                BasicCache.ForearmR.localRotation = Quaternion.identity;
            }
            else
                BasicCache.ForearmR.localScale = Vector3.one;
            BasicCache.ForearmSmokeL.transform.position = BasicCache.ForearmL.position;
            BasicCache.ForearmSmokeR.transform.position = BasicCache.ForearmR.position;
            if (!AI && Animation.IsPlaying(BasicAnimations.RunCrawler))
            {
                var body = BasicCache.Body;
                body.localRotation = Quaternion.Euler(-90f, 0f, 0f);
                BasicCache.Core.localPosition = new Vector3(0f, -0.05f, 0f);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (BasicCache.ForearmSmokeL != null)
                Destroy(BasicCache.ForearmSmokeL.gameObject);
            if (BasicCache.ForearmSmokeR != null)
                Destroy(BasicCache.ForearmSmokeR.gameObject);
        }

        protected override int GetFootstepPhase()
        {
            if (Animation.IsPlaying(BasicAnimations.Walk))
            {
                float time = Animation.GetCurrentNormalizedTime() % 1f;
                return (time >= 0.1f && time < 0.6f) ? 1 : 0;
            }
            string run = GetPlayingRunAnimation();
            if (run != "")
            {
                float time = Animation.GetCurrentNormalizedTime() % 1f;
                return (time >= 0f && time < 0.5f) ? 0 : 1;
            }
            return _stepPhase;
        }

        protected string GetPlayingRunAnimation()
        {
            if (Animation.IsPlaying(BasicAnimations.RunCrawler))
                return BasicAnimations.RunCrawler;
            foreach (string anim in BasicAnimations.Runs)
            {
                if (Animation.IsPlaying(anim))
                    return anim;
            }
            return "";
        }

        protected override void CheckGround()
        {
            var collider = (CapsuleCollider)(BaseTitanCache.Movebox);
            if (State == TitanState.Jump || State == TitanState.StartJump)
            {
                if (IsCrawler)
                    collider.height = _originalCapsuleValue * 0.7f;
                else
                    collider.radius = _originalCapsuleValue * 0.7f;
            }
            else
            {
                if (IsCrawler)
                {
                    if (collider.height != _originalCapsuleValue)
                        collider.height = _originalCapsuleValue;
                }
                else
                {
                    if (collider.radius != _originalCapsuleValue)
                        collider.radius = _originalCapsuleValue;
                }
            }
            if (IsCrawler)
            {
                float radius = BaseTitanCache.Movebox.transform.lossyScale.x * collider.radius;
                float height = BaseTitanCache.Movebox.transform.lossyScale.x * collider.height - radius * 2f;
                float halfHeight = 0.5f * height;
                Vector3 position = Cache.Transform.position + Vector3.up * (radius + 1f);
                Vector3 start = position - Cache.Transform.forward * halfHeight;
                Vector3 end = position + Cache.Transform.forward * halfHeight;
                RaycastHit hit;
                JustGrounded = false;
                if (Physics.CapsuleCast(start, end, radius, Vector3.down, out hit, 1f + GroundDistance, GroundMask.value))
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
            else
                base.CheckGround();
        }
    }
}
