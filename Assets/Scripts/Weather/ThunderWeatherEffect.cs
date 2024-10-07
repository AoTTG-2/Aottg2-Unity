using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Settings;
using ApplicationManagers;
using Utility;

namespace Weather
{
    class ThunderWeatherEffect : BaseWeatherEffect
    {
        public static List<List<LightningParticle>> LightningPool = new List<List<LightningParticle>>();
        protected override Vector3 _positionOffset => Vector3.up * 0f;
        protected float _lightningWaitTime = 15f;
        const int MaxLightningParticles = 4;

        public static void OnFinishInit()
        {
            for (int i = 0; i < 10; i++)
            {
                Vector3 strikeStart = Vector3.up * 1500f + Vector3.right * Random.Range(-1000f, 1000f);
                Vector3 strikeEnd = Vector3.down * 300f + Vector3.right * Random.Range(-1000f, 1000f);
                float distance = Vector3.Distance(strikeStart, strikeEnd);
                int generations = 9;
                if (SettingsManager.GraphicsSettings.WeatherEffects.Value == (int)WeatherEffectLevel.Medium)
                    generations = 8;
                List<LightningParticle> lightningParticles = new List<LightningParticle>();
                for (int j = 0; j < MaxLightningParticles; j++)
                {
                    LightningParticle particle = ResourceManager.InstantiateAsset<GameObject>(ResourcePaths.Weather, "Prefabs/LightningParticle").AddComponent<LightningParticle>();
                    DontDestroyOnLoad(particle.gameObject);
                    particle.Setup(strikeStart, strikeEnd, generations);
                    lightningParticles.Add(particle);
                    particle.Disable();
                }
                LightningPool.Add(lightningParticles);
            }
        }

        public override void Randomize()
        {
        }

        public override void Setup(Transform parent)
        {
            base.Setup(parent);
        }

        public override void SetLevel(float level)
        {
            base.SetLevel(level);
            if (level <= 0f)
                return;
            if (level < 0.5f)
            {
                SetActiveAudio(0, 1f);
            }
            else
            {
                SetActiveAudio(1, 1f);
            }
        }

        protected void FixedUpdate()
        {
            _lightningWaitTime -= Time.fixedDeltaTime;
            if (_lightningWaitTime <= 0f)
            {
                _lightningWaitTime = 20f - _level * 15f;
                _lightningWaitTime = Random.Range(_lightningWaitTime * 0.5f, _lightningWaitTime * 1.5f);
                CreateLightning();
            }
        }

        protected void CreateLightning()
        {
            List<LightningParticle> particles = LightningPool[Random.Range(0, LightningPool.Count)];
            int count = Random.Range(1, MaxLightningParticles);
            float fov = SceneLoader.CurrentCamera.Camera.fieldOfView;
            Vector3 camForward = new Vector3(_parent.forward.x, 0f, _parent.forward.z).normalized;
            Vector3 camForwardStart = Quaternion.AngleAxis(Random.Range(-fov * 0.5f, fov * 0.5f), Vector3.up) * camForward;
            float distance = Random.Range(900f, 1400f);
            Vector3 position = transform.position + camForward * distance;
            for (int i = 0; i < count; i++)
            {
                particles[i].transform.position = position;
                particles[i].transform.LookAt(_parent);
                particles[i].Enable();
                particles[i].Strike(i == 0);
            }
        }
    }
}
