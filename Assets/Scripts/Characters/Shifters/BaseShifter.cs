using System;
using UnityEngine;
using ApplicationManagers;
using GameManagers;
using UnityEngine.UI;
using Utility;
using Controllers;
using SimpleJSONFixed;
using Effects;
using UI;
using Settings;
using System.Collections;
using CustomLogic;
using CustomSkins;
using Photon.Pun;

namespace Characters
{
    class BaseShifter: BaseTitan
    {
        protected override int DefaultMaxHealth => 1000;
        protected override float DefaultRunSpeed => 100f;
        protected override float DefaultWalkSpeed => 20f;
        protected override float DefaultRotateSpeed => 10f;
        protected override float DefaultJumpForce => 200f;
        protected override float SizeMultiplier => 3f;
        public override float DefaultCrippleTime => 3.5f;
        protected bool _needRoar = true;
        public bool TransformingToHuman;
        public float PreviousHumanGas;
        public BaseUseable PreviousHumanWeapon;
        public float DeathAnimationLength = 2f;
        protected BaseCustomSkinLoader _customSkinLoader;

        protected override void Start()
        {
            _inGameManager.Shifters.Add(this);
            base.Start();
            if (IsMine())
            {
                EffectSpawner.Spawn(EffectPrefabs.ShifterThunder, BaseTitanCache.Neck.position, Quaternion.identity, Size);
                PlaySound(ShifterSounds.Thunder);
                LoadSkin();
                CheckGround();
                if (!Grounded && BaseTitanAnimations.Jump != "")
                    StateActionWithTime(TitanState.Jump, BaseTitanAnimations.Jump, 0.2f);
            }
        }

        public override void Kick()
        {
            Attack("AttackKick");
        }

        [PunRPC]
        public void MarkTransformingRPC(PhotonMessageInfo info)
        {
            if (info.Sender != Cache.PhotonView.Owner)
                return;
            TransformingToHuman = true;
        }

        public void Init(bool ai, string team, JSONNode data, float liveTime)
        {
            if (ai)
            {
                var controller = gameObject.AddComponent<BaseTitanAIController>();
                controller.Init(data);
                Name = data["Name"].Value;
            }
            else
            {
                gameObject.AddComponent<ShifterPlayerController>();
                if (liveTime > 0f)
                    StartCoroutine(WaitAndBecomeHuman(liveTime));
            }
            base.Init(ai, team, data);
        }

        protected IEnumerator WaitAndBecomeHuman(float time)
        {
            yield return new WaitForSeconds(time);
            Cache.PhotonView.RPC("MarkTransformingRPC", RpcTarget.AllBuffered);
            Cache.PhotonView.RPC("MarkDeadRPC", RpcTarget.AllBuffered);
            StartCoroutine(WaitAndDie());
            yield return new WaitForSeconds(2f);
            _inGameManager.SpawnPlayerAt(false, BaseTitanCache.Neck.position, BaseTitanCache.Neck.rotation.eulerAngles.y);
            Human currentCharacter = ((Human)_inGameManager.CurrentCharacter);
            currentCharacter.StartCoroutine(currentCharacter.WaitAndTransformFromShifter(PreviousHumanGas, PreviousHumanWeapon));
        }

        protected override IEnumerator WaitAndDie()
        {
            StateActionWithTime(TitanState.Dead, BaseTitanAnimations.Die, 0f, 0.1f);
            yield return new WaitForSeconds(DeathAnimationLength);
            EffectSpawner.Spawn(EffectPrefabs.TitanDie1, BaseTitanCache.Hip.position, Quaternion.Euler(-90f, 0f, 0f), GetSpawnEffectSize(), false);
            yield return new WaitForSeconds(3f);
            EffectSpawner.Spawn(EffectPrefabs.TitanDie2, BaseTitanCache.Hip.position, Quaternion.Euler(-90f, 0f, 0f), GetSpawnEffectSize(), false);
            PhotonNetwork.Destroy(gameObject);
        }

        protected override void Awake()
        {
            base.Awake();
            _customSkinLoader = CreateCustomSkinLoader();
        }

        [PunRPC]
        public override void GetHitRPC(int viewId, string name, int damage, string type, string collider)
        {
            if (Dead)
                return;
            if (type == "CannonBall" || type == "Rock")
            {
                base.GetHitRPC(viewId, name, damage, type, collider);
                return;
            }
            var settings = SettingsManager.InGameCurrent.Titan;
            if (settings.TitanArmorEnabled.Value)
            {
                if (damage < settings.TitanArmor.Value)
                    damage = 0;
            }
            if (type == "TitanStun" || type == "ShifterStun")
            {
                Stun();
                var killer = Util.FindCharacterByViewId(viewId);
                if (killer != null)
                {
                    Vector3 direction = killer.Cache.Transform.position - Cache.Transform.position;
                    direction.y = 0f;
                    Cache.Transform.forward = direction.normalized;
                }
                base.GetHitRPC(viewId, name, damage, type, collider);
            }
            else if (BaseTitanCache.EyesHurtbox != null && collider == BaseTitanCache.EyesHurtbox.name)
                Blind();
            else if (BaseTitanCache.LegLHurtbox != null && (collider == BaseTitanCache.LegLHurtbox.name || collider == BaseTitanCache.LegRHurtbox.name))
                Cripple();
            else if (collider == BaseTitanCache.NapeHurtbox.name)
                base.GetHitRPC(viewId, name, damage, type, collider);
        }

        public override void OnHit(BaseHitbox hitbox, object victim, Collider collider, string type, bool firstHit)
        {
            int damage = 100;
            if (CustomDamageEnabled)
                damage = CustomDamage;
            if (victim is CustomLogicCollisionHandler)
            {
                ((CustomLogicCollisionHandler)victim).GetHit(this, Name, damage, type);
                return;
            }
            var victimChar = (BaseCharacter)victim;
            if (victimChar is BaseTitan)
            {
                if (firstHit)
                {
                    EffectSpawner.Spawn(EffectPrefabs.PunchHit, hitbox.transform.position, Quaternion.identity);
                    PlaySound(TitanSounds.Hit);
                    if (!victimChar.Dead)
                    {
                        if (IsMainCharacter())
                            ((InGameMenu)UIManager.CurrentMenu).ShowKillScore(damage);
                        victimChar.GetHit(this, damage, "ShifterStun", collider.name);
                    }
                }
            }
            else
            {
                if (!victimChar.Dead)
                {
                    if (IsMainCharacter())
                        ((InGameMenu)UIManager.CurrentMenu).ShowKillScore(damage);
                    victimChar.GetHit(this, damage, "Shifter", collider.name);
                }
            }
        }

        protected override void Update()
        {
            base.Update();
            if (IsMine())
            {
                if (_needRoar && Grounded && CanAction())
                {
                    Emote("Roar");
                    _needRoar = false;
                }
            }
        }

        public override void Land()
        {
            if (_needRoar)
            {
                Emote("Roar");
                _needRoar = false;
            }
            else
                StateAction(TitanState.Land, BaseTitanAnimations.Land);
            EffectSpawner.Spawn(EffectPrefabs.Boom2, Cache.Transform.position + Vector3.down * _currentGroundDistance,
                Quaternion.Euler(270f, 0f, 0f), Size * SizeMultiplier);
        }

        protected void LoadSkin()
        {
            if (IsMine())
            {
                if (SettingsManager.CustomSkinSettings.Shifter.SkinsEnabled.Value)
                {
                    BaseCustomSkinSettings<ShifterCustomSkinSet> settings = SettingsManager.CustomSkinSettings.Shifter;
                    string url = GetSkinURL((ShifterCustomSkinSet)settings.GetSelectedSet());
                    Cache.PhotonView.RPC("LoadSkinRPC", RpcTarget.AllBuffered, new object[] { url });
                }
            }
        }

        protected virtual string GetSkinURL(ShifterCustomSkinSet set)
        {
            return "";
        }

        protected virtual BaseCustomSkinLoader CreateCustomSkinLoader()
        {
            return null;
        }


        [PunRPC]
        public void LoadSkinRPC(string url, PhotonMessageInfo info)
        {
            if (info.Sender != Cache.PhotonView.Owner)
                return;
            if (_customSkinLoader == null)
                return;
            BaseCustomSkinSettings<ShifterCustomSkinSet> settings = SettingsManager.CustomSkinSettings.Shifter;
            if (settings.SkinsEnabled.Value && (!settings.SkinsLocal.Value || Cache.PhotonView.IsMine))
            {
                StartCoroutine(_customSkinLoader.LoadSkinsFromRPC(new object[] { url }));
            }
        }
    }
}
