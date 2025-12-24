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

namespace Characters
{
    partial class WallColossalShifter : BaseShifter
    {
        public ColossalSteamState SteamState => _steamState;
        public ColossalHandState LeftHandState => _leftHandState;
        public ColossalHandState RightHandState => _rightHandState;

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

        protected ColossalHandState _leftHandState = ColossalHandState.Healthy;
        protected ColossalHandState _rightHandState = ColossalHandState.Healthy;
        public float LeftHandRecoveryTimeLeft = 0f;
        public float RightHandRecoveryTimeLeft = 0f;
        public float HandRecoveryTime = 15f;

        public float SteamBlowAwayForce = 30f;
        public float DefaultBlowAwayForce = 50f;
        public float BlowAwayMaxDistance = 60f;
        public float BlowAwaySteamTime = 0.5f;


        public override bool CheckNapeAngle(Vector3 hitPosition, float maxAngle)
        {
            return true;
        }

        public void SteamAttack()
        {
            if (CanAttack())
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
                ApplyLeftHandState(ColossalHandState.Broken);
                LeftHandRecoveryTimeLeft = HandRecoveryTime;
                if (IsMine())
                    photonView.RPC(nameof(SetLeftHandStateRPC), RpcTarget.Others, new object[] { (byte)ColossalHandState.Broken });
            }

            if (IsMine())
                photonView.RPC(nameof(SetLeftHandHealthRPC), RpcTarget.All, new object[] { CurrentLeftHandHealth, MaxLeftHandHealth });
        }

        protected virtual void OnRightHandHealthChange()
        {

            if (CurrentRightHandHealth <= 0 && _rightHandState == ColossalHandState.Healthy)
            {
                ApplyRightHandState(ColossalHandState.Broken);
                RightHandRecoveryTimeLeft = HandRecoveryTime;
                if (IsMine())
                    photonView.RPC(nameof(SetRightHandStateRPC), RpcTarget.Others, new object[] { (byte)ColossalHandState.Broken });
            }

            if (IsMine())
                photonView.RPC(nameof(SetRightHandHealthRPC), RpcTarget.All, new object[] { CurrentRightHandHealth, MaxRightHandHealth });
        }

        [PunRPC]
        public void SetLeftHandHealthRPC(int currentHealth, int maxHealth, PhotonMessageInfo info)
        {
            if (info.Sender == photonView.Owner)
            {
                CurrentLeftHandHealth = currentHealth;
                MaxLeftHandHealth = maxHealth;
            }
        }

        [PunRPC]
        public void SetRightHandHealthRPC(int currentHealth, int maxHealth, PhotonMessageInfo info)
        {
            if (info.Sender == photonView.Owner)
            {
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
                if (data.HasKey("HandRecoveryTime"))
                    HandRecoveryTime = data["HandRecoveryTime"].AsFloat;
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

                    // Disable warning zone
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

                    // Enable warning zone
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

                    // Keep warning zone active during damage phase
                    // (players already in the zone should still see the warning effect)
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
            _leftHandState = newState;

            switch (newState)
            {
                case ColossalHandState.Healthy:
                    ToggleParticleSystem(ColossalCache.LeftHandSteam, false);
                    break;

                case ColossalHandState.Broken:
                    EffectSpawner.Spawn(EffectPrefabs.Blood1, ColossalCache.HandLHitbox.transform.position,
                        Quaternion.Euler(-90f, 0f, 0f), Size * 100);
                    ToggleParticleSystem(ColossalCache.LeftHandSteam, true);
                    break;
            }
        }

        public void LateUpdate()
        {
            // Apply hand scaling based on state (must be done in LateUpdate as animations override transforms)
            if (LeftHandState == ColossalHandState.Broken)
            {
                ColossalCache.LeftHand.localScale = Vector3.zero;
                ColossalCache.HandLHitbox.transform.localRotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-10f, 10f));
            }
            else
            {
                ColossalCache.LeftHand.localScale = Vector3.one;
                ColossalCache.HandLHitbox.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            }

            if (RightHandState == ColossalHandState.Broken)
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

        public void ApplyRightHandState(ColossalHandState newState)
        {
            _rightHandState = newState;

            switch (newState)
            {
                case ColossalHandState.Healthy:
                    ToggleParticleSystem(ColossalCache.RightHandSteam, false);
                    break;

                case ColossalHandState.Broken:
                    EffectSpawner.Spawn(EffectPrefabs.Blood1, ColossalCache.HandRHitbox.transform.position,
                        Quaternion.Euler(-90f, 0f, 0f), 100);
                    ToggleParticleSystem(ColossalCache.RightHandSteam, true);
                    break;
            }
        }

        public void Update()
        {
            base.Update();
            UpdateSteam();
            UpdateHandRecovery();
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
                ApplySteamState(ColossalSteamState.Off);
                photonView.RPC(nameof(SetSteamStateRPC), RpcTarget.Others, new object[] { (byte)ColossalSteamState.Off });
            }
        }

        protected void StartSteam()
        {
            _steamTimeLeft = WarningSteamTime;
            _steamBlowAwayTimeLeft = BlowAwaySteamTime;

            if (IsMine())
            {
                ApplySteamState(ColossalSteamState.Warning);
                photonView.RPC(nameof(SetSteamStateRPC), RpcTarget.Others, new object[] { (byte)ColossalSteamState.Warning });
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
                    ApplySteamState(ColossalSteamState.Damage);
                    photonView.RPC(nameof(SetSteamStateRPC), RpcTarget.Others, new object[] { (byte)ColossalSteamState.Damage });
                }
            }
        }

        protected void UpdateHandRecovery()
        {
            if (!IsMine())
                return;

            // Update left hand recovery
            if (_leftHandState == ColossalHandState.Broken)
            {
                LeftHandRecoveryTimeLeft -= Time.deltaTime;

                if (LeftHandRecoveryTimeLeft <= 0f)
                {
                    ApplyLeftHandState(ColossalHandState.Healthy);
                    SetCurrentLeftHandHealth(MaxLeftHandHealth);
                    photonView.RPC(nameof(SetLeftHandStateRPC), RpcTarget.Others, new object[] { (byte)ColossalHandState.Healthy });
                }
            }

            // Update right hand recovery
            if (_rightHandState == ColossalHandState.Broken)
            {
                RightHandRecoveryTimeLeft -= Time.deltaTime;

                if (RightHandRecoveryTimeLeft <= 0f)
                {
                    ApplyRightHandState(ColossalHandState.Healthy);
                    SetCurrentRightHandHealth(MaxRightHandHealth);
                    photonView.RPC(nameof(SetRightHandStateRPC), RpcTarget.Others, new object[] { (byte)ColossalHandState.Healthy });
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
                    if (_leftHandState == ColossalHandState.Healthy)
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
                    if (_rightHandState == ColossalHandState.Healthy)
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
                if (_leftHandState == ColossalHandState.Healthy)
                {
                    SetCurrentLeftHandHealth(CurrentLeftHandHealth - damage);
                }
            }
            else if (collider == ColossalCache.HandRHurtbox.name)
            {
                if (_rightHandState == ColossalHandState.Healthy)
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
        Broken
    }
}
