using System;
using UnityEngine;
using ApplicationManagers;
using GameManagers;
using UnityEngine.UI;
using Utility;
using Controllers;
using CustomSkins;
using Effects;
using Settings;
using Photon.Pun;
using System.Collections;
using SimpleJSONFixed;

namespace Characters
{
    class WallColossalShifter : BaseShifter
    {
        protected WallColossalComponentCache ColossalCache;
        protected WallColossalAnimations ColossalAnimations;
        protected float _steamTimeLeft;
        public ColossalSteamState _steamState;
        protected const float WarningSteamTime = 2f;
        protected override float SizeMultiplier => 22f;

        public int MaxHandHealth = 1000;
        public int CurrentHandHealth = 1000;

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

        public void SetHandHealth(int health)
        {
            MaxHandHealth = health;
            SetCurrentHandHealth(health);
        }

        public void SetCurrentHandHealth(int health)
        {
            CurrentHandHealth = Mathf.Min(health, MaxHandHealth);
            CurrentHandHealth = Mathf.Max(CurrentHandHealth, 0);
            OnHandHealthChange();
        }

        public void SetMaxHandHealth(int maxHealth)
        {
            MaxHandHealth = maxHealth;
            SetCurrentHandHealth(CurrentHandHealth);
        }

        protected virtual void OnHandHealthChange()
        {
            if (IsMine())
                photonView.RPC("SetHandHealthRPC", RpcTarget.All, new object[] { CurrentHandHealth, MaxHandHealth });
        }

        [PunRPC]
        public void SetHandHealthRPC(int currentHealth, int maxHealth, PhotonMessageInfo info)
        {
            if (info.Sender == photonView.Owner)
            {
                CurrentHandHealth = currentHealth;
                MaxHandHealth = maxHealth;
            }
        }

        public override void Init(bool ai, string team, JSONNode data, float liveTime)
        {
            if (ai)
            {
                if (data.HasKey("HandHealth"))
                    SetHandHealth(data["HandHealth"].AsInt);
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

        protected void StopAllSteamEffects(bool stopSound)
        {
            ColossalCache.ColossalSteam1.Stop();
            ColossalCache.ColossalSteam2.Stop();
            if (stopSound)
            {
                FadeSound(ShifterSounds.ColossalSteam1, 0f, 1f);
                FadeSound(ShifterSounds.ColossalSteam2, 0f, 1f);
            }
        }

        public void StopSteam()
        {
            if (_steamState != ColossalSteamState.Off)
            {
                StopAllSteamEffects(true);
                _steamState = ColossalSteamState.Off;
                ColossalCache.SteamHitbox.Deactivate();
            }
        }

        protected void StartSteam()
        {
            StopAllSteamEffects(false);
            ColossalCache.ColossalSteam1.Play();
            FadeSound(ShifterSounds.ColossalSteam1, 0.6f, 0f);
            PlaySound(ShifterSounds.ColossalSteam1);
            _steamTimeLeft = WarningSteamTime;
            _steamState = ColossalSteamState.Warning;
            ColossalCache.SteamHitbox.Deactivate();
        }

        protected void UpdateSteam()
        {
            if (_steamState == ColossalSteamState.Off)
                return;
            _steamTimeLeft -= Time.deltaTime;
            if (_steamTimeLeft <= 0f)
            {
                if (_steamState == ColossalSteamState.Warning)
                {
                    StopAllSteamEffects(false);
                    ColossalCache.ColossalSteam2.Play();
                    FadeSound(ShifterSounds.ColossalSteam1, 0f, 1f);
                    FadeSound(ShifterSounds.ColossalSteam2, 1f, 0f);
                    // FadeSound(ShifterSounds.ColossalSteam2, 1f, 1f);
                    PlaySound(ShifterSounds.ColossalSteam2);
                    _steamState = ColossalSteamState.Damage;
                    ColossalCache.SteamHitbox.Activate();
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

        protected override void Update()
        {
            base.Update();
            UpdateSteam();
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
                if (_currentAttackStage == 0 && animationTime > 0.37f)
                {
                    _currentAttackStage = 1;
                    ColossalCache.HandRHitbox.Activate(0f, GetHitboxTime(0.18f));
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
                    ColossalCache.HandLHitbox.Activate(0f, GetHitboxTime(0.02f));
                    EffectSpawner.Spawn(EffectPrefabs.Boom2, ColossalCache.HandLHitbox.transform.position, Quaternion.Euler(-90f, 0f, 0f),
                        Size * 5f);
                }
            }
            else if (_currentAttackAnimation == ColossalAnimations.AttackWallSlap1R || _currentAttackAnimation == ColossalAnimations.AttackWallSlap2R)
            {
                if (_currentAttackStage == 0 && animationTime > 0.34f)
                {
                    _currentAttackStage = 1;
                    ColossalCache.HandRHitbox.Activate(0f, GetHitboxTime(0.02f));
                    EffectSpawner.Spawn(EffectPrefabs.Boom2, ColossalCache.HandRHitbox.transform.position, Quaternion.Euler(-90f, 0f, 0f),
                        Size * 5f);
                }
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
            else if (collider == ColossalCache.HandLHurtbox.name || collider == ColossalCache.HandRHurtbox.name)
            {
                SetCurrentHandHealth(CurrentHandHealth - damage);
            }
        }
    }

    enum ColossalSteamState
    {
        Off,
        Warning,
        Damage
    }
}
