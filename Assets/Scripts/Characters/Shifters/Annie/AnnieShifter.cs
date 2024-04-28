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
    class AnnieShifter : BaseShifter
    {
        protected AnnieComponentCache AnnieCache;
        protected AnnieAnimations AnnieAnimations;
        public override List<string> EmoteActions => new List<string>() { "Salute", "Roar", "Taunt", "Wave" };

        protected override void CreateCache(BaseComponentCache cache)
        {
            AnnieCache = new AnnieComponentCache(gameObject);
            base.CreateCache(AnnieCache);
        }

        protected override void CreateAnimations(BaseTitanAnimations animations)
        {
            AnnieAnimations = new AnnieAnimations();
            base.CreateAnimations(AnnieAnimations);
        }

        public override void Emote(string emote)
        {
            string anim = string.Empty;
            if (emote == "Salute")
                anim = AnnieAnimations.EmoteSalute;
            else if (emote == "Roar")
            {
                anim = AnnieAnimations.EmoteRoar;
                StartCoroutine(WaitAndPlaySound(ShifterSounds.AnnieRoar, 0.1f));
            }
            else if (emote == "Taunt")
                anim = AnnieAnimations.EmoteTaunt;
            else if (emote == "Wave")
                anim = AnnieAnimations.EmoteWave;
            if (anim != "")
                StateAction(TitanState.Emote, anim);
        }

        protected override BaseCustomSkinLoader CreateCustomSkinLoader()
        {
            return gameObject.AddComponent<AnnieCustomSkinLoader>();
        }

        protected override string GetSkinURL(ShifterCustomSkinSet set)
        {
            return set.Annie.Value;
        }

        protected override Dictionary<string, float> GetRootMotionAnimations()
        {
            return new Dictionary<string, float>() { { AnnieAnimations.AttackCombo, 0.95f } };
        }

        protected override void UpdateAttack()
        {
            float animationTime = GetAnimationTime();
            if (_currentAttackAnimation == AnnieAnimations.AttackCombo)
            {
                if(_currentAttackStage == 0 && animationTime > 0.15f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.FootRHitbox.Activate(0f, 0.15f / _currentAttackSpeed);
                    PlaySound(TitanSounds.Swing1);
                }
                else if (_currentAttackStage ==  1 && animationTime > 0.31f)
                {
                    _currentAttackStage = 2;
                    AnnieCache.FootLHitbox.Activate(0f, 0.15f / _currentAttackSpeed);
                    PlaySound(TitanSounds.Swing2);
                }
                else if (_currentAttackStage == 2 && animationTime > 0.59f)
                {
                    _currentAttackStage = 3;
                    AnnieCache.FootRHitbox.Activate(0f, 0.15f / _currentAttackSpeed);
                    PlaySound(TitanSounds.Swing3);
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackKick)
            {
                if (_currentAttackStage == 0 && animationTime > 0.38f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.FootRHitbox.Activate(0f, 0.13f / _currentAttackSpeed);
                    PlaySound(TitanSounds.Swing1);
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackSwing)
            {
                if (_currentAttackStage == 0 && animationTime > 0.43f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.HandRHitbox.Activate(0f, 0.25f / _currentAttackSpeed);
                    PlaySound(TitanSounds.Swing1);
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackStomp)
            {
                if (_currentAttackStage == 0 && animationTime > 0.23f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.FootLHitbox.Activate(0f, 0.15f / _currentAttackSpeed);
                    var position = BaseTitanCache.FootLHitbox.transform.position;
                    position.y = BaseTitanCache.Transform.position.y;
                    EffectSpawner.Spawn(EffectPrefabs.Boom5, position, BaseTitanCache.Transform.rotation, Size * SizeMultiplier);
                    SpawnShatter(position);
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackHead)
            {
                if (_currentAttackStage == 0 && animationTime > 0.21f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.HandLHitbox.Activate(0f, 0.2f / _currentAttackSpeed);
                    AnnieCache.HandRHitbox.Activate(0f, 0.2f / _currentAttackSpeed);
                    PlaySound(TitanSounds.Swing1);
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackBite)
            {
                if (_currentAttackStage == 0 && animationTime > 0.25f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.MouthHitbox.Activate(0f, 0.2f / _currentAttackSpeed);
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackBrushBack)
            {
                if (_currentAttackStage == 0 && animationTime > 0.25f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.HandRHitbox.Activate(0f, 0.45f / _currentAttackSpeed);
                    PlaySound(TitanSounds.Swing1);
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackBrushFrontL)
            {
                if (_currentAttackStage == 0 && animationTime > 0.25f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.HandRHitbox.Activate(0f, 0.45f / _currentAttackSpeed);
                    PlaySound(TitanSounds.Swing1);
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackBrushFrontR)
            {
                if (_currentAttackStage == 0 && animationTime > 0.25f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.HandLHitbox.Activate(0f, 0.45f / _currentAttackSpeed);
                    PlaySound(TitanSounds.Swing1);
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackBrushHeadL)
            {
                if (_currentAttackStage == 0 && animationTime > 0.37f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.HandLHitbox.Activate(0f, 0.3f / _currentAttackSpeed);
                    PlaySound(TitanSounds.Swing1);
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackBrushHeadR)
            {
                if (_currentAttackStage == 0 && animationTime > 0.37f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.HandRHitbox.Activate(0f, 0.3f / _currentAttackSpeed);
                    PlaySound(TitanSounds.Swing1);
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackGrabBottomLeft)
            {
                if (_currentAttackStage == 0 && animationTime > 0.23f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.HandRHitbox.Activate(0f, 0.31f / _currentAttackSpeed);
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackGrabBottomRight)
            {
                if (_currentAttackStage == 0 && animationTime > 0.23f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.HandLHitbox.Activate(0f, 0.31f / _currentAttackSpeed);
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackGrabMidLeft)
            {
                if (_currentAttackStage == 0 && animationTime > 0.23f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.HandRHitbox.Activate(0f, 0.31f / _currentAttackSpeed);
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackGrabMidRight)
            {
                if (_currentAttackStage == 0 && animationTime > 0.23f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.HandLHitbox.Activate(0f, 0.31f / _currentAttackSpeed);
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackGrabUp)
            {
                if (_currentAttackStage == 0 && animationTime > 0.23f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.HandLHitbox.Activate(0f, 0.31f / _currentAttackSpeed);
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackGrabUpLeft)
            {
                if (_currentAttackStage == 0 && animationTime > 0.23f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.HandLHitbox.Activate(0f, 0.31f / _currentAttackSpeed);
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackGrabUpRight)
            {
                if (_currentAttackStage == 0 && animationTime > 0.23f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.HandRHitbox.Activate(0f, 0.31f / _currentAttackSpeed);
                }
            }
        }
    }
}
