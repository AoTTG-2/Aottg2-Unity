using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomSkins;
using Settings;
using UI;
using ApplicationManagers;
using Weather;
using GameProgress;
using GameManagers;
using Controllers;

namespace Characters
{
    class DummyTitan: DummyCharacter
    {
        public BasicTitanSetup Setup;
        public BasicTitanComponentCache Cache;
        protected BasicTitanAnimations BasicAnimations;

        protected override void Awake()
        {
            base.Awake();
            Cache = new BasicTitanComponentCache(gameObject);
            Cache.Rigidbody.freezeRotation = true;
            Cache.Rigidbody.useGravity = false;
            Setup = gameObject.AddComponent<BasicTitanSetup>();
            BasicAnimations = new BasicTitanAnimations();
            transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }

        protected override string GetIdleAnimation()
        {
            return BasicAnimations.Idle;
        }

        protected override string GetEmoteAnimation(string emote)
        {
            if (emote == "Laugh")
                return BasicAnimations.EmoteLaugh;
            else if (emote == "Nod")
                return BasicAnimations.EmoteNod;
            else if (emote == "Shake")
                return BasicAnimations.EmoteShake;
            else if (emote == "Roar")
                return BasicAnimations.EmoteRoar;
            return string.Empty;
        }
    }
}
