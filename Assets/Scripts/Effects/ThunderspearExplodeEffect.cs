using Photon;
using System;
using UnityEngine;
using System.Collections;
using Settings;
using Photon.Realtime;
using Projectiles;

namespace Effects
{
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

            TSKillType killType = (TSKillType)(settings[1]);
            bool impact = (bool)(settings[2]);

            switch (killType)
            {
                case TSKillType.Air:
                    transform.Find($"TSAir{UnityEngine.Random.Range(1, 2)}").GetComponent<AudioSource>().Play();
                    break;
                case TSKillType.Ground:
                    transform.Find($"TSHitTitan{UnityEngine.Random.Range(1, 2)}").GetComponent<AudioSource>().Play();
                    break;
                case TSKillType.Kill:
                    transform.Find($"TSKill{UnityEngine.Random.Range(1, 2)}").GetComponent<AudioSource>().Play();
                    break;
                case TSKillType.ArmorHit:
                    transform.Find($"TSArmor{UnityEngine.Random.Range(1, 2)}").GetComponent<AudioSource>().Play();
                    break;
                case TSKillType.CloseShot:
                    transform.Find($"TSKill{UnityEngine.Random.Range(1, 2)}").GetComponent<AudioSource>().Play();
                    break;
                case TSKillType.MaxRangeShot:
                    transform.Find($"TSArmor{UnityEngine.Random.Range(1, 2)}").GetComponent<AudioSource>().Play();
                    break;
                default:
                    transform.Find($"TSAir{UnityEngine.Random.Range(1, 2)}").GetComponent<AudioSource>().Play();
                    break;
            }
        }
    }
}