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
    class ArmoredShifter : BaseShifter
    {
        protected ArmoredComponentCache ArmoredCache;
        protected ArmoredAnimations ArmoredAnimations;
        public override List<string> EmoteActions => new List<string>();

		protected override float DefaultRunSpeed => 110;

        protected override void CreateCache(BaseComponentCache cache)
        {
            ArmoredCache = new ArmoredComponentCache(gameObject);
            base.CreateCache(ArmoredCache);
        }

        protected override void CreateAnimations(BaseTitanAnimations animations)
        {
            ArmoredAnimations = new ArmoredAnimations();
            base.CreateAnimations(ArmoredAnimations);
        }

        public override void Emote(string emote)
        {
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
            return new Dictionary<string, float>();
        }

        protected override void UpdateAttack()
        {
            float animationTime = GetAnimationTime();
            if (_currentStateAnimation == ArmoredAnimations.AttackSwipe)
            {
                if (_currentAttackStage == 0 && animationTime > 0.06f)
                {
                    _currentAttackStage = 1;
                    ArmoredCache.HandLHitbox.Activate(0f, 0.35f / _currentAttackSpeed);
                    PlaySound(TitanSounds.Swing1);
                }
            }
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            transform.Find("Amarture_VER2").localScale = Vector3.one * 15f;
        }

        public override Transform GetCameraAnchor()
        {
            return transform.Find("Anchor");
        }
    }
}
