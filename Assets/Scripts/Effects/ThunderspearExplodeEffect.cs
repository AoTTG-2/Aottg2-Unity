using Photon;
using System;
using UnityEngine;
using System.Collections;
using Settings;
using Photon.Realtime;

namespace Effects {
    class ThunderspearExplodeEffect : BaseEffect
    {
        public static float SizeMultiplier = 1.1f;

        public override void Setup(Player owner, float liveTime, object[] settings)
        {
            base.Setup(owner, liveTime, settings);
            ParticleSystem particle = GetComponent<ParticleSystem>();
            if (SettingsManager.AbilitySettings.UseOldEffect.Value)
            {
                particle.Stop();
                particle.Clear();
                particle = transform.Find("OldExplodeEffect").GetComponent<ParticleSystem>();
                particle.gameObject.SetActive(true);
            }
            else
            {
                var main = particle.main;
                main.startSizeMultiplier *= SizeMultiplier;
            }
            if (SettingsManager.AbilitySettings.ShowBombColors.Value)
            {
                var c = (Color)(settings[0]);
                var main = particle.main;
                main.startColor = new Color(c.r, c.g, c.b, Mathf.Max(c.a, 0.5f));
            }
           
            bool kill = (bool)(settings[1]);
            if (kill)
                transform.Find("KillSound").GetComponent<AudioSource>().Play();
            else
                transform.Find("ExplodeSound").GetComponent<AudioSource>().Play();
        }
    }
}