using Settings;
using UnityEngine;

namespace Map
{
    class MapLight
    {
        public Light Light;
        public float MaxIntensity;
        private Transform _transform;

        public MapLight(Light light)
        {
            Light = light;
            MaxIntensity = light.intensity;
            _transform = light.transform;
        }

        public void UpdateCull(Transform cameraPosition)
        {
            if (_transform == null || Light == null)
                return;
            float distance = Vector3.Distance(cameraPosition.position, _transform.position);
            float lightDistance = SettingsManager.GraphicsSettings.LightDistance.Value;
            if (distance >= lightDistance || lightDistance <= 0f)
                UpdateIntensity(0f);
            else
            {
                float intensity = Mathf.Clamp(1f - (distance / lightDistance), 0f, 1f) * MaxIntensity;
                UpdateIntensity(intensity);
            }
        }

        private void UpdateIntensity(float intensity)
        {
            if (intensity == 0f)
            {
                if (Light.enabled)
                    Light.enabled = false;
            }
            else
            {
                if (!Light.enabled)
                    Light.enabled = true;
                if (Light.intensity != intensity)
                    Light.intensity = intensity;
            }
        }
    }
}