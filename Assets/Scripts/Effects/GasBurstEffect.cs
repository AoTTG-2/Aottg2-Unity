using Photon;
using System;
using UnityEngine;
using System.Collections;
using Settings;
using Photon.Realtime;
using Utility;
using GameManagers;
using Photon.Pun;
using Characters;

namespace Effects {
    class GasBurstEffect : BaseEffect
    {
        public override void Setup(Player owner, float liveTime, object[] settings)
        {
            base.Setup(owner, liveTime, settings);
            ParticleSystem particle = GetComponentInChildren<ParticleSystem>();
            int viewId = owner.GetIntProperty(PlayerProperty.CharacterViewId, 0);
            if (viewId > 0)
            {
                var photonView = PhotonView.Find(viewId);
                if (photonView != null)
                {
                    var human = photonView.GetComponent<Human>();
                    if (human != null && !human.Dead)
                        particle.GetComponent<Renderer>().material = human.HumanCache.Smoke.GetComponent<Renderer>().material;
                }
            }
        }
    }
}