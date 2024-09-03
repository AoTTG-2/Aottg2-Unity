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
                if(_currentAttackStage == 0 && animationTime > 0.155f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.FootRHitbox.Activate(0f, GetHitboxTime(0.07f));
                    PlaySound(TitanSounds.Swing1);
                }
                else if (_currentAttackStage ==  1 && animationTime > 0.32f)
                {
                    _currentAttackStage = 2;
                    AnnieCache.FootLHitbox.Activate(0f, GetHitboxTime(0.06f));
                    PlaySound(TitanSounds.Swing2);
                }
                else if (_currentAttackStage == 2 && animationTime > 0.59f)
                {
                    _currentAttackStage = 3;
                    AnnieCache.FootRHitbox.Activate(0f, GetHitboxTime(0.05f));
                    PlaySound(TitanSounds.Swing3);
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackKick)
            {
                if (_currentAttackStage == 0 && animationTime > 0.395f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.FootRHitbox.Activate(0f, GetHitboxTime(0.12f));
                    PlaySound(TitanSounds.Swing1);
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackSwing)
            {
                if (_currentAttackStage == 0 && animationTime > 0.47f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.HandRHitbox.Activate(0f, GetHitboxTime(0.09f));
                    PlaySound(TitanSounds.Swing1);
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackStomp)
            {
                if (_currentAttackStage == 0 && animationTime > 0.24f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.FootLHitbox.Activate(0f, GetHitboxTime(0.09f));
                }
                else if (_currentAttackStage == 1 && animationTime > 0.33f)
                {
                    var position = BaseTitanCache.FootLHitbox.transform.position;
                    position.y = BaseTitanCache.Transform.position.y;
                    EffectSpawner.Spawn(EffectPrefabs.Boom5, position, BaseTitanCache.Transform.rotation, Size * SizeMultiplier);
                    SpawnShatter(position);
                    _currentAttackStage = 2;
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackHead)
            {
                if (_currentAttackStage == 0 && animationTime > 0.26f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.HandLHitbox.Activate(0f, GetHitboxTime(0.08f));
                    AnnieCache.HandRHitbox.Activate(0f, GetHitboxTime(0.08f));
                    PlaySound(TitanSounds.Swing1);
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackBite)
            {
                if (_currentAttackStage == 0 && animationTime > 0.27f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.MouthHitbox.Activate(0f, GetHitboxTime(0.12f));
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackBrushBack)
            {
                if (_currentAttackStage == 0 && animationTime > 0.41f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.HandRHitbox.Activate(0f, GetHitboxTime(0.18f));
                    PlaySound(TitanSounds.Swing1);
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackBrushFrontL)
            {
                if (_currentAttackStage == 0 && animationTime > 0.45f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.HandRHitbox.Activate(0f, GetHitboxTime(0.18f));
                    PlaySound(TitanSounds.Swing1);
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackBrushFrontR)
            {
                if (_currentAttackStage == 0 && animationTime > 0.45f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.HandLHitbox.Activate(0f, GetHitboxTime(0.18f));
                    PlaySound(TitanSounds.Swing1);
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackBrushHeadL)
            {
                if (_currentAttackStage == 0 && animationTime > 0.42f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.HandLHitbox.Activate(0f, GetHitboxTime(0.09f));
                    PlaySound(TitanSounds.Swing1);
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackBrushHeadR)
            {
                if (_currentAttackStage == 0 && animationTime > 0.42f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.HandRHitbox.Activate(0f, GetHitboxTime(0.09f));
                    PlaySound(TitanSounds.Swing1);
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackGrabBottomLeft)
            {
                if (_currentAttackStage == 0 && animationTime > 0.31f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.HandRHitbox.Activate(0f, GetHitboxTime(0.04f));
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackGrabBottomRight)
            {
                if (_currentAttackStage == 0 && animationTime > 0.31f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.HandLHitbox.Activate(0f, GetHitboxTime(0.04f));
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackGrabMidLeft)
            {
                if (_currentAttackStage == 0 && animationTime > 0.31f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.HandRHitbox.Activate(0f, GetHitboxTime(0.05f));
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackGrabMidRight)
            {
                if (_currentAttackStage == 0 && animationTime > 0.31f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.HandLHitbox.Activate(0f, GetHitboxTime(0.05f));
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackGrabUp)
            {
                if (_currentAttackStage == 0 && animationTime > 0.27f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.HandLHitbox.Activate(0f, GetHitboxTime(0.1f));
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackGrabUpLeft)
            {
                if (_currentAttackStage == 0 && animationTime > 0.28f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.HandLHitbox.Activate(0f, GetHitboxTime(0.05f));
                }
            }
            else if (_currentAttackAnimation == AnnieAnimations.AttackGrabUpRight)
            {
                if (_currentAttackStage == 0 && animationTime > 0.28f)
                {
                    _currentAttackStage = 1;
                    AnnieCache.HandRHitbox.Activate(0f, GetHitboxTime(0.05f));
                }
            }
        }

        protected override void DamagedGrunt(float chance = 1f)
        {
            if (SettingsManager.SoundSettings.TitanVocalEffect.Value && RandomGen.Roll(chance))
            {
                PlaySound(ShifterSounds.AnnieHurt);
            }
        }
    }
}
