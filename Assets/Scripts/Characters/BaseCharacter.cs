using UnityEngine;
using ApplicationManagers;
using GameManagers;
using Utility;
using System.Collections.Generic;
using Settings;
using System.Collections;
using CustomLogic;
using UI;
using Cameras;
using Photon.Pun;
using Photon.Realtime;
using System;

namespace Characters
{
    class BaseCharacter: Photon.Pun.MonoBehaviourPunCallbacks, ITargetable
    {
        protected virtual int DefaultMaxHealth => 1;
        protected virtual Vector3 Gravity => Vector3.down * 20f;
        public virtual List<string> EmoteActions => new List<string>();
        public bool FootstepsEnabled = true;
        public bool SoundsEnabled = true;
        public float MaxFootstepDistance = 200f;
        public float MaxSoundDistance = 500f;
        protected float _disableKinematicTimeLeft = 0f;

        public string Name { 
            get
            {
                return RichTextName;
            }
            set
            {
                RichTextName = value;
                VisibleName = RichTextName.StripColor();
            }
        }
        public string RichTextName = "";
        public string VisibleName = "";
        public string Guild = "";
        public string FeedKillerName = "";
        public string FeedVictimName = "";
        public bool Dead;
        public bool CustomDamageEnabled;
        public int CustomDamage;
        public bool CustomDamageReceivedEnabled;
        public int CustomDamageReceived;

        // setup
        public BaseComponentCache Cache;
        public bool AI;
        public int MaxHealth;
        public int CurrentHealth;
        public string Team;
        public List<BaseUseable> Items = new List<BaseUseable>();
        protected InGameManager _inGameManager;
        protected bool _cameraFPS = false;
        protected bool _wasMainCharacter = false;
        public BaseMovementSync MovementSync;
        public AnimationHandler Animation;
        public BaseDetection Detection;
        public float CurrentSpeed;

        // movement
        public bool Grounded;
        public bool JustGrounded;
        public float TargetAngle;
        public bool HasDirection;
        protected int _stepPhase = 0;
        private LayerMask GroundMaskLayers = PhysicsLayer.GetMask(PhysicsLayer.TitanMovebox, PhysicsLayer.MapObjectEntities,
                PhysicsLayer.MapObjectCharacters, PhysicsLayer.MapObjectAll);

        public virtual LayerMask GroundMask => GroundMaskLayers;
        protected virtual float GroundDistance => 0.3f;

        // Visuals
        protected Outline OutlineComponent = null;
        public event Action<ExitGames.Client.Photon.Hashtable> OnPlayerPropertiesChanged;

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            if (targetPlayer == photonView.Owner)
            {
                OnPlayerPropertiesChanged?.Invoke(changedProps);
            }
        }

        public void SetKinematic(bool kinematic, float forTime = 0f)
        {
            Cache.Rigidbody.isKinematic = kinematic;
            _disableKinematicTimeLeft = forTime;
        }

        public Vector3 GetVelocity()
        {
            if (!IsMine() && MovementSync != null)
                return MovementSync._correctVelocity;
            return Cache.Rigidbody.velocity;
        }

        public void Reveal(float startDelay, float activeTime)
        {
            StartCoroutine(RevealAndRemove(startDelay, activeTime));
        }

        public void AddOutline()
        {
            AddOutlineWithColor(Color.white, Outline.Mode.OutlineAll);
        }

        public void AddVisibleOutlineWithColor(Color color)
        {
            AddOutlineWithColor(color, Outline.Mode.OutlineVisible);
        }

        public void AddOutlineWithColor(Color color, Outline.Mode mode)
        {
            if (OutlineComponent != null)
            {
                OutlineComponent.OutlineMode = mode;
                OutlineComponent.OutlineColor = color;
                OutlineComponent.enabled = true;
            }
        }

        public void ChangeOutlineColor(Color color)
        {
            if (OutlineComponent != null)
            {
                OutlineComponent.OutlineColor = color;
            }
        }

        public void ChangeOutlineMode(Outline.Mode mode)
        {
            if (OutlineComponent != null)
            {
                OutlineComponent.OutlineMode = mode;
            }
        }

        public void ChangeOutlineWidth(float width)
        {
            if (OutlineComponent != null)
            {
                OutlineComponent.OutlineWidth = width;
            }
        }

        public void RemoveOutline()
        {
            if (OutlineComponent != null)
            {
                OutlineComponent.enabled = false;
            }
        }

        private IEnumerator RevealAndRemove(float startDelay, float seconds)
        {
            yield return new WaitForSeconds(startDelay);
            AddOutline();
            yield return new WaitForSeconds(seconds);
            RemoveOutline();
        }

        public virtual string GetTeam()
        {
            return Team;
        }

        public virtual Vector3 GetPosition()
        {
            if (Cache != null && Cache.Transform != null)
                return Cache.Transform.position;
            return Vector3.zero;
        }

        public virtual bool ValidTarget()
        {
            if (this == null)
                return false;
            return !Dead;
        }

        public bool IsMine()
        {
            return Cache.PhotonView.IsMine;
        }

        public bool IsMainCharacter()
        {
            return _inGameManager.CurrentCharacter == this;
        }

        public virtual void Init(bool ai, string team)
        {
            AI = ai;
            
            if (!ai)
            {
                Name = PhotonNetwork.LocalPlayer.GetStringProperty(PlayerProperty.Name).StripIllegalRichText();
                Guild = PhotonNetwork.LocalPlayer.GetStringProperty(PlayerProperty.Guild);
            }
            Cache.PhotonView.RPC("InitRPC", RpcTarget.AllBuffered, new object[] { AI, Name, Guild });
            SetTeam(team);
        }

        public virtual Vector3 GetAimPoint()
        {
            Ray ray = SceneLoader.CurrentCamera.Camera.ScreenPointToRay(CursorManager.GetInGameMousePosition());
            Vector3 target = ray.origin + ray.direction * 1000f;
            return target;
        }

        public void SetTeam(string team)
        {
            Cache.PhotonView.RPC("SetTeamRPC", RpcTarget.All, new object[] { team });
        }

        public virtual Transform GetCameraAnchor()
        {
            return Cache.Transform;
        }

        protected virtual void CreateCache(BaseComponentCache cache)
        {
            Cache = cache;
            if (cache == null)
                Cache = new BaseComponentCache(gameObject);
        }

        protected virtual void CreateDetection()
        {
            Detection = null;
        }

        public virtual void Emote(string emote)
        {
        }

        public virtual void ForceAnimation(string animation, float fade)
        {
        }

        [PunRPC]
        public void InitRPC(bool ai, string name, string guild)
        {
            AI = ai;
            Name = name;
            Guild = guild;
        }

        [PunRPC]
        public void SetHealthRPC(int currentHealth, int maxHealth, PhotonMessageInfo info)
        {
            if (info.Sender == photonView.Owner)
            {
                CurrentHealth = currentHealth;
                MaxHealth = maxHealth;
            }
        }

        [PunRPC]
        public void SetTeamRPC(string team, PhotonMessageInfo info)
        {
            if (info.Sender == photonView.Owner)
            {
                string previousTeam = Team;
                Team = team;
                if (Detection != null && team != previousTeam)
                    Detection.OnTeamChanged();
            }
        }

        public void SetCurrentHealth(int currentHealth)
        {
            CurrentHealth = Mathf.Min(currentHealth, MaxHealth);
            CurrentHealth = Mathf.Max(CurrentHealth, 0);
            OnHealthChange();
            if (CurrentHealth <= 0)
                Die();
        }

        public void SetMaxHealth(int maxHealth)
        {
            MaxHealth = maxHealth;
            SetCurrentHealth(CurrentHealth);
        }

        public void SetHealth(int health)
        {
            MaxHealth = health;
            SetCurrentHealth(health);
        }

        public virtual void TakeDamage(int damage)
        {
            SetCurrentHealth(CurrentHealth - damage);
        }

        public virtual void Die()
        {
            Cache.PhotonView.RPC("MarkDeadRPC", RpcTarget.AllBuffered, new object[0]);
            if (IsMainCharacter())
                _inGameManager.RegisterMainCharacterDie();
            StartCoroutine(WaitAndDie());
        }

        protected virtual IEnumerator WaitAndDie()
        {
            PhotonNetwork.Destroy(gameObject);
            yield break;
        }

        public virtual void UseItem(int item)
        {
            Items[item].SetInput(true);
        }

        public override void OnPlayerEnteredRoom(Player player)
        {
            if (Cache.PhotonView.IsMine)
            {
                Cache.PhotonView.RPC("SetHealthRPC", player, new object[] { CurrentHealth, MaxHealth });
                Cache.PhotonView.RPC("SetTeamRPC", player, new object[] { Team });
                string currentAnimation = GetCurrentAnimation();
                if (currentAnimation != "")
                    Cache.PhotonView.RPC("PlayAnimationRPC", player, new object[] { currentAnimation, Animation.GetCurrentNormalizedTime() });
            }
        }

        public void PlayAnimation(string animation, float startTime = 0f)
        {
            if (IsMine())
                Cache.PhotonView.RPC("PlayAnimationRPC", RpcTarget.All, new object[] { animation, startTime });
        }

        public void PlayAnimationReset(string animation)
        {
            if (IsMine())
                Cache.PhotonView.RPC("PlayAnimationResetRPC", RpcTarget.All, new object[] { animation });
        }

        [PunRPC]
        public void PlayAnimationRPC(string animation, float startTime, PhotonMessageInfo info)
        {
            if (info.Sender != Cache.PhotonView.Owner)
                return;
            Animation.Play(animation, startTime);
        }

        [PunRPC]
        public void PlayAnimationResetRPC(string animation, PhotonMessageInfo info)
        {
            if (info.Sender != Cache.PhotonView.Owner)
                return;
            Animation.Play(animation, 0f, true);
        }

        public void PlayAnimationIfNotPlaying(string animation, float startTime = 0f)
        {
            if (!Animation.IsPlaying(animation))
                PlayAnimation(animation, startTime);
        }

        private object[] crossfadeCache = new object[3];
        public void CrossFade(string animation, float fadeTime = 0f, float startTime = 0f)
        {
            if (IsMine())
            {
                crossfadeCache[0] = animation;
                crossfadeCache[1] = fadeTime;
                crossfadeCache[2] = startTime;
                Cache.PhotonView.RPC("CrossFadeRPC", RpcTarget.All, crossfadeCache);
            }
        }

        public void CrossFadeWithSpeed(string animation, float speed, float fadeTime = 0f, float startTime = 0f)
        {
            if (IsMine())
                Cache.PhotonView.RPC("CrossFadeWithSpeedRPC", RpcTarget.All, new object[] { animation, speed, fadeTime, startTime });
        }

        public void CrossFadeIfNotPlaying(string animation, float fadeTime = 0f, float startTime = 0f)
        {
            if (!Animation.IsPlaying(animation))
                CrossFade(animation, fadeTime, startTime);
        }

        [PunRPC]
        public void CrossFadeRPC(string animation, float fadeTime, float startTime, PhotonMessageInfo info)
        {
            if (info.Sender != Cache.PhotonView.Owner)
                return;
            Animation.CrossFade(animation, fadeTime, startTime);
        }

        [PunRPC]
        public void CrossFadeWithSpeedRPC(string animation, float speed, float fadeTime, float startTime, PhotonMessageInfo info)
        {
            if (info.Sender != Cache.PhotonView.Owner)
                return;
            Animation.SetSpeed(animation, speed);
            Animation.CrossFade(animation, fadeTime, startTime);
        }

        public void PlaySound(string sound)
        {
            if (IsMine())
                Cache.PhotonView.RPC("PlaySoundRPC", RpcTarget.All, new object[] { sound });
        }

        public bool IsPlayingSound(string sound)
        {
            return Cache.AudioSources[sound].isPlaying;
        }

        protected IEnumerator WaitAndPlaySound(string sound, float delay)
        {
            yield return new WaitForSeconds(delay);
            PlaySound(sound);
        }

        [PunRPC]
        public void PlaySoundRPC(string sound, PhotonMessageInfo info)
        {
            if (info.Sender != null && info.Sender != Cache.PhotonView.Owner)
                return;
            if (!SoundsEnabled)
                return;
            if (Cache.AudioSources.ContainsKey(sound))
                Cache.AudioSources[sound].Play();
        }

        public void StopSound(string sound)
        {
            if (IsMine())
                Cache.PhotonView.RPC("StopSoundRPC", RpcTarget.All, new object[] { sound });
        }

        [PunRPC]
        public void StopSoundRPC(string sound, PhotonMessageInfo info)
        {
            if (info.Sender != null && info.Sender != Cache.PhotonView.Owner)
                return;
            if (Cache.AudioSources.ContainsKey(sound))
                Cache.AudioSources[sound].Stop();
        }

        protected virtual void OnHealthChange()
        {
            if (IsMine())
                photonView.RPC("SetHealthRPC", RpcTarget.All, new object[] { CurrentHealth, MaxHealth });
        }

        public virtual void OnHit(BaseHitbox hitbox, object victim, Collider collider, string type, bool firstHit)
        {
        }

        [PunRPC]
        public virtual void GetHitRPC(int viewId, string name, int damage, string type, string collider)
        {
            if (Dead)
                return;
            var killer = Util.FindCharacterByViewId(viewId);
            if (killer != null && name == "")
                name = killer.Name;
            if (CustomLogicManager.Evaluator != null)
            {
                CustomLogicManager.Evaluator.OnCharacterDamaged(this, killer, name, damage);
                if (CustomDamageReceivedEnabled)
                {
                    damage = CustomDamageReceived;
                    CustomDamageReceivedEnabled = false;
                }
            }
            if (damage == 0)
                return;
            TakeDamage(damage);
            Cache.PhotonView.RPC("NotifyDamagedRPC", RpcTarget.All, new object[] { viewId, name, damage });
            if (CurrentHealth <= 0f)
            {
                if (CustomLogicManager.Evaluator != null && CustomLogicManager.Evaluator.DefaultShowKillFeed)
                    RPCManager.PhotonView.RPC("ShowKillFeedRPC", RpcTarget.All, new object[] { name, Name, damage, type});
                Cache.PhotonView.RPC("NotifyDieRPC", RpcTarget.All, new object[] { viewId, name });
            }
        }

        [PunRPC]
        public virtual void GetDamagedRPC(string name, int damage)
        {
            if (!Cache.PhotonView.IsMine || Dead)
                return;
            if (CustomLogicManager.Evaluator != null)
            {
                CustomLogicManager.Evaluator.OnCharacterDamaged(this, null, name, damage);
                if (CustomDamageReceivedEnabled)
                {
                    damage = CustomDamageReceived;
                    CustomDamageReceivedEnabled = false;
                    if (damage == 0)
                        return;
                }
            }
            TakeDamage(damage);
            Cache.PhotonView.RPC("NotifyDamagedRPC", RpcTarget.All, new object[] { -1, name, damage });
            if (CurrentHealth <= 0f)
            {
                if (CustomLogicManager.Evaluator != null && CustomLogicManager.Evaluator.DefaultShowKillFeed)
                    RPCManager.PhotonView.RPC("ShowKillFeedRPC", RpcTarget.All, new object[] { name, Name, damage, "" });
                Cache.PhotonView.RPC("NotifyDieRPC", RpcTarget.All, new object[] { -1, name });
            }
        }

        [PunRPC]
        public virtual void GetKilledRPC(string name)
        {
            if (!Cache.PhotonView.IsMine || Dead)
                return;
            SetCurrentHealth(0);
            if (CustomLogicManager.Evaluator != null && CustomLogicManager.Evaluator.DefaultShowKillFeed)
                RPCManager.PhotonView.RPC("ShowKillFeedRPC", RpcTarget.All, new object[] { name, Name, 0, "" });
            Cache.PhotonView.RPC("NotifyDieRPC", RpcTarget.All, new object[] { -1, name });
        }

        [PunRPC]
        public virtual void MarkDeadRPC(PhotonMessageInfo info)
        {
            if (info.Sender != Cache.PhotonView.Owner)
                return;
            Dead = true;
        }

        [PunRPC]
        public void NotifyDieRPC(int viewId, string name, PhotonMessageInfo info)
        {
            if (info.Sender != Cache.PhotonView.Owner)
                return;
            var killer = Util.FindCharacterByViewId(viewId);
            if (killer != null)
                name = killer.Name;
            if (killer != null)
            {
                if (killer.IsMainCharacter() && CustomLogicManager.Evaluator != null && CustomLogicManager.Evaluator.DefaultAddKillScore)
                    _inGameManager.RegisterMainCharacterKill(this);
            }
            if (CustomLogicManager.Evaluator == null)
                return;
            CustomLogicManager.Evaluator.OnCharacterDie(this, killer, name);
            if (_inGameManager.CurrentCharacter == this)
                InGameManager.OnLocalPlayerDied(Cache.PhotonView.Owner);
        }

        [PunRPC]
        public void NotifyDamagedRPC(int viewId, string name, int damage, PhotonMessageInfo info)
        {
            if (info.Sender != Cache.PhotonView.Owner)
                return;
            var killer = Util.FindCharacterByViewId(viewId);
            if (killer != null)
                name = killer.Name;
            if (killer != null)
            {
                if (killer.IsMainCharacter() && CustomLogicManager.Evaluator != null && CustomLogicManager.Evaluator.DefaultAddKillScore)
                    _inGameManager.RegisterMainCharacterDamage(this, damage);
            }
            if (CustomLogicManager.Evaluator != null && !Cache.PhotonView.IsMine)
            {
                CustomLogicManager.Evaluator.OnCharacterDamaged(this, killer, name, damage);
            }
            if (SettingsManager.UISettings.GameFeed.Value)
            {
                string keyword = " killed ";
                if (CurrentHealth > 0)
                    keyword = " damaged ";
                string feed = ChatManager.GetColorString("(" + Util.FormatFloat(CustomLogicManager.Evaluator.CurrentTime, 2) + ") ", ChatTextColor.System) + name +
                    keyword + Name + " (" + damage.ToString() + ")";
                ChatManager.AddFeed(feed);
            }
        }

        public virtual void GetHit(BaseCharacter enemy, int damage, string type, string collider)
        {
            int viewId = -1;
            if (enemy != null)
                viewId = enemy.Cache.PhotonView.ViewID;
            Cache.PhotonView.RPC("GetHitRPC", Cache.PhotonView.Owner, new object[] { viewId, "", damage, type, collider });
        }

        public virtual void GetHit(string name, int damage, string type, string collider)
        {
            if (!Dead)
                Cache.PhotonView.RPC("GetHitRPC", Cache.PhotonView.Owner, new object[] { -1, name, damage, type, collider });
        }

        public virtual void GetDamaged(string name, int damage)
        {
            if (!Dead)
                Cache.PhotonView.RPC("GetDamagedRPC", Cache.PhotonView.Owner, new object[] { name, damage });
        }

        public virtual void GetKilled(string name)
        {
            if (!Dead)
                Cache.PhotonView.RPC("GetKilledRPC", Cache.PhotonView.Owner, new object[] { name });
        }

        protected virtual void Awake()
        {
            if (SceneLoader.CurrentGameManager is InGameManager)
                _inGameManager = (InGameManager)SceneLoader.CurrentGameManager;
            CreateCache(null);
            SetColliders();
            CurrentHealth = MaxHealth = DefaultMaxHealth;
            MovementSync = GetComponent<BaseMovementSync>();
            OutlineComponent = gameObject.AddComponent<Outline>();
            OutlineComponent.enabled = false;
            Animation = new AnimationHandler(gameObject);
            if (!IsMine())
                SetKinematic(true);
        }

        protected virtual void CreateCharacterIcon()
        {
        }

        protected virtual void SetColliders()
        {
        }

        protected virtual void Start()
        {
            MinimapHandler.CreateMinimapIcon(this);
            StartCoroutine(WaitAndNotifySpawn());
            if (IsMine())
                CreateDetection();
        }

        protected IEnumerator WaitAndNotifySpawn()
        {
            while (CustomLogicManager.Evaluator == null)
                yield return new WaitForEndOfFrame();

            // Wait one frame after evaluator is initialized so that Main and Component Init calls can be done first.
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            if (CustomLogicManager.Evaluator != null)
            {
                CustomLogicManager.Evaluator.OnCharacterSpawn(this);
                if (!AI)
                    CustomLogicManager.Evaluator.OnPlayerSpawn(this.Cache.PhotonView.Owner, this);
            }
        }

        protected IEnumerator WaitAndNotifyReloaded()
        {
            while (CustomLogicManager.Evaluator == null)
                yield return new WaitForEndOfFrame();

            // Wait one frame after evaluator is initialized so that Main and Component Init calls can be done first.
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            if (CustomLogicManager.Evaluator != null)
            {
                CustomLogicManager.Evaluator.OnCharacterReloaded(this);
            }
        }

        public string GetCurrentAnimation()
        {
            return Animation.GetCurrentAnimation();
        }

        public virtual Quaternion GetTargetRotation()
        {
            return Quaternion.Euler(0f, TargetAngle, 0f);
        }

        public virtual Vector3 GetTargetDirection()
        {
            float angleRadians = (90f - TargetAngle) * Mathf.Deg2Rad;
            var v = new Vector3(Mathf.Cos(angleRadians), 0f, Mathf.Sin(angleRadians));
            return v.normalized;
        }

        protected float GetAngleToTarget(Vector3 targetDirection)
        {
            float angleX = -Mathf.Atan2(targetDirection.z, targetDirection.x) * Mathf.Rad2Deg;
            angleX = -Mathf.DeltaAngle(angleX, Cache.Transform.rotation.eulerAngles.y - 90f);
            return angleX;
        }

        protected virtual void CheckGround()
        {
            
            JustGrounded = false;
            if (CheckRaycastIgnoreTriggers(Cache.Transform.position + Vector3.up * 0.1f, -Vector3.up, GroundDistance, GroundMask.value))
            {
                if (!Grounded)
                    Grounded = JustGrounded = true;
            }
            else
                Grounded = false;
        }

        public virtual bool CheckRaycastIgnoreTriggers(Vector3 origin, Vector3 direction, float distance, int layerMask)
        {
            return RaycastIgnoreTriggers(origin, direction, distance, layerMask).HasValue;
        }

        protected virtual RaycastHit? RaycastIgnoreTriggers(Vector3 origin, Vector3 direction, float distance, int layerMask)
        {
            var hits = Physics.RaycastAll(origin, direction, distance, GroundMask.value);
            foreach (var hit in hits)
            {
                if (!hit.collider.isTrigger)
                    return hit;
            }
            return null;
        }

        protected virtual void ToggleSound(string sound, bool toggle)
        {
            if (toggle && !Cache.AudioSources[sound].isPlaying)
                PlaySound(sound);
            else if (!toggle && Cache.AudioSources[sound].isPlaying)
                StopSound(sound);
        }

        protected virtual void ToggleSoundLocal(string sound, bool toggle)
        {
            if (toggle && !Cache.AudioSources[sound].isPlaying)
            {
                if (Cache.AudioSources.ContainsKey(sound))
                    Cache.AudioSources[sound].Play();
            }
            else if (!toggle && Cache.AudioSources[sound].isPlaying)
            {
                if (Cache.AudioSources.ContainsKey(sound))
                    Cache.AudioSources[sound].Stop();
            }
        }

        protected virtual void OnDestroy()
        {
        }

        protected virtual void FixedUpdate()
        {
            CurrentSpeed = GetVelocity().magnitude;
            if (Detection != null)
                Detection.OnFixedUpdate();
            _disableKinematicTimeLeft -= Time.deltaTime;
        }

        protected virtual void LateUpdate()
        {
            LateUpdateFootstep();
            LateUpdateFPS();
        }

        protected virtual void LateUpdateFootstep()
        {
            if (!FootstepsEnabled)
                return;
            int phase = GetFootstepPhase();
            if (_stepPhase != phase)
            {
                string audio = GetFootstepAudio(_stepPhase);
                if (audio != string.Empty)
                {
                    _stepPhase = phase;
                    var local = Util.CreateLocalPhotonInfo();
                    StopSoundRPC(audio, local);
                    PlaySoundRPC(audio, local);
                }
            }
        }

        protected virtual void LateUpdateFPS()
        {
            if (!IsMine())
                return;
            var camera = ((InGameCamera)SceneLoader.CurrentCamera);
            if (camera._follow == this && camera.GetCameraDistance() == 0f)
            {
                if (!_cameraFPS)
                {
                    _cameraFPS = true;
                    foreach (var renderer in GetFPSDisabledRenderers())
                        renderer.enabled = false;
                }
            }
            else if (_cameraFPS)
            {
                _cameraFPS = false;
                if (!Dead || !(this is Human))
                {
                    foreach (var renderer in GetFPSDisabledRenderers())
                        renderer.enabled = true;
                }
            }
        }

        protected virtual int GetFootstepPhase()
        {
            return 0;
        }

        protected virtual string GetFootstepAudio(int phase)
        {
            return "";
        }

        protected virtual List<Renderer> GetFPSDisabledRenderers()
        {
            return new List<Renderer>();
        }
     
        protected void AddRendererIfExists(List<Renderer> renderers, GameObject go, bool multiple=false)
        {
            if (go != null)
            {
                if (multiple)
                {
                    foreach (var renderer in go.GetComponentsInChildren<Renderer>())
                    {
                        if (renderer != null)
                            renderers.Add(renderer);
                    }
                }
                else
                {
                    var renderer = go.GetComponentInChildren<Renderer>();
                    if (renderer != null)
                        renderers.Add(renderer);
                }
            }
        }

        public virtual Vector3 GetCenterPosition()
        {
            return Cache.Transform.position;
        }
    }
}
