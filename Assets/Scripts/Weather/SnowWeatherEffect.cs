using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Settings;
using ApplicationManagers;
using Utility;

namespace Weather
{
    class SnowWeatherEffect : BaseWeatherEffect
    {
        protected override Vector3 _positionOffset => Vector3.up * 0f;

        public override void Randomize()
        {

        }

        public override void SetLevel(float level)
        {
            base.SetLevel(level);
            if (level <= 0f)
            {
                return;
            }
            float scale = level;
            SetActiveParticleSystem(0);
            var main = _particleSystems[0].main;
            var emission = _particleSystems[0].emission;
            emission.rateOverTime = ClampParticles(100f + scale * 200f);
            main.startSize = 25f;
            SetActiveAudio(0, 0.25f + 0.25f * scale);
        }

        public override void Setup(Transform parent)
        {
            base.Setup(parent);
        }
    }
}
