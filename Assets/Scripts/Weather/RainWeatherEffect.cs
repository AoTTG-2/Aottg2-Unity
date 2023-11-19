using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Settings;
using ApplicationManagers;

namespace Weather
{
    class RainWeatherEffect : BaseWeatherEffect
    {
        protected override Vector3 _positionOffset => Vector3.up * 30f;

        public override void Randomize()
        {
            float angle1 = Random.Range(0f, 20f);
            angle1 = Random.Range(-angle1, angle1);
            foreach (ParticleSystem system in _particleSystems)
            {
                system.transform.localPosition = _positionOffset;
                system.transform.localRotation = Quaternion.identity;
                system.transform.RotateAround(_transform.position, Vector3.forward, angle1);
                system.transform.RotateAround(_transform.position, Vector3.up, Random.Range(0f, 360f));
            }
        }

        public override void SetLevel(float level)
        {
            base.SetLevel(level);
            if (level <= 0f)
                return;
            if (level < 0.5f)
            {
                float scale = level / 0.5f;
                SetActiveParticleSystem(0);
                var main = _particleSystems[0].main;
                var emission = _particleSystems[0].emission;
                emission.rateOverTime = ClampParticles(50f + (150f * scale));
                main.startSize = 30f + 30f * scale;
                SetActiveAudio(0, 0.25f + 0.25f * scale);
            }
            else
            {
                float scale = (level - 0.5f) / 0.5f;
                SetActiveParticleSystem(1);
                var main = _particleSystems[1].main;
                var emission = _particleSystems[1].emission;
                emission.rateOverTime = ClampParticles(100f + (150f * scale));
                main.startSize = 50f + scale * 10f;
                SetActiveAudio(1, 0.25f + 0.25f * scale);
            }
        }

        public override void Setup(Transform parent)
        {
            base.Setup(parent);
        }
    }
}
