using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Settings;
using ApplicationManagers;

namespace Weather
{
    class FlashlightWeatherEffect : BaseWeatherEffect
    {
        protected override Vector3 _positionOffset => Vector3.up * 0f;
        private Light _light;

        public override void Randomize()
        {
            
        }

        public override void Setup(Transform parent)
        {
            base.Setup(parent);
            _light = GetComponentInChildren<Light>();
            _light.range = 120f;
            _light.intensity = 1f;
            _light.spotAngle = 60f;
            SetColor(Color.black);
        }

        public virtual void SetColor(Color color)
        {
            _light.color = color;
        }

        protected override void LateUpdate()
        {
            if (_parent != null)
            {
                if (!_light.gameObject.activeSelf)
                    _light.gameObject.SetActive(true);
                _transform.rotation = _parent.rotation * Quaternion.Euler(353f, 0f, 0f);
                _transform.position = _parent.position;
            }
            else if (_light.gameObject.activeSelf)
                _light.gameObject.SetActive(false);
        }
    }
}
