using System;
using UnityEngine;
using ApplicationManagers;
using GameManagers;
using UnityEngine.UI;
using Utility;
using Controllers;
using CustomSkins;
using System.Collections.Generic;
using SimpleJSONFixed;
using Effects;
using Settings;

namespace Characters
{
    class ErenShifter : BaseShifter
    {
        protected ErenComponentCache ErenCache;
        protected ErenAnimations ErenAnimations;
        public override List<string> EmoteActions => new List<string>() { "Nod", "Roar" };
        protected int _stepSoundPhase = 0;

		protected override float DefaultRunSpeed => 90;

        protected override void CreateCache(BaseComponentCache cache)
        {
            ErenCache = new ErenComponentCache(gameObject);
            base.CreateCache(ErenCache);
        }

        protected override void CreateAnimations(BaseTitanAnimations animations)
        {
            ErenAnimations = new ErenAnimations();
            base.CreateAnimations(ErenAnimations);
        }

        protected override BaseCustomSkinLoader CreateCustomSkinLoader()
        {
            return gameObject.AddComponent<ErenCustomSkinLoader>();
        }

        protected override string GetSkinURL(ShifterCustomSkinSet set)
        {
            return set.Eren.Value;
        }

        public override void Emote(string emote)
        {
            string anim = string.Empty;
            if (emote == "Nod")
                anim = ErenAnimations.EmoteNod;
            else if (emote == "Roar")
            {
                StartCoroutine(WaitAndPlaySound(ShifterSounds.ErenRoar, 0.9f));
                anim = ErenAnimations.EmoteRoar;
            }
            if (anim != "")
                StateAction(TitanState.Emote, anim);
        }

        protected override Dictionary<string, float> GetRootMotionAnimations()
        {
            return new Dictionary<string, float>() { {ErenAnimations.Attack, 1f }, { ErenAnimations.Kick, 1f } };
        }

        public override void Attack(string attack)
        {
            ResetAttackState(attack);
            if (_currentAttack == ShifterAttacks.AttackDefault)
                StateAttack(ErenAnimations.Attack);
            else if (_currentAttack == ShifterAttacks.AttackKick)
                StateAttack(ErenAnimations.Kick);
        }

        protected override void UpdateAttack()
        {
            float animationTime = GetAnimationTime();
            if (_currentStateAnimation == ErenAnimations.Attack)
            {
                if (_currentAttackStage == 0 && animationTime > 0.15f)
                {
                    _currentAttackStage = 1;
                    ErenCache.HandRHitbox.Activate(0f, 0.045f / _currentAttackSpeed);
                    PlaySound(TitanSounds.Swing1);
                }
                else if (_currentAttackStage == 1 && animationTime > 0.28f)
                {
                    _currentAttackStage = 2;
                    ErenCache.HandLHitbox.Activate(0f, 0.09f / _currentAttackSpeed);
                    PlaySound(TitanSounds.Swing2);
                }
                else if (_currentAttackStage == 2 && animationTime > 0.565f)
                {
                    _currentAttackStage = 3;
                    ErenCache.HandRHitbox.Activate(0f, 0.09f / _currentAttackSpeed);
                    PlaySound(TitanSounds.Swing3);
                }
            }
            else if (_currentStateAnimation == ErenAnimations.Kick)
            {
                if (_currentAttackStage == 0 && animationTime > 0.3f)
                {
                    _currentAttackStage = 1;
                    ErenCache.FootRHitbox.Activate(0f, 0.22f / _currentAttackSpeed);
                    PlaySound(TitanSounds.Swing1);
                }
            }
        }
    }
}
