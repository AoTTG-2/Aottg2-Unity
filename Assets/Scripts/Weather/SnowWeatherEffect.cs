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
            return;
            if (level <= 0f)
            {
                return;
            }
            if (level <= 0.5f)
            {
                float scale = level / 0.5f;
                //SetActiveEmitter(0);
                SetActiveAudio(0, 0.25f + 0.25f * scale);
            }
            else
            {
                float scale = (level - 0.5f) / 0.5f;
                //SetActiveEmitter(1);
                SetAudioVolume(1, 0.25f + 0.25f * scale);
            }
        }

        public override void Setup(Transform parent)
        {
            base.Setup(parent);
        }
    }
}
