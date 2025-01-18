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
    class HumanDummy: MonoBehaviour
    {
        public HumanComponentCache Cache;
        public HumanState State = HumanState.Idle;
        public HumanSetup Setup;
        public AnimationHandler Animation;

        // actions
        private float _stateTimeLeft = 0f;

        protected void Awake()
        {
            Cache = new HumanComponentCache(gameObject);
            Cache.Rigidbody.freezeRotation = true;
            Cache.Rigidbody.useGravity = false;
            Cache.Rigidbody.velocity = Vector3.zero;
            Setup = gameObject.GetComponent<HumanSetup>();
            if (Setup == null)
                Setup = gameObject.AddComponent<HumanSetup>();
            Animation = new AnimationHandler(gameObject);
        }

        protected void Start()
        {
        }

        public void Idle()
        {
            State = HumanState.Idle;
            bool male = Setup.CustomSet.Sex.Value == (int)HumanSex.Male;
            string animation;
            if (Setup.Weapon == HumanWeapon.AHSS || Setup.Weapon == HumanWeapon.APG)
                animation = male ? HumanAnimations.IdleAHSSM : HumanAnimations.IdleAHSSF;
            else if (Setup.Weapon == HumanWeapon.Thunderspear)
                animation = male ? HumanAnimations.IdleTSM : HumanAnimations.IdleTSF;
            else
                animation = male ? HumanAnimations.IdleM : HumanAnimations.IdleF;
            Animation.CrossFade(animation, 0.1f);
        }

        public void EmoteAction(string emote)
        {
            string animation = HumanAnimations.EmoteSalute;
            if (emote == "Salute")
                animation = HumanAnimations.EmoteSalute;
            else if (emote == "Dance")
                animation = HumanAnimations.SpecialArmin;
            else if (emote == "Flip")
                animation = HumanAnimations.Dodge;
            else if (emote == "Wave")
                animation = HumanAnimations.EmoteWave;
            else if (emote == "Nod")
                animation = HumanAnimations.EmoteYes;
            else if (emote == "Shake")
                animation = HumanAnimations.EmoteNo;
            else if (emote == "Eat")
                animation = HumanAnimations.SpecialSasha;
            State = HumanState.EmoteAction;
            Animation.CrossFade(animation, 0.1f);
            _stateTimeLeft = Animation.GetLength(animation);
        }

        protected void Update()
        {
            if (State != HumanState.Idle)
            {
                _stateTimeLeft -= Time.deltaTime;
                if (_stateTimeLeft <= 0f)
                {
                    Idle();
                }
            }
        }

        protected void FixedUpdate()
        {
        }
    }
}
