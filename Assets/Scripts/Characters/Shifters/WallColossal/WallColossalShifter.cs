using ApplicationManagers;
using Controllers;
using CustomSkins;
using Effects;
using GameManagers;
using NUnit.Framework.Internal;
using Photon.Pun;
using Settings;
using SimpleJSONFixed;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using Photon.Realtime;

namespace Characters
{
    partial class WallColossalShifter : BaseShifter
    {
        public ColossalSteamState SteamState => _steamState;
        public ColossalHandState LeftHandState => _leftHandState;
        public ColossalHandState RightHandState => _rightHandState;
        public ColossalStunState StunState => _stunState;

        public WallColossalComponentCache ColossalCache;
        protected WallColossalAnimations ColossalAnimations;
        protected float _steamTimeLeft;
        protected float _steamBlowAwayTimeLeft;
        protected ColossalSteamState _steamState;
        public float WarningSteamTime = 3f;
        protected override float SizeMultiplier => 22f;

        public int MaxLeftHandHealth = 1000;
        public int MaxRightHandHealth = 1000;
        public int CurrentLeftHandHealth = 1000;
        public int CurrentRightHandHealth = 1000;
        public bool CanDamageLeftHand = true;
        public bool CanDamageRightHand = true;

        protected ColossalHandState _leftHandState = ColossalHandState.Healthy;
        protected ColossalHandState _rightHandState = ColossalHandState.Healthy;
        protected ColossalStunState _stunState = ColossalStunState.None;

        public float LeftHandSeverTimeLeft = 0f;
        public float RightHandSeverTimeLeft = 0f;
        public float HandSeverWindow = 10f;

        public float StunDuration = 5f;
        public float StunTimeLeft = 0f;

        public float RecoveryDuration = 10f;
        public float RecoveryTimeLeft = 0f;

        public float SteamBlowAwayForce = 30f;
        public float DefaultBlowAwayForce = 50f;
        public float BlowAwayMaxDistance = 60f;
        public float BlowAwaySteamTime = 0.5f;

        public override void OnPlayerEnteredRoom(Player player)
        {
            base.OnPlayerEnteredRoom(player);
            if (IsMine())
            {
                Cache.PhotonView.RPC(nameof(SetLeftHandStateRPC), player, new object[] { (byte)_leftHandState });
                Cache.PhotonView.RPC(nameof(SetRightHandStateRPC), player, new object[] { (byte)_rightHandState });
                Cache.PhotonView.RPC(nameof(SetSteamStateRPC), player, new object[] { (byte)_steamState });
                Cache.PhotonView.RPC(nameof(SetStunStateRPC), player, new object[] { (byte)_stunState });
            }
        }

        public override bool CheckNapeAngle(Vector3 hitPosition, float maxAngle)
        {
            return true;
        }

        public void SteamAttack()
        {
            if (CanAttack() && _stunState == ColossalStunState.None)
            {
                Attack("AttackSteam");
            }
        }

        public void SetLeftHandHealth(int health)
        {
            MaxLeftHandHealth = health;
            SetCurrentLeftHandHealth(health);
        }

        public void SetRightHandHealth(int health)
        {
            MaxRightHandHealth = health;
            SetCurrentRightHandHealth(health);
        }

        public void SetCurrentLeftHandHealth(int health)
        {
            CurrentLeftHandHealth = Mathf.Clamp(health, 0, MaxLeftHandHealth);
            OnLeftHandHealthChange();
        }

        public void SetCurrentRightHandHealth(int health)
        {
            CurrentRightHandHealth = Mathf.Clamp(health, 0, MaxRightHandHealth);
            OnRightHandHealthChange();
        }

        public void SetMaxLeftHandHealth(int maxHealth)
        {
            MaxLeftHandHealth = maxHealth;
            SetCurrentLeftHandHealth(CurrentLeftHandHealth);
        }

        public void SetMaxRightHandHealth(int maxHealth)
        {
            MaxRightHandHealth = maxHealth;
            SetCurrentRightHandHealth(CurrentRightHandHealth);
        }

        protected virtual void OnLeftHandHealthChange()
        {
            if (CurrentLeftHandHealth <= 0 && _leftHandState == ColossalHandState.Healthy)
            {
                if (IsMine())
                {
                    //DebugConsole.Log($"[WallColossal] Left Hand: Healthy -> Severed (Health: {CurrentLeftHandHealth}/{MaxLeftHandHealth})", false);
                    LeftHandSeverTimeLeft = HandSeverWindow;
                    photonView.RPC(nameof(SetLeftHandStateRPC), RpcTarget.All, new object[] { (byte)ColossalHandState.Severed });
                    CheckStunCondition();
                }
            }
            else if (CurrentLeftHandHealth <= 0 && _leftHandState == ColossalHandState.Damaged)
            {
                if (IsMine())
                {
                    //DebugConsole.Log($"[WallColossal] Left Hand: Damaged -> Severed (Health: {CurrentLeftHandHealth}/{MaxLeftHandHealth})", false);
                    LeftHandSeverTimeLeft = HandSeverWindow;
                    photonView.RPC(nameof(SetLeftHandStateRPC), RpcTarget.All, new object[] { (byte)ColossalHandState.Severed });
                    CheckStunCondition();
                }
            }
            else if (CurrentLeftHandHealth > 0 && CurrentLeftHandHealth < MaxLeftHandHealth && _leftHandState == ColossalHandState.Healthy)
            {
                if (IsMine())
                {
                    //DebugConsole.Log($"[WallColossal] Left Hand: Healthy -> Damaged (Health: {CurrentLeftHandHealth}/{MaxLeftHandHealth})", false);
                    photonView.RPC(nameof(SetLeftHandStateRPC), RpcTarget.All, new object[] { (byte)ColossalHandState.Damaged });
                }
            }
            else if (CurrentLeftHandHealth >= MaxLeftHandHealth && _leftHandState == ColossalHandState.Damaged)
            {
                if (IsMine())
                {
                    //DebugConsole.Log($"[WallColossal] Left Hand: Damaged -> Healthy (Health: {CurrentLeftHandHealth}/{MaxLeftHandHealth})", false);
                    photonView.RPC(nameof(SetLeftHandStateRPC), RpcTarget.All, new object[] { (byte)ColossalHandState.Healthy });
                }
            }

            if (IsMine())
                photonView.RPC(nameof(SetLeftHandHealthRPC), RpcTarget.All, new object[] { CurrentLeftHandHealth, MaxLeftHandHealth });
        }

        protected virtual void OnRightHandHealthChange()
        {
            if (CurrentRightHandHealth <= 0 && _rightHandState == ColossalHandState.Healthy)
            {
                if (IsMine())
                {
                    //DebugConsole.Log($"[WallColossal] Right Hand: Healthy -> Severed (Health: {CurrentRightHandHealth}/{MaxRightHandHealth})", false);
                    RightHandSeverTimeLeft = HandSeverWindow;
                    photonView.RPC(nameof(SetRightHandStateRPC), RpcTarget.All, new object[] { (byte)ColossalHandState.Severed });
                    CheckStunCondition();
                }
            }
            else if (CurrentRightHandHealth <= 0 && _rightHandState == ColossalHandState.Damaged)
            {
                if (IsMine())
                {
                    //DebugConsole.Log($"[WallColossal] Right Hand: Damaged -> Severed (Health: {CurrentRightHandHealth}/{MaxRightHandHealth})", false);
                    RightHandSeverTimeLeft = HandSeverWindow;
                    photonView.RPC(nameof(SetRightHandStateRPC), RpcTarget.All, new object[] { (byte)ColossalHandState.Severed });
                    CheckStunCondition();
                }
            }
            else if (CurrentRightHandHealth > 0 && CurrentRightHandHealth < MaxRightHandHealth && _rightHandState == ColossalHandState.Healthy)
            {
                if (IsMine())
                {
                    //DebugConsole.Log($"[WallColossal] Right Hand: Healthy -> Damaged (Health: {CurrentRightHandHealth}/{MaxRightHandHealth})", false);
                    photonView.RPC(nameof(SetRightHandStateRPC), RpcTarget.All, new object[] { (byte)ColossalHandState.Damaged });
                }
            }
            else if (CurrentRightHandHealth >= MaxRightHandHealth && _rightHandState == ColossalHandState.Damaged)
            {
                if (IsMine())
                {
                    //DebugConsole.Log($"[WallColossal] Right Hand: Damaged -> Healthy (Health: {CurrentRightHandHealth}/{MaxRightHandHealth})", false);
                    photonView.RPC(nameof(SetRightHandStateRPC), RpcTarget.All, new object[] { (byte)ColossalHandState.Healthy });
                }
            }

            if (IsMine())
                photonView.RPC(nameof(SetRightHandHealthRPC), RpcTarget.All, new object[] { CurrentRightHandHealth, MaxRightHandHealth });
        }

        protected void CheckStunCondition()
        {
            if (_leftHandState == ColossalHandState.Severed && _rightHandState == ColossalHandState.Severed)
            {
                //DebugConsole.Log($"[WallColossal] Both hands severed - Entering stun state", false);
                EnterStunState();
            }
        }

        protected void EnterStunState()
        {
            if (_stunState != ColossalStunState.None)
                return;

            //DebugConsole.Log($"[WallColossal] Stun State: None -> Stunned (Duration: {StunDuration}s)", false);
            StopSteam();
            StunTimeLeft = StunDuration;
            if (IsMine())
            {
                photonView.RPC(nameof(SetStunStateRPC), RpcTarget.All, new object[] { (byte)ColossalStunState.Stunned });
            }
        }

        [PunRPC]
        public void SetLeftHandHealthRPC(int currentHealth, int maxHealth, PhotonMessageInfo info)
        {
            if (info.Sender == photonView.Owner)
            {
                //DebugConsole.Log($"[WallColossal] SetLeftHandHealthRPC: {CurrentLeftHandHealth} -> {currentHealth} (Max: {maxHealth})", false);
                CurrentLeftHandHealth = currentHealth;
                MaxLeftHandHealth = maxHealth;
            }
        }

        [PunRPC]
        public void SetRightHandHealthRPC(int currentHealth, int maxHealth, PhotonMessageInfo info)
        {
            if (info.Sender == photonView.Owner)
            {
                //DebugConsole.Log($"[WallColossal] SetRightHandHealthRPC: {CurrentRightHandHealth} -> {currentHealth} (Max: {maxHealth})", false);
                CurrentRightHandHealth = currentHealth;
                MaxRightHandHealth = maxHealth;
            }
        }

        [PunRPC]
        public void SetLeftHandStateRPC(byte state, PhotonMessageInfo info)
        {
            if (info.Sender == photonView.Owner)
            {
                ApplyLeftHandState((ColossalHandState)state);
            }
        }

        [PunRPC]
        public void SetRightHandStateRPC(byte state, PhotonMessageInfo info)
        {
            if (info.Sender == photonView.Owner)
            {
                ApplyRightHandState((ColossalHandState)state);
            }
        }

        [PunRPC]
        public void SetSteamStateRPC(byte state, PhotonMessageInfo info)
        {
            if (info.Sender == photonView.Owner)
            {
                ApplySteamState((ColossalSteamState)state);
            }
        }

        [PunRPC]
        public void SetStunStateRPC(byte state, PhotonMessageInfo info)
        {
            if (info.Sender == photonView.Owner)
            {
                ApplyStunState((ColossalStunState)state);
            }
        }

        public override void Init(bool ai, string team, JSONNode data, float liveTime)
        {
            if (ai)
            {
                if (data.HasKey("HandHealth"))
                {
                    int handHealth = data["HandHealth"].AsInt;
                    SetLeftHandHealth(handHealth);
                    SetRightHandHealth(handHealth);
                }
                if (data.HasKey("LeftHandHealth"))
                    SetLeftHandHealth(data["LeftHandHealth"].AsInt);
                if (data.HasKey("RightHandHealth"))
                    SetRightHandHealth(data["RightHandHealth"].AsInt);
                if (data.HasKey("HandSeverWindow"))
                    HandSeverWindow = data["HandSeverWindow"].AsFloat;
                if (data.HasKey("StunDuration"))
                    StunDuration = data["StunDuration"].AsFloat;
                if (data.HasKey("RecoveryDuration"))
                    RecoveryDuration = data["RecoveryDuration"].AsFloat;
                if (data.HasKey("WarningSteamTime"))
                    WarningSteamTime = data["WarningSteamTime"].AsFloat;
            }
            base.Init(ai, team, data, liveTime);
        }

        protected override void CreateCache(BaseComponentCache cache)
        {
            ColossalCache = new WallColossalComponentCache(gameObject);
            base.CreateCache(ColossalCache);
        }

        protected override void CreateAnimations(BaseTitanAnimations animations)
        {
            ColossalAnimations = new WallColossalAnimations();
            base.CreateAnimations(ColossalAnimations);
        }

        protected override BaseCustomSkinLoader CreateCustomSkinLoader()
        {
            return gameObject.AddComponent<ColossalCustomSkinLoader>();
        }

        protected override string GetSkinURL(ShifterCustomSkinSet set)
        {
            return set.Colossal.Value;
        }

        protected override IEnumerator WaitAndDie()
        {
            yield return new WaitForSeconds(0.1f);
            Vector3 basePosition = BaseTitanCache.Transform.position;
            Vector3 headPosition = BaseTitanCache.Head.position;
            float y = (headPosition.y - basePosition.y) / 10f;
            for (int i = 0; i < 10; i++)
            {
                EffectSpawner.Spawn(EffectPrefabs.TitanDie2, basePosition + Vector3.up * y * i, Quaternion.Euler(-90f, 0f, 0f), Size * 10f, false);
            }
            PhotonNetwork.Destroy(gameObject);
        }

        public void ApplySteamState(ColossalSteamState newState)
        {
            _steamState = newState;

            switch (newState)
            {
                case ColossalSteamState.Off:
                    ToggleParticleSystem(ColossalCache.ColossalSteam1, false);
                    ToggleParticleSystem(ColossalCache.ColossalSteam2, false);
                    FadeSound(ShifterSounds.ColossalSteam1, 0f, 1f);
                    FadeSound(ShifterSounds.ColossalSteam2, 0f, 1f);

                    if (ColossalCache?.SteamHitbox != null)
                        ColossalCache.SteamHitbox.Deactivate();

                    if (ColossalCache?.SteamWarningZone != null)
                    {
                        ColossalCache.SteamWarningZone.SetActive(false);
                        ColossalCache.SteamWarningZoneComponent?.SetActive(false);
                    }
                    break;

                case ColossalSteamState.Warning:
                    ToggleParticleSystem(ColossalCache.ColossalSteam1, true);
                    ToggleParticleSystem(ColossalCache.ColossalSteam2, false);
                    FadeSound(ShifterSounds.ColossalSteam1, 0.6f, 0f);
                    PlaySound(ShifterSounds.ColossalSteam1);

                    if (ColossalCache?.SteamHitbox != null)
                        ColossalCache.SteamHitbox.Deactivate();

                    if (ColossalCache?.SteamWarningZone != null)
                    {
                        ColossalCache.SteamWarningZone.SetActive(true);
                        if (ColossalCache.SteamWarningZoneComponent != null)
                        {
                            ColossalCache.SteamWarningZoneComponent.Initialize(this);
                            ColossalCache.SteamWarningZoneComponent.SetActive(true);
                        }
                    }
                    break;

                case ColossalSteamState.Damage:
                    ToggleParticleSystem(ColossalCache.ColossalSteam1, false);
                    ToggleParticleSystem(ColossalCache.ColossalSteam2, true);
                    FadeSound(ShifterSounds.ColossalSteam1, 0f, 1f);
                    FadeSound(ShifterSounds.ColossalSteam2, 1f, 0f);
                    PlaySound(ShifterSounds.ColossalSteam2);

                    if (ColossalCache?.SteamHitbox != null)
                        ColossalCache.SteamHitbox.Activate();

                    if (ColossalCache?.SteamWarningZone != null)
                    {
                        ColossalCache.SteamWarningZone.SetActive(true);
                        if (ColossalCache.SteamWarningZoneComponent != null)
                        {
                            ColossalCache.SteamWarningZoneComponent.Initialize(this);
                            ColossalCache.SteamWarningZoneComponent.SetActive(true);
                        }
                    }
                    break;
            }
        }

        public void ApplyLeftHandState(ColossalHandState newState)
        {
            var oldState = _leftHandState;
            _leftHandState = newState;

            if (oldState != newState)
            {
                //DebugConsole.Log($"[WallColossal] ApplyLeftHandState: {oldState} -> {newState}", false);
            }

            switch (newState)
            {
                case ColossalHandState.Healthy:
                    ToggleParticleSystem(ColossalCache.LeftHandSteam, false);
                    break;

                case ColossalHandState.Damaged:
                    ToggleParticleSystem(ColossalCache.LeftHandSteam, false);
                    break;

                case ColossalHandState.Severed:
                    EffectSpawner.Spawn(EffectPrefabs.Blood1, ColossalCache.HandLHitbox.transform.position,
                        Quaternion.Euler(-90f, 0f, 0f), Size * 100);
                    ToggleParticleSystem(ColossalCache.LeftHandSteam, true);
                    break;

                case ColossalHandState.Recovering:
                    ToggleParticleSystem(ColossalCache.LeftHandSteam, true);
                    break;
            }
        }

        public void ApplyRightHandState(ColossalHandState newState)
        {
            var oldState = _rightHandState;
            _rightHandState = newState;

            if (oldState != newState)
            {
                //DebugConsole.Log($"[WallColossal] ApplyRightHandState: {oldState} -> {newState}", false);
            }

            switch (newState)
            {
                case ColossalHandState.Healthy:
                    ToggleParticleSystem(ColossalCache.RightHandSteam, false);
                    break;

                case ColossalHandState.Damaged:
                    ToggleParticleSystem(ColossalCache.RightHandSteam, false);
                    break;

                case ColossalHandState.Severed:
                    EffectSpawner.Spawn(EffectPrefabs.Blood1, ColossalCache.HandRHitbox.transform.position,
                        Quaternion.Euler(-90f, 0f, 0f), Size * 100);
                    ToggleParticleSystem(ColossalCache.RightHandSteam, true);
                    break;

                case ColossalHandState.Recovering:
                    ToggleParticleSystem(ColossalCache.RightHandSteam, true);
                    break;
            }
        }

        public void ApplyStunState(ColossalStunState newState)
        {
            var oldState = _stunState;
            _stunState = newState;

            if (oldState != newState)
            {
                //DebugConsole.Log($"[WallColossal] ApplyStunState: {oldState} -> {newState}", false);
            }

            switch (newState)
            {
                case ColossalStunState.None:
                    break;

                case ColossalStunState.Stunned:
                    // TODO: Play stun animation when available
                    // StateAction(TitanState.Stun, ColossalAnimations.Stun);
                    break;

                case ColossalStunState.Recovering:
                    break;
            }
        }

        public void LateUpdate()
        {
            if (LeftHandState == ColossalHandState.Severed || LeftHandState == ColossalHandState.Recovering)
            {
                ColossalCache.LeftHand.localScale = Vector3.zero;
                ColossalCache.HandLHitbox.transform.localRotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-10f, 10f));
            }
            else
            {
                ColossalCache.LeftHand.localScale = Vector3.one;
                ColossalCache.HandLHitbox.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }

            if (RightHandState == ColossalHandState.Severed || RightHandState == ColossalHandState.Recovering)
            {
                ColossalCache.RightHand.localScale = Vector3.zero;
                ColossalCache.HandRHitbox.transform.localRotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-10f, 10f));
            }
            else
            {
                ColossalCache.RightHand.localScale = Vector3.one;
                ColossalCache.HandRHitbox.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }

        public void Update()
        {
            base.Update();
            UpdateSteam();
            UpdateHandSeverWindows();
            UpdateStunRecovery();
        }

        protected void ToggleParticleSystem(ParticleSystem system, bool enabled)
        {
            if (enabled)
            {
                if (!system.isPlaying)
                    system.Play();
            }
            else
            {
                if (system.isPlaying)
                    system.Stop();
            }
        }

        public void StopSteam()
        {
            if (_steamState != ColossalSteamState.Off && IsMine())
            {
                photonView.RPC(nameof(SetSteamStateRPC), RpcTarget.All, new object[] { (byte)ColossalSteamState.Off });
            }
        }

        protected void StartSteam()
        {
            _steamTimeLeft = WarningSteamTime;
            _steamBlowAwayTimeLeft = BlowAwaySteamTime;

            if (IsMine())
            {
                photonView.RPC(nameof(SetSteamStateRPC), RpcTarget.All, new object[] { (byte)ColossalSteamState.Warning });
            }
        }

        protected void UpdateSteam()
        {
            if (_steamState == ColossalSteamState.Off)
                return;

            _steamTimeLeft -= Time.deltaTime;
            _steamBlowAwayTimeLeft -= Time.deltaTime;

            if (_steamBlowAwayTimeLeft <= 0f)
            {
                // BlowAwayHumans(ColossalCache.NapeHurtbox.transform.position, SteamBlowAwayForce);
                _steamBlowAwayTimeLeft = BlowAwaySteamTime;
            }

            if (_steamTimeLeft <= 0f && _steamState == ColossalSteamState.Warning)
            {
                if (IsMine())
                {
                    photonView.RPC(nameof(SetSteamStateRPC), RpcTarget.All, new object[] { (byte)ColossalSteamState.Damage });
                }
            }
        }

        protected void UpdateHandSeverWindows()
        {
            if (!IsMine())
                return;

            if (_stunState != ColossalStunState.None)
                return;

            if (_leftHandState == ColossalHandState.Severed)
            {
                LeftHandSeverTimeLeft -= Time.deltaTime;

                if (LeftHandSeverTimeLeft <= 0f)
                {
                    //DebugConsole.Log($"[WallColossal] Left Hand sever window expired - recovering independently", false);
                    SetCurrentLeftHandHealth(MaxLeftHandHealth);
                    photonView.RPC(nameof(SetLeftHandStateRPC), RpcTarget.All, new object[] { (byte)ColossalHandState.Healthy });
                    LeftHandSeverTimeLeft = 0f;
                }
            }

            if (_rightHandState == ColossalHandState.Severed)
            {
                RightHandSeverTimeLeft -= Time.deltaTime;

                if (RightHandSeverTimeLeft <= 0f)
                {
                    //DebugConsole.Log($"[WallColossal] Right Hand sever window expired - recovering independently", false);
                    SetCurrentRightHandHealth(MaxRightHandHealth);
                    photonView.RPC(nameof(SetRightHandStateRPC), RpcTarget.All, new object[] { (byte)ColossalHandState.Healthy });
                    RightHandSeverTimeLeft = 0f;
                }
            }
        }

        protected void UpdateStunRecovery()
        {
            if (!IsMine())
                return;

            if (_stunState == ColossalStunState.Stunned)
            {
                StunTimeLeft -= Time.deltaTime;

                if (StunTimeLeft <= 0f)
                {
                    //DebugConsole.Log($"[WallColossal] Stun State: Stunned -> Recovering (Duration: {RecoveryDuration}s)", false);
                    RecoveryTimeLeft = RecoveryDuration;
                    photonView.RPC(nameof(SetStunStateRPC), RpcTarget.All, new object[] { (byte)ColossalStunState.Recovering });
                    photonView.RPC(nameof(SetLeftHandStateRPC), RpcTarget.All, new object[] { (byte)ColossalHandState.Recovering });
                    photonView.RPC(nameof(SetRightHandStateRPC), RpcTarget.All, new object[] { (byte)ColossalHandState.Recovering });
                }
            }
            else if (_stunState == ColossalStunState.Recovering)
            {
                RecoveryTimeLeft -= Time.deltaTime;

                if (RecoveryTimeLeft <= 0f)
                {
                    //DebugConsole.Log($"[WallColossal] Recovery complete - returning to normal state", false);
                    SetCurrentLeftHandHealth(MaxLeftHandHealth);
                    SetCurrentRightHandHealth(MaxRightHandHealth);
                    photonView.RPC(nameof(SetStunStateRPC), RpcTarget.All, new object[] { (byte)ColossalStunState.None });
                    photonView.RPC(nameof(SetLeftHandStateRPC), RpcTarget.All, new object[] { (byte)ColossalHandState.Healthy });
                    photonView.RPC(nameof(SetRightHandStateRPC), RpcTarget.All, new object[] { (byte)ColossalHandState.Healthy });
                    LeftHandSeverTimeLeft = 0f;
                    RightHandSeverTimeLeft = 0f;
                }
            }
        }

        protected override void DeactivateAllHitboxes()
        {
            foreach (var hitbox in BaseTitanCache.Hitboxes)
            {
                if (hitbox == ColossalCache.SteamHitbox && _steamState == ColossalSteamState.Damage)
                    continue;
                hitbox.Deactivate();
            }
        }

        protected override void UpdateAttack()
        {
            float animationTime = GetAnimationTime();
            if (_currentAttackAnimation == ColossalAnimations.AttackSteam)
            {
                if (_currentAttackStage == 0 && animationTime > 0.37f)
                {
                    _currentAttackStage = 1;
                    StartSteam();
                }
            }
            else if (_currentAttackAnimation == ColossalAnimations.AttackKick)
            {
                if (_currentAttackStage == 0 && animationTime > 0.42f)
                {
                    _currentAttackStage = 1;
                    ColossalCache.FootRHitbox.Activate(0f, GetHitboxTime(0.1f));
                    EffectSpawner.Spawn(EffectPrefabs.Boom2, ColossalCache.FootRHitbox.transform.position, BaseTitanCache.Transform.rotation,
                        Size * 5f);
                }
            }
            else if (_currentAttackAnimation == ColossalAnimations.AttackSweep)
            {
                if (_currentAttackStage == 0 && animationTime > 0.38f)
                {
                    _currentAttackStage = 1;
                    ColossalCache.ForearmRHitbox.Activate(0f, GetHitboxTime(0.14f));
                }
                else if (_currentAttackStage == 1 && animationTime > 0.45f)
                {
                    PlaySound(TitanSounds.Swing1);
                    _currentAttackStage = 2;
                }
            }
            else if (_currentAttackAnimation == ColossalAnimations.AttackWallSlap1L  || _currentAttackAnimation == ColossalAnimations.AttackWallSlap2L)
            {
                if (_currentAttackStage == 0 && animationTime > 0.34f)
                {
                    _currentAttackStage = 1;
                    if (_leftHandState == ColossalHandState.Healthy || _leftHandState == ColossalHandState.Damaged)
                    {
                        ColossalCache.HandLHitbox.Activate(0f, GetHitboxTime(0.02f));
                        EffectSpawner.Spawn(EffectPrefabs.Boom8, ColossalCache.HandLHitbox.transform.position + Vector3.down * 8f, Quaternion.Euler(-90f, 0f, 0f),
                            Size * 4f);
                        BlowAwayHumans(ColossalCache.HandLHitbox.transform.position + Vector3.down * 10f, DefaultBlowAwayForce);
                    }
                }
            }
            else if (_currentAttackAnimation == ColossalAnimations.AttackWallSlap1R || _currentAttackAnimation == ColossalAnimations.AttackWallSlap2R)
            {
                if (_currentAttackStage == 0 && animationTime > 0.34f)
                {
                    _currentAttackStage = 1;
                    if (_rightHandState == ColossalHandState.Healthy || _rightHandState == ColossalHandState.Damaged)
                    {
                        ColossalCache.HandRHitbox.Activate(0f, GetHitboxTime(0.02f));
                        EffectSpawner.Spawn(EffectPrefabs.Boom8, ColossalCache.HandRHitbox.transform.position + Vector3.down * 8f, Quaternion.Euler(-90f, 0f, 0f),
                            Size * 4f);
                        BlowAwayHumans(ColossalCache.HandRHitbox.transform.position + Vector3.down * 10f, DefaultBlowAwayForce);
                    }
                }
            }
        }

        protected void BlowAwayHumans(Vector3 source, float force)
        {
            foreach (var human in _inGameManager.Humans)
            {
                if (Vector3.Distance(human.Cache.Transform.position, source) < BlowAwayMaxDistance)
                    human.BlowAway(source, force, BlowAwayMaxDistance);
            }
        }

        [PunRPC]
        public override void GetHitRPC(int viewId, string name, int damage, string type, string collider)
        {
            if (Dead)
                return;
            var settings = SettingsManager.InGameCurrent.Titan;
            if (settings.TitanArmorEnabled.Value)
            {
                if (damage < settings.TitanArmor.Value)
                    damage = 0;
            }
            if (collider == ColossalCache.NapeHurtbox.name)
            {
                base.GetHitRPC(viewId, name, damage, type, collider);
            }
            else if (collider == ColossalCache.HandLHurtbox.name)
            {
                if (!CanDamageLeftHand)
                    return;

                if (_leftHandState == ColossalHandState.Healthy || _leftHandState == ColossalHandState.Damaged)
                {
                    SetCurrentLeftHandHealth(CurrentLeftHandHealth - damage);
                }
            }
            else if (collider == ColossalCache.HandRHurtbox.name)
            {
                if (!CanDamageRightHand)
                    return;

                if (_rightHandState == ColossalHandState.Healthy || _rightHandState == ColossalHandState.Damaged)
                {
                    SetCurrentRightHandHealth(CurrentRightHandHealth - damage);
                }
            }
        }
    }

    public enum ColossalSteamState
    {
        Off,
        Warning,
        Damage
    }

    public enum ColossalHandState
    {
        Healthy,
        Damaged,
        Severed,
        Recovering
    }

    public enum ColossalStunState
    {
        None,
        Stunned,
        Recovering
    }
}
