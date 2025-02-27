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
    class DummyCharacter: MonoBehaviour
    {
        public AnimationHandler Animation;
        public DummyState State;
        protected float _stateTimeLeft = 0f;

        protected virtual void Awake()
        {
            Animation = new AnimationHandler(gameObject);
        }

        protected virtual string GetIdleAnimation()
        {
            return string.Empty;
        }

        protected virtual string GetEmoteAnimation(string emote)
        {
            return string.Empty;
        }

        public void Idle()
        {
            State = DummyState.Idle;
            Animation.CrossFade(GetIdleAnimation(), 0.1f, 0f);
        }

        public void EmoteAction(string emote)
        {
            State = DummyState.Emote;
            string animation = GetEmoteAnimation(emote);
            Animation.CrossFade(animation, 0.1f, 0f);
            _stateTimeLeft = Animation.GetLength(animation);
        }

        protected void Update()
        {
            if (State != DummyState.Idle)
            {
                _stateTimeLeft -= Time.deltaTime;
                if (_stateTimeLeft <= 0f)
                    Idle();
            }
        }
    }

    enum DummyState
    {
        Idle,
        Emote
    }
}
