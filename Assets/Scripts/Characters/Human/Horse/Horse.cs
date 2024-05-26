using UnityEngine;
using ApplicationManagers;
using GameManagers;
using Utility;
using Settings;
using Photon.Pun;

namespace Characters
{
    class Horse: BaseCharacter
    {
        Human _owner;
        HorseComponentCache HorseCache;
        public HorseState State;
        private float WalkSpeed = 15f;
        private float RunCloseSpeed = 20f;
        private float TeleportTime = 10f;
        protected override Vector3 Gravity => Vector3.down * 30f;
        private float JumpForce = 30f;
        private float _idleTimeLeft;
        private float _teleportTimeLeft;
        private float _jumpCooldownLeft;

        public void Init(Human human)
        {
            base.Init(true, human.Team);
            _owner = human;
            EmVariables.HorseAutorun= false;
        }

        protected override void CreateCache(BaseComponentCache cache)
        {
            HorseCache = new HorseComponentCache(gameObject);
            base.CreateCache(HorseCache);
        }

        public void Jump()
        {
            if (_jumpCooldownLeft > 0f || !Grounded)
                return;
            Cache.Rigidbody.AddForce(Vector3.up * JumpForce, ForceMode.VelocityChange);
            _jumpCooldownLeft = 0f;
        }

        protected override void Awake()
        {
            base.Awake();
            if (IsMine())
                CrossFade(HorseAnimations.Idle0, 0.1f);
            HorseCache.Dust.Play();
            ToggleDust(false);
        }

        private void ToggleDust(bool toggle)
        {
            var emsission = HorseCache.Dust.emission;
            if (toggle && !emsission.enabled)
                emsission.enabled = true;
            else if (!toggle && emsission.enabled)
                emsission.enabled = false;
        }

        private void TeleportToHuman()
        {
            Vector3 position = _owner.Cache.Transform.position + Vector3.right * UnityEngine.Random.Range(-2f, 2f) + Vector3.forward * UnityEngine.Random.Range(-2f, 2f);
            position.y = GetHeight(position) + 1f;
            Cache.Transform.position = position;
            _teleportTimeLeft = TeleportTime;
        }

        private float GetHeight(Vector3 pt)
        {
            RaycastHit hit;
            if (Physics.Raycast(pt + Vector3.up * 1f, -Vector3.up, out hit, 1000f, GroundMask))
            {
                return hit.point.y;
            }
            return 0f;
        }

        private void UpdateIdle()
        {
            _idleTimeLeft -= Time.deltaTime;
            if (_idleTimeLeft > 0f)
                return;
            if (!Cache.Animation.IsPlaying(HorseAnimations.Idle0))
            {
                CrossFade(HorseAnimations.Idle0, 0.1f);
                _idleTimeLeft = UnityEngine.Random.Range(3f, 6f);
                return;
            }
            float choose = UnityEngine.Random.Range(0f, 1f);
            if (choose < 0.25f)
                IdleOneShot(HorseAnimations.Idle1);
            else if (choose < 0.5f)
            {
                IdleOneShot(HorseAnimations.Idle2);
                PlaySound(HorseSounds.Idle1);
            }
            else if (choose < 0.75f)
            {
                IdleOneShot(HorseAnimations.Idle3);
                PlaySound(HorseSounds.Idle2);
            }
            else
            {
                IdleOneShot(HorseAnimations.Crazy);
                PlaySound(HorseSounds.Idle3);
            }
        }

        private void IdleOneShot(string animation)
        {
            CrossFade(animation, 0.1f);
            _idleTimeLeft = Cache.Animation[animation].length;
        }

        private void Update()
        {
            if (IsMine())
            {
                _jumpCooldownLeft -= Time.deltaTime;
                if (_owner == null || _owner.Dead)
                {
                    PhotonNetwork.Destroy(gameObject);
                    return;
                }
                if (_owner.MountState == HumanMountState.Horse)
                {
                    if (_owner.HasDirection)
                    {
                        Cache.Transform.rotation = Quaternion.Lerp(Cache.Transform.rotation, _owner.GetTargetRotation(), 5f * Time.deltaTime);
                        if (_owner.IsWalk)
                            State = HorseState.ControlledWalk;
                        else if (!_owner.IsWalk)
                            State = HorseState.ControlledRun;
                    }
                    else
                        State = HorseState.ControlledIdle;
                }
                else
                {
                    _teleportTimeLeft -= Time.deltaTime;
                    float distance = Vector3.Distance(_owner.Cache.Transform.position, Cache.Transform.position);
                    float flatDistance = Util.DistanceIgnoreY(_owner.Cache.Transform.position, Cache.Transform.position);
                    if (distance > 20f && _teleportTimeLeft <= 0f)
                        TeleportToHuman();
                    else if (flatDistance < 5f)
                    {
                        State = HorseState.Idle;
                        _teleportTimeLeft = TeleportTime;
                    }
                    else if (flatDistance < 20f)
                    {
                        State = HorseState.WalkToPoint;
                        _teleportTimeLeft = TeleportTime;
                    }
                    else
                        State = HorseState.RunToPoint;
                    if (State == HorseState.WalkToPoint || State == HorseState.RunToPoint)
                    {
                        Vector3 direction = (_owner.Cache.Transform.position - Cache.Transform.position);
                        direction.y = 0f;
                        Cache.Transform.rotation = Quaternion.Lerp(Cache.Transform.rotation, Quaternion.LookRotation(direction.normalized), 10f * Time.deltaTime);
                    }
                }
                
                //Handling Horse Auto Running, Snake 24 May 24      
                if (EmVariables.HorseAutorun&& _owner.MountState == HumanMountState.Horse )
                {
                    State = HorseState.ControlledRun;
                }
            }
        }

        private void FixedUpdate()
        {
            if (IsMine())
            {
                if (_owner == null || _owner.Dead)
                    return;
                CheckGround();
                if (Grounded)
                {
                    if (State == HorseState.ControlledIdle || State == HorseState.Idle)
                    {
                        if (Cache.Rigidbody.velocity.magnitude < 1f)
                            Cache.Rigidbody.velocity = Vector3.up * Cache.Rigidbody.velocity.y;
                        else
                        {
                            Cache.Rigidbody.AddForce(-Cache.Rigidbody.velocity.normalized * Mathf.Min(_owner.HorseSpeed, Cache.Rigidbody.velocity.magnitude * 0.5f),
                                ForceMode.Acceleration);
                        }
                    }
                    else if (State == HorseState.WalkToPoint || State == HorseState.RunToPoint  || 
                        State == HorseState.ControlledWalk || State == HorseState.ControlledRun)
                    {
                        float speed = _owner.HorseSpeed;
                        if (State == HorseState.ControlledWalk)
                            speed = WalkSpeed;
                        else if (State == HorseState.WalkToPoint)
                            speed = RunCloseSpeed;
                        Cache.Rigidbody.AddForce(Cache.Transform.forward * _owner.HorseSpeed, ForceMode.Acceleration);
                        if (Cache.Rigidbody.velocity.magnitude >= speed)
                        {
                            if (speed == _owner.HorseSpeed)
                                Cache.Rigidbody.AddForce((speed - Cache.Rigidbody.velocity.magnitude) * Cache.Rigidbody.velocity.normalized, ForceMode.VelocityChange);
                            else
                                Cache.Rigidbody.AddForce((Mathf.Max(speed - Cache.Rigidbody.velocity.magnitude, -1f)) * Cache.Rigidbody.velocity.normalized, ForceMode.VelocityChange);
                        }
                    }
                }
                Cache.Rigidbody.AddForce(Gravity, ForceMode.Acceleration);
            }
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            if (IsMine())
            {
                if (_owner == null || _owner.Dead)
                    return;
                if (Cache.Rigidbody.velocity.magnitude > 8f)
                {
                    CrossFadeIfNotPlaying(HorseAnimations.Run, 0.1f);
                    if (_owner.MountState == HumanMountState.Horse)
                        _owner.CrossFadeIfNotPlaying(HumanAnimations.HorseRun, 0.1f);
                    _idleTimeLeft = 0f;
                }
                else if (Cache.Rigidbody.velocity.magnitude > 1f)
                {
                    CrossFadeIfNotPlaying(HorseAnimations.Walk, 0.1f);
                    if (_owner.MountState == HumanMountState.Horse)
                        _owner.CrossFadeIfNotPlaying(HumanAnimations.HorseIdle, 0.1f);
                    _idleTimeLeft = 0f;
                }
                else
                {
                    UpdateIdle();
                    if (_owner.MountState == HumanMountState.Horse)
                        _owner.CrossFadeIfNotPlaying(HumanAnimations.HorseIdle, 0.1f);
                }
            }
            if (Cache.Animation.IsPlaying(HorseAnimations.Run) && Grounded)
            {
                ToggleDust(true);
                ToggleSound(HorseSounds.Run, true);
            }
            else
            {
                ToggleDust(false);
                ToggleSound(HorseSounds.Run, false);
            }
        }

        protected override void CheckGround()
        {
            RaycastHit hit;
            JustGrounded = false;
            if (Physics.SphereCast(Cache.Transform.position + Vector3.up * 0.8f, 0.6f, Vector3.down,
                out hit, 0.8f, GroundMask.value))
            {
                if (!Grounded)
                    Grounded = JustGrounded = true;
            }
            else
                Grounded = false;
        }
    }

    enum HorseState
    {
        Idle,
        WalkToPoint,
        RunToPoint,
        ControlledIdle,
        ControlledRun,
        ControlledWalk
    }
}
